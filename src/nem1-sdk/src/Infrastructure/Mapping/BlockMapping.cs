using System;
using System.Linq;
using io.nem1.sdk.Core.Crypto.Chaso.NaCl;
using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Blockchain;
using Newtonsoft.Json.Linq;

namespace io.nem1.sdk.Infrastructure.Mapping
{
    internal class BlockMapping
    {
        internal BlockInfo Apply(string blockData)
        {
            var jobject = JObject.Parse(blockData);

            return new BlockInfo
            {
                PreviousBlockHash = jobject["prevBlockHash"]["data"].ToString(),
                Height = ulong.Parse(jobject["height"].ToString()),
                Signer = new PublicAccount(jobject["signer"].ToString(), ExtractNetworkType(Int32.Parse(jobject["version"].ToString()))),
                Signature = jobject["signature"].ToString(),
                TimeStamp = int.Parse(jobject["timeStamp"].ToString()),
                Transactions = jobject["transactions"].ToList().Select(e => new TransactionMapping().Apply(e.ToString())).ToList(),
                Type = int.Parse(jobject["type"].ToString()),
                Version = ExtractVersion(int.Parse(jobject["version"].ToString())),
                Network = ExtractNetworkType(int.Parse(jobject["version"].ToString()))
                
            };
        }

        private int ExtractVersion(int version)
        {
            var netBytes = BitConverter.GetBytes(version).ToHexLower();
            return Convert.ToInt32(netBytes.Substring(0, 2), 16);
        }

        private NetworkType.Types ExtractNetworkType(int version)
        {
            var netBytes = BitConverter.GetBytes(version).ToHexLower();
            return NetworkType.GetRawValue(Convert.ToInt32(netBytes.Substring(4, 4), 16));
        }    
    }
}
