using System.Collections.Generic;

namespace io.nem1.sdk.Model.Network
{
    /// <summary>
    /// Class PeerListInfo.
    /// </summary>
    public class PeerListInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PeerListInfo"/> class.
        /// </summary>
        /// <param name="active">The active.</param>
        /// <param name="inactive">The inactive.</param>
        /// <param name="busy">The busy.</param>
        /// <param name="failure">The failure.</param>
        public PeerListInfo(List<NodeInfo> active, List<NodeInfo> inactive, List<NodeInfo> busy, List<NodeInfo> failure)
        {
            Active = active;
            Inactive = inactive;
            Busy = busy;
            Failure = failure;
        }

        /// <summary>
        /// Gets the active.
        /// </summary>
        /// <value>The active.</value>
        public List<NodeInfo> Active { get; }
        /// <summary>
        /// Gets the inactive.
        /// </summary>
        /// <value>The inactive.</value>
        public List<NodeInfo> Inactive { get; }
        /// <summary>
        /// Gets the busy.
        /// </summary>
        /// <value>The busy.</value>
        public List<NodeInfo> Busy { get; }
        /// <summary>
        /// Gets the failure.
        /// </summary>
        /// <value>The failure.</value>
        public List<NodeInfo> Failure { get; }
    }
}
