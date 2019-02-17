using io.nem1.sdk.Model.Accounts;
using Newtonsoft.Json.Linq;

namespace io.nem1.sdk.Model.Namespace
{
    /// <summary>
    /// NamespaceInfo.
    /// </summary>
    public class NamespaceInfo
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string NamespaceId { get; internal set; }
        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public ulong Height { get; internal set; }
        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public Address Owner { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceInfo"/> class.
        /// </summary>
        /// <param name="oNsInfo"> A JObject with the parsed NamespaceInfo JSON</param>
        public NamespaceInfo(JObject oNsInfo) 
            : this(oNsInfo["fqn"].ToString(), ulong.Parse(oNsInfo["height"].ToString()), new Address(oNsInfo["owner"].ToString())) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceInfo"/> class.
        /// </summary>
        /// <param name="fqn">The FQN.</param>
        /// <param name="height">The height.</param>
        /// <param name="owner">The owner.</param>
        public NamespaceInfo(string fqn, ulong height, Address owner)
        {
            NamespaceId = fqn;
            Height = height;
            Owner = owner;
        }

    }
}
