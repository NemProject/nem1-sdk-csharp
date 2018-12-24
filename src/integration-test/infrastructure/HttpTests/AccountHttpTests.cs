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

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using io.nem1.sdk.Infrastructure.HttpRepositories;
using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Blockchain;
using io.nem1.sdk.Model.Mosaics;
using io.nem1.sdk.Model.Network.Messages;
using io.nem1.sdk.Model.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTest.infrastructure.HttpTests
{
    [TestClass]
    public class AccountHttpTests
    {
        readonly string host = "http://" + Config.Domain + ":7890";

        [TestMethod, Timeout(20000)]
        public async Task GetAccountInfoFromAddress()
        {
            const string plain = "TACOPEXRLZTUWBQA3UXV66R455L76ENWK6OYITBJ";

            var acctInfo = await new AccountHttp(host).GetAccountInfo(Address.CreateFromEncoded(plain));
            AssertAcctInfo(acctInfo);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetAccountInfoFromPublicKey()
        {
            const string pubkey = "856f39436e33129afff95b89aca998fa23cd751a6f4d79ce4fb9da9641ecb59c";

            var acctInfo = await new AccountHttp(host).GetAccountInfo(new PublicAccount(pubkey, NetworkType.Types.TEST_NET));
            AssertAcctInfo(acctInfo);
        }

        private void AssertAcctInfo(AccountInfo acctInfo)
        {
            const string plain = "TACOPEXRLZTUWBQA3UXV66R455L76ENWK6OYITBJ";
            const string pretty = "TACOPE-XRLZTU-WBQA3U-XV66R4-55L76E-NWK6OY-ITBJ";
            const string pubkey = "856f39436e33129afff95b89aca998fa23cd751a6f4d79ce4fb9da9641ecb59c";

            Assert.AreEqual(NetworkType.Types.TEST_NET, acctInfo.Address.Networktype);
            Assert.AreEqual(plain, acctInfo.Address.Plain);
            Assert.AreEqual(pretty, acctInfo.Address.Pretty);
            Assert.AreNotEqual((ulong)0, acctInfo.Balance);
            Assert.IsNull(acctInfo.Cosignatories);
            Assert.AreEqual((ulong)0, acctInfo.HarvestedBlocks);
            Assert.AreEqual((ulong)0, acctInfo.Importance);
            Assert.AreEqual(0, acctInfo.MultisigAccountInfo.CosginatoryOf.Count);
            Assert.AreEqual(0, acctInfo.MultisigAccountInfo.Cosignatories.Count);
            Assert.AreEqual(NetworkType.Types.TEST_NET, acctInfo.PublicAccount.Address.Networktype);
            Assert.AreEqual(plain, acctInfo.PublicAccount.Address.Plain);
            Assert.AreEqual(pretty, acctInfo.PublicAccount.Address.Pretty);
            Assert.AreEqual(pubkey, acctInfo.PublicAccount.PublicKey);
            Assert.AreEqual(pubkey, acctInfo.PublicKey);
            Assert.AreNotEqual((ulong)0, acctInfo.VestedBalance);
        }

        [TestMethod]
        public async Task GetAllTransactions()  // Last 25 in- or outgoing transactions
        {
            const string multisigPubkey = "29c4a4aa674953749053c8a35399b37b713dedd5d002cb29b3331e56ff1ea65a";


            var transactions = await new AccountHttp(host).Transactions(new PublicAccount(multisigPubkey, NetworkType.Types.TEST_NET));
            Assert.IsTrue(0 < transactions.Count);
            Assert.IsTrue(25 >= transactions.Count);
            AssertTx(transactions[0], transactions[0].TransactionInfo);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetIncomingTransactions() // Last 25 incomming transactions
        {
            const string multisigPubkey = "29c4a4aa674953749053c8a35399b37b713dedd5d002cb29b3331e56ff1ea65a";// TBDJXU-ULP2BR-YNS7MW-HY2WAF-WKQNAF-273KYB-PFY5

            var transactions = await new AccountHttp(host).IncomingTransactions(new PublicAccount(multisigPubkey, NetworkType.Types.TEST_NET));
            Assert.IsTrue(0 < transactions.Count);
            Assert.IsTrue(25 >= transactions.Count);
            AssertTx(transactions[0], transactions[0].TransactionInfo);
        }
        
        private void AssertTx(Transaction tx, TransactionInfo txinfo)
        {
            const string hashMS1 = "2ec51d4e2e0a11cd6d3747848e272c51bae213ebfb4d2912b2cb6a8d86d36f86";  // See http://bob.nem.ninja:8765/#/multisig/2ec51d4e2e0a11cd6d3747848e272c51bae213ebfb4d2912b2cb6a8d86d36f86
            const string hashMS2 = "093760ef679029698802b636d2dbf919b19d4270183a777bbe703d7c44ed19d2";  // See http://bob.nem.ninja:8765/#/multisig/093760ef679029698802b636d2dbf919b19d4270183a777bbe703d7c44ed19d2

            if (txinfo.Hash == hashMS1)
            {
                const string signerPubKey = "9d7ea57169a56a1bb821e1abf744610c639d7545f976f09808b68a6ad1415eb0";  // TATFWN-33FARG-365IXE-3CSKQI-7KZZA4-OJAVFS-Z66D
                const string signerAddress = "TATFWN-33FARG-365IXE-3CSKQI-7KZZA4-OJAVFS-Z66D";                   // Initiator of the transaction
                const string multisigPubkey = "29c4a4aa674953749053c8a35399b37b713dedd5d002cb29b3331e56ff1ea65a";// TBDJXU-ULP2BR-YNS7MW-HY2WAF-WKQNAF-273KYB-PFY5
                const string multisigAddress = "TBDJXU-ULP2BR-YNS7MW-HY2WAF-WKQNAF-273KYB-PFY5";                 // Multisig Account = Sending Account
                const string recipientAddress = "TBDJXU-ULP2BR-YNS7MW-HY2WAF-WKQNAF-273KYB-PFY5";                // Recipient Account
                const string cosignerPubKey = "eb100d6b2da10fc5359ab35a5801b0e6f0b6cc18d849c0aa78ba1aab2b945dea";// TAAVI7-FXUQZE-OIRY4N-WLPTLK-Y2GJTP-K2KN4N-W4TQ
                const string cosignerAddress = "TAAVI7-FXUQZE-OIRY4N-WLPTLK-Y2GJTP-K2KN4N-W4TQ";                 // Cosigner of the transaction

                Assert.AreEqual((ulong)1503213, txinfo.Height);
                Assert.AreEqual(true, txinfo.IsMultisig);

                Assert.IsTrue(tx.Deadline.TimeStamp >= tx.NetworkTime.TimeStamp);
                Assert.AreEqual(101132904, tx.NetworkTime.TimeStamp);
                Assert.AreEqual(DateTime.Parse("11/06/2018 12:34:49"), tx.NetworkTime.GetUtcDateTime());
                Assert.AreEqual(DateTime.Parse("11/06/2018 14:34:49"), tx.NetworkTime.GetLocalDateTime());
                Assert.AreEqual(101136504, tx.Deadline.TimeStamp);
                Assert.AreEqual(DateTime.Parse("11/06/2018 13:34:49"), tx.Deadline.GetUtcDateTime());
                Assert.AreEqual(DateTime.Parse("11/06/2018 15:34:49"), tx.Deadline.GetLocalDateTime());
                Assert.AreEqual((ulong)150000, tx.Fee);    // Multisig Wrapper fee
                Assert.AreEqual(NetworkType.Types.TEST_NET, tx.NetworkType);
                Assert.AreEqual(signerAddress, tx.Signer.Address.Pretty);
                Assert.AreEqual(signerPubKey, tx.Signer.PublicKey);
                Assert.AreEqual(signerAddress, Address.CreateFromPublicKey(tx.Signer.PublicKey, NetworkType.Types.TEST_NET).Pretty);
                Assert.AreEqual(TransactionTypes.Types.Multisig, tx.TransactionType);
                Assert.AreEqual(1, tx.Version);

                var mstx = (MultisigTransaction)tx;
                Assert.AreEqual(1, mstx.Cosignatures.Count);    //Cosignatures gives the Inner cosigning transaction List
                var cstx = mstx.Cosignatures[0];   
                Assert.AreEqual(101133019, cstx.NetworkTime.TimeStamp);
                Assert.AreEqual(DateTime.Parse("11/06/2018 12:36:44"), cstx.NetworkTime.GetUtcDateTime());
                Assert.AreEqual(DateTime.Parse("11/06/2018 14:36:44"), cstx.NetworkTime.GetLocalDateTime());
                Assert.AreEqual(101136619, cstx.Deadline.TimeStamp);
                Assert.AreEqual(DateTime.Parse("11/06/2018 13:36:44"), cstx.Deadline.GetUtcDateTime());
                Assert.AreEqual(DateTime.Parse("11/06/2018 15:36:44"), cstx.Deadline.GetLocalDateTime());
                Assert.AreEqual((ulong)150000, cstx.Fee);
                Assert.AreEqual(multisigAddress, cstx.MultisigAddress.Pretty);
                Assert.AreEqual(NetworkType.Types.TEST_NET, cstx.NetworkType);
                Assert.AreEqual(cosignerPubKey, cstx.Signer.PublicKey);
                Assert.AreEqual(cosignerAddress, cstx.Signer.Address.Pretty);
                Assert.AreEqual(TransactionTypes.Types.SignatureTransaction, cstx.TransactionType);
                Assert.IsNull(cstx.TransactionInfo);

                Assert.IsNotNull(mstx.InnerTransaction);
                Assert.AreEqual(TransactionTypes.Types.Transfer, mstx.InnerTransaction.TransactionType);
                var ttx = (TransferTransaction)mstx.InnerTransaction;
                Assert.IsNotNull(ttx.Address);
                Assert.AreEqual(recipientAddress, ttx.Address.Pretty);
                Assert.AreEqual(101132904, ttx.NetworkTime.TimeStamp);
                Assert.AreEqual(DateTime.Parse("11/06/2018 12:34:49"), ttx.NetworkTime.GetUtcDateTime());
                Assert.AreEqual(DateTime.Parse("11/06/2018 14:34:49"), ttx.NetworkTime.GetLocalDateTime());
                Assert.AreEqual(101136504, ttx.Deadline.TimeStamp);
                Assert.AreEqual(DateTime.Parse("11/06/2018 13:34:49"), ttx.Deadline.GetUtcDateTime());
                Assert.AreEqual(DateTime.Parse("11/06/2018 15:34:49"), ttx.Deadline.GetLocalDateTime());
                Assert.AreEqual((ulong)50000, ttx.Fee); // Transfer transaction fee
                Assert.IsNotNull(ttx.Signer);
                Assert.AreEqual(multisigAddress, ttx.Signer.Address.Pretty);
                Assert.AreEqual(multisigPubkey, ttx.Signer.PublicKey);
                Assert.AreEqual(MessageType.Type.UNENCRYPTED, ttx.Message.GetMessageType());
                Assert.AreEqual(0, ttx.Message.GetLength());
                Assert.IsNotNull(ttx.Mosaics);
                Assert.AreEqual(1, ttx.Mosaics.Count);
                Assert.AreEqual("nem", ttx.Mosaics[0].NamespaceName);
                Assert.AreEqual("xem", ttx.Mosaics[0].MosaicName);
                Assert.AreEqual((ulong)1000, ttx.Mosaics[0].Amount);
                Assert.IsNull(ttx.TransactionInfo);
                Assert.AreEqual(NetworkType.Types.TEST_NET, ttx.NetworkType);
                Assert.AreEqual(2, ttx.Version);
            }
            else if (txinfo.Hash == hashMS2)
            {
                const string signerpubkey = "9d7ea57169a56a1bb821e1abf744610c639d7545f976f09808b68a6ad1415eb0";
                const string signerAddress = "TATFWN-33FARG-365IXE-3CSKQI-7KZZA4-OJAVFS-Z66D";
                const string multisigPubkey = "29c4a4aa674953749053c8a35399b37b713dedd5d002cb29b3331e56ff1ea65a";// TBDJXU-ULP2BR-YNS7MW-HY2WAF-WKQNAF-273KYB-PFY5
                const string multisigAddress = "TBDJXU-ULP2BR-YNS7MW-HY2WAF-WKQNAF-273KYB-PFY5";                 // Multisig Account = Sending Account
                const string recipientAddress = "TALIC3-5ULCU2-PFUIHM-6J7WKK-MSLKPV-ZKPK36-PP4W";                 // Recipient Account
                const string cosignerPubKey = "fbe95048d0325e2553a5e2aa88b9e12ed59f7c8c0fb8f84a638f43a390116c22";// TBPAMO-PRIATP-T76TAZ-ZWERHO-K72FIK-N4YCD4-VJMJ
                const string cosignerAddress = "TBPAMO-PRIATP-T76TAZ-ZWERHO-K72FIK-N4YCD4-VJMJ";                 // Cosigner of the transaction

                Assert.AreEqual((ulong)1515002, txinfo.Height);
                Assert.AreEqual(true, txinfo.IsMultisig);

                Assert.IsTrue(tx.Deadline.TimeStamp >= tx.NetworkTime.TimeStamp);
                Assert.AreEqual(101845918, tx.NetworkTime.TimeStamp);
                Assert.AreEqual(101849518, tx.Deadline.TimeStamp);
                Assert.AreEqual((ulong)150000, tx.Fee);
                Assert.AreEqual(NetworkType.Types.TEST_NET, tx.NetworkType);
                Assert.AreEqual(signerAddress, tx.Signer.Address.Pretty);
                Assert.AreEqual(signerpubkey, tx.Signer.PublicKey);
                Assert.AreEqual(signerAddress, Address.CreateFromPublicKey(tx.Signer.PublicKey, NetworkType.Types.TEST_NET).Pretty);
                Assert.AreEqual(TransactionTypes.Types.Multisig, tx.TransactionType);
                Assert.AreEqual(1, tx.Version);

                var mstx = (MultisigTransaction)tx;
                Assert.AreEqual(1, mstx.Cosignatures.Count);    //Cosignatures gives the Inner cosigning transaction List
                var cstx = mstx.Cosignatures[0];
                Assert.AreEqual(101845953, cstx.NetworkTime.TimeStamp);
                Assert.AreEqual(101849553, cstx.Deadline.TimeStamp);
                Assert.AreEqual((ulong)150000, cstx.Fee);
                Assert.AreEqual(multisigAddress, cstx.MultisigAddress.Pretty);
                Assert.AreEqual(NetworkType.Types.TEST_NET, cstx.NetworkType);
                Assert.AreEqual(cosignerPubKey, cstx.Signer.PublicKey);
                Assert.AreEqual(cosignerAddress, cstx.Signer.Address.Pretty);
                Assert.AreEqual(TransactionTypes.Types.SignatureTransaction, cstx.TransactionType);
                Assert.IsNull(cstx.TransactionInfo);

                Assert.IsNotNull(mstx.InnerTransaction);
                Assert.AreEqual(TransactionTypes.Types.Transfer, mstx.InnerTransaction.TransactionType);
                var ttx = (TransferTransaction)mstx.InnerTransaction;
                Assert.IsNotNull(ttx.Address);
                Assert.AreEqual(recipientAddress, ttx.Address.Pretty);
                Assert.AreEqual(101845897, ttx.NetworkTime.TimeStamp);
                Assert.AreEqual(101849497, ttx.Deadline.TimeStamp);
                Assert.AreEqual((ulong)50000, ttx.Fee); // Transfer transaction fee
                Assert.IsNotNull(ttx.Signer);
                Assert.AreEqual(multisigAddress, ttx.Signer.Address.Pretty);
                Assert.AreEqual(multisigPubkey, ttx.Signer.PublicKey);
                Assert.AreEqual(MessageType.Type.UNENCRYPTED, ttx.Message.GetMessageType());
                Assert.AreEqual(0, ttx.Message.GetLength());
                Assert.IsNotNull(ttx.Mosaics);
                Assert.AreEqual(1, ttx.Mosaics.Count);
                Assert.AreEqual("nem", ttx.Mosaics[0].NamespaceName);
                Assert.AreEqual("xem", ttx.Mosaics[0].MosaicName);
                Assert.AreEqual((ulong)1000000, ttx.Mosaics[0].Amount);
                Assert.IsNull(ttx.TransactionInfo);
                Assert.AreEqual(NetworkType.Types.TEST_NET, ttx.NetworkType);
                Assert.AreEqual(1, ttx.Version);
            }
        }

        [TestMethod]
        public async Task GetMosaicsOwned()
        {
            const String pretty = "TCTUIF-557ZCQ-OQPW2M-6GH4TC-DPM2ZY-BBL54K-GNHR";
            var mosaics = await new AccountHttp(host).MosaicsOwned(Address.CreateFromEncoded(pretty));

            Assert.AreEqual(4, mosaics.Count);
            Assert.AreEqual("nem", mosaics[0].NamespaceName);
            Assert.AreEqual("xem", mosaics[0].MosaicName);
            Assert.IsTrue(1000000 < mosaics[0].Amount);
            Assert.AreEqual("nis1porttest", mosaics[1].NamespaceName);
            Assert.AreEqual("test", mosaics[1].MosaicName);
            Assert.IsTrue(100000000000 >= mosaics[1].Amount);
            Assert.AreEqual("myspace", mosaics[2].NamespaceName);
            Assert.AreEqual("subspacewithlevy", mosaics[2].MosaicName);
            Assert.IsTrue(10000000000000 >= mosaics[2].Amount);
            Assert.AreEqual("myspace", mosaics[3].NamespaceName);
            Assert.AreEqual("subspace", mosaics[3].MosaicName);
            Assert.IsTrue(10001000000000 >= mosaics[3].Amount);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetOutgoingTransactionsWithTransfer()
        {
            const string expected = "9d7ea57169a56a1bb821e1abf744610c639d7545f976f09808b68a6ad1415eb0";

            var response = await new AccountHttp(host).OutgoingTransactions(new PublicAccount("eb100d6b2da10fc5359ab35a5801b0e6f0b6cc18d849c0aa78ba1aab2b945dea", NetworkType.Types.TEST_NET));
            
          
            var tx = (MultisigTransaction)response[5];
            Assert.AreEqual(((TransferTransaction)tx.InnerTransaction).Mosaics[0].MosaicName, Xem.MosaicName);
            Assert.AreEqual(expected, tx.Signer.PublicKey);
            Assert.AreEqual("b41462f6b28bd8446f45fd90b6bda6d8eb33174b7b0c168b618d63472a815fd2", tx.TransactionInfo.InnerHash);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetOutgoingTransactionsWithImportance()
        {
            const string expected = "9d7ea57169a56a1bb821e1abf744610c639d7545f976f09808b68a6ad1415eb0";

            var response = await new AccountHttp(host).OutgoingTransactions(new PublicAccount("eb100d6b2da10fc5359ab35a5801b0e6f0b6cc18d849c0aa78ba1aab2b945dea", NetworkType.Types.TEST_NET));
           
            var tx = (MultisigTransaction)response[4];
            Assert.AreEqual(((ImportanceTransferTransaction)tx.InnerTransaction).Mode.GetValue(), 1);
            Assert.AreEqual(((ImportanceTransferTransaction)tx.InnerTransaction).TransactionInfo, null);
            Assert.AreEqual("627b03264e51fa12870a923738506c27a20a3bc50051aeb75f545db7d7725060", ((ImportanceTransferTransaction)tx.InnerTransaction).RemoteAccount.PublicKey);
            Assert.AreEqual(expected, tx.Signer.PublicKey);
            Assert.AreEqual("c48c267efe736e8950da4a6ea9fcdc5d7c48f03a3e30965552a4dc51883da486", tx.TransactionInfo.InnerHash);
        }

        [TestMethod]
        public async Task GetTransactionWithEncryptedMessage()
        {
            const string expected = "57786d3560ae151ec9790c73713379524700bc6a2b34888b26b736c35c8c5e14";

            var response = await new AccountHttp(host).IncomingTransactions(Address.CreateFromEncoded("NB2GO2-AAZDWU-YHG3HM-VC4DHI-X6HBMD-FZXDNE-5FYZ"));

            var tx = (TransferTransaction)response[2];

            Assert.AreEqual("664dccae0a45c03ae83c4ebe42e64cb4a8efbc897a3cb8421a19b1978c48a7b8", tx.TransactionInfo.Hash);
            Assert.AreEqual(tx.Mosaics[0].MosaicName, "fluffs");
            Assert.AreEqual(tx.Mosaics[0].Amount, (ulong)3000000);
            Assert.IsTrue(tx.Message.GetMessageType() == MessageType.Type.ENCRYPTED);
            Assert.AreEqual("ZXCVBN", ((SecureMessage)tx.Message).GetDecodedPayload("523b0b58512cba0da5e8d4fa829e241326cd126bad22a5055a8cb39fdcd1bc00", "57786d3560ae151ec9790c73713379524700bc6a2b34888b26b736c35c8c5e14"));
            Assert.AreEqual(expected, tx.Signer.PublicKey);
            Assert.AreEqual("664dccae0a45c03ae83c4ebe42e64cb4a8efbc897a3cb8421a19b1978c48a7b8", tx.TransactionInfo.Hash);
        }

        [TestMethod]
        public async Task GetTransactionWithHexidecimalMessage()
        {
            const string expected = "7b1a93132b8c5b8001a07f973307bee2b37bcd6dc279a59ea98179b238d44e2d";

            var response = await new AccountHttp(host).OutgoingTransactions(new PublicAccount("7b1a93132b8c5b8001a07f973307bee2b37bcd6dc279a59ea98179b238d44e2d", NetworkType.Types.TEST_NET));

            var tx = (TransferTransaction)response[0];
            Assert.AreEqual(tx.Mosaics[0].MosaicName, Xem.MosaicName);
            Assert.AreEqual(tx.Mosaics[0].Amount, (ulong)10000000);
            Assert.IsTrue(tx.Message.GetMessageType() == MessageType.Type.UNENCRYPTED);
            Assert.AreEqual("abcd1234", ((HexMessage)tx.Message).GetStringPayload());
            Assert.AreEqual(expected, tx.Signer.PublicKey);
            Assert.AreEqual("5853eaebe86307bf8a5dbddb5248490cb1f9ca6cb76c4733dab8eea157988f7a", tx.TransactionInfo.Hash);
        }

    }
}
