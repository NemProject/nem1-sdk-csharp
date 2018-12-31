using System;
using System.Collections.Generic;
using System.Linq;
using io.nem1.sdk.Model.Network;
using Newtonsoft.Json.Linq;

namespace io.nem1.sdk.Infrastructure.Mapping
{
    internal class NodeInfoMapping
    {
        internal List<NodeInfo> MapNodes(List<JObject> nodes)
        {
            return nodes.Select(MapNode).ToList();
        }

        internal NodeInfo MapNode(JToken i)
        {
            
            return new NodeInfo(
                int.Parse(i["metaData"]["features"].ToString()),
                i["metaData"]["application"]?.ToString(),
                NetworkType.GetRawValue((byte)Convert.ToInt32(i["metaData"]["networkId"].ToString())),
                i["metaData"]["version"].ToString(),
                i["metaData"]["platform"].ToString(),
                i["endpoint"]["protocol"].ToString(),
                i["endpoint"]["port"].ToString(),
                i["endpoint"]["host"].ToString(),
                i["identity"]["public-key"].ToString(),
                i["identity"]["name"].ToString());
        }
    }
}
