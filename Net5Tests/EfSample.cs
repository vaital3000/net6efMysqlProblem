using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;

namespace Net5Tests
{
	public sealed class EntityMetadata
	{
		public bool IsRemovedInMetadata { get; set; }
	}

	public sealed class MyEntity
	{
		public int Id { get; set; }
		public bool IsRemovedInRoot { get; set; }
		public EntityMetadata Metadata { get; set; }
	}

	public static class HasColumnTypeInOwnedClassSample
	{
		public static async Task TryToGetData(bool setupColumnTypeForOwned)
		{
			Helpers.RecreateCleanDatabase(setupColumnTypeForOwned);

			await using var context = new SomeDbContext(setupColumnTypeForOwned);
			await context.Entities.Where(x => !x.Metadata.IsRemovedInMetadata).ToArrayAsync();
		}

		private static class Helpers
		{
			public static void RecreateCleanDatabase(bool setupColumnTypeForOwned)
			{
				using var context = new SomeDbContext(setupColumnTypeForOwned);

				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();
			}
		}


		public class SomeDbContext : DbContext
		{
			private readonly bool _setupColumnTypeForOwned;

			public SomeDbContext(bool setupColumnTypeForOwned)
			{
				_setupColumnTypeForOwned = setupColumnTypeForOwned;
			}
			public DbSet<MyEntity> Entities { get; set; }

			protected override void OnModelCreating(ModelBuilder modelBuilder)
			{
				modelBuilder.Entity<MyEntity>(BuildAccountingUnit);
			}

			private void BuildAccountingUnit(EntityTypeBuilder<MyEntity> builder)
			{
				builder.Property(x => x.IsRemovedInRoot).IsRequired().HasColumnType("bit(1)");
				builder.OwnsOne(x => x.Metadata, BuildMetadata);
			}

			private void BuildMetadata<T>(OwnedNavigationBuilder<T, EntityMetadata> metadata)
				where T : class
			{
				if (_setupColumnTypeForOwned)
				{
					metadata.Property(x => x.IsRemovedInMetadata).IsRequired().HasColumnType("bit(1)");
				}
			}

			protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
			{
				optionsBuilder
					.UseMySql(
						"Server=localhost;Port=3306;Database=test;Uid=root;SSL Mode=None;Allow User Variables=True",
						new MySqlServerVersion(new Version(5, 7, 28)));
			}
		}
	}
}
