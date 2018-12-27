using System.Reactive.Linq;
using System.Threading.Tasks;
using io.nem1.sdk.Infrastructure.HttpRepositories;
using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Blockchain;
using io.nem1.sdk.Model.Mosaics;
using io.nem1.sdk.Model.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTest.infrastructure.Transactions
{
    [TestClass]
    public class SupplyChangeTransactionTest
    {
        [TestMethod]
        public async Task CanIncreaseSupply()
        {
            var keyPair = new KeyPair(Config.PrivateKeyMain);

            var transaction = SupplyChangeTransaction.CreateIncrease(
                    NetworkType.Types.TEST_NET,
                    Deadline.CreateHours(1), 
                    100000, 
                    MosaicId.CreateFromMosaicIdentifier("myspace:subspace")
                ).SignWith(keyPair);

            var response = await new TransactionHttp("http://" + Config.Domain + ":7890").Announce(transaction);

            Assert.AreEqual("SUCCESS", response.Message);
        }

        [TestMethod]
        public async Task CanDecreaseSupply()
        {
            var keyPair = new KeyPair(Config.PrivateKeyMain);

            var transaction = SupplyChangeTransaction.CreateDecrease(
                    NetworkType.Types.TEST_NET,
                    Deadline.CreateHours(1), 
                    100000, 
                    MosaicId.CreateFromMosaicIdentifier("myspace:subspace")
                 ).SignWith(keyPair);

            var response = await new TransactionHttp("http://" + Config.Domain + ":7890").Announce(transaction);

            Assert.AreEqual("SUCCESS", response.Message);
        }
    }
}
