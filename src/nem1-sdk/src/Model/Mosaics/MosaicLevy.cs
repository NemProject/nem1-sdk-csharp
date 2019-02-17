using io.nem1.sdk.Model.Accounts;
using Newtonsoft.Json.Linq;

namespace io.nem1.sdk.Model.Mosaics
{
    /// <summary>
    /// Class MosaicLevy.
    /// </summary>
    public class MosaicLevy : MosaicId
    {
        /// <summary>
        /// Gets the Amount of the Fee.
        /// </summary>
        public ulong Fee { get; }

        /// <summary>
        /// Gets the levy recipient.
        /// </summary>
        /// <value>The levy recipient.</value>
        public Address Recipient { get; }

        /// <summary>
        /// Gets the type of the fee.
        /// </summary>
        /// <value>The type of the fee.</value>
        public int FeeType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MosaicLevy"/> class.
        /// </summary>
        /// <param name="oMosaicLevy"></param>
        public MosaicLevy(JToken oMosaicLevy) : this(   oMosaicLevy["mosaicId"]["namespaceId"].ToString(), 
                                                        oMosaicLevy["mosaicId"]["name"].ToString(),
                                                        ulong.Parse(oMosaicLevy["fee"].ToString()), 
                                                        new Address(oMosaicLevy["recipient"].ToString()),
                                                        int.Parse(oMosaicLevy["type"].ToString())
                                                ) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MosaicLevy"/> class.
        /// </summary>
        /// <param name="namespaceId"></param>
        /// <param name="mosaicName"></param>
        /// <param name="fee"></param>
        /// <param name="recipient"></param>
        /// <param name="feeType"></param>
        public MosaicLevy(string namespaceId, string mosaicName, ulong fee, Address recipient, int feeType) : base(namespaceId, mosaicName)
        {
            Fee = fee;
            Recipient = recipient;
            FeeType = feeType;
        }

    }
}
