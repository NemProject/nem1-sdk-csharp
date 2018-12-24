using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using io.nem1.sdk.Infrastructure.HttpRepositories;
using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Blockchain;
using io.nem1.sdk.Model.Mosaics;
using io.nem1.sdk.Model.Network;
using io.nem1.sdk.Model.Network.Messages;
using IntegrationTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.nem1.sdk.Model.Transactions;

namespace IntegrationTest.infrastructure.HttpTests
{

    class MultisigTransactionTests
    {
        [TestMethod]
        public async Task AnnounceMultisigTransaction()
        {
            var cosignatory = KeyPair.CreateFromPrivateKey("8db858dcc8e2827074498204b3829154ec4c4f24d13738d3f501003b518ef256");
            var multisigAccount = PublicAccount.CreateFromPublicKey("29c4a4aa674953749053c8a35399b37b713dedd5d002cb29b3331e56ff1ea65a", NetworkType.Types.TEST_NET);
            var recipient = new Account("E45030D2A22D97FDC4C78923C4BBF7602BBAC3B018FFAD2ED278FB49CD6F218C", NetworkType.Types.TEST_NET);

            var transaction = TransferTransaction.Create(
                NetworkType.Types.TEST_NET,
                Deadline.CreateHours(2),
                recipient.Address,
                new List<Mosaic> { Mosaic.CreateFromIdentifier("nem:xem", 1000000) },
                PlainMessage.Create("hello")
            );

            var multisigTransaction = MultisigTransaction.Create(
                    NetworkType.Types.TEST_NET, 
                    Deadline.CreateHours(1), 
                    transaction
                ).SignWith(cosignatory, multisigAccount);

            var response = await new TransactionHttp("http://" + Config.Domain + ":7890").Announce(multisigTransaction);

            Assert.AreEqual("SUCCESS", response.Message);
        }
    }
}
