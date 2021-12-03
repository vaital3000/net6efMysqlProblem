using System.Threading.Tasks;
using Xunit;

namespace Net6Testa
{
	public class EfCoreTests
	{
		[Fact]
		public async Task TryToGetDataWithSetupColumnTypeForOwned()
		{
			await HasColumnTypeInOwnedClassInvalidSample.TryToGetData();
		}

		[Fact]
		public async Task TryToGetDataWithoutSetupColumnTypeForOwned()
		{
			await HasColumnTypeInOwnedClassValidSample.TryToGetData();
		}
	}
}
