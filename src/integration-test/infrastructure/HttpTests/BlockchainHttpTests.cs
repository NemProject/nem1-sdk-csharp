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

using System.Reactive.Linq;
using System.Threading.Tasks;
using io.nem1.sdk.Core.Crypto.Chaso.NaCl;
using io.nem1.sdk.Infrastructure.HttpRepositories;
using io.nem1.sdk.Model.Network;
using io.nem1.sdk.Model.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTest.infrastructure.HttpTests
{
    [TestClass]
    public class BlockchainHttpTests
    {
        readonly string host = "http://" + Config.Domain + ":7890";

        [TestMethod, Timeout(20000)]
        public async Task GetHeight()
        {
            var a = await new BlockchainHttp(host).GetBlockchainHeight();

            Assert.IsTrue(a > 100);    
        }

        [TestMethod, Timeout(40000)]
        public async Task GetBlockByHeightWithAggregateModificationTransactions()
        {
            var block = await new BlockchainHttp(host).GetBlockByHeight(1514920);

            Assert.AreEqual("1b9aeadf89075616d7aeca034375f95e48afc036f407d7e042cf27d0e9f76310", block.PreviousBlockHash);
            Assert.AreEqual((ulong)1514920, block.Height);
            Assert.AreEqual("492bb087c66d516d1cf86f37ffb812aeec7a55f5dc304e6f5c13b3b2746d93a648f59fcce8c0e1ac7ebcc3a3023244b072a257d0db2b1af1129d678748597005", block.Signature);
            Assert.AreEqual("9822cf9571a5551ec19720b87a567a20797b75ec4b6711387643fc352fef704e", block.Signer.PublicKey);
            Assert.AreEqual(101840619, block.TimeStamp);
            Assert.AreEqual(1, block.Type);
            Assert.AreEqual(1, block.Version);
            Assert.AreEqual(NetworkType.Types.TEST_NET, block.Network);

            var tx = (MultisigAggregateModificationTransaction)block.Transactions[0];

            Assert.AreEqual(101844173, tx.Deadline.TimeStamp);
            Assert.AreEqual((ulong)500000, tx.Fee);
            Assert.AreEqual(0x98, tx.NetworkType.GetNetwork());
            Assert.AreEqual("7f8f04574139efcd1734fc1f2ad7e5bbe8d99deb2ab46abe8c3e1e29697b80962d93339dd1d28c37feb3b574b66abcc93a32ff2560234f7d99d6d444fac2d800", tx.Signature);
            Assert.AreEqual("dd7e157f1ed039d25824ff5684e4a656e28814da087738f9545b25eece325e50", tx.Signer.PublicKey);
            Assert.AreEqual(TransactionTypes.Types.MultisigAggregateModification, tx.TransactionType);
            Assert.AreEqual("a8e1b69a265d32358d4d56f4ae35ff821e62673a98c36e6e6ba2b74a23b85ebb", tx.Modifications[0].CosignatoryPublicKey.PublicKey);
            Assert.AreEqual(1, tx.Modifications[0].ModificationType.GetValue());
            Assert.AreEqual(3, tx.RelativeChange);
        }

        [TestMethod, Timeout(40000)]
        public async Task GetBlockByHeightWithCosginatureTransactions()
        {
            var block = await new BlockchainHttp(host).GetBlockByHeight(1515002);

            Assert.AreEqual("9b09382dc711a5eebd9323f1aaa8d790443c7e6f36015ca4c5d7236c93e29102", block.PreviousBlockHash);
            Assert.AreEqual((ulong)1515002, block.Height);
            Assert.AreEqual("33a58f7a8ef06e7290b08b3f3c79653be6ff24d9986e6c838f9cdb46ab624a29500c9ef2a1207ffed3b7463cfa295461c6eaff3300d2a6c87814e8cbf1f4a50b", block.Signature);
            Assert.AreEqual("727643fb1d18214334c11280df986a0afee128fc1f8be7f9118e89c417e07771", block.Signer.PublicKey);
            Assert.AreEqual(101846050, block.TimeStamp);
            Assert.AreEqual(1, block.Type);
            Assert.AreEqual(1, block.Version);
            Assert.AreEqual(NetworkType.Types.TEST_NET, block.Network);

            var tx = (MultisigTransaction)block.Transactions[0];

            Assert.AreEqual(101849518, tx.Deadline.TimeStamp);
            Assert.AreEqual((ulong)150000, tx.Fee);
            Assert.AreEqual(0x98, tx.NetworkType.GetNetwork());
            Assert.AreEqual("8a3eea425d4904fc17fe93f917692b450ea67c9d8140d19ff32ee874ff0057d9bddf6cc3aaaac9c95cf6cb2ebbe15fd7a5a4fe0020f1e7e2c6a8b539c09da702", tx.Signature);
            Assert.AreEqual("9d7ea57169a56a1bb821e1abf744610c639d7545f976f09808b68a6ad1415eb0", tx.Signer.PublicKey);
            Assert.AreEqual(TransactionTypes.Types.Multisig, tx.TransactionType);

            var cosignature = tx.Cosignatures[0];
                
            Assert.AreEqual(101849553, cosignature.Deadline.TimeStamp);
            Assert.AreEqual((ulong)150000, cosignature.Fee);
            Assert.AreEqual(0x98, cosignature.NetworkType.GetNetwork());
            Assert.AreEqual("0102d2937edfa9d26b4854d2d298b3e500e66dce419b72be2dab8fad6e2a1ea495d5c509047dacc092877161680ffcd589a378348b7c02fed00a133d7c65d90e", cosignature.Signature);
            Assert.AreEqual("fbe95048d0325e2553a5e2aa88b9e12ed59f7c8c0fb8f84a638f43a390116c22", cosignature.Signer.PublicKey);
            Assert.AreEqual(TransactionTypes.Types.SignatureTransaction, cosignature.TransactionType);

            Assert.AreEqual("TBDJXUULP2BRYNS7MWHY2WAFWKQNAF273KYBPFY5", tx.Cosignatures[0].MultisigAddress.Plain);
            Assert.AreEqual("6d69eaf9503183da0b4c25ab0becdd236246594ceaad2e24dc86017e9d2b8129", tx.Cosignatures[0].OtherHash);

        }

        [TestMethod, Timeout(20000)]
        public async Task GetBlockByHeightWithTransactions()
        {
            var block = await new BlockchainHttp(host).GetBlockByHeight(1508118);
           
            Assert.AreEqual("fbff8b2befbe1b6a28545eb38bad9ee175fe8ecf6754aec16aa21eb84bc75333", block.PreviousBlockHash);
            Assert.AreEqual((ulong)1508118, block.Height);
            Assert.AreEqual("ecd516de193b4046382f36fa84cda4c0fcb657e16d05de07b556a41f8033cc674a54d836abe77b1baa6dc4bcb20128ee003e9d98e4e9cf13dfba5b6b1d241903", block.Signature);
            Assert.AreEqual("b3180ba3814a293f6b0ade952a65abc2aad6a430e4c33a20c216c8c6ceba584f", block.Signer.PublicKey);
            Assert.AreEqual(101429059, block.TimeStamp);
            Assert.AreEqual(1, block.Type);
            Assert.AreEqual(1, block.Version);
            Assert.AreEqual(NetworkType.Types.TEST_NET, block.Network);

            var tx = (TransferTransaction)block.Transactions[0];

            Assert.AreEqual(101436255, tx.Deadline.TimeStamp);
            Assert.AreEqual((ulong)100000, tx.Fee);
            Assert.AreEqual(0x98, tx.NetworkType.GetNetwork());
            Assert.AreEqual("286734306db83992b4d7e192d473c5f46d9150bd5dbefb0eee6acf88f9a5897e1fd92e5973c1ac12e23c13f405e02e67d7a05b3203371c79cf18b75c70e1ff00", tx.Signature);
            Assert.AreEqual("7b1a93132b8c5b8001a07f973307bee2b37bcd6dc279a59ea98179b238d44e2d", tx.Signer.PublicKey);
            Assert.AreEqual(257, tx.TransactionType.GetValue());
            Assert.AreEqual("68656c6c6f", tx.Message.GetPayload().ToHexLower());
            Assert.AreEqual("TAVPDJDR3R3X4FJUKNPY2IQBNNRFV2QR5NJZJ3WR", tx.Address.Plain);
            Assert.AreEqual((ulong)1000000, tx.Mosaics[0].Amount);
           
        }

        [TestMethod, Timeout(20000)]
        public async Task GetBlockByHeightWithImportanceTransactions()
        {
            var block = await new BlockchainHttp(host).GetBlockByHeight(1510853);

            Assert.AreEqual("d3ea0a4e1a818ed0489a1ea2b8bdb5bbd5610c3e6f894d786f2fa068c37fd5b6", block.PreviousBlockHash);
            Assert.AreEqual((ulong)1510853, block.Height);
            Assert.AreEqual("817170e57a022eeecf1f57144315f126a60189c96259d0499a1f1ae77ee2b10359d1f73b0b912966f623ec47c5283024a62bbed2f1dd885b7104a4f6556ea90e", block.Signature);
            Assert.AreEqual("76164494236c9aa3e4731dddfa67f28b4cb0be077690abe52a664302e3e57d24", block.Signer.PublicKey);
            Assert.AreEqual(101595012, block.TimeStamp);
            Assert.AreEqual(1, block.Type);
            Assert.AreEqual(1, block.Version);
            Assert.AreEqual(NetworkType.Types.TEST_NET, block.Network);

            var tx = (ImportanceTransferTransaction)block.Transactions[0];

            Assert.AreEqual(101598553, tx.Deadline.TimeStamp);
            Assert.AreEqual((ulong)150000, tx.Fee);
            Assert.AreEqual(0x98, tx.NetworkType.GetNetwork());
            Assert.AreEqual("1afa6a903a9bf02ae64dcda5f407dd001cbf6abe41e808325f55c5911e5ef8a5e3e85865cf9769e9f1efd70ba30aa683ab75de0bee5a9b30f68ad6aaf3855c02", tx.Signature);
            Assert.AreEqual("7b1a93132b8c5b8001a07f973307bee2b37bcd6dc279a59ea98179b238d44e2d", tx.Signer.PublicKey);
            Assert.AreEqual(2049, tx.TransactionType.GetValue());
            Assert.AreEqual(ImportanceTransferMode.Mode.Add, tx.Mode);
            Assert.AreEqual("6ea3fd5f2cf4fbeb54cd96a48d11cd2ff0b4106472c6a97c7e4e5736243cb2db", tx.RemoteAccount.PublicKey);

        }

        [TestMethod, Timeout(20000)]
        public async Task GetBlockByHeightWithMosaicDefinitionWithoutLevyTransaction()
        {
            var block = await new BlockchainHttp(host).GetBlockByHeight(1518510);

            Assert.AreEqual("3463a6de6fd412f6cf33cfca8cdef1f4113689353b45905350894c98e3f6eef9", block.PreviousBlockHash);
            Assert.AreEqual((ulong)1518510, block.Height);
            Assert.AreEqual("becdb409443f955a2fbb421e8ba5cb78fd1bf65a63a457bf207a27bd75ac0882ef1fd3404afd5bb0bdabee8424f10245ed90a4af2f2ca1658b3d0e3169afa201", block.Signature);
            Assert.AreEqual("9822cf9571a5551ec19720b87a567a20797b75ec4b6711387643fc352fef704e", block.Signer.PublicKey);
            Assert.AreEqual(102057753, block.TimeStamp);
            Assert.AreEqual(1, block.Type);
            Assert.AreEqual(1, block.Version);
            Assert.AreEqual(NetworkType.Types.TEST_NET, block.Network);

            var tx = (MosaicDefinitionTransaction)block.Transactions[0];

            Assert.AreEqual(102061222, tx.Deadline.TimeStamp);
            Assert.AreEqual((ulong)150000, tx.Fee);
            Assert.AreEqual(0x98, tx.NetworkType.GetNetwork());
            Assert.AreEqual("b6adf94d3d54cf9e2c88cf1c668bb102f019f1c6f1dc569885668862603802dc5edeae964a3a72cbd19a930145b7d803dc3f0f259370f2ab8f900e15ad1dd704", tx.Signature);
            Assert.AreEqual("7b1a93132b8c5b8001a07f973307bee2b37bcd6dc279a59ea98179b238d44e2d", tx.Signer.PublicKey);
            Assert.AreEqual(16385, tx.TransactionType.GetValue());
        }

        [TestMethod, Timeout(20000)]
        public async Task GetBlockByHeightWitSupplyChangeTransaction()
        {
            var block = await new BlockchainHttp(host).GetBlockByHeight(1518258);

            
            Assert.AreEqual("3707e3bb936b1d149a8d4ddfef8faf6009ec4d0778e107ad06d3fb6cecfa1c41", block.PreviousBlockHash);
            Assert.AreEqual((ulong)1518258, block.Height);
            Assert.AreEqual("8a41299e5e585636242a44c691a40df1cd766096a4806b81ae724f750434f153bb971245260c57056e3a42a16d650d02134db154e98617a424789ac2c7481e09", block.Signature);
            Assert.AreEqual("f60ab8a28a42637062e6ed43a20793735c58cb3e8f3a0ab74148d591a82eba4d", block.Signer.PublicKey);
            Assert.AreEqual(102043342, block.TimeStamp);
            Assert.AreEqual(1, block.Type);
            Assert.AreEqual(1, block.Version);
            Assert.AreEqual(NetworkType.Types.TEST_NET, block.Network);

            var tx = (SupplyChangeTransaction)block.Transactions[0];

            Assert.AreEqual(102046580, tx.Deadline.TimeStamp);
            Assert.AreEqual((ulong)150000, tx.Fee);
            Assert.AreEqual(0x98, tx.NetworkType.GetNetwork());
            Assert.AreEqual("e3974d7ccea1f620dd79b950fa32da9b8f09f79a5859d97e4758e6cda5c32bc33fd7ff229986bc04384cb5b0568b500eb4418dc57f7f04e04a83127fc9620701", tx.Signature);
            Assert.AreEqual("7b1a93132b8c5b8001a07f973307bee2b37bcd6dc279a59ea98179b238d44e2d", tx.Signer.PublicKey);
            Assert.AreEqual(16386, tx.TransactionType.GetValue());

            Assert.AreEqual((ulong)100000, tx.Delta);
            Assert.AreEqual("myspace:subspace", tx.MosaicId.FullName);
            Assert.AreEqual(1, tx.SupplyType);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetBlockByHeightWithMosaicDefinitionWithLevyTransaction()
        {
            var block = await new BlockchainHttp(host).GetBlockByHeight(1518184);

            Assert.AreEqual("a6e1725bd76e6123aa9f0c520b7804adee282ae65111d4fbd7f96b61ed237275", block.PreviousBlockHash);
            Assert.AreEqual((ulong)1518184, block.Height);
            Assert.AreEqual("badb28e16e56c26dbd5945b6dee96c556e9987080c7ed95370656c15c7ce86a7bce72c6281d4d625bb5417d8822c0e15282577802b27b6fadadbf7f7cc4a840a", block.Signature);
            Assert.AreEqual("76164494236c9aa3e4731dddfa67f28b4cb0be077690abe52a664302e3e57d24", block.Signer.PublicKey);
            Assert.AreEqual(102038505, block.TimeStamp);
            Assert.AreEqual(1, block.Type);
            Assert.AreEqual(1, block.Version);
            Assert.AreEqual(NetworkType.Types.TEST_NET, block.Network);

            var tx = (MosaicDefinitionTransaction)block.Transactions[0];

            Assert.AreEqual(102042046, tx.Deadline.TimeStamp);
            Assert.AreEqual((ulong)150000, tx.Fee);
            Assert.AreEqual(0x98, tx.NetworkType.GetNetwork());
            Assert.AreEqual("6f2cf6a00120e58b48f13a7f38d4c16ca1dc089b775b91b5213fbe4286dddeef1cf4a7e0e9f7f0c32b254532e5b0f270e47f1676d6096519db93972c10f63000", tx.Signature);
            Assert.AreEqual("7b1a93132b8c5b8001a07f973307bee2b37bcd6dc279a59ea98179b238d44e2d", tx.Signer.PublicKey);
            Assert.AreEqual(16385, tx.TransactionType.GetValue());

            Assert.AreEqual("TCTUIF557ZCQOQPW2M6GH4TCDPM2ZYBBL54KGNHR", tx.Creator.Address.Plain);
            Assert.AreEqual("new mosaic test", tx.Description);
            Assert.AreEqual("myspace:subspacewithlevy", tx.Mosaic.FullName);
            Assert.AreEqual(1, tx.MosaicLevy.FeeType);
            Assert.AreEqual("TCTUIF557ZCQOQPW2M6GH4TCDPM2ZYBBL54KGNHR", tx.MosaicLevy.LevyRecipient.Plain);
            Assert.AreEqual("subspace", tx.MosaicLevy.Mosaic.MosaicName);
            Assert.AreEqual("myspace", tx.MosaicLevy.Mosaic.NamespaceName);         
        }

        [TestMethod, Timeout(20000)]
        public async Task GetBlockByHeightWithMultisigTransferTransactions()
        {
            var block = await new BlockchainHttp(host).GetBlockByHeight(1503213);

            Assert.AreEqual("6bd2beaa3336c3b73d5ffb5094941643c2138d92b8343421e35b79cf1726bd70", block.PreviousBlockHash);
            Assert.AreEqual((ulong)1503213, block.Height);
            Assert.AreEqual("513b0a14cc6d25c7bc5e5ff0daa6574ca1d4a22f24f39145e16292b8032564340754ead72b6672c185e2d5b11d7d06f904f5e77c345711386e1ec9e5e8beb703", block.Signature);
            Assert.AreEqual("74375c15c6ce6bdbde59be88a069745a0de34444ea933f8c9f46ef407cf30196", block.Signer.PublicKey);
            Assert.AreEqual(101133172, block.TimeStamp);
            Assert.AreEqual(1, block.Type);
            Assert.AreEqual(1, block.Version);
            Assert.AreEqual(NetworkType.Types.TEST_NET, block.Network);

            var tx = (MultisigTransaction)block.Transactions[0];

            Assert.AreEqual(101136504, tx.Deadline.TimeStamp);
            Assert.AreEqual((ulong)150000, tx.Fee);
            Assert.AreEqual(0x98, tx.NetworkType.GetNetwork());
            Assert.AreEqual("8c3abfd275c72fae0042d9a92959ae53c6282a34afca39b637a53fd73092b02e72d5860549cd697884e5c9ec7c04f3beb7a702fee74f646a0e107656cdae320a", tx.Signature);
            Assert.AreEqual("9d7ea57169a56a1bb821e1abf744610c639d7545f976f09808b68a6ad1415eb0", tx.Signer.PublicKey);
            Assert.AreEqual(TransactionTypes.Types.Multisig, tx.TransactionType);

            Assert.AreEqual(101136504, ((TransferTransaction)tx.InnerTransaction).Deadline.TimeStamp);
            Assert.AreEqual(2, ((TransferTransaction)tx.InnerTransaction).Version);
            Assert.AreEqual(TransactionTypes.Types.Transfer, ((TransferTransaction)tx.InnerTransaction).TransactionType);
            Assert.AreEqual((ulong)50000, ((TransferTransaction)tx.InnerTransaction).Fee);
            Assert.AreEqual("29c4a4aa674953749053c8a35399b37b713dedd5d002cb29b3331e56ff1ea65a", ((TransferTransaction)tx.InnerTransaction).Signer.PublicKey);
            Assert.AreEqual("TBDJXUULP2BRYNS7MWHY2WAFWKQNAF273KYBPFY5", ((TransferTransaction)tx.InnerTransaction).Address.Plain);
            Assert.AreEqual((ulong)1000, ((TransferTransaction)tx.InnerTransaction).Mosaics[0].Amount);

        }

        [TestMethod, Timeout(20000)]
        public async Task GetChainScore()
        {
            var a = await new BlockchainHttp(host).GetBlockchainScore();

            Assert.IsNotNull(a);
            
        }

        [TestMethod, Timeout(20000)]
        public async Task GetLastBlock()
        {
            var a = await new BlockchainHttp(host).GetLastBlock();

            Assert.AreEqual(1, a.Type);
        }
    }
}
