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

        readonly string host = "http://" + Config.Domain + ":7890";

        [TestMethod]
        public async Task GetNamespaceRootInfo()
        {
            NamespaceRootInfo namespaceRootInfo;

            var namespaceRootInfos = await new NamespaceMosaicHttp(host).NamespaceRootInfoPage();   // Gets the last 25 created Namespaces

            for (int i = 0; i<25; i++)
            {
                namespaceRootInfo = namespaceRootInfos[i];
                if (namespaceRootInfo.Name == NS)
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
            NamespaceInfo namespaceInfo = await new NamespaceMosaicHttp(host).NamespaceInfo(NS);

            Assert.AreEqual(NS, namespaceInfo.Name);
            Assert.AreEqual((ulong)1817448, namespaceInfo.Height);
            Assert.AreEqual(PRETTY, namespaceInfo.Owner.Pretty);
        }

        [TestMethod]
        public async Task GetNamespaceMosaics()
        {
            var mosaicInfos = await new NamespaceMosaicHttp(host).GetNamespaceMosaics(NS);
            MosaicInfo mosaicInfo = mosaicInfos[0];

            Assert.AreEqual(4763, mosaicInfo.Id);
            Assert.AreEqual(PRETTY, mosaicInfo.Creator.Address.Pretty);
            Assert.AreEqual("london:stock", mosaicInfo.MosaicId.FullName);
            Assert.IsNull(mosaicInfo.Levy);
            Assert.AreEqual(4763, mosaicInfo.Id);
            Assert.IsTrue(mosaicInfo.Properties.Mutable);
            Assert.IsTrue(mosaicInfo.Properties.Transferable);
        }
    }
}
