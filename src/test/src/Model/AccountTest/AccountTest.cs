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
using io.nem1.sdk.Model.Network;
using io.nem1.sdk.Core.Crypto.Chaso.NaCl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.nem1.sdk.Model.Transactions;

namespace Tests.Model.AccountTest
{
    [TestClass]
    public class AccountTest
    {
        [TestMethod]
        public void ShouldCreateAccountViaConstructor()
        {
            PrivateAccount account = new PrivateAccount("c02059f81a44e1a1a4a6eb0ff3b10fb289f255c8b17caafb75d92a69665d508b".ToUpper(), NetworkType.Types.MAIN_NET);
            
            
            Assert.AreEqual("C02059F81A44E1A1A4A6EB0FF3B10FB289F255C8B17CAAFB75D92A69665D508B", account.PrivateKey);
            Assert.AreEqual("B5E726F735A9D59F3452FB199CFAB04662D349C8254C96EEE2C7AA2427BA3BBE", account.PublicKey);
            Assert.AreEqual("NBABGDFTMXWWU6TGFZW76HNZF4YMQQJP2GMMV5C5", account.Address.Plain);
        }

        [TestMethod]
        public void ShouldCreateAccountViaStaticConstructor()
        {
            PrivateAccount account = PrivateAccount.CreateFromPrivateKey("6EA3FD5F2CF4FBEB54CD96A48D11CD2FF0B4106472C6A97C7E4E5736243CB2DB", NetworkType.Types.TEST_NET);
            Assert.AreEqual("6EA3FD5F2CF4FBEB54CD96A48D11CD2FF0B4106472C6A97C7E4E5736243CB2DB", account.PrivateKey);
            Assert.AreEqual("TCTUIF557ZCQOQPW2M6GH4TCDPM2ZYBBL54KGNHR", account.Address.Plain);
            Assert.AreEqual("7B1A93132B8C5B8001A07F973307BEE2B37BCD6DC279A59EA98179B238D44E2D", account.PublicKey);
        }

        [TestMethod]
        public void ShouldCreateAccountViaStaticConstructor2()
        {
            PrivateAccount account = PrivateAccount.CreateFromPrivateKey("D0841A3D27AED59BB25135DA3A17DC8E2A62E70B35A0E46D91CCD7CDB75754F1", NetworkType.Types.TEST_NET);
            Assert.AreEqual("D0841A3D27AED59BB25135DA3A17DC8E2A62E70B35A0E46D91CCD7CDB75754F1", account.PrivateKey);
            Assert.AreEqual("856F39436E33129AFFF95B89ACA998FA23CD751A6F4D79CE4FB9DA9641ECB59C", account.PublicKey);
            Assert.AreEqual("TACOPEXRLZTUWBQA3UXV66R455L76ENWK6OYITBJ", account.Address.Plain);
        }

        [TestMethod]
        public void ShouldCreateAccountViaStaticConstructor3()
        {
            PrivateAccount account = PrivateAccount.CreateFromPrivateKey("CFE47DD9801A5D4FE37183E8F6CA49FFF532A2FE6FE099436DF93B3D62FE17D5", NetworkType.Types.TEST_NET);
            Assert.AreEqual("CFE47DD9801A5D4FE37183E8F6CA49FFF532A2FE6FE099436DF93B3D62FE17D5", account.PrivateKey);
            Assert.AreEqual("FBE95048D0325E2553A5E2AA88B9E12ED59F7C8C0FB8F84A638F43A390116C22", account.PublicKey);
            Assert.AreEqual("TBPAMOPRIATPT76TAZZWERHOK72FIKN4YCD4VJMJ", account.Address.Plain);
        }

        [TestMethod]
        public void ShouldSignTransaction()
        {
            var privateKey = "6ea3fd5f2cf4fbeb54cd96a48d11cd2ff0b4106472c6a97c7e4e5736243cb2db";
            var data = "0101000001000098d8f8f905200000007b1a93132b8c5b8001a07f973307bee2b37bcd6dc279a59ea98179b238d44e2da086010000000000e806fa0528000000544250414d4f50524941545054373654415a5a574552484f4b373246494b4e3459434434564a4d4a80969800000000000a00000001000000020000006869";
            var expected = "2200B5B4BD3CA5589FA9C9AD2DE66C380E5EE31B329F6C186E8D21A57E55E335E8B3E4101C8647809F463AC2D25E9C9910564B241325135C8FFC4D7DD3276303";

            var sig = TransactionExtensions.SignTransaction(new KeyPair(privateKey), data.FromHex());

            Assert.AreEqual(sig.ToHexUpper(), expected);  
        }
    }
}
