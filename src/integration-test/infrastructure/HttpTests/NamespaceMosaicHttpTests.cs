using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using io.nem1.sdk.Infrastructure.HttpRepositories;
using io.nem1.sdk.Model.Mosaics;
using io.nem1.sdk.Model.Namespace;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTest.infrastructure.HttpTests
{
    [TestClass]
    public class NamespaceMosaicHttpTests
    {
        const string NS = "london";
        const string PRETTY = "TAB27E-DNZN2A-JKYBVW-LK5ERP-5KWUMW-PCKL65-44GX"; // Account owning the london namespace and the london:stock mosaic.
        const string PUBKEY = "3c02e57695cda96ee6ab0149e5f07fb0e88eddf0f35eff6bf188fbc146dc4926";

        readonly string host = "http://" + Config.Domain + ":7890";

        [TestMethod]
        public async Task GetNamespaceRootInfo()
        {
            NamespaceRootInfo namespaceRootInfo;

            var namespaceRootInfos = await new NamespaceMosaicHttp(host).GetNamespaceRootInfo();   // Gets the last 25 created Namespaces

            for (int i = 0; i<25; i++)
            {
                namespaceRootInfo = namespaceRootInfos[i];
                if (namespaceRootInfo.NamespaceId == NS)
                {
                    Assert.AreEqual(3745, namespaceRootInfo.Id);
                    Assert.AreEqual((ulong)1817448, namespaceRootInfo.Height);
                    Assert.AreEqual(PRETTY, namespaceRootInfo.Owner.Pretty);
                }
            }
        }

        [TestMethod]
        public async Task GetNamespaceInfo()
        {
            NamespaceInfo namespaceInfo = await new NamespaceMosaicHttp(host).GetNamespaceInfo(NS);

            Assert.AreEqual(NS, namespaceInfo.NamespaceId);
            Assert.AreEqual((ulong)1817448, namespaceInfo.Height);
            Assert.AreEqual(PRETTY, namespaceInfo.Owner.Pretty);
        }

        [TestMethod]
        public async Task GetNamespaceMosaics()
        {
            var mosaicInfos = await new NamespaceMosaicHttp(host).GetNamespaceMosaics(NS);
            MosaicInfo mosaicInfo = mosaicInfos[0];

            Assert.AreEqual(PUBKEY, mosaicInfo.CreatorPubKey);
            Assert.AreEqual("london:stock", mosaicInfo.FullName());
            Assert.IsNull(mosaicInfo.Levy);
            Assert.IsTrue(mosaicInfo.Properties.Mutable);
            Assert.IsTrue(mosaicInfo.Properties.Transferable);
        }
    }
}
