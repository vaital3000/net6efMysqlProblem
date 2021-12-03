using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Net6Testa
{
	public static class HasColumnTypeInOwnedClassInvalidSample
	{
		public static async Task TryToGetData()
		{
			Helpers.RecreateCleanDatabase();

			await using var context = new SomeDbContext();
			await context.Entities.Where(x => !x.Metadata.IsRemovedInMetadata).ToArrayAsync();
		}

		private static class Helpers
		{
			public static void RecreateCleanDatabase()
			{
				using var context = new SomeDbContext();

				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();
			}
		}


		public class SomeDbContext : DbContext
		{
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
				metadata.Property(x => x.IsRemovedInMetadata).IsRequired().HasColumnType("bit(1)");
			}

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
