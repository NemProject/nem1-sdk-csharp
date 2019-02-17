using System.Collections.Generic;
using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Transactions;

namespace io.nem1.sdk.Model.Network
{
    /// <summary>
    /// BlockInfo 
    /// </summary>
    public class BlockInfo
    {
        /// <summary>
        /// Gets the time stamp as the network time when the block was created.
        /// </summary>
        /// <value>The time stamp.</value>
        public int TimeStamp { get; internal set; }

        /// <summary>
        /// Gets the block signature of the block. All blocks in the chain are signed by the harvesters.
        /// This way any node can check if the block has been altered by some evil entity.
        /// </summary>
        /// <value>The signature.</value>
        public string Signature { get; internal set; }

        /// <summary>
        /// Gets the previous block hash. The sha3-256 hash of the previous block as hexadecimal string.
        /// </summary>
        /// <value>The previous block hash.</value>
        public string PreviousBlockHash { get; internal set; }

        /// <summary>
        /// Gets the block type. 
        ///     -1: Nemesis block type. This block type appears only once in the chain.
        ///     1: Regular block type.All blocks with height > 1 have this type.
        /// </summary>
        /// <value>The type.</value>
        public int Type { get; internal set; }

        /// <summary>
        /// Gets the transactions contained in a block. A block can contain up to 120 transactions. 
        /// </summary>
        /// <value>The transactions.</value>
        public List<Transaction> Transactions { get; internal set; }

        /// <summary>
        /// Gets the block version.
        /// </summary>
        /// <value>The version.</value>
        public int Version { get; internal set; }

        /// <summary>
        /// Gets the block signer. The public key of the harvester of the block as hexadecimal string.
        /// </summary>
        /// <value>The signer.</value>
        public PublicAccount Signer { get; internal set; }

        /// <summary>
        /// Gets the block height. Each block has a unique height. Subsequent blocks differ in height by 1.
        /// </summary>
        /// <value>The height.</value>
        public ulong Height { get; internal set; }

        /// <summary>
        /// Gets the network type.
        /// </summary>
        /// <value>The network.</value>
        public NetworkType.Types Network { get; internal set; }

        /// <summary>
        /// BlockInfo Default Constructor
        /// </summary>
        public BlockInfo() { }

        /// <summary>
        /// BlockInfo Constructor
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="signature"></param>
        /// <param name="previousBlockHash"></param>
        /// <param name="type"></param>
        /// <param name="transactions"></param>
        /// <param name="version"></param>
        /// <param name="signer"></param>
        /// <param name="height"></param>
        public BlockInfo(
            int timestamp,
            string signature,
            string previousBlockHash,
            int type,
            List<Transaction> transactions,
            int version,
            PublicAccount signer,
            ulong height)
        {
            TimeStamp = timestamp;
            Signature = signature;
            PreviousBlockHash = previousBlockHash;
            Type = type;
            Transactions = transactions;
            Version = version;
            Signer = signer;
            Height = height;
        }
    }
}
