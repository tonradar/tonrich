using Tonrich.Shared.Test.Common;

namespace Tonrich.Shared.Test.UnitTest
{
    [TestClass]
    public class FragmentProviderTest : TestBase
    {
        public static IFragmentProvider FragmentProvider { get; set; } = default!;
        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            FragmentProvider = ServiceProvider.GetRequiredService<IFragmentProvider>();
        }

        [TestMethod]
        [DataRow("mehran")]
        [DataRow("mehran.t.me")]
        [DataRow("0:0e41dc1dc3c9067ed24248580e12b3359818d83dee0304fabcf80845eafafdb2")]
        public async Task GetUserNamePrice_MustWork(string userName)
        {
            var price = await FragmentProvider.GetUserNamePriceAsync(userName);
        }

        [TestMethod]
        //[DataRow("+888 0066 6600")]
        //[DataRow("+88800666600")]
        [DataRow("88800666600")]
        public async Task GetNumberPrice_MustWork(string number)
        {
            var price = await FragmentProvider.GetNumberPriceAsync(number);
        }
    }
}
