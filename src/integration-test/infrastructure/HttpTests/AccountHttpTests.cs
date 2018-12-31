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
                AssertAcct(acctInfo);
                Assert.AreEqual(acctInfo.Status, AccountInfo.StatusValue.LOCKED);
                Assert.AreEqual(acctInfo.RemoteStatus, AccountInfo.RemoteStatusValue.INACTIVE);
                Assert.AreEqual(acctInfo.MinCosigners, 0);
                Assert.IsNotNull(acctInfo.Cosigners);
                Assert.AreEqual(acctInfo.Cosigners.Count, 0);
                Assert.IsNotNull(acctInfo.CosginatoryOf);
                Assert.AreEqual(acctInfo.CosginatoryOf.Count, 0);
                break;
            case PUBKEYMS:
                AssertAcct(acctInfo);
                Assert.AreEqual(acctInfo.Status, AccountInfo.StatusValue.LOCKED);
                Assert.AreEqual(acctInfo.RemoteStatus, AccountInfo.RemoteStatusValue.ACTIVE);
                Assert.AreEqual(acctInfo.MinCosigners, 2);
                Assert.IsNotNull(acctInfo.Cosigners);
                Assert.AreEqual(acctInfo.Cosigners.Count, 2);
                AssertAcct(acctInfo.Cosigners[0]);
                AssertAcct(acctInfo.Cosigners[1]);
                Assert.IsNotNull(acctInfo.CosginatoryOf);
                Assert.AreEqual(acctInfo.CosginatoryOf.Count, 0);
                break;
            case PUBKEYCS1:
                AssertAcct(acctInfo);
                Assert.AreEqual(acctInfo.Status, AccountInfo.StatusValue.LOCKED);
                Assert.AreEqual(acctInfo.RemoteStatus, AccountInfo.RemoteStatusValue.INACTIVE);
                Assert.AreEqual(acctInfo.MinCosigners, 0);
                Assert.IsNotNull(acctInfo.Cosigners);
                Assert.AreEqual(acctInfo.Cosigners.Count, 0);
                Assert.IsNotNull(acctInfo.CosginatoryOf);
                Assert.AreEqual(acctInfo.CosginatoryOf.Count, 1);
                AssertAcct(acctInfo.CosginatoryOf[0]);
                break;
            case PUBKEYCS2:
                AssertAcct(acctInfo);
                Assert.AreEqual(acctInfo.Status, AccountInfo.StatusValue.LOCKED);
                Assert.AreEqual(acctInfo.RemoteStatus, AccountInfo.RemoteStatusValue.INACTIVE);
                Assert.AreEqual(acctInfo.MinCosigners, 0);
                Assert.IsNotNull(acctInfo.Cosigners);
                Assert.AreEqual(acctInfo.Cosigners.Count, 0);
                Assert.IsNotNull(acctInfo.CosginatoryOf);
                Assert.AreEqual(acctInfo.CosginatoryOf.Count, 1);
                AssertAcct(acctInfo.CosginatoryOf[0]);
                break;
            case PUBKEYCSA:
                AssertAcct(acctInfo);
                Assert.AreEqual(acctInfo.Status, AccountInfo.StatusValue.LOCKED);
                Assert.AreEqual(acctInfo.RemoteStatus, AccountInfo.RemoteStatusValue.INACTIVE);
                Assert.AreEqual(acctInfo.MinCosigners, 0);
                Assert.IsNotNull(acctInfo.Cosigners);
                Assert.AreEqual(acctInfo.Cosigners.Count, 0);
                Assert.IsNotNull(acctInfo.CosginatoryOf);
                Assert.AreEqual(acctInfo.CosginatoryOf.Count, 0);
                break;
            default:
                throw new Exception("AssertAcctInfo: Unsupported Public Key");
            }
        }

        private void AssertAcct(AccountInfo acct)
        {
            switch (acct.PublicKey)
            {
            case PUBKEY:
                Assert.AreEqual(acct.Address.Networktype(), NetworkType.Types.TEST_NET);
                Assert.AreEqual(acct.Address.Plain, PLAIN);
                Assert.AreEqual(acct.Address.Pretty, PRETTY);
                Assert.AreNotEqual(acct.Balance, (ulong)0);
                Assert.AreNotEqual(acct.VestedBalance, (ulong)0);
                Assert.AreEqual(acct.Importance, (ulong)0);
                Assert.AreEqual(acct.HarvestedBlocks, (ulong)0);
                break;
            case PUBKEYMS:
                Assert.AreEqual(NetworkType.Types.TEST_NET, acct.Address.Networktype());
                Assert.AreEqual(acct.Address.Plain, PLAINMS);
                Assert.AreEqual(acct.Address.Pretty, PRETTYMS);
                Assert.IsTrue(acct.Balance > (ulong)1000000);
                Assert.IsTrue(acct.VestedBalance > (ulong)1000000);
                Assert.AreEqual(acct.Importance, (ulong)0);
                Assert.AreEqual(acct.HarvestedBlocks, (ulong)0);
                break;
            case PUBKEYCS1:
                Assert.AreEqual(NetworkType.Types.TEST_NET, acct.Address.Networktype());
                Assert.AreEqual(PLAINCS1, acct.Address.Plain);
                Assert.AreEqual(PRETTYCS1, acct.Address.Pretty);
                Assert.IsTrue(acct.Balance > (ulong)1000000);
                Assert.IsTrue(acct.VestedBalance > (ulong)1000000);
                Assert.AreEqual(acct.Importance, (ulong)0);
                Assert.AreEqual(acct.HarvestedBlocks, (ulong)0);
                break;
            case PUBKEYCS2:
                Assert.AreEqual(NetworkType.Types.TEST_NET, acct.Address.Networktype());
                Assert.AreEqual(PLAINCS2, acct.Address.Plain);
                Assert.AreEqual(PRETTYCS2, acct.Address.Pretty);
                Assert.IsTrue(acct.Balance >= (ulong)900000);
                Assert.IsTrue(acct.VestedBalance >= (ulong)900000);
                Assert.AreEqual(acct.Importance, (ulong)0);
                Assert.AreEqual(acct.HarvestedBlocks, (ulong)0);
                break;
            case PUBKEYCSA:
                Assert.AreEqual(NetworkType.Types.TEST_NET, acct.Address.Networktype());
                Assert.AreEqual(PLAINCSA, acct.Address.Plain);
                Assert.AreEqual(PRETTYCSA, acct.Address.Pretty);
                Assert.IsTrue(acct.Balance >= (ulong)450000);
                Assert.IsTrue(acct.VestedBalance >= (ulong)450000);
                Assert.AreEqual(acct.Importance, (ulong)0);
                Assert.AreEqual(acct.HarvestedBlocks, (ulong)0);
                break;
            default:
                throw new Exception("AssertAcct: Unsupported Public Key");
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
            Assert.AreEqual(ttx.Mosaics[0].MosaicName, Xem.MosaicName);
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
            Mosaic mosaic;

            Assert.IsNotNull(tx);
            Assert.IsNotNull(txinfo);
            switch (txinfo.Hash)
            {   
            case hashMS1:   // Multisig transaction with Transfer 
                Assert.AreEqual(txinfo.Height, (ulong)1503213);
                Assert.AreEqual(txinfo.IsMultisig, true);

                Assert.AreEqual(tx.NetworkType, NetworkType.Types.TEST_NET);
                Assert.AreEqual(tx.Version, 1);
                Assert.AreEqual(tx.TransactionType, TransactionTypes.Types.Multisig);
                Assert.IsTrue(tx.Deadline.TimeStamp >= tx.NetworkTime.TimeStamp);
                Assert.AreEqual(tx.NetworkTime.TimeStamp, 101132904);
                Assert.AreEqual(tx.NetworkTime.GetUtcDateTime(), DateTime.Parse("11/06/2018 12:34:49"));
                Assert.AreEqual(tx.NetworkTime.GetLocalDateTime(), DateTime.Parse("11/06/2018 14:34:49"));
                Assert.AreEqual(tx.Deadline.TimeStamp, 101136504);
                Assert.AreEqual(tx.Deadline.GetUtcDateTime(), DateTime.Parse("11/06/2018 13:34:49"));
                Assert.AreEqual(tx.Deadline.GetLocalDateTime(), DateTime.Parse("11/06/2018 15:34:49"));
                Assert.AreEqual(tx.Fee, (ulong)150000);    // Multisig Wrapper fee
                Assert.IsNotNull(tx.Signer);
                Assert.AreEqual(tx.Signer.PublicKey, PUBKEYCS2);
                Assert.IsNotNull(tx.Signer.Address);
                Assert.AreEqual(tx.Signer.Address.Networktype(), NetworkType.Types.TEST_NET);
                Assert.AreEqual(tx.Signer.Address.Plain, PLAINCS2);
                Assert.AreEqual(tx.Signer.Address.Pretty, PRETTYCS2);

                mstx = (MultisigTransaction)tx;
                Assert.AreEqual(mstx.Cosignatures.Count, 1);    //Cosignatures gives the Inner cosigning transaction List
                cstx = mstx.Cosignatures[0];
                Assert.IsNull(cstx.TransactionInfo);
                Assert.AreEqual(cstx.NetworkType, NetworkType.Types.TEST_NET);
                Assert.AreEqual(cstx.Version, 1);
                Assert.AreEqual(cstx.TransactionType, TransactionTypes.Types.SignatureTransaction);
                Assert.AreEqual(cstx.NetworkTime.TimeStamp, 101133019);
                Assert.AreEqual(cstx.NetworkTime.GetUtcDateTime(), DateTime.Parse("11/06/2018 12:36:44"));
                Assert.AreEqual(cstx.NetworkTime.GetLocalDateTime(), DateTime.Parse("11/06/2018 14:36:44"));
                Assert.AreEqual(cstx.Deadline.TimeStamp, 101136619);
                Assert.AreEqual(cstx.Deadline.GetUtcDateTime(), DateTime.Parse("11/06/2018 13:36:44"));
                Assert.AreEqual(cstx.Deadline.GetLocalDateTime(), DateTime.Parse("11/06/2018 15:36:44"));
                Assert.AreEqual(cstx.Fee, (ulong)150000);   // Cosingning transaction fee
                Assert.IsNotNull(cstx.Signer);
                Assert.AreEqual(cstx.Signer.PublicKey, PUBKEYCSA);
                Assert.IsNotNull(cstx.Signer.Address);
                Assert.AreEqual(cstx.Signer.Address.Networktype(), NetworkType.Types.TEST_NET);
                Assert.AreEqual(cstx.Signer.Address.Plain, PLAINCSA);
                Assert.AreEqual(cstx.Signer.Address.Pretty, PRETTYCSA);
                Assert.IsNotNull(cstx.MultisigAddress);
                Assert.AreEqual(cstx.MultisigAddress.Networktype(), NetworkType.Types.TEST_NET);
                Assert.AreEqual(cstx.MultisigAddress.Plain, PLAINMS);
                Assert.AreEqual(cstx.MultisigAddress.Pretty, PRETTYMS);

                itx = mstx.InnerTransaction;
                Assert.IsNotNull(itx);
                Assert.IsNull(itx.TransactionInfo);
                Assert.AreEqual(itx.NetworkType, NetworkType.Types.TEST_NET);
                Assert.AreEqual(itx.Version, 2);
                Assert.AreEqual(itx.TransactionType, TransactionTypes.Types.Transfer);
                ttx = (TransferTransaction)itx;
                Assert.AreEqual(ttx.NetworkTime.TimeStamp, 101132904);
                Assert.AreEqual(ttx.NetworkTime.GetUtcDateTime(), DateTime.Parse("11/06/2018 12:34:49"));
                Assert.AreEqual(ttx.NetworkTime.GetLocalDateTime(), DateTime.Parse("11/06/2018 14:34:49"));
                Assert.AreEqual(ttx.Deadline.TimeStamp, 101136504);
                Assert.AreEqual(ttx.Deadline.GetUtcDateTime(), DateTime.Parse("11/06/2018 13:34:49"));
                Assert.AreEqual(ttx.Deadline.GetLocalDateTime(), DateTime.Parse("11/06/2018 15:34:49"));
                Assert.AreEqual(ttx.Fee, (ulong)50000); // Transfer transaction fee
                Assert.IsNotNull(ttx.Signer);   // Sender
                Assert.AreEqual(ttx.Signer.PublicKey, PUBKEYMS);
                Assert.IsNotNull(ttx.Signer.Address);
                Assert.AreEqual(ttx.Signer.Address.Networktype(), NetworkType.Types.TEST_NET);
                Assert.AreEqual(ttx.Signer.Address.Plain, PLAINMS);
                Assert.AreEqual(ttx.Signer.Address.Pretty, PRETTYMS);
                Assert.IsNotNull(ttx.Address);  // Recipient
                Assert.AreEqual(ttx.Address.Networktype(), NetworkType.Types.TEST_NET);
                Assert.AreEqual(ttx.Address.Plain, PLAINMS);
                Assert.AreEqual(ttx.Address.Pretty, PRETTYMS);
                Assert.AreEqual(ttx.Message.GetMessageType(), MessageType.Type.UNENCRYPTED);
                Assert.AreEqual(ttx.Message.GetLength(), 0);
                Assert.IsNotNull(ttx.Mosaics);
                Assert.AreEqual(ttx.Mosaics.Count, 1);
                mosaic = ttx.Mosaics[0];
                Assert.AreEqual(mosaic.NamespaceName, Xem.NamespaceName);
                Assert.AreEqual(mosaic.MosaicName, Xem.MosaicName);
                Assert.AreEqual(mosaic.Amount, (ulong)1000);
                break;
            case hashMS2:   // Multisig Transaction with Transfer
                const string plainRecipient = "TALIC35ULCU2PFUIHM6J7WKKMSLKPVZKPK36PP4W";   // Recipient Account
                const string prettyRecipient = "TALIC3-5ULCU2-PFUIHM-6J7WKK-MSLKPV-ZKPK36-PP4W"; 
                Assert.AreEqual((ulong)1515002, txinfo.Height);
                Assert.AreEqual(true, txinfo.IsMultisig);

                Assert.AreEqual(tx.NetworkType, NetworkType.Types.TEST_NET);
                Assert.AreEqual(tx.Version, 1);
                Assert.AreEqual(tx.TransactionType, TransactionTypes.Types.Multisig);
                Assert.IsTrue(tx.Deadline.TimeStamp >= tx.NetworkTime.TimeStamp);
                Assert.AreEqual(tx.NetworkTime.TimeStamp, 101845918);
                Assert.AreEqual(tx.Deadline.TimeStamp, 101849518);
                Assert.AreEqual(tx.Fee, (ulong)150000);   // Multisig Wrapper fee
                Assert.IsNotNull(tx.Signer);
                Assert.AreEqual(tx.Signer.PublicKey, PUBKEYCS2);
                Assert.IsNotNull(tx.Signer.Address);
                Assert.AreEqual(tx.Signer.Address.Networktype(), NetworkType.Types.TEST_NET);
                Assert.AreEqual(tx.Signer.Address.Plain, PLAINCS2);
                Assert.AreEqual(tx.Signer.Address.Pretty, PRETTYCS2);

                mstx = (MultisigTransaction)tx;
                Assert.AreEqual(mstx.Cosignatures.Count, 1);    //Cosignatures gives the Inner cosigning transaction List
                cstx = mstx.Cosignatures[0];
                Assert.IsNull(cstx.TransactionInfo);
                Assert.AreEqual(cstx.NetworkType, NetworkType.Types.TEST_NET);
                Assert.AreEqual(cstx.Version, 1);
                Assert.AreEqual(cstx.TransactionType, TransactionTypes.Types.SignatureTransaction);
                Assert.AreEqual(cstx.NetworkTime.TimeStamp, 101845953);
                Assert.AreEqual(cstx.Deadline.TimeStamp, 101849553);
                Assert.AreEqual(cstx.Fee, (ulong)150000);   // Cosingning transaction fee
                Assert.IsNotNull(cstx.Signer);
                Assert.AreEqual(cstx.Signer.PublicKey, PUBKEYCS1);
                Assert.IsNotNull(cstx.Signer.Address);
                Assert.AreEqual(cstx.Signer.Address.Networktype(), NetworkType.Types.TEST_NET);
                Assert.AreEqual(cstx.Signer.Address.Plain, PLAINCS1);
                Assert.AreEqual(cstx.Signer.Address.Pretty, PRETTYCS1);
                Assert.IsNotNull(cstx.MultisigAddress);
                Assert.AreEqual(cstx.MultisigAddress.Networktype(), NetworkType.Types.TEST_NET);
                Assert.AreEqual(cstx.MultisigAddress.Plain, PLAINMS);
                Assert.AreEqual(cstx.MultisigAddress.Pretty, PRETTYMS);

                itx = mstx.InnerTransaction;
                Assert.IsNotNull(itx);
                Assert.IsNull(itx.TransactionInfo);
                Assert.AreEqual(itx.NetworkType, NetworkType.Types.TEST_NET);
                Assert.AreEqual(itx.Version, 1);
                Assert.AreEqual(itx.TransactionType, TransactionTypes.Types.Transfer);
                ttx = (TransferTransaction)mstx.InnerTransaction;
                Assert.AreEqual(ttx.NetworkTime.TimeStamp, 101845897);
                Assert.AreEqual(ttx.Deadline.TimeStamp, 101849497);
                Assert.AreEqual(ttx.Fee, (ulong)50000); // Transfer transaction fee
                Assert.IsNotNull(ttx.Signer);
                Assert.AreEqual(ttx.Signer.PublicKey, PUBKEYMS);
                Assert.IsNotNull(ttx.Signer.Address);
                Assert.AreEqual(ttx.Signer.Address.Networktype(), NetworkType.Types.TEST_NET);
                Assert.AreEqual(ttx.Signer.Address.Plain, PLAINMS);
                Assert.AreEqual(ttx.Signer.Address.Pretty, PRETTYMS);
                Assert.IsNotNull(ttx.Address);
                Assert.AreEqual(ttx.Address.Networktype(), NetworkType.Types.TEST_NET);
                Assert.AreEqual(ttx.Address.Plain, plainRecipient);
                Assert.AreEqual(ttx.Address.Pretty, prettyRecipient);
                Assert.AreEqual(ttx.Message.GetMessageType(), MessageType.Type.UNENCRYPTED);
                Assert.AreEqual(ttx.Message.GetLength(), 0);
                Assert.IsNotNull(ttx.Mosaics);
                Assert.AreEqual(ttx.Mosaics.Count, 1);
                mosaic = ttx.Mosaics[0];
                Assert.AreEqual(mosaic.NamespaceName, Xem.NamespaceName);
                Assert.AreEqual(mosaic.MosaicName, Xem.MosaicName);
                Assert.AreEqual(mosaic.Amount, (ulong)1000000);
                break;
            case hashMS3:   // Multisig transaction with Importance Transfer (to remote harvesting account)
                const string pubkeyRemote = "627b03264e51fa12870a923738506c27a20a3bc50051aeb75f545db7d7725060";
                const string plainRemote = "TABBQV6ZQZNKQNC646WNET6CPAHTRWIR4HSLAJZC";  // Remote account 
                const string prettyRemote = "TABBQV-6ZQZNK-QNC646-WNET6C-PAHTRW-IR4HSL-AJZC";
                Assert.AreEqual(txinfo.Height, (ulong)1511723);
                Assert.AreEqual(txinfo.IsMultisig, true);

                Assert.AreEqual(tx.NetworkType, NetworkType.Types.TEST_NET);
                Assert.AreEqual(tx.Version, 1);
                Assert.AreEqual(tx.TransactionType, TransactionTypes.Types.Multisig);
                Assert.IsTrue(tx.Deadline.TimeStamp >= tx.NetworkTime.TimeStamp);
                Assert.AreEqual(tx.NetworkTime.TimeStamp, 101647249);
                Assert.AreEqual(tx.Deadline.TimeStamp, 101650849);
                Assert.AreEqual(tx.Fee, (ulong)150000);
                Assert.IsNotNull(tx.Signer);
                Assert.AreEqual(tx.Signer.PublicKey, PUBKEYCS2);
                Assert.IsNotNull(tx.Signer.Address);
                Assert.AreEqual(tx.Signer.Address.Networktype(), NetworkType.Types.TEST_NET);
                Assert.AreEqual(tx.Signer.Address.Plain, PLAINCS2);
                Assert.AreEqual(tx.Signer.Address.Pretty, PRETTYCS2);

                mstx = (MultisigTransaction)tx;
                Assert.AreEqual(mstx.Cosignatures.Count, 1);    //Cosignatures gives the Inner cosigning transaction List
                cstx = mstx.Cosignatures[0];
                Assert.IsNull(cstx.TransactionInfo);
                Assert.AreEqual(cstx.NetworkType, NetworkType.Types.TEST_NET);
                Assert.AreEqual(cstx.Version, 1);
                Assert.AreEqual(cstx.TransactionType, TransactionTypes.Types.SignatureTransaction);
                Assert.AreEqual(cstx.NetworkTime.TimeStamp, 101647257);
                Assert.AreEqual(cstx.Deadline.TimeStamp, 101650857);
                Assert.AreEqual(cstx.Fee, (ulong)150000);
                Assert.IsNotNull(cstx.Signer);
                Assert.AreEqual(cstx.Signer.PublicKey, PUBKEYCSA);
                Assert.IsNotNull(cstx.Signer.Address);
                Assert.AreEqual(cstx.Signer.Address.Networktype(), NetworkType.Types.TEST_NET);
                Assert.AreEqual(cstx.Signer.Address.Plain, PLAINCSA);
                Assert.AreEqual(cstx.Signer.Address.Pretty, PRETTYCSA);

                Assert.AreEqual(PRETTYMS, cstx.MultisigAddress.Pretty);
                itx = mstx.InnerTransaction;
                Assert.IsNotNull(itx);
                Assert.IsNull(itx.TransactionInfo);
                Assert.AreEqual(itx.NetworkType, NetworkType.Types.TEST_NET);
                Assert.AreEqual(itx.Version, 1);
                Assert.AreEqual(itx.TransactionType, TransactionTypes.Types.ImportanceTransfer);
                ittx = (ImportanceTransferTransaction)itx;
                Assert.AreEqual(ittx.TransactionInfo, null);
                Assert.IsNotNull(ittx.Signer);
                Assert.AreEqual(ittx.Signer.PublicKey, PUBKEYMS);
                Assert.IsNotNull(ittx.Signer.Address);
                Assert.IsNotNull(ittx.Signer.Address.Plain, PLAINMS);
                Assert.IsNotNull(ittx.Signer.Address.Pretty, PRETTYMS);
                Assert.AreEqual(ittx.Mode, ImportanceTransferMode.Mode.Add);
                Assert.IsNotNull(ittx.RemoteAccount);
                Assert.AreEqual(ittx.RemoteAccount.PublicKey, pubkeyRemote);
                Assert.IsNotNull(ittx.RemoteAccount.Address);
                Assert.AreEqual(ittx.RemoteAccount.Address.Networktype(), NetworkType.Types.TEST_NET);
                Assert.AreEqual(ittx.RemoteAccount.Address.Plain, plainRemote);
                Assert.AreEqual(ittx.RemoteAccount.Address.Pretty, prettyRemote);
                break;
            }
        }

        [TestMethod]
        public async Task GetMosaicsOwned()
        {
            const string prettyM = "TCTUIF-557ZCQ-OQPW2M-6GH4TC-DPM2ZY-BBL54K-GNHR";
            List<Mosaic> mosaics = await new AccountHttp(host).MosaicsOwned(new Address(prettyM));

            Assert.AreEqual(mosaics.Count, 4);
            Assert.AreEqual(mosaics[0].NamespaceName, Xem.NamespaceName);
            Assert.AreEqual(mosaics[0].MosaicName, Xem.MosaicName);
            Assert.IsTrue(mosaics[0].Amount > 1000000);
            Assert.AreEqual(mosaics[1].NamespaceName, "nis1porttest");
            Assert.AreEqual(mosaics[1].MosaicName, "test");
            Assert.IsTrue(mosaics[1].Amount <= 100000000000);
            Assert.AreEqual(mosaics[2].NamespaceName, "myspace");
            Assert.AreEqual(mosaics[2].MosaicName, "subspacewithlevy");
            Assert.IsTrue(mosaics[2].Amount <= 10000000000000);
            Assert.AreEqual(mosaics[3].NamespaceName, "myspace");
            Assert.AreEqual(mosaics[3].MosaicName, "subspace");
            Assert.IsTrue(mosaics[3].Amount <= 10001000000000);
        }

    }
}
