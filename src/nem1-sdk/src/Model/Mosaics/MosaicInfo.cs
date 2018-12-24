using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Network;

namespace io.nem1.sdk.Model.Mosaics
{
    /// <summary>
    /// MosaicInfo.
    /// </summary>
    public class MosaicInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MosaicInfo"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="creator">The creator.</param>
        /// <param name="description">The description.</param>
        /// <param name="mosaicId">The mosaic identifier.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="levy">The levy.</param>
        public MosaicInfo(int id, PublicAccount creator, string description, MosaicId mosaicId, MosaicProperties properties, Mosaic levy)
        {
            Id = id;
            Creator = creator;
            Description = description;
            MosaicId = mosaicId;
            Properties = properties;
            Levy = levy;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; }
        /// <summary>
        /// Gets the creator.
        /// </summary>
        /// <value>The creator.</value>
        public PublicAccount Creator { get; }
        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; }
        /// <summary>
        /// Gets the mosaic identifier.
        /// </summary>
        /// <value>The mosaic identifier.</value>
        public MosaicId MosaicId { get; }
        /// <summary>
        /// Gets the mosaic properties.
        /// </summary>
        /// <value>The properties.</value>
        public MosaicProperties Properties { get; }
        /// <summary>
        /// Gets the levy.
        /// </summary>
        /// <value>The levy.</value>
        public Mosaic Levy { get; }
    }
}
