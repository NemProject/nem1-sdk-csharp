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
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using io.nem1.sdk.Infrastructure.HttpRepositories;
using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Network;
using io.nem1.sdk.Model.Mosaics;
using io.nem1.sdk.Model.Transactions.Messages;
using io.nem1.sdk.Model.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTest.infrastructure.HttpTests
{
    [TestClass]
    public class AccountHttpTests
    {
        // Regular account (not a multisig account)
        const string PUBKEY = "856f39436e33129afff95b89aca998fa23cd751a6f4d79ce4fb9da9641ecb59c";
        const string PLAIN = "TACOPEXRLZTUWBQA3UXV66R455L76ENWK6OYITBJ";    // see http://104.128.226.60:7890/account/get?address=TACOPEXRLZTUWBQA3UXV66R455L76ENWK6OYITBJ
        const string PRETTY = "TACOPE-XRLZTU-WBQA3U-XV66R4-55L76E-NWK6OY-ITBJ";
        // Multisig Account with 2 cosigners
        const string PUBKEYMS = "29c4a4aa674953749053c8a35399b37b713dedd5d002cb29b3331e56ff1ea65a";
        const string PLAINMS = "TBDJXUULP2BRYNS7MWHY2WAFWKQNAF273KYBPFY5";  // see http://104.128.226.60:7890/account/get?address=TBDJXUULP2BRYNS7MWHY2WAFWKQNAF273KYBPFY5
        const string PRETTYMS = "TBDJXU-ULP2BR-YNS7MW-HY2WAF-WKQNAF-273KYB-PFY5";
        // Cosigner1 of Multisig Account
        const string PUBKEYCS1 = "fbe95048d0325e2553a5e2aa88b9e12ed59f7c8c0fb8f84a638f43a390116c22";
        const string PLAINCS1 = "TBPAMOPRIATPT76TAZZWERHOK72FIKN4YCD4VJMJ"; // see http://104.128.226.60:7890/account/get?address=TBPAMOPRIATPT76TAZZWERHOK72FIKN4YCD4VJMJ
        const string PRETTYCS1 = "TBPAMO-PRIATP-T76TAZ-ZWERHO-K72FIK-N4YCD4-VJMJ";
        // Cosigner2 of Multisig Account
        const string PUBKEYCS2 = "9d7ea57169a56a1bb821e1abf744610c639d7545f976f09808b68a6ad1415eb0";
        const string PLAINCS2 = "TATFWN33FARG365IXE3CSKQI7KZZA4OJAVFSZ66D"; // see http://104.128.226.60:7890/account/get?address=TATFWN33FARG365IXE3CSKQI7KZZA4OJAVFSZ66D
        const string PRETTYCS2 = "TATFWN-33FARG-365IXE-3CSKQI-7KZZA4-OJAVFS-Z66D";
        // Cosigner Account with transferred Importance
        const string PUBKEYCSA = "eb100d6b2da10fc5359ab35a5801b0e6f0b6cc18d849c0aa78ba1aab2b945dea";
        const string PLAINCSA = "TAAVI7FXUQZEOIRY4NWLPTLKY2GJTPK2KN4NW4TQ";  // see http://104.128.226.60:7890/account/get?address=TAAVI7FXUQZEOIRY4NWLPTLKY2GJTPK2KN4NW4TQ
        const string PRETTYCSA = "TAAVI7-FXUQZE-OIRY4N-WLPTLK-Y2GJTP-K2KN4N-W4TQ";
        // Account that sent hexadecimal and encrypted messages
        const string PUBKEYMESS = "7b1a93132b8c5b8001a07f973307bee2b37bcd6dc279a59ea98179b238d44e2d";
        const string PLAINMESS = "TCTUIF-557ZCQ-OQPW2M-6GH4TC-DPM2ZY-BBL54K-GNHR";
        const string PRETTYMESS = "TCTUIF557ZCQOQPW2M6GH4TCDPM2ZYBBL54KGNHR";

        readonly string host = "http://" + Config.Domain + ":7890";

        [TestMethod, Timeout(20000)]
        public async Task GetAccountInfoFromAddress()
        {
            var acctInfo = await new AccountHttp(host).GetAccountInfo(new Address(PLAIN));
            AssertAcctInfo(acctInfo);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetAccountInfoFromPublicKey()
        {
            var acctInfo = await new AccountHttp(host).GetAccountInfo(new PublicAccount(PUBKEY, NetworkType.Types.TEST_NET));
            AssertAcctInfo(acctInfo);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetAccountInfoFromAddressMultisig()
        {
            var acctInfo = await new AccountHttp(host).GetAccountInfo(new Address(PLAINMS));
            AssertAcctInfo(acctInfo);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetAccountInfoFromAddressCosign1()
        {
            var acctInfo = await new AccountHttp(host).GetAccountInfo(new Address(PRETTYCS1));
            AssertAcctInfo(acctInfo);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetAccountInfoFromAddressCosign2()
        {
            var acctInfo = await new AccountHttp(host).GetAccountInfo(new Address(PRETTYCS2));
            AssertAcctInfo(acctInfo);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetAccountInfoFromAddressCosignA()
        {
            var acctInfo = await new AccountHttp(host).GetAccountInfo(new Address(PRETTYCSA));
            AssertAcctInfo(acctInfo);
        }

        private void AssertAcctInfo(AccountInfo acctInfo)  
        {
            switch (acctInfo.PublicKey)
            {
            case PUBKEY:
                AssertCosigAcctInfo(acctInfo);
                Assert.AreEqual(AccountInfo.StatusValue.LOCKED, acctInfo.Status);
                Assert.AreEqual(AccountInfo.RemoteStatusValue.INACTIVE, acctInfo.RemoteStatus);
                Assert.AreEqual(0, acctInfo.MinCosigners);
                Assert.IsNotNull(acctInfo.Cosigners);
                Assert.AreEqual(0, acctInfo.Cosigners.Count);
                Assert.IsNotNull(acctInfo.CosignatoryOf);
                Assert.AreEqual(0, acctInfo.CosignatoryOf.Count);
                break;
            case PUBKEYMS:
                AssertCosigAcctInfo(acctInfo);
                Assert.AreEqual(AccountInfo.StatusValue.LOCKED, acctInfo.Status);
                Assert.AreEqual(AccountInfo.RemoteStatusValue.ACTIVE, acctInfo.RemoteStatus);
                Assert.AreEqual(2, acctInfo.MinCosigners);
                Assert.IsNotNull(acctInfo.Cosigners);
                Assert.AreEqual(2, acctInfo.Cosigners.Count);
                AssertCosigAcctInfo(acctInfo.Cosigners[0]);
                AssertCosigAcctInfo(acctInfo.Cosigners[1]);
                Assert.IsNotNull(acctInfo.CosignatoryOf);
                Assert.AreEqual(0, acctInfo.CosignatoryOf.Count);
                break;
            case PUBKEYCS1:
                AssertCosigAcctInfo(acctInfo);
                Assert.AreEqual(AccountInfo.StatusValue.LOCKED, acctInfo.Status);
                Assert.AreEqual(AccountInfo.RemoteStatusValue.INACTIVE, acctInfo.RemoteStatus);
                Assert.AreEqual(0, acctInfo.MinCosigners);
                Assert.IsNotNull(acctInfo.Cosigners);
                Assert.AreEqual(0, acctInfo.Cosigners.Count);
                Assert.IsNotNull(acctInfo.CosignatoryOf);
                Assert.AreEqual(1, acctInfo.CosignatoryOf.Count);
                AssertCosigAcctInfo(acctInfo.CosignatoryOf[0]);
                break;
            case PUBKEYCS2:
                AssertCosigAcctInfo(acctInfo);
                Assert.AreEqual(AccountInfo.StatusValue.LOCKED, acctInfo.Status);
                Assert.AreEqual(AccountInfo.RemoteStatusValue.INACTIVE, acctInfo.RemoteStatus);
                Assert.AreEqual(0, acctInfo.MinCosigners);
                Assert.IsNotNull(acctInfo.Cosigners);
                Assert.AreEqual(0, acctInfo.Cosigners.Count);
                Assert.IsNotNull(acctInfo.CosignatoryOf);
                Assert.AreEqual(1, acctInfo.CosignatoryOf.Count);
                AssertCosigAcctInfo(acctInfo.CosignatoryOf[0]);
                break;
            case PUBKEYCSA:
                AssertCosigAcctInfo(acctInfo);
                Assert.AreEqual(AccountInfo.StatusValue.LOCKED, acctInfo.Status);
                Assert.AreEqual(AccountInfo.RemoteStatusValue.INACTIVE, acctInfo.RemoteStatus);
                Assert.AreEqual(0, acctInfo.MinCosigners);
                Assert.IsNotNull(acctInfo.Cosigners);
                Assert.AreEqual(0, acctInfo.Cosigners.Count);
                Assert.IsNotNull(acctInfo.CosignatoryOf);
                Assert.AreEqual(0, acctInfo.CosignatoryOf.Count);
                break;
            default:
                throw new Exception("AssertAcctInfo: Unsupported Public Key");
            }
        }

        private void AssertCosigAcctInfo(AccountInfo cosigAcctInfo)
        {
            switch (cosigAcctInfo.PublicKey)
            {
            case PUBKEY:
                Assert.AreEqual(NetworkType.Types.TEST_NET, cosigAcctInfo.Address.GetNetworktype());
                Assert.AreEqual(PLAIN, cosigAcctInfo.Address.Plain);
                Assert.AreEqual(PRETTY, cosigAcctInfo.Address.Pretty);
                Assert.AreNotEqual((ulong)0, cosigAcctInfo.Balance);
                Assert.AreNotEqual((ulong)0, cosigAcctInfo.VestedBalance);
                Assert.AreEqual((double)0, cosigAcctInfo.Importance);
                Assert.AreEqual((ulong)0, cosigAcctInfo.HarvestedBlocks);
                break;
            case PUBKEYMS:
                Assert.AreEqual(NetworkType.Types.TEST_NET, cosigAcctInfo.Address.GetNetworktype());
                Assert.AreEqual(PLAINMS, cosigAcctInfo.Address.Plain);
                Assert.AreEqual(PRETTYMS, cosigAcctInfo.Address.Pretty);
                Assert.IsTrue(cosigAcctInfo.Balance > (ulong)1000000);
                Assert.IsTrue(cosigAcctInfo.VestedBalance > (ulong)1000000);
                Assert.AreEqual((double)0, cosigAcctInfo.Importance);
                Assert.AreEqual((ulong)0, cosigAcctInfo.HarvestedBlocks);
                break;
            case PUBKEYCS1:
                Assert.AreEqual(NetworkType.Types.TEST_NET, cosigAcctInfo.Address.GetNetworktype());
                Assert.AreEqual(PLAINCS1, cosigAcctInfo.Address.Plain);
                Assert.AreEqual(PRETTYCS1, cosigAcctInfo.Address.Pretty);
                Assert.IsTrue(cosigAcctInfo.Balance > (ulong)1000000);
                Assert.IsTrue(cosigAcctInfo.VestedBalance > (ulong)1000000);
                Assert.AreEqual((double)0, cosigAcctInfo.Importance);
                Assert.AreEqual((ulong)0, cosigAcctInfo.HarvestedBlocks);
                break;
            case PUBKEYCS2:
                Assert.AreEqual(NetworkType.Types.TEST_NET, cosigAcctInfo.Address.GetNetworktype());
                Assert.AreEqual(PLAINCS2, cosigAcctInfo.Address.Plain);
                Assert.AreEqual(PRETTYCS2, cosigAcctInfo.Address.Pretty);
                Assert.IsTrue(cosigAcctInfo.Balance >= (ulong)900000);
                Assert.IsTrue(cosigAcctInfo.VestedBalance >= (ulong)900000);
                Assert.AreEqual((double)0, cosigAcctInfo.Importance);
                Assert.AreEqual((ulong)0, cosigAcctInfo.HarvestedBlocks);
                break;
            case PUBKEYCSA:
                Assert.AreEqual(NetworkType.Types.TEST_NET, cosigAcctInfo.Address.GetNetworktype());
                Assert.AreEqual(PLAINCSA, cosigAcctInfo.Address.Plain);
                Assert.AreEqual(PRETTYCSA, cosigAcctInfo.Address.Pretty);
                Assert.IsTrue(cosigAcctInfo.Balance >= (ulong)450000);
                Assert.IsTrue(cosigAcctInfo.VestedBalance >= (ulong)450000);
                Assert.AreEqual((double)0, cosigAcctInfo.Importance);
                Assert.AreEqual((ulong)0, cosigAcctInfo.HarvestedBlocks);
                break;
            default:
                throw new Exception("AssertCosigAcctInfo: Unsupported Public Key");
            }
        }

        [TestMethod]
        public async Task GetAllTransactions()  // Last 25 in- or outgoing transactions
        {
            var transactions = await new AccountHttp(host).Transactions(new PublicAccount(PUBKEYMS, NetworkType.Types.TEST_NET));
            Assert.IsTrue(0 < transactions.Count);
            Assert.IsTrue(25 >= transactions.Count);
            AssertTx(transactions[0], transactions[0].TransactionInfo);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetIncomingTransactions() // Last 25 incomming transactions
        {
            var transactions = await new AccountHttp(host).IncomingTransactions(new PublicAccount(PUBKEYMS, NetworkType.Types.TEST_NET));
            Assert.IsTrue(0 < transactions.Count);
            Assert.IsTrue(25 >= transactions.Count);
            AssertTx(transactions[0], transactions[0].TransactionInfo);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetOutgoingTransactionsImportanceTransfer()
        {
            var transactions = await new AccountHttp(host).OutgoingTransactions(new PublicAccount(PUBKEYCSA, NetworkType.Types.TEST_NET));
            AssertTx(transactions[4], transactions[4].TransactionInfo);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetOutgoingTransactionsTransfer()
        {
            const string expectedHash = "b41462f6b28bd8446f45fd90b6bda6d8eb33174b7b0c168b618d63472a815fd2";
            List<Transaction> txs = await new AccountHttp(host).OutgoingTransactions(new Address(PLAINCSA));
            MultisigTransaction mstx = (MultisigTransaction)txs[5];
            Assert.AreEqual(PUBKEYCS2, mstx.Signer.PublicKey);
            Assert.AreEqual(expectedHash, mstx.TransactionInfo.InnerHash);
            TransferTransaction ttx = (TransferTransaction)mstx.InnerTransaction;
            Assert.AreEqual(MosaicId.XEM, ttx.Mosaics[0].MosaicInfo.Name);
        }

        private void AssertTx(Transaction tx, TransactionInfo txinfo)
        {
            // See https://forum.nem.io/t/how-to-get-a-transaction-data-with-nis-api-solved/3130
            const string hashMS1 = "2ec51d4e2e0a11cd6d3747848e272c51bae213ebfb4d2912b2cb6a8d86d36f86";  // See http://104.128.226.60:7890/transaction/get?hash=b3250dc5d596c05bff4e501cc2c74b1099bc83f8d99c548377224f2f5a8d994d and http://bob.nem.ninja:8765/#/multisig/2ec51d4e2e0a11cd6d3747848e272c51bae213ebfb4d2912b2cb6a8d86d36f86
            const string hashMS2 = "093760ef679029698802b636d2dbf919b19d4270183a777bbe703d7c44ed19d2";  // See http://104.128.226.60:7890/transaction/get?hash=093760ef679029698802b636d2dbf919b19d4270183a777bbe703d7c44ed19d2 and http://bob.nem.ninja:8765/#/multisig/093760ef679029698802b636d2dbf919b19d4270183a777bbe703d7c44ed19d2
            const string hashMS3 = "3835c88efaf3b42acbe6db3b9a3995acc0686a141553b07560ebe5e5ae5e7c72";  // See http://104.128.226.60:7890/transaction/get?hash=3835c88efaf3b42acbe6db3b9a3995acc0686a141553b07560ebe5e5ae5e7c72 and http://bob.nem.ninja:8765/#/multisig/3835c88efaf3b42acbe6db3b9a3995acc0686a141553b07560ebe5e5ae5e7c72"

            MultisigTransaction mstx;
            Transaction itx;    // Inner Transaction of a Multisig transaction
            CosignatureTransaction cstx;
            TransferTransaction ttx;
            ImportanceTransferTransaction ittx;
            MosaicAmount mosaic;

            Assert.IsNotNull(tx);
            Assert.IsNotNull(txinfo);
            switch (txinfo.Hash)
            {   
            case hashMS1:   // Multisig transaction with Transfer 
                Assert.AreEqual((ulong)1503213, txinfo.Height);
                Assert.AreEqual(true, txinfo.IsMultisig);

                Assert.AreEqual(NetworkType.Types.TEST_NET, tx.NetworkType);
                Assert.AreEqual(1, tx.Version);
                Assert.AreEqual(TransactionTypes.Types.Multisig, tx.TransactionType);
                Assert.IsTrue(tx.Deadline.TimeStamp >= tx.NetworkTime.TimeStamp);
                Assert.AreEqual(101132904, tx.NetworkTime.TimeStamp);
                Assert.AreEqual(DateTime.Parse("11/06/2018 12:34:49"), tx.NetworkTime.GetUtcDateTime());
                //Assert.AreEqual(DateTime.Parse("11/06/2018 14:34:49"), tx.NetworkTime.GetLocalDateTime());    // result depends on the OS settings of the machine running the tests
                Assert.AreEqual(101136504, tx.Deadline.TimeStamp);
                Assert.AreEqual(DateTime.Parse("11/06/2018 13:34:49"), tx.Deadline.GetUtcDateTime());
                //Assert.AreEqual(DateTime.Parse("11/06/2018 15:34:49"), tx.Deadline.GetLocalDateTime());    // result depends on the OS settings of the machine running the tests
                Assert.AreEqual((ulong)150000, tx.Fee);    // Multisig Wrapper fee
                Assert.IsNotNull(tx.Signer);
                Assert.AreEqual(PUBKEYCS2, tx.Signer.PublicKey);
                Assert.IsNotNull(tx.Signer.Address);
                Assert.AreEqual(NetworkType.Types.TEST_NET, tx.Signer.Address.GetNetworktype());
                Assert.AreEqual(PLAINCS2, tx.Signer.Address.Plain);
                Assert.AreEqual(PRETTYCS2, tx.Signer.Address.Pretty);

                mstx = (MultisigTransaction)tx;
                Assert.IsNotNull(mstx.Cosignatures);
                Assert.AreEqual(1, mstx.Cosignatures.Count);    //Cosignatures gives the Inner cosigning transaction List
                cstx = mstx.Cosignatures[0];
                Assert.IsNull(cstx.TransactionInfo);
                Assert.AreEqual(NetworkType.Types.TEST_NET, cstx.NetworkType);
                Assert.AreEqual(1, cstx.Version);
                Assert.AreEqual(TransactionTypes.Types.SignatureTransaction, cstx.TransactionType);
                Assert.AreEqual(101133019, cstx.NetworkTime.TimeStamp);
                Assert.AreEqual(DateTime.Parse("11/06/2018 12:36:44"), cstx.NetworkTime.GetUtcDateTime());
                //Assert.AreEqual(DateTime.Parse("11/06/2018 14:36:44"), cstx.NetworkTime.GetLocalDateTime());    // result depends on the OS settings of the machine running the tests
                Assert.AreEqual(101136619, cstx.Deadline.TimeStamp);
                Assert.AreEqual(DateTime.Parse("11/06/2018 13:36:44"), cstx.Deadline.GetUtcDateTime());
                //Assert.AreEqual(DateTime.Parse("11/06/2018 15:36:44"), cstx.Deadline.GetLocalDateTime());    // result depends on the OS settings of the machine running the tests
                Assert.AreEqual((ulong)150000, cstx.Fee);   // Cosingning transaction fee
                Assert.IsNotNull(cstx.Signer);
                Assert.AreEqual(PUBKEYCSA, cstx.Signer.PublicKey);
                Assert.IsNotNull(cstx.Signer.Address);
                Assert.AreEqual(NetworkType.Types.TEST_NET, cstx.Signer.Address.GetNetworktype());
                Assert.AreEqual(PLAINCSA, cstx.Signer.Address.Plain);
                Assert.AreEqual(PRETTYCSA, cstx.Signer.Address.Pretty);
                Assert.IsNotNull(cstx.MultisigAddress);
                Assert.AreEqual(NetworkType.Types.TEST_NET, cstx.MultisigAddress.GetNetworktype());
                Assert.AreEqual(PLAINMS, cstx.MultisigAddress.Plain);
                Assert.AreEqual(PRETTYMS, cstx.MultisigAddress.Pretty);

                itx = mstx.InnerTransaction;
                Assert.IsNotNull(itx);
                Assert.IsNull(itx.TransactionInfo);
                Assert.AreEqual(NetworkType.Types.TEST_NET, itx.NetworkType);
                Assert.AreEqual(2, itx.Version);
                Assert.AreEqual(TransactionTypes.Types.Transfer, itx.TransactionType);
                ttx = (TransferTransaction)itx;
                Assert.AreEqual(101132904, ttx.NetworkTime.TimeStamp);
                Assert.AreEqual(DateTime.Parse("11/06/2018 12:34:49"), ttx.NetworkTime.GetUtcDateTime());
                //Assert.AreEqual(DateTime.Parse("11/06/2018 14:34:49"), ttx.NetworkTime.GetLocalDateTime());    // result depends on the OS settings of the machine running the tests
                Assert.AreEqual(101136504, ttx.Deadline.TimeStamp);
                Assert.AreEqual(DateTime.Parse("11/06/2018 13:34:49"), ttx.Deadline.GetUtcDateTime());
                //Assert.AreEqual(DateTime.Parse("11/06/2018 15:34:49"), ttx.Deadline.GetLocalDateTime());    // result depends on the OS settings of the machine running the tests
                Assert.AreEqual((ulong)50000, ttx.Fee); // Transfer transaction fee
                Assert.IsNotNull(ttx.Signer);   // Sender
                Assert.AreEqual(PUBKEYMS, ttx.Signer.PublicKey);
                Assert.IsNotNull(ttx.Signer.Address);
                Assert.AreEqual(NetworkType.Types.TEST_NET, ttx.Signer.Address.GetNetworktype());
                Assert.AreEqual(PLAINMS, ttx.Signer.Address.Plain);
                Assert.AreEqual(PRETTYMS, ttx.Signer.Address.Pretty);
                Assert.IsNotNull(ttx.Address);  // Recipient
                Assert.AreEqual(NetworkType.Types.TEST_NET, ttx.Address.GetNetworktype());
                Assert.AreEqual(PLAINMS, ttx.Address.Plain);
                Assert.AreEqual(PRETTYMS, ttx.Address.Pretty);
                Assert.AreEqual(MessageType.Type.UNENCRYPTED, ttx.Message.GetMessageType());
                Assert.AreEqual(0, ttx.Message.GetLength());
                Assert.IsNotNull(ttx.Mosaics);
                Assert.AreEqual(1, ttx.Mosaics.Count);
                mosaic = ttx.Mosaics[0];
                Assert.AreEqual(MosaicId.NEM, mosaic.MosaicInfo.NamespaceId);
                Assert.AreEqual(MosaicId.XEM, mosaic.MosaicInfo.Name);
                Assert.AreEqual((ulong)1000, mosaic.Amount);
                break;
            case hashMS2:   // Multisig Transaction with Transfer
                const string RECIPIENT_PLAIN = "TALIC35ULCU2PFUIHM6J7WKKMSLKPVZKPK36PP4W";   // Recipient Account
                const string RECIPIENT_PRETTY = "TALIC3-5ULCU2-PFUIHM-6J7WKK-MSLKPV-ZKPK36-PP4W"; 
                Assert.AreEqual((ulong)1515002, txinfo.Height);
                Assert.AreEqual(true, txinfo.IsMultisig);

                Assert.AreEqual(NetworkType.Types.TEST_NET, tx.NetworkType);
                Assert.AreEqual(1, tx.Version);
                Assert.AreEqual(TransactionTypes.Types.Multisig, tx.TransactionType);
                Assert.IsTrue(tx.Deadline.TimeStamp >= tx.NetworkTime.TimeStamp);
                Assert.AreEqual(101845918, tx.NetworkTime.TimeStamp);
                Assert.AreEqual(101849518, tx.Deadline.TimeStamp);
                Assert.AreEqual((ulong)150000, tx.Fee);   // Multisig Wrapper fee
                Assert.IsNotNull(tx.Signer);
                Assert.AreEqual(PUBKEYCS2, tx.Signer.PublicKey);
                Assert.IsNotNull(tx.Signer.Address);
                Assert.AreEqual(NetworkType.Types.TEST_NET, tx.Signer.Address.GetNetworktype());
                Assert.AreEqual(PLAINCS2, tx.Signer.Address.Plain);
                Assert.AreEqual(PRETTYCS2, tx.Signer.Address.Pretty);

                mstx = (MultisigTransaction)tx;
                Assert.IsNotNull(mstx.Cosignatures);
                Assert.AreEqual(1, mstx.Cosignatures.Count);    //Cosignatures gives the Inner cosigning transaction List
                cstx = mstx.Cosignatures[0];
                Assert.IsNull(cstx.TransactionInfo);
                Assert.AreEqual(NetworkType.Types.TEST_NET, cstx.NetworkType);
                Assert.AreEqual(1, cstx.Version);
                Assert.AreEqual(TransactionTypes.Types.SignatureTransaction, cstx.TransactionType);
                Assert.AreEqual(101845953, cstx.NetworkTime.TimeStamp);
                Assert.AreEqual(101849553, cstx.Deadline.TimeStamp);
                Assert.AreEqual((ulong)150000, cstx.Fee);   // Cosingning transaction fee
                Assert.IsNotNull(cstx.Signer);
                Assert.AreEqual(PUBKEYCS1, cstx.Signer.PublicKey);
                Assert.IsNotNull(cstx.Signer.Address);
                Assert.AreEqual(NetworkType.Types.TEST_NET, cstx.Signer.Address.GetNetworktype());
                Assert.AreEqual(PLAINCS1, cstx.Signer.Address.Plain);
                Assert.AreEqual(PRETTYCS1, cstx.Signer.Address.Pretty);
                Assert.IsNotNull(cstx.MultisigAddress);
                Assert.AreEqual(NetworkType.Types.TEST_NET, cstx.MultisigAddress.GetNetworktype());
                Assert.AreEqual(PLAINMS, cstx.MultisigAddress.Plain);
                Assert.AreEqual(PRETTYMS, cstx.MultisigAddress.Pretty);

                itx = mstx.InnerTransaction;
                Assert.IsNotNull(itx);
                Assert.IsNull(itx.TransactionInfo);
                Assert.AreEqual(NetworkType.Types.TEST_NET, itx.NetworkType);
                Assert.AreEqual(1, itx.Version);
                Assert.AreEqual(TransactionTypes.Types.Transfer, itx.TransactionType);
                ttx = (TransferTransaction)mstx.InnerTransaction;
                Assert.AreEqual(101845897, ttx.NetworkTime.TimeStamp);
                Assert.AreEqual(101849497, ttx.Deadline.TimeStamp);
                Assert.AreEqual((ulong)50000, ttx.Fee); // Transfer transaction fee
                Assert.IsNotNull(ttx.Signer);
                Assert.AreEqual(PUBKEYMS, ttx.Signer.PublicKey);
                Assert.IsNotNull(ttx.Signer.Address);
                Assert.AreEqual(NetworkType.Types.TEST_NET, ttx.Signer.Address.GetNetworktype());
                Assert.AreEqual(PLAINMS, ttx.Signer.Address.Plain);
                Assert.AreEqual(PRETTYMS, ttx.Signer.Address.Pretty);
                Assert.IsNotNull(ttx.Address);
                Assert.AreEqual(NetworkType.Types.TEST_NET, ttx.Address.GetNetworktype());
                Assert.AreEqual(RECIPIENT_PLAIN, ttx.Address.Plain);
                Assert.AreEqual(RECIPIENT_PRETTY, ttx.Address.Pretty);
                Assert.AreEqual(MessageType.Type.UNENCRYPTED, ttx.Message.GetMessageType());
                Assert.AreEqual(0, ttx.Message.GetLength());
                Assert.IsNotNull(ttx.Mosaics);
                Assert.AreEqual(1, ttx.Mosaics.Count);
                mosaic = ttx.Mosaics[0];
                Assert.AreEqual(MosaicId.XEM, mosaic.MosaicInfo.NamespaceId);
                Assert.AreEqual(MosaicId.NEM, mosaic.MosaicInfo.Name);
                Assert.AreEqual((ulong)1000000, mosaic.Amount);
                break;
            case hashMS3:   // Multisig transaction with Importance Transfer (to remote harvesting account)
                const string REMOTE_PUBKEY = "627b03264e51fa12870a923738506c27a20a3bc50051aeb75f545db7d7725060";
                const string REMOTE_PLAIN = "TABBQV6ZQZNKQNC646WNET6CPAHTRWIR4HSLAJZC";  // Remote account 
                const string REMOTE_PRETTY = "TABBQV-6ZQZNK-QNC646-WNET6C-PAHTRW-IR4HSL-AJZC";
                Assert.AreEqual((ulong)1511723, txinfo.Height);
                Assert.AreEqual(true, txinfo.IsMultisig);

                Assert.AreEqual(NetworkType.Types.TEST_NET, tx.NetworkType);
                Assert.AreEqual(1, tx.Version);
                Assert.AreEqual(TransactionTypes.Types.Multisig, tx.TransactionType);
                Assert.IsTrue(tx.Deadline.TimeStamp >= tx.NetworkTime.TimeStamp);
                Assert.AreEqual(101647249, tx.NetworkTime.TimeStamp);
                Assert.AreEqual(101650849, tx.Deadline.TimeStamp);
                Assert.AreEqual((ulong)150000, tx.Fee);
                Assert.IsNotNull(tx.Signer);
                Assert.AreEqual(PUBKEYCS2, tx.Signer.PublicKey);
                Assert.IsNotNull(tx.Signer.Address);
                Assert.AreEqual(NetworkType.Types.TEST_NET, tx.Signer.Address.GetNetworktype());
                Assert.AreEqual(PLAINCS2, tx.Signer.Address.Plain);
                Assert.AreEqual(PRETTYCS2, tx.Signer.Address.Pretty);

                mstx = (MultisigTransaction)tx;
                Assert.IsNotNull(mstx.Cosignatures);
                Assert.AreEqual(1, mstx.Cosignatures.Count);    //Cosignatures gives the Inner cosigning transaction List
                cstx = mstx.Cosignatures[0];
                Assert.IsNull(cstx.TransactionInfo);
                Assert.AreEqual(NetworkType.Types.TEST_NET, cstx.NetworkType);
                Assert.AreEqual(1, cstx.Version);
                Assert.AreEqual(TransactionTypes.Types.SignatureTransaction, cstx.TransactionType);
                Assert.AreEqual(101647257, cstx.NetworkTime.TimeStamp);
                Assert.AreEqual(101650857, cstx.Deadline.TimeStamp);
                Assert.AreEqual((ulong)150000, cstx.Fee);
                Assert.IsNotNull(cstx.Signer);
                Assert.AreEqual(PUBKEYCSA, cstx.Signer.PublicKey);
                Assert.IsNotNull(cstx.Signer.Address);
                Assert.AreEqual(NetworkType.Types.TEST_NET, cstx.Signer.Address.GetNetworktype());
                Assert.AreEqual(PLAINCSA, cstx.Signer.Address.Plain);
                Assert.AreEqual(PRETTYCSA, cstx.Signer.Address.Pretty);

                Assert.AreEqual(PRETTYMS, cstx.MultisigAddress.Pretty);
                itx = mstx.InnerTransaction;
                Assert.IsNotNull(itx);
                Assert.IsNull(itx.TransactionInfo);
                Assert.AreEqual(NetworkType.Types.TEST_NET, itx.NetworkType);
                Assert.AreEqual(1, itx.Version);
                Assert.AreEqual(TransactionTypes.Types.ImportanceTransfer, itx.TransactionType);
                ittx = (ImportanceTransferTransaction)itx;
                Assert.IsNull(ittx.TransactionInfo);
                Assert.IsNotNull(ittx.Signer);
                Assert.AreEqual(PUBKEYMS, ittx.Signer.PublicKey);
                Assert.IsNotNull(ittx.Signer.Address);
                Assert.AreEqual(PLAINMS, ittx.Signer.Address.Plain);
                Assert.AreEqual(PRETTYMS, ittx.Signer.Address.Pretty);
                Assert.AreEqual(ImportanceTransferMode.Mode.Add, ittx.Mode);
                Assert.IsNotNull(ittx.RemoteAccount);
                Assert.AreEqual(REMOTE_PUBKEY, ittx.RemoteAccount.PublicKey);
                Assert.IsNotNull(ittx.RemoteAccount.Address);
                Assert.AreEqual(NetworkType.Types.TEST_NET, ittx.RemoteAccount.Address.GetNetworktype());
                Assert.AreEqual(REMOTE_PLAIN, ittx.RemoteAccount.Address.Plain);
                Assert.AreEqual(REMOTE_PRETTY, ittx.RemoteAccount.Address.Pretty);
                break;
            }
        }

        [TestMethod]
        public async Task GetMosaicsOwned()
        {
            const string MOSAIC_PRETTY = "TCTUIF-557ZCQ-OQPW2M-6GH4TC-DPM2ZY-BBL54K-GNHR";
            List<MosaicAmount> mosaics = await new AccountHttp(host).MosaicsOwned(new Address(MOSAIC_PRETTY));
            Assert.IsNotNull(mosaics);
            Assert.AreEqual(4, mosaics.Count);

            MosaicAmount mosaic = mosaics[0];
            Assert.AreEqual(MosaicId.NEM, mosaic.MosaicInfo.NamespaceId);
            Assert.AreEqual(MosaicId.XEM, mosaic.MosaicInfo.Name);
            Assert.IsTrue(mosaic.Amount > 1000000);

            mosaic = mosaics[1];
            Assert.AreEqual("nis1porttest", mosaic.MosaicInfo.NamespaceId);
            Assert.AreEqual("test", mosaic.MosaicInfo.Name);
            Assert.IsTrue(mosaic.Amount <= 100000000000);

            mosaic = mosaics[2];
            Assert.AreEqual("myspace", mosaic.MosaicInfo.NamespaceId);
            Assert.AreEqual("subspacewithlevy", mosaic.MosaicInfo.Name);
            Assert.IsTrue(mosaic.Amount <= 10000000000000);

            mosaic = mosaics[3];
            Assert.AreEqual("myspace", mosaic.MosaicInfo.NamespaceId);
            Assert.AreEqual("subspace", mosaic.MosaicInfo.Name);
            Assert.IsTrue(mosaic.Amount <= 10001000000000);
        }

    }
}
