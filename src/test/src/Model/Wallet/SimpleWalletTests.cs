using io.nem1.sdk.Model.Network;
using io.nem1.sdk.Model.Wallet;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Model.Wallet
{
    [TestClass]
    public class SimpleWalletTests
    {
        [TestMethod]
        public void ImportWallet()
        {
            var wlt = SimpleWallet.ImportFromNanoWalletFile("eyJwcml2YXRlS2V5IjoiIiwibmFtZSI6InNpbXBsZXdhbGxldCIsImFjY291bnRzIjp7IjAiOnsiYnJhaW4iOnRydWUsImFsZ28iOiJwYXNzOmJpcDMyIiwiZW5jcnlwdGVkIjoiNTkwYzY3NWJlMzBkYzg1ZDI1MTI2MjBiMDUyNmM1ZGRiNjc1NmFkZjk4ZWJmODI3ZjdjMTI0ZTlmMTE1YTgxOTQzYjc0MWQ3MWRlNDM5NzkwMWNkYjFiOTE3YmYxZDY1IiwiaXYiOiJkNTkwYWIxNTUzNTFhYmRkOWM1MTFlOGZiNDZlZTdhOSIsImFkZHJlc3MiOiJOQ0RET1E1SDI0U1Q0NVA1SEwzSk1aSzRITlJXUFhWRkJBNVRXN0FGIiwibGFiZWwiOiJQcmltYXJ5IiwibmV0d29yayI6MTA0LCJjaGlsZCI6ImQxYWMyN2Y4NTc0Y2IxNTcxNzFmMTRiNWZkMjZlYmMxMjRlMzgzN2I0ODZkOTcyMTdjOWQ4NWFjOGFjNjMwOTkifX19", NetworkType.Types.MAIN_NET);           

            Assert.AreEqual("", wlt.WalletObj.PrivateKey);
            Assert.AreEqual("simplewallet", wlt.WalletObj.Name);
            Assert.AreEqual("590c675be30dc85d2512620b0526c5ddb6756adf98ebf827f7c124e9f115a81943b741d71de4397901cdb1b917bf1d65", wlt.EncryptedPrivateKey.EncryptedKey);
            Assert.AreEqual("d590ab155351abdd9c511e8fb46ee7a9", wlt.EncryptedPrivateKey.Iv);
            Assert.AreEqual("9b4ed1b56be9e8106da2ef4fc681db0f9f4c22ccbad5f9bb9d3fbe68a1b20007", wlt.EncryptedPrivateKey.Decrypt(new Password("simplewallet")));
            Assert.AreEqual((byte)104, wlt.Network.GetNetwork());
            Assert.AreEqual("simplewallet", wlt.Name);
            Assert.AreEqual("NCDDOQ5H24ST45P5HL3JMZK4HNRWPXVFBA5TW7AF", wlt.WalletObj.Accounts.Account[0].Address);
            Assert.AreEqual("590c675be30dc85d2512620b0526c5ddb6756adf98ebf827f7c124e9f115a81943b741d71de4397901cdb1b917bf1d65", wlt.WalletObj.Accounts.Account[0].Encrypted);
            Assert.AreEqual("d590ab155351abdd9c511e8fb46ee7a9", wlt.WalletObj.Accounts.Account[0].Iv);
            Assert.AreEqual("pass:bip32", wlt.WalletObj.Accounts.Account[0].Algo);
            Assert.AreEqual("d1ac27f8574cb157171f14b5fd26ebc124e3837b486d97217c9d85ac8ac63099", wlt.WalletObj.Accounts.Account[0].Child);
            Assert.AreEqual("Primary", wlt.WalletObj.Accounts.Account[0].Label);
        }

        [TestMethod]
        public void CreateNewWallet()
        {
            var wlt = SimpleWallet.CreateNewSimpleWallet("one", new Password("testtest"), NetworkType.Types.MAIN_NET);

            Assert.AreEqual("", wlt.WalletObj.PrivateKey);
            Assert.AreEqual("one", wlt.WalletObj.Name);          
            Assert.AreEqual((byte)104, wlt.Network.GetNetwork());
            Assert.AreEqual("one", wlt.Name);
            Assert.IsNotNull(wlt.WalletObj.Accounts.Account[0].Address);
            Assert.IsNotNull(wlt.WalletObj.Accounts.Account[0].Encrypted);
            Assert.IsNotNull(wlt.WalletObj.Accounts.Account[0].Iv);
            Assert.AreEqual("pass:bip32", wlt.WalletObj.Accounts.Account[0].Algo);
            Assert.AreEqual("", wlt.WalletObj.Accounts.Account[0].Child);
            Assert.AreEqual("Primary", wlt.WalletObj.Accounts.Account[0].Label);
        }

        [TestMethod]
        public void CreateNewWalletWithKey()
        {
            var wlt = SimpleWallet.CreateNewSimpleWallet("two", new Password("testtest"), "9822cf9571a5551ec19720b87a567a20797b75ec4b6711387643fc352fef704e", NetworkType.Types.MAIN_NET);

            Assert.AreEqual("", wlt.WalletObj.PrivateKey);
            Assert.AreEqual("two", wlt.WalletObj.Name);
            Assert.AreEqual(wlt.EncryptedPrivateKey.Decrypt(new Password("testtest")), "9822cf9571a5551ec19720b87a567a20797b75ec4b6711387643fc352fef704e");
            Assert.AreEqual((byte)104, wlt.Network.GetNetwork());
            Assert.AreEqual("two", wlt.Name);
            Assert.IsNotNull(wlt.WalletObj.Accounts.Account[0].Address);
            Assert.IsNotNull(wlt.WalletObj.Accounts.Account[0].Encrypted);
            Assert.IsNotNull(wlt.WalletObj.Accounts.Account[0].Iv);
            Assert.AreEqual("pass:bip32", wlt.WalletObj.Accounts.Account[0].Algo);
            Assert.AreEqual("", wlt.WalletObj.Accounts.Account[0].Child);
            Assert.AreEqual("Primary", wlt.WalletObj.Accounts.Account[0].Label);
        }
    }
}
