using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using io.nem1.sdk.Infrastructure.HttpRepositories;
using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Blockchain;
using io.nem1.sdk.Model.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTest.infrastructure.Transactions
{
    [TestClass]
    public class MultisigModificationTest
    {
        [TestMethod]
        public async Task CanModifyMultisigAccount()
        {
            var keyPair = new KeyPair("8db858dcc8e2827074498204b3829154ec4c4f24d13738d3f501003b518ef256");

            var transaction = MultisigAggregateModificationTransaction.Create(
                NetworkType.Types.TEST_NET,
                Deadline.CreateHours(1), 
                1,
                new List<MultisigModification>()
                {
                    new MultisigModification(new PublicAccount("eb100d6b2da10fc5359ab35a5801b0e6f0b6cc18d849c0aa78ba1aab2b945dea", NetworkType.Types.TEST_NET),
                        CosignatoryModificationType.Types.Add)
                });

            var multisigTransaction = MultisigTransaction.Create(NetworkType.Types.TEST_NET, Deadline.CreateHours(1), transaction)
                    .SignWith(keyPair, new PublicAccount("29c4a4aa674953749053c8a35399b37b713dedd5d002cb29b3331e56ff1ea65a", NetworkType.Types.TEST_NET));

            var response = await new TransactionHttp("http://" + Config.Domain + ":7890").Announce(multisigTransaction);

            Assert.AreEqual("FAILURE_MULTISIG_ALREADY_A_COSIGNER", response.Message);
        }
    }
}
