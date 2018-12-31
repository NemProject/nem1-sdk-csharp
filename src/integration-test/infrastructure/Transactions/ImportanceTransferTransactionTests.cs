using System.Reactive.Linq;
using System.Threading.Tasks;
using io.nem1.sdk.Infrastructure.HttpRepositories;
using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Network;
using io.nem1.sdk.Model.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTest.infrastructure.Transactions
{
    [TestClass]
    public class ImportanceTransferTransactionTests
    {
        /// <summary>
        /// Determines whether this instance [can delegate importance using importance transfer].
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task CanDelegateImportanceUsingImportanceTransfer()
        {
            var keyPair = new KeyPair(Config.PrivateKeyMain);

            var importanceTransfer = ImportanceTransferTransaction.Create(
                    NetworkType.Types.TEST_NET,
                    Deadline.CreateHours(1),
                    ImportanceTransferMode.Mode.Add,
                    new PublicAccount("6ea3fd5f2cf4fbeb54cd96a48d11cd2ff0b4106472c6a97c7e4e5736243cb2db", NetworkType.Types.TEST_NET))
                .SignWith(keyPair);

            var response = await new TransactionHttp("http://" + Config.Domain + ":7890").Announce(importanceTransfer);

            Assert.AreEqual("FAILURE_IMPORTANCE_TRANSFER_NEEDS_TO_BE_DEACTIVATED", response.Message);
        }
    }
}
