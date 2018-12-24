using io.nem1.sdk.Model.Network;

namespace io.nem1.sdk.Model.Node
{
    /// <summary>
    /// Class NisInfo.
    /// </summary>
    public class NisInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NisInfo"/> class.
        /// </summary>
        /// <param name="currentTimeStamp">The current timestamp.</param>
        /// <param name="application">The application.</param>
        /// <param name="startTimeStamp">The start timestamp.</param>
        /// <param name="version">The version.</param>
        /// <param name="signer">The signer.</param>
        public NisInfo(int currentTimeStamp, string application, int startTimeStamp, string version, string signer)
        {
            CurrentTime = new NetworkTime(currentTimeStamp);
            Application = application;
            StartTime = new NetworkTime(startTimeStamp);
            Version = version;
            Signer = signer;
        }

        /// <summary>
        /// Gets the current time of the NIS.
        /// </summary>
        /// <value>The current time.</value>
        public NetworkTime CurrentTime { get; }
        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <value>The application.</value>
        public string Application { get; }
        /// <summary>
        /// Gets the start time of the NIS.
        /// </summary>
        /// <value>The start time.</value>
        public NetworkTime StartTime { get; }
        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version { get; }
        /// <summary>
        /// Gets the signer.
        /// </summary>
        /// <value>The signer.</value>
        public string Signer { get; }
    }
}
