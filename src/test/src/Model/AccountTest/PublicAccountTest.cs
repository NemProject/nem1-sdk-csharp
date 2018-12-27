//
// Copyright 2018 NEM
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Blockchain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Model.AccountTest
{
    [TestClass]
    public class PublicAccountTest
    {
        private readonly string publicKey = "29c4a4aa674953749053c8a35399b37b713dedd5d002cb29b3331e56ff1ea65a";

        [TestMethod]
        public void ShouldCreatePublicAccountViaConstructor()
        {
            var publicAccount = new PublicAccount(publicKey, NetworkType.Types.TEST_NET);
            Assert.AreEqual(publicKey, publicAccount.PublicKey);
            Assert.AreEqual("TBDJXUULP2BRYNS7MWHY2WAFWKQNAF273KYBPFY5", publicAccount.Address.Plain);
        }

        [TestMethod]
        public void ShouldCreatePublicAccountViaStaticConstructor()
        {
            var publicAccount = new PublicAccount(publicKey, NetworkType.Types.TEST_NET);
            Assert.AreEqual(publicKey, publicAccount.PublicKey);
            Assert.AreEqual("TBDJXUULP2BRYNS7MWHY2WAFWKQNAF273KYBPFY5", publicAccount.Address.Plain);
        }

        [TestMethod]
        public void EqualityIsBasedOnPublicKeyAndNetwork()
        {
            var publicAccount = new PublicAccount(publicKey, NetworkType.Types.TEST_NET);
            var publicAccount2 = new PublicAccount(publicKey, NetworkType.Types.TEST_NET);
            Assert.AreEqual(publicAccount.Address.Pretty, publicAccount2.Address.Pretty);
        }

        [TestMethod]
        public void EqualityReturnsFalseIfNetworkIsDifferent()
        {
            var publicAccount = new PublicAccount(publicKey, NetworkType.Types.TEST_NET);
            var publicAccount2 = new PublicAccount(publicKey, NetworkType.Types.MAIN_NET);
            Assert.AreNotEqual(publicAccount.Address.Plain, publicAccount2.Address.Plain);
            Assert.AreNotEqual(publicAccount.Address.Pretty, publicAccount2.Address.Pretty);
            Assert.AreNotEqual(publicAccount.Address.Networktype(), publicAccount2.Address.Networktype());
        }
    }
}
