// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : realgarp
// Created          : 24-01-2019
//
// Last Modified By : realgarp
// Last Modified On : 24-01-2019
// ***********************************************************************
// <copyright file="ListenerTests.cs" company="Nem.io">
// Copyright 2019 NEM
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
using io.nem1.sdk.src.Infrastructure.Listener;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using io.nem1.sdk.Model.Network;
using System.Diagnostics;
using io.nem1.sdk.Model.Accounts;
using System;
using io.nem1.sdk.Model.Namespace;
using io.nem1.sdk.Model.Mosaics;

namespace IntegrationTest.infrastructure.Listeners
{
    [TestClass]
    public class ListenerTests
    {
        [TestMethod, Timeout(120000)]
        public async Task ListenForBlock()
        {
            var listener = new Listener(Config.Domain);
            Assert.AreEqual(true, await listener.ConnectAsync(AssertOnError));
            await listener.SubscribeToBlocksAsync(AssertOnBlock);
            await Task.Delay(60000);    // Additional time to allow the listener to read incomming StompMsgs and call the event handlers
            Assert.AreEqual(true, await listener.DisconnectAsync());
        }

        public void AssertOnError(string error)
        {
            Debug.WriteLine("AssertOnError: " + error);
            Assert.AreEqual("", error);
        }

        public void AssertOnBlock(ulong height)
        {
            Debug.WriteLine(string.Format("AssertOnBlock: new block with height {0}", height));
            Assert.IsTrue(1832000 < height);
        }

        [TestMethod]
        public async Task ListenForAccount()
        {
            var listener = new Listener(Config.Domain);
            Assert.AreEqual(true, await listener.ConnectAsync(AssertOnError));
            await listener.SubscribeToAccountAsync(Config.TACCOUNT_TO_WATCH1, AssertOnAccount);
            await Task.Delay(5000);    // Additional time to allow the listener to read incomming StompMsgs and call the event handlers
            Assert.AreEqual(true, await listener.DisconnectAsync());
        }

        private void AssertOnAccount(AccountInfo acctInfo)
        {
            Debug.WriteLine(string.Format("AssertOnAccount: AccountInfo received for address '{0}'",  acctInfo.Address.Pretty));
            switch (acctInfo.PublicKey)
            {
                case Config.TACCOUNT_TO_WATCH1_PUBKEY:
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
                    throw new Exception("AssertOnAccount: Unsupported Public Key");
            }
            // If the Account is a cosigner of other accounts (multisig), it is a CosignatoryOf those multisig accounts
            //  Then all info of these multisig accounts should be subscribed and requested as well!
        }

        private void AssertCosigAcctInfo(AccountInfo cosigAcctInfo)
        {
            switch (cosigAcctInfo.PublicKey)
            {
                case Config.TACCOUNT_TO_WATCH1_PUBKEY:
                    Assert.AreEqual(NetworkType.Types.TEST_NET, cosigAcctInfo.Address.GetNetworktype());
                    Assert.AreEqual(Config.TACCOUNT_TO_WATCH1, cosigAcctInfo.Address.Pretty);
                    Assert.AreNotEqual((ulong)0, cosigAcctInfo.Balance);
                    Assert.AreNotEqual((ulong)0, cosigAcctInfo.VestedBalance);
                    Assert.AreNotEqual((double)0, cosigAcctInfo.Importance);
                    Assert.AreEqual((ulong)0, cosigAcctInfo.HarvestedBlocks);
                    break;
                default:
                    throw new Exception("AssertCosigAcctInfo: Unsupported Public Key");
            }
        }

        [TestMethod]
        public async Task ListenForNamespaces()
        {
            var listener = new Listener(Config.Domain);
            Assert.AreEqual(true, await listener.ConnectAsync(AssertOnError));
            await listener.SubscribeToOwnedNamespacesAsync(Config.TACCOUNT_TO_WATCH2, AssertOnNamespace);  // The callback will be invoked for each msg received per Namespace owned
            await Task.Delay(5000);    // Additional time to allow the listener to read incomming StompMsgs and call the event handlers
            Assert.AreEqual(true, await listener.DisconnectAsync());
        }

