using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Blockchain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Model.AccountTest
{
    [TestClass]
    public class CreateAccount
    {
        [TestMethod]
        public void CreateNewAccount()
        {
            var acc = PrivateAccount.GenerateNewAccount(NetworkType.Types.MIJIN_TEST);

            Assert.AreEqual(64, acc.PublicKey.Length);

        }

        [TestMethod]
        public void CreateNewAccountFromKey()
        {
            var acc = PrivateAccount.CreateFromPrivateKey("52b62ec8fafe1d5b7dc2896749f979d5c9ec852a4d37cff9f10656629f4efbf7", NetworkType.Types.MIJIN_TEST);

            Assert.AreEqual(64, acc.PublicKey.Length);

        }
    }
}
