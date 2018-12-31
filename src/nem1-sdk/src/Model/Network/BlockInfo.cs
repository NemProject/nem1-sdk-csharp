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
        /// Gets the network type.
        /// </summary>
        /// <value>The network.</value>
        public NetworkType.Types Network { get; internal set; }

        /// <summary>
        /// Gets the time stamp.
        /// </summary>
        /// <value>The time stamp.</value>
        public int TimeStamp { get; internal set; }

        /// <summary>
        /// Gets the block signature.
        /// </summary>
        /// <value>The signature.</value>
        public string Signature { get; internal set; }

        /// <summary>
        /// Gets the previous block hash.
        /// </summary>
        /// <value>The previous block hash.</value>
        public string PreviousBlockHash { get; internal set; }

        /// <summary>
        /// Gets the block type.
        /// </summary>
        /// <value>The type.</value>
        public int Type { get; internal set; }

        /// <summary>
        /// Gets the transactions contained in a block.
        /// </summary>
        /// <value>The transactions.</value>
        public List<Transaction> Transactions { get; internal set; }

        /// <summary>
        /// Gets the block version.
        /// </summary>
        /// <value>The version.</value>
        public int Version { get; internal set; }

        /// <summary>
        /// Gets the block signer.
        /// </summary>
        /// <value>The signer.</value>
        public PublicAccount Signer { get; internal set; }

        /// <summary>
        /// Gets the block height.
        /// </summary>
        /// <value>The height.</value>
        public ulong Height { get; internal set; }
    }
}
