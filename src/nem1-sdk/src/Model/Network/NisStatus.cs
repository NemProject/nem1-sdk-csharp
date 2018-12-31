namespace io.nem1.sdk.Model.Network
{
    /// <summary>
    /// Class NisStatus.
    /// </summary>
    public class NisStatus
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NisStatus"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        public NisStatus(int code, int type, string message)
        {
            Code = code;
            Type = type;
            Message = message;
        }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>The code.</value>
        public int Code { get; }
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public int Type { get; }
        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; }
    }
}
