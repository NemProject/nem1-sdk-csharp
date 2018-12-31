using System.Linq;
using io.nem1.sdk.Core.Crypto.Chaso.NaCl;

namespace io.nem1.sdk.Model.Transactions.Messages
{
    /// <summary>
    /// Class HexMessage.
    /// </summary>
    /// <seealso cref="io.nem1.sdk.Model.Transactions.Messages.PlainMessage" />
    public class HexMessage : PlainMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HexMessage"/> class.
        /// </summary>
        /// <param name="payload">The payload.</param>
        internal HexMessage(byte[] payload) : base(payload) {}

        /// <summary>
        /// Gets the payload.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public override byte[] GetPayload()
        {
            return base.GetPayload().ToArray();
        }

        /// <summary>
        /// Gets the String payload.
        /// </summary>
        /// <returns>System.String.</returns>
        public override string GetStringPayload()
        {
            return base.GetPayload().Skip(1).ToArray().ToHexLower();
        }
    }
}
