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
    public class ProvisionNamespaceTests
    {
        [TestMethod]
        public async Task CreateRootNamespace()
        {
            var keyPair = KeyPair.CreateFromPrivateKey(Config.PrivateKeyMain);

            var transaction = ProvisionNamespaceTransaction
                .CreateRootnamespace(
                NetworkType.Types.TEST_NET, 
                Deadline.CreateHours(1), 
                "myspace")
                .SignWith(keyPair);

            var response = await new TransactionHttp("http://" + Config.Domain + ":7890").Announce(transaction);

            Assert.AreEqual("FAILURE_NAMESPACE_PROVISION_TOO_EARLY", response.Message);
        }

        [TestMethod]
        public async Task CreateSubNamespace()
        {
            var keyPair = KeyPair.CreateFromPrivateKey(Config.PrivateKeyMain);

            var transaction = ProvisionNamespaceTransaction
                .CreateSubNamespace(
                    NetworkType.Types.TEST_NET,
                    Deadline.CreateHours(1),
                    "subspace",
                    "myspace"
                ).SignWith(keyPair);

            var response = await new TransactionHttp("http://" + Config.Domain + ":7890").Announce(transaction);

            Assert.AreEqual("FAILURE_NAMESPACE_ALREADY_EXISTS", response.Message);
        }
    }
}
