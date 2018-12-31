using System;
using System.Reactive.Linq;
using io.nem1.sdk.Infrastructure.Imported.Api;
using io.nem1.sdk.Model.Network;

namespace io.nem1.sdk.Infrastructure.HttpRepositories
{
    /// <summary>
    /// Nis Http Repository
    /// </summary>
    /// <seealso cref="io.nem1.sdk.Infrastructure.HttpRepositories.HttpRouter" />
    public class NisHttp : HttpRouter
    {
        // <summary>
        /// Gets or sets the network routes API.
        /// </summary>
        /// <value>The network routes API.</value>
        internal NisRoutesApi NisRoutesApi { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NisHttp" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <exception cref="ArgumentException">Value cannot be null or empty. - host</exception>
        public NisHttp(string host) : base(host)
        {
            NisRoutesApi = new NisRoutesApi(host);
        }

        /// <summary>
        /// Gets the nis status.
        /// </summary>
        /// <returns>An IObservableo of NisStatus.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>GetNisStatus</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// NisStatus status = await new NisHttp("<!--insert host like: http://0.0.0.0:7890-->").GetNisStatus("test");
        /// </code>
        /// </example>
        public IObservable<NisStatus> GetNisStatus()
        {
            return Observable.FromAsync(async ar => await NisRoutesApi.NisStatusAsync())
                .Select(e => new NisStatus((int) e["code"], (int) e["code"], (string) e["code"]));
        }

        /// <summary>
        /// Gets the nis heart beat.
        /// </summary>
        /// <returns>An IObservable of HeartBeat.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>GetNisHeartBeat</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// NisStatus status = await new NisHttp("<!--insert host like: http://0.0.0.0:7890-->").GetNisHeartBeat();
        /// </code>
        /// </example>
        public IObservable<HeartBeat> GetNisHeartBeat()
        {
            return Observable.FromAsync(async ar => await NisRoutesApi.NisHeartBeatAsync())
                .Select(e => new HeartBeat((int)e["code"], (int)e["code"], (string)e["code"]));
        }
    }
}