        public void AssertOnNamespace(Address addr, NamespaceInfo namespaceInfo)
        {
            const string NS1 = "myspace";
            const string NS2 = "myspace.subspace";
            const string NS3 = "spacetest";
            const string NS4 = "nis1porttest";

            Debug.WriteLine(string.Format("AssertOnNamespace: NamespaceInfo received for namespace '{0}', owned by {1}", namespaceInfo.NamespaceId, addr.Pretty));
            Assert.AreEqual(Config.TACCOUNT_TO_WATCH2, namespaceInfo.Owner.Pretty);
            switch (namespaceInfo.NamespaceId)
            {
                case NS1:
                    Assert.AreEqual((ulong)1516704, namespaceInfo.Height);
                    break;
                case NS2:
                    Assert.AreEqual((ulong)1516704, namespaceInfo.Height);
                    break;
                case NS3:
                    Assert.AreEqual((ulong)1494919, namespaceInfo.Height);
                    break;
                case NS4:
                    Assert.AreEqual((ulong)1494918, namespaceInfo.Height);
                    break;
                default:
                    throw new Exception("AssertOnOwnedNamespace: Unsupported Namespace");
            }
        }

        [TestMethod]
        public async Task ListenForMosaics()
        {
            var listener = new Listener(Config.Domain);
            Assert.AreEqual(true, await listener.ConnectAsync(AssertOnError));
            await listener.SubscribeToOwnedMosaicsAsync(Config.TACCOUNT_TO_WATCH2, AssertOnMosaic);  // The callback will be invoked for each msg received per Namespace owned
            await Task.Delay(5000);    // Additional time to allow the listener to read incomming StompMsgs and call the event handlers
            Assert.AreEqual(true, await listener.DisconnectAsync());
        }

        public void AssertOnMosaic(Address addr, MosaicAmount mosaicAmount)
        {
            const string NS1 = "myspace";
            const string NS2 = "nis1porttest";

            Debug.WriteLine(string.Format("AssertOnMosaic: MosaicAmount received for mosaic '{0}', owned by {1}", mosaicAmount.MosaicInfo.FullName(), addr.Pretty));
            switch (mosaicAmount.MosaicInfo.NamespaceId)
            {
                case MosaicId.NEM:
                    Assert.AreEqual(MosaicId.XEM, mosaicAmount.MosaicInfo.Name);
                    Assert.AreEqual((ulong)8673811248, mosaicAmount.Amount);
                    break;
                case NS1:
                    switch (mosaicAmount.MosaicInfo.Name)
                    {
                        case "subspace":
                            Assert.AreEqual((ulong)10001000000000, mosaicAmount.Amount);
                            break;
                        case "subspacewithlevy":
                            Assert.AreEqual((ulong)10000000000000, mosaicAmount.Amount);
                            break;
                        default:
                            throw new Exception("AssertOnOwnedMosaic: Unsupported Mosaic " + mosaicAmount.MosaicInfo.Name);
                    }
                    break;
                case NS2:
                    Assert.AreEqual("test", mosaicAmount.MosaicInfo.Name);
                    Assert.AreEqual((ulong)100000000000, mosaicAmount.Amount);
                    break;
                default:
                    throw new Exception("AssertOnOwnedMosaic: Unsupported Namespace " + mosaicAmount.MosaicInfo.NamespaceId);
            }
        }

        [TestMethod]
        public async Task ListenForMosaicDefinitions()
        {
            var listener = new Listener(Config.Domain);
            Assert.AreEqual(true, await listener.ConnectAsync(AssertOnError));
            await listener.SubscribeToOwnedMosaicDefinitionsAsync(Config.TACCOUNT_TO_WATCH2, AssertOnMosaicDefinition);  // The callback will be invoked for each msg received per Namespace owned
            await Task.Delay(5000);    // Additional time to allow the listener to read incomming StompMsgs and call the event handlers
            Assert.AreEqual(true, await listener.DisconnectAsync());
        }

