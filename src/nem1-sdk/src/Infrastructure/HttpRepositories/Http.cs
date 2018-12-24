// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 11-07-2018
// ***********************************************************************
// <copyright file="Http.cs" company="Nem.io">   
// Copyright 2018 NEM
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Net;
using System.Reactive.Linq;
using io.nem1.sdk.Model.Blockchain;

namespace io.nem1.sdk.Infrastructure.HttpRepositories
{
    /// <exclude />
    public abstract class HttpRouter
    {
        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        protected WebClient Client { get; }
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        internal string Url { get;}
        /// <summary>
        /// Gets or sets the type of the network.
        /// </summary>
        /// <value>The type of the network.</value>
        private NetworkType.Types _NetworkType { get; set; }
        /// <summary>
        /// Gets or sets the network HTTP.
        /// </summary>
        /// <value>The network HTTP.</value>
        private NodeHttp NodeHttp { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRouter"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="networkHttp">The network HTTP.</param>
        protected HttpRouter(string host, NodeHttp networkHttp)
        {
            if (string.IsNullOrEmpty(host)) throw new ArgumentException("Value cannot be null or empty.", nameof(host));

            NodeHttp = networkHttp ?? throw new ArgumentNullException(nameof(networkHttp));

            Url = host;
            
            Client = new WebClient();             
        }

        /// <summary>
        /// Gets the HTTP instance.
        /// </summary>
        /// <returns>HttpRouter.</returns>
        internal HttpRouter GetHttp()
        {
            return this;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRouter"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        protected HttpRouter(string host)
        {
            if (string.IsNullOrEmpty(host)) throw new ArgumentException("Value cannot be null or empty.", nameof(host));

            Url = host;

            Client = new WebClient();
        }

        /// <summary>
        /// Gets the network type observable.
        /// </summary>
        /// <returns>IObservable&lt;NetworkType.Types&gt;.</returns>
        internal IObservable<NetworkType.Types> GetNetworkTypeObservable()
        {
            if (_NetworkType != 0x00) return Observable.Return(_NetworkType);

            var typeObservable = Observable.FromAsync(async ar =>
                await NodeHttp.GetNetworkType());

            typeObservable.Subscribe(ar =>
            {
                _NetworkType = ar;
            });

            return typeObservable;          
        }
    }
}
