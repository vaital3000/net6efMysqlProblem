using System.Threading.Tasks;
using Xunit;

namespace Net5Tests
{
	public class EfCoreTests
	{
		[Fact]
		public async Task TryToGetDataWithSetupColumnTypeForOwned()
		{
			await HasColumnTypeInOwnedClassSample.TryToGetData(true);
		}

		[Fact]
		public async Task TryToGetDataWithoutSetupColumnTypeForOwned()
		{
			await HasColumnTypeInOwnedClassSample.TryToGetData(false);
		}
	}
}
