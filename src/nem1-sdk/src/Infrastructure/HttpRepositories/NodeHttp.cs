using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using io.nem1.sdk.Infrastructure.Imported.Api;
using io.nem1.sdk.Infrastructure.Mapping;
using io.nem1.sdk.Model.Network;
using Newtonsoft.Json.Linq;

namespace io.nem1.sdk.Infrastructure.HttpRepositories
{
    /// <summary>
    /// Node Http Repository.
    /// </summary>
    /// <seealso cref="HttpRouter" />
    public class NodeHttp : HttpRouter
    {    
        internal NodeRoutesApi NodeRoutesApi { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NisHttp" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <exception cref="ArgumentException">Value cannot be null or empty. - host</exception>
        public NodeHttp(string host) : base(host)
        {
            NodeRoutesApi = new NodeRoutesApi(host);
        }

        /// <summary>
        /// Gets the network type.
        /// </summary>
        /// <returns>An IObservable of NetworkType.Types.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>GetNetworkType</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// NetworkType.Types networkType = await new NodeHttp("<!--insert host like: http://0.0.0.0:7890-->").GetNetworkType();
        /// </code>
        /// </example>
        public IObservable<NetworkType.Types> GetNetworkType()
        {
            return Observable.FromAsync(async ar => await NodeRoutesApi.ExtendedNodeInfoAsync()).Select(a => NetworkType.GetRawValue((byte) int.Parse(a["node"]["metaData"]["networkId"].ToString())));
        }

        /// <summary>
        /// Gets the active node list.
        /// </summary>
        /// <returns>An IObservable List of NodeInfo.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>GetActiveNodeList</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// List&lt;NodeInfo&gt; nodeInfo = await new NodeHttp("<!--insert host like: http://0.0.0.0:7890-->").GetActiveNodeList();
        /// </code>
        /// </example>
        public IObservable<List<NodeInfo>> GetActiveNodeList()
        {
            return Observable.FromAsync(async ar => await NodeRoutesApi.ActiveNodePeerListAllAsync())
                .Select(e => new NodeInfoMapping().MapNodes(e));
        }

        /// <summary>
        /// Gets a list of all active, inactive, busy and unresponsive nodes.
        /// </summary>
        /// <returns>An IObservable of PeerListInfo.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>GetNodePeerListAll</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// PeerListInfo peerListInfo = await new NodeHttp("<!--insert host like: http://0.0.0.0:7890-->").GetNodePeerListAll();
        /// </code>
        /// </example>
        internal IObservable<PeerListInfo> GetNodePeerListAll()
        {
            return Observable.FromAsync(async ar => await NodeRoutesApi.NodePeerListAllAsync())
                .Select(e => new PeerListInfo(
                    new NodeInfoMapping().MapNodes(e["active"].ToObject<List<JObject>>()), 
                    new NodeInfoMapping().MapNodes(e["inactive"].ToObject<List<JObject>>()),
                    new NodeInfoMapping().MapNodes(e["busy"].ToObject<List<JObject>>()),
                    new NodeInfoMapping().MapNodes(e["failure"].ToObject<List<JObject>>())));
        }

        /// <summary>
        /// Gets the maximum height of the active node list.
        /// </summary>
        /// <returns>An IObservable of ulong.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>GetActiveNodeMaxHeight</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// ulong maxHeight = await new NodeHttp("<!--insert host like: http://0.0.0.0:7890-->").GetActiveNodeMaxHeight();
        /// </code>
        /// </example>
        public IObservable<ulong> GetActiveNodeMaxHeight()
        {
            return Observable.FromAsync(async ar => await NodeRoutesApi.ActiveNodePeerMaxHeightAsync()).Select(e => ulong.Parse(e["height"].ToString()));
        }

        /// <summary>
        /// Gets extended node information.
        /// </summary>
        /// <returns>An IObservable of ExtendedNodeInfo.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>GetExtendedNodeInfo</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// ExtendedNodeInfo extendedNodeInfo = await new NodeHttp("<!--insert host like: http://0.0.0.0:7890-->").GetExtendedNodeInfo();
        /// </code>
        /// </example>
        public IObservable<ExtendedNodeInfo> GetExtendedNodeInfo()
        {
            return Observable.FromAsync(async ar => await NodeRoutesApi.ExtendedNodeInfoAsync()).Select(i =>
                new ExtendedNodeInfo(
                    new NodeInfoMapping().MapNode(i["node"]),
                    new NisInfo(
                        int.Parse(i["nisInfo"]["currentTime"].ToString()),
                        i["nisInfo"]["application"].ToString(),
                        int.Parse(i["nisInfo"]["startTime"].ToString()),
                        i["nisInfo"]["version"].ToString(),
                        i["nisInfo"]["signer"]?.ToString()
                    )));
        }

        /// <summary>
        /// Gets the node information.
        /// </summary>
        /// <returns>An IObservable of NodeInfo.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>GetNodeInfo</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// NodeInfo nodeInfo = await new NodeHttp("<!--insert host like: http://0.0.0.0:7890-->").GetNodeInfo();
        /// </code>
        /// </example>
        public IObservable<NodeInfo> GetNodeInfo()
        {
            return Observable.FromAsync(async ar => await NodeRoutesApi.NodeInfoAsync()).Select(i => new NodeInfoMapping().MapNode(i));
        }
    }
}
