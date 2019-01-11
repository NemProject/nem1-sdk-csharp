namespace io.nem1.sdk.Model.Transactions
{
    /// <summary>
    /// Class TransactionResponse.
    /// </summary>
    public class TransactionResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionResponse"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="code">The code.</param>
        /// <param name="status">The status.</param>
        /// <param name="message">The message.</param>
        /// <param name="hash">The hash.</param>
        /// <param name="innerHash">The inner hash.</param>
        public TransactionResponse(int type, int code, string message, string hash, string innerHash)
        {
            Type = type;
            Code = code;
            Message = message;
            Hash = hash;
            InnerHash = innerHash;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public int Type { get; }
        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>The code.</value>
        public int Code { get; }
        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; }
        /// <summary>
        /// Gets the hash.
        /// </summary>
        /// <value>The hash.</value>
        public string Hash { get; }
        /// <summary>
        /// Gets the inner hash.
        /// </summary>
        /// <value>The inner hash.</value>
        public string InnerHash { get; }
    }
}
