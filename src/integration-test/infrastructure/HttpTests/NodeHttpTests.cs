using System.Reactive.Linq;
using System.Threading.Tasks;
using io.nem1.sdk.Infrastructure.HttpRepositories;
using io.nem1.sdk.Model.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTest.infrastructure.HttpTests
{
    [TestClass]
    public class NodeHttpTests
    {
        readonly string host = "http://" + Config.Domain + ":7890";

        [TestMethod]
        public async Task GetExtendedNodeInfo()
        {
            var extNodeInfo = await new NodeHttp(host).GetExtendedNodeInfo();
            Assert.AreEqual("NEM Deploy", extNodeInfo.NisInfo.Application);
            Assert.IsTrue(117989066 < extNodeInfo.NisInfo.CurrentTime.TimeStamp);
            Assert.AreEqual(114429223, extNodeInfo.NisInfo.StartTime.TimeStamp);
            Assert.AreEqual("0.6.95-BETA", extNodeInfo.NisInfo.Version);
            AssertNodeInfo(extNodeInfo.NodeInfo);
        }

        [TestMethod]
        public async Task GetMaxHeight()
        {
            var height = await new NodeHttp(host).GetActiveNodeMaxHeight();
            Assert.IsTrue(1 < height);
        }

        [TestMethod]
        public async Task GetNodeActiveList()
        {
            var nodeInfos = await new NodeHttp(host).GetActiveNodeList();
            AssertNodeInfo(nodeInfos[0]);
        }

        [TestMethod]
        public async Task GetNodeInfo()
        {
            var nodeInfo = await new NodeHttp(host).GetNodeInfo();
            AssertNodeInfo(nodeInfo);
        }

        [TestMethod]
        public async Task GetNodeListAll() // figure out why only the queried node meta data comes back as nulls/0's
        {
            var peerListInfos = await new NodeHttp(host).GetNodePeerListAll();
            AssertNodeInfo(peerListInfos.Active[0]);
        }

        private void AssertNodeInfo(NodeInfo nodeInfo)
        {
            const string PROTOCOL = "http";
            const string PORT = "7890";

            switch (nodeInfo.Host)
            {
                case "37.61.200.170":
                    Assert.AreEqual(1, nodeInfo.Features);
                    Assert.AreEqual("", nodeInfo.Application);
                    Assert.AreEqual("TCSTORZFCDEPBSHVG3IYJB2YBQR2CPJXESSISB5N", nodeInfo.Name);
                    Assert.AreEqual(NetworkType.Types.TEST_NET, nodeInfo.Networktype);
                    Assert.AreEqual("Oracle Corporation (1.8.0_161) on Windows Server 2012 R2", nodeInfo.Platform);
                    Assert.AreEqual(PORT, nodeInfo.Port);
                    Assert.AreEqual(PROTOCOL, nodeInfo.Protocol);
                    Assert.AreEqual("e927d00de512e9de70097b31752a65b54deab3fff51b891a1efedb8af0320525", nodeInfo.PublicKey);
                    Assert.AreEqual("0.6.93-BETA", nodeInfo.Version);
                    break;
                case "188.68.50.161":
                    Assert.AreEqual(1, nodeInfo.Features);
                    Assert.AreEqual("", nodeInfo.Application);
                    Assert.AreEqual("Pretestnet2", nodeInfo.Name);
                    Assert.AreEqual(NetworkType.Types.TEST_NET, nodeInfo.Networktype);
                    Assert.AreEqual("Oracle Corporation (1.8.0_121) on Linux", nodeInfo.Platform);
                    Assert.AreEqual(PORT, nodeInfo.Port);
                    Assert.AreEqual(PROTOCOL, nodeInfo.Protocol);
                    Assert.AreEqual("40515e7039dde3336134c8826be921a6e419b18b1a6c9bee27d90ac2aca90dd2", nodeInfo.PublicKey);
                    Assert.AreEqual("0.6.93-BETA", nodeInfo.Version);
                    break;
                case "80.93.182.146":
                    Assert.AreEqual(1, nodeInfo.Features);
                    Assert.AreEqual("", nodeInfo.Application);
                    Assert.AreEqual("hxr.team", nodeInfo.Name);
                    Assert.AreEqual(NetworkType.Types.TEST_NET, nodeInfo.Networktype);
                    Assert.AreEqual("Oracle Corporation (1.8.0_144) on Linux", nodeInfo.Platform);
                    Assert.AreEqual(PORT, nodeInfo.Port);
                    Assert.AreEqual(PROTOCOL, nodeInfo.Protocol);
                    Assert.AreEqual("1e412cb4dda9d33cafaa0c8b575e8d073824b0b7a3073c98d1f17314a357350a", nodeInfo.PublicKey);
                    Assert.AreEqual("0.6.95-BETA", nodeInfo.Version);
                    break;
                case "117.52.172.103":
                    Assert.AreEqual(1, nodeInfo.Features);
                    Assert.AreEqual("", nodeInfo.Application);
                    Assert.AreEqual("TATOG2NMBV56ZXBI6F2W34253AMXHLKOZCXLU6LB", nodeInfo.Name);
                    Assert.AreEqual(NetworkType.Types.TEST_NET, nodeInfo.Networktype);
                    Assert.AreEqual("Oracle Corporation (1.8.0_162) on Linux", nodeInfo.Platform);
                    Assert.AreEqual(PORT, nodeInfo.Port);
                    Assert.AreEqual(PROTOCOL, nodeInfo.Protocol);
                    Assert.AreEqual("2ed4ff1855b49060a5a293391ac68a87ed03b3d391dd66c6692f2d0dcd42c968", nodeInfo.PublicKey);
                    Assert.AreEqual("0.6.95-BETA", nodeInfo.Version);
                    break;
                case "104.128.226.60":
                    Assert.IsTrue(0 <= nodeInfo.Features);
                    Assert.AreEqual("", nodeInfo.Application);
                    Assert.IsTrue("bigalice2" == nodeInfo.Name || "Hi, I am BigAlice2" == nodeInfo.Name);
                    Assert.IsTrue(NetworkType.Types.UNDETERMINED_NET == nodeInfo.Networktype || NetworkType.Types.TEST_NET == nodeInfo.Networktype);
                    Assert.IsTrue("" == nodeInfo.Platform || "Oracle Corporation (1.8.0_40) on Linux" == nodeInfo.Platform);
                    Assert.AreEqual(PORT, nodeInfo.Port);
                    Assert.AreEqual(PROTOCOL, nodeInfo.Protocol);
                    Assert.AreEqual("147eb3e4fccb655c03f4b6b12fc145f6a740c9334a8f3c59131dffd1fd42a996", nodeInfo.PublicKey);
                    Assert.IsTrue("0.0.0" == nodeInfo.Version || "0.6.95-BETA" == nodeInfo.Version);
                    break;
                case "110.134.77.58":
                    Assert.AreEqual(1, nodeInfo.Features);
                    Assert.AreEqual("", nodeInfo.Application);
                    Assert.AreEqual("planethouki.ddns.net", nodeInfo.Name);
                    Assert.AreEqual(NetworkType.Types.TEST_NET, nodeInfo.Networktype);
                    Assert.AreEqual("Oracle Corporation (1.8.0_144) on Linux", nodeInfo.Platform);
                    Assert.AreEqual(PORT, nodeInfo.Port);
                    Assert.AreEqual(PROTOCOL, nodeInfo.Protocol);
                    Assert.AreEqual("127e7db84be0d297cb03e856e3923bc5ea61a385c2dd4ec41f157dcc877d01af", nodeInfo.PublicKey);
                    Assert.AreEqual("0.6.96-BETA", nodeInfo.Version);
                    break;
                default:
                    break;
            }
        }
    }
}
