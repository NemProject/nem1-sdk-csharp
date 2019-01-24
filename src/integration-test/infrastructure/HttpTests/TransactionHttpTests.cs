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
            const string HASH = "5853eaebe86307bf8a5dbddb5248490cb1f9ca6cb76c4733dab8eea157988f7a";
            var tx = await new TransactionHttp(host).GetTransaction(HASH);
            Assert.IsNotNull(tx);
            Assert.AreEqual(NetworkType.Types.TEST_NET, tx.NetworkType);
            Assert.AreEqual(1, tx.Version);
            Assert.IsNotNull(tx.TransactionInfo);
            Assert.AreEqual(HASH, tx.TransactionInfo.Hash);
        }

        [TestMethod]
        public async Task GetTransactionWithHexidecimalMessage()
        {
            const string HASH = "5853eaebe86307bf8a5dbddb5248490cb1f9ca6cb76c4733dab8eea157988f7a";
            var tx = await new TransactionHttp(host).GetTransaction(HASH);
            Assert.IsNotNull(tx);
            Assert.IsNotNull(tx.Signer);
            Assert.AreEqual(PUBKEYMESS, tx.Signer.PublicKey);
            Assert.IsNotNull(tx.TransactionInfo);
            Assert.AreEqual(tx.TransactionInfo.Hash, HASH);
            Assert.AreEqual(tx.TransactionType, TransactionTypes.Types.Transfer);
            var ttx = (TransferTransaction)tx;
            Assert.IsNotNull(ttx.Mosaics);
            Assert.IsTrue(ttx.Mosaics.Count > 0);
            Mosaic mosaic = ttx.Mosaics[0];
            Assert.AreEqual(Xem.MosaicName, mosaic.MosaicName);
            Assert.AreEqual((ulong)10000000, mosaic.Amount);
            Assert.AreEqual(MessageType.Type.UNENCRYPTED, ttx.Message.GetMessageType());
            Assert.AreEqual("abcd1234", ((HexMessage)ttx.Message).GetStringPayload());
        }

        [TestMethod]
        public async Task GetTransactionWithEncryptedMessage()
        {
            const string HASH = "fc60dbe36b99769261b86dc562e7dd2189a440cf878511e2e310208335bc5e9d";

            var tx = await new TransactionHttp(host).GetTransaction(HASH);

            Assert.AreEqual(PUBKEYMESS, tx.Signer.PublicKey);
            Assert.AreEqual(TransactionTypes.Types.Transfer, tx.TransactionType);
            var ttx = (TransferTransaction)tx;
            Assert.AreEqual(HASH, ttx.TransactionInfo.Hash);
            Assert.AreEqual(MessageType.Type.ENCRYPTED, ttx.Message.GetMessageType());
            SecureMessage smsg = (SecureMessage)ttx.Message;
            //Assert.AreEqual("Decrypted message", smsg.GetDecodedPayload("PRIVATE KEY", PUBKEYMESS));
        }

    }
}