        public void AssertOnMosaicDefinition(Address addr, MosaicInfo mosaicInfo)
        {
            const string M1 = "myspace:subspace";
            const string M2 = "myspace:subspacewithlevy";
            const string M3 = "nis1porttest:test";


            Debug.WriteLine(string.Format("AssertOnMosaicDefinition: MosaicDefinition received for mosaic '{0}', owned by {1}", mosaicInfo.FullName(), addr.Pretty));
            //AssertOnMosaicDefinition: MosaicDefinition received for mosaic '{"mosaicDefinition":{"creator":"7b1a93132b8c5b8001a07f973307bee2b37bcd6dc279a59ea98179b238d44e2d","description":"new mosaic test","id":{"namespaceId":"myspace","name":"subspace"},"properties":[{"name":"divisibility","value":"4"},{"name":"initialSupply","value":"1000000000"},{"name":"supplyMutable","value":"true"},{"name":"transferable","value":"true"}],"levy":{}},"supply":1000100000}', owned by TCTUIF - 557ZCQ - OQPW2M - 6GH4TC - DPM2ZY - BBL54K - GNHR
            //AssertOnMosaicDefinition: MosaicDefinition received for mosaic '{"mosaicDefinition":{"creator":"7b1a93132b8c5b8001a07f973307bee2b37bcd6dc279a59ea98179b238d44e2d","description":"test","id":{"namespaceId":"nis1porttest","name":"test"},"properties":[{"name":"divisibility","value":"6"},{"name":"initialSupply","value":"100000"},{"name":"supplyMutable","value":"true"},{"name":"transferable","value":"true"}],"levy":{}},"supply":100000}', owned by TCTUIF - 557ZCQ - OQPW2M - 6GH4TC - DPM2ZY - BBL54K - GNHR
            //AssertOnMosaicDefinition: MosaicDefinition received for mosaic '{"mosaicDefinition":{"creator":"3e82e1c1e4a75adaa3cba8c101c3cd31d9817a2eb966eb3b511fb2ed45b8e262","description":"reserved xem mosaic","id":{"namespaceId":"nem","name":"xem"},"properties":[{"name":"divisibility","value":"6"},{"name":"initialSupply","value":"8999999999"},{"name":"supplyMutable","value":"false"},{"name":"transferable","value":"true"}],"levy":{}},"supply":8999999999}', owned by TCTUIF - 557ZCQ - OQPW2M - 6GH4TC - DPM2ZY - BBL54K - GNHR
            //AssertOnMosaicDefinition: MosaicDefinition received for mosaic '{"mosaicDefinition":{"creator":"7b1a93132b8c5b8001a07f973307bee2b37bcd6dc279a59ea98179b238d44e2d","description":"new mosaic test","id":{"namespaceId":"myspace","name":"subspacewithlevy"},"properties":[{"name":"divisibility","value":"4"},{"name":"initialSupply","value":"1000000000"},{"name":"supplyMutable","value":"true"},{"name":"transferable","value":"true"}],"levy":{"fee":100,"recipient":"TCTUIF557ZCQOQPW2M6GH4TCDPM2ZYBBL54KGNHR","type":1,"mosaicId":{"namespaceId":"myspace","name":"subspace"}}},"supply":1000000000}', owned by TCTUIF - 557ZCQ - OQPW2M - 6GH4TC - DPM2ZY - BBL54K - GNHR

            switch (mosaicInfo.FullName())
            {
                case MosaicId.XEMfullName:
                    Assert.AreEqual("3e82e1c1e4a75adaa3cba8c101c3cd31d9817a2eb966eb3b511fb2ed45b8e262", mosaicInfo.CreatorPubKey);
                    Assert.AreEqual("reserved xem mosaic", mosaicInfo.Description);
                    Assert.AreEqual(MosaicInfo.Xem.Properties.Divisibility, mosaicInfo.Properties.Divisibility);
                    Assert.AreEqual(MosaicInfo.Xem.Properties.InitialSupply, mosaicInfo.Properties.InitialSupply);
                    Assert.AreEqual(MosaicInfo.Xem.Properties.Mutable, mosaicInfo.Properties.Mutable);
                    Assert.AreEqual(MosaicInfo.Xem.Properties.Transferable, mosaicInfo.Properties.Transferable);
                    Assert.IsNull(mosaicInfo.Levy);
                    break;
                case M1:
                    Assert.AreEqual(Config.TACCOUNT_TO_WATCH2_PUBKEY, mosaicInfo.CreatorPubKey);
                    Assert.AreEqual("new mosaic test", mosaicInfo.Description);
                    Assert.AreEqual(4, mosaicInfo.Properties.Divisibility);
                    Assert.AreEqual((ulong)1000000000, mosaicInfo.Properties.InitialSupply);
                    Assert.AreEqual(true, mosaicInfo.Properties.Mutable);
                    Assert.AreEqual(true, mosaicInfo.Properties.Transferable);
                    Assert.IsNull(mosaicInfo.Levy);
                    break;
                case M2:
                    Assert.AreEqual(Config.TACCOUNT_TO_WATCH2_PUBKEY, mosaicInfo.CreatorPubKey);
                    Assert.AreEqual("new mosaic test", mosaicInfo.Description);
                    Assert.AreEqual(4, mosaicInfo.Properties.Divisibility);
                    Assert.AreEqual((ulong)1000000000, mosaicInfo.Properties.InitialSupply);
                    Assert.AreEqual(true, mosaicInfo.Properties.Mutable);
                    Assert.AreEqual(true, mosaicInfo.Properties.Transferable);
                    Assert.IsNotNull(mosaicInfo.Levy);
                    // Assert The Levy
                    Assert.AreEqual(Config.TACCOUNT_TO_WATCH2, mosaicInfo.Levy.Recipient.Pretty);
                    Assert.AreEqual(M1, mosaicInfo.Levy.FullName());
                    Assert.AreEqual((ulong)100, mosaicInfo.Levy.Fee);
                    Assert.AreEqual(1, mosaicInfo.Levy.FeeType);
                    break;
                case M3:
                    Assert.AreEqual(Config.TACCOUNT_TO_WATCH2_PUBKEY, mosaicInfo.CreatorPubKey);
                    Assert.AreEqual("test", mosaicInfo.Description);
                    Assert.AreEqual(6, mosaicInfo.Properties.Divisibility);
                    Assert.AreEqual((ulong)100000, mosaicInfo.Properties.InitialSupply);
                    Assert.AreEqual(true, mosaicInfo.Properties.Mutable);
                    Assert.AreEqual(true, mosaicInfo.Properties.Transferable);
                    Assert.IsNull(mosaicInfo.Levy);
                    break;
                default:
                    throw new Exception("AssertOnMosaicDefinition: Unsupported MosaicInfo " + mosaicInfo.FullName());
            }
        }

