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

using System.Threading.Tasks;
using io.nem1.sdk.Infrastructure.HttpRepositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reactive.Linq;
using io.nem1.sdk.Model.Network;
using io.nem1.sdk.Model.Transactions.Messages;
using io.nem1.sdk.Model.Transactions;
using io.nem1.sdk.Model.Mosaics;

namespace IntegrationTest.infrastructure.HttpTests
{
    [TestClass]
    public class TransactionHttpTests
    {
        const string PUBKEYMESS = "7b1a93132b8c5b8001a07f973307bee2b37bcd6dc279a59ea98179b238d44e2d";
        readonly string host = "http://" + Config.Domain + ":7890";

        [TestMethod]
        public async Task GetTransaction()
        {
            const string hash = "5853eaebe86307bf8a5dbddb5248490cb1f9ca6cb76c4733dab8eea157988f7a";
            var tx = await new TransactionHttp(host).GetTransaction(hash);
            Assert.AreEqual(tx.NetworkType, NetworkType.Types.TEST_NET);
            Assert.AreEqual(tx.Version, 1);
            Assert.IsNotNull(tx.TransactionInfo);
            Assert.AreEqual(tx.TransactionInfo.Hash, hash);
        }

        [TestMethod]
        public async Task GetTransactionWithHexidecimalMessage()
        {
            const string HASH = "5853eaebe86307bf8a5dbddb5248490cb1f9ca6cb76c4733dab8eea157988f7a";

            var tx = await new TransactionHttp(host).GetTransaction(HASH);

            Assert.AreEqual(tx.Signer.PublicKey, PUBKEYMESS);
            Assert.AreEqual(tx.TransactionInfo.Hash, HASH);
            var ttx = (TransferTransaction)tx;
            Assert.AreEqual(ttx.Mosaics[0].MosaicName, Xem.MosaicName);
            Assert.AreEqual(ttx.Mosaics[0].Amount, (ulong)10000000);
            Assert.AreEqual(ttx.Message.GetMessageType() , MessageType.Type.UNENCRYPTED);
            Assert.AreEqual(((HexMessage)ttx.Message).GetStringPayload(), "abcd1234");
        }

        [TestMethod]
        public async Task GetTransactionWithEncryptedMessage()
        {
            const string HASH = "fc60dbe36b99769261b86dc562e7dd2189a440cf878511e2e310208335bc5e9d";

            var tx = await new TransactionHttp(host).GetTransaction(HASH);

            Assert.AreEqual(tx.Signer.PublicKey, PUBKEYMESS);
            Assert.AreEqual(tx.TransactionType, TransactionTypes.Types.Transfer);
            var ttx = (TransferTransaction)tx;
            Assert.AreEqual(HASH, ttx.TransactionInfo.Hash);
            Assert.AreEqual(ttx.Message.GetMessageType(), MessageType.Type.ENCRYPTED);
            SecureMessage smsg = (SecureMessage)ttx.Message;
            //Assert.AreEqual("Decrypted message", smsg.GetDecodedPayload("PRIVATE KEY", PUBKEYMESS));
        }

    }
}
