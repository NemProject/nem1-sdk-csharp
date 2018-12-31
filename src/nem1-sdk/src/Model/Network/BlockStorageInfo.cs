namespace io.nem1.sdk.Model.Network
{
    public class BlockchainStorageInfo
    {
        /// <summary>
        /// Returns the number of accounts published in the blockchain
        /// </summary>
        /// <value>The number accounts.</value>
        public int NumAccounts { get; }

        /// <summary>
        /// Returns number of confirmed blocks in the blockchain.
        /// </summary>
        /// <value>The number blocks.</value>
        public int NumBlocks { get; }

        /// <summary>
        /// Returns number of confirmed transactions in the blockchain history.
        /// </summary>
        /// <value>The number transactions.</value>
        public int NumTransactions { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockchainStorageInfo"/> class.
        /// </summary>
        /// <param name="numAccounts">The number accounts.</param>
        /// <param name="numBlocks">The number blocks.</param>
        /// <param name="numTransactions">The number transactions.</param>
        public BlockchainStorageInfo(int numAccounts, int numBlocks, int numTransactions)
        {
            NumAccounts = numAccounts;
            NumBlocks = numBlocks;
            NumTransactions = numTransactions;
        }
    }
}