        //public void AssertAccount(string msg)
        //{

        //}

        //public void AssertAccount(string msg)
        //{

        //}

        //public void AssertAccount(string msg)
        //{

        //}

        [TestMethod, Timeout(120000)]
        public async Task ListenForEverything()
        {
            const string ACCT = Config.TACCOUNT_TO_WATCH1;    // or Config.TACCOUNT_TO_WATCH2
            var listener = new Listener(Config.Domain);
            Assert.AreEqual(true, await listener.ConnectAsync(AssertOnError));
            await listener.SubscribeToBlocksAsync(AssertOnBlock);
            await listener.SubscribeToAccountAsync(ACCT, AssertOnAccount);                          // The callback will be invoked for each block
            await listener.SubscribeToOwnedNamespacesAsync(ACCT, AssertOnNamespace);                // The callback will be invoked for each Namespace owned
            await listener.SubscribeToOwnedMosaicsAsync(ACCT, AssertOnMosaic);                      // The callback will be invoked for each Mosaic owned
            await listener.SubscribeToOwnedMosaicDefinitionsAsync(ACCT, AssertOnMosaicDefinition);  // The callback will be invoked for each Definition of a Mosaic owned
            await Task.Delay(60000);    // Additional time to allow the listener to read incomming StompMsgs and call the event handlers
            Assert.AreEqual(true, await listener.DisconnectAsync());
        }
    }
}
