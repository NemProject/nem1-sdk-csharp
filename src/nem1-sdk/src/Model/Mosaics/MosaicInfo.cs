using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Network;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace io.nem1.sdk.Model.Mosaics
{
    /// <summary>
    /// MosaicInfo.
    /// </summary>
    public class MosaicInfo : MosaicId
    {
        /// <summary>
        /// 
        /// </summary>
        public static string XemPubKey { get; set; }        // Is different on different NetworkType's

        /// <summary>
        /// 
        /// </summary>
        public static string XemDescription { get; set; }   // Is different on different NetworkType's

        /// <summary>
        /// Divisibility
        /// </summary>
        public const int XemDivisibility = 6;

        /// <summary>
        /// Initial supply
        /// </summary>
        public const ulong XemInitialSupply = 8999999999;

        /// <summary>
        /// Is supply mutable
        /// </summary>
        public const bool XemIsSupplyMutable = false;

        /// <summary>
        /// Is transferable
        /// </summary>
        public const bool XemIsTransferable = true;

        /// <summary>
        /// Represents the default predefined nem:xem Mosaic
        /// </summary>
        public static MosaicInfo Xem = new MosaicInfo(  XemPubKey,
                                                        XemDescription,
                                                        MosaicId.NEM,
                                                        MosaicId.XEM,
                                                        XemInitialSupply,   // Supply is not mutable, so it cannot have changed.
                                                        new MosaicProperties(XemDivisibility, XemInitialSupply, XemIsSupplyMutable, XemIsTransferable),
                                                        null
                                                      );

        /// <summary>
        /// Gets the creator.
        /// </summary>
        /// <value>The creator.</value>
        public string CreatorPubKey { get; }
        
        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public ulong Supply { get; }

        /// <summary>
        /// Gets the mosaic properties.
        /// </summary>
        /// <value>The properties.</value>
        public MosaicProperties Properties { get; }
        
        /// <summary>
        /// Gets the levy.
        /// </summary>
        /// <value>The levy.</value>
        public MosaicLevy Levy { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MosaicInfo"/> class.
        /// </summary>
        /// <param name="oMosaicInfo"></param>
        public MosaicInfo(JObject oMosaicInfo, ulong supply)
            : this(oMosaicInfo["creator"].ToString(),
                    oMosaicInfo["description"].ToString(),
                    oMosaicInfo["id"]["namespaceId"].ToString(), 
                    oMosaicInfo["id"]["name"].ToString(),
                    supply,
                    new MosaicProperties(
                        int.Parse(oMosaicInfo["properties"].ToList()[0]["value"].ToString()),
                        ulong.Parse(oMosaicInfo["properties"].ToList()[1]["value"].ToString()),
                        bool.Parse(oMosaicInfo["properties"].ToList()[2]["value"].ToString()),
                        bool.Parse(oMosaicInfo["properties"].ToList()[3]["value"].ToString())
                    ),
                    oMosaicInfo["levy"].ToString() == "{}" ? null : new MosaicLevy(oMosaicInfo["levy"])
                  )
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MosaicInfo"/> class.
        /// </summary>
        /// <param name="creatorPubKey"></param>
        /// <param name="description"></param>
        /// <param name="namespaceId"></param>
        /// <param name="mosaicName"></param>
        /// <param name="supply"></param>
        /// <param name="properties"></param>
        /// <param name="levy"></param>
        public MosaicInfo(string creatorPubKey, string description, string namespaceId, string mosaicName, ulong supply, MosaicProperties properties, MosaicLevy levy)
            : base(namespaceId, mosaicName)
        {
            CreatorPubKey = creatorPubKey;
            Description = description;
            Supply = supply;
            Properties = properties;
            Levy = levy;
        }

        public MosaicInfo(string namespaceId, string mosaicName) : base(namespaceId, mosaicName) { }
        public MosaicInfo(string fullName) : base(fullName) { }

    }
}
