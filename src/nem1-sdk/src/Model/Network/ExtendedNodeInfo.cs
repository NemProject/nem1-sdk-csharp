namespace io.nem1.sdk.Model.Network
{
    /// <summary>
    /// Class ExtendedNodeInfo.
    /// </summary>
    public class ExtendedNodeInfo
    {
        /// <summary>
        /// Gets the node information.
        /// </summary>
        /// <value>The node information.</value>
        public NodeInfo NodeInfo { get; }
        /// <summary>
        /// Gets the nis information.
        /// </summary>
        /// <value>The nis information.</value>
        public NisInfo NisInfo { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedNodeInfo"/> class.
        /// </summary>
        /// <param name="nodeInfo">The node information.</param>
        /// <param name="nisInfo">The nis information.</param>
        public ExtendedNodeInfo(NodeInfo nodeInfo, NisInfo nisInfo)
        {
            NodeInfo = nodeInfo;
            NisInfo = nisInfo;
        }
    }
}
