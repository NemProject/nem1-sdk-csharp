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
    public class MosaicDefinitonCreationTransactionTest
    {
        [TestMethod]
        public async Task CanCreateMosaicWithoutLevy()
        {
            var keyPair = new KeyPair(Config.PrivateKeyMain);
            var transaction = MosaicDefinitionTransaction.CreateWithoutLevy(
                NetworkType.Types.TEST_NET,
                Deadline.CreateHours(1),
                new MosaicProperties(4, 1000000000, true, true),
                MosaicId.CreateFromMosaicIdentifier("myspace:subspace"),
                new PublicAccount(keyPair.PublicKeyString, NetworkType.Types.TEST_NET),
                "new mosaic test"
            ).SignWith(keyPair);

            var response = await new TransactionHttp("http://" + Config.Domain + ":7890").Announce(transaction);

            Assert.AreEqual("FAILURE_MOSAIC_MODIFICATION_NOT_ALLOWED", response.Message);
        }

        [TestMethod]
        public async Task CanCreateMosaicWithLevy()
        {
            var keyPair = new KeyPair(Config.PrivateKeyMain);
            var transaction = MosaicDefinitionTransaction.CreateWithLevy(
                NetworkType.Types.TEST_NET,
                Deadline.CreateHours(1),
                new MosaicProperties(4, 1000000000, true, true),
                MosaicId.CreateFromMosaicIdentifier("myspace:subspacewithlevy"),
                new MosaicLevy(Mosaic.CreateFromIdentifier("myspace:subspace", 100), 1, new Address("TCTUIF-557ZCQ-OQPW2M-6GH4TC-DPM2ZY-BBL54K-GNHR") ),
                new PublicAccount(keyPair.PublicKeyString, NetworkType.Types.TEST_NET),
                "new mosaic test"
            ).SignWith(keyPair);

            var response = await new TransactionHttp("http://" + Config.Domain + ":7890").Announce(transaction);

            Assert.AreEqual("FAILURE_MOSAIC_MODIFICATION_NOT_ALLOWED", response.Message);
        }
    }
}
