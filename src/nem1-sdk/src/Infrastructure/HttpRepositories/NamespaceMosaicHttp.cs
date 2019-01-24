// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 11-07-2018
// ***********************************************************************
// <copyright file="MosaicHttp.cs" company="Nem.io">   
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
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using io.nem1.sdk.Infrastructure.Imported.Api;
using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Mosaics;
using io.nem1.sdk.Model.Namespace;
using io.nem1.sdk.Model.Network;
using Newtonsoft.Json.Linq;

namespace io.nem1.sdk.Infrastructure.HttpRepositories
{
    /// <summary>
    /// Mosaic Http Repository.
    /// </summary>
    /// <seealso cref="HttpRouter" />
    /// <seealso cref="IMosaicRepository" />
    /// <seealso cref="HttpRouter" />
    /// <seealso cref="IMosaicRepository" />
    public class NamespaceMosaicHttp : HttpRouter //, IMosaicRepository
    {
        /// <summary>
        /// Gets or sets the mosaic routes API.
        /// </summary>
        /// <value>The mosaic routes API.</value>
        private NamespaceMosaicRoutesApi NamespaceMosaicRoutesApi { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceMosaicHttp" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <exception cref="ArgumentException">Value cannot be null or empty. - host</exception>
        public NamespaceMosaicHttp(string host) 
            : this(host, new NodeHttp(host)) { }


        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceMosaicHttp" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="networkHttp">The network HTTP.</param>
        /// <exception cref="ArgumentNullException">networkHttp</exception>
        /// <exception cref="ArgumentException">Value cannot be null or empty. - host</exception>
        public NamespaceMosaicHttp(string host, NodeHttp networkHttp) : base(host, networkHttp)
        {
            NamespaceMosaicRoutesApi = new NamespaceMosaicRoutesApi(host);
        }

        /// <summary>
        /// Gets the namespace information for a given namespace.
        /// </summary>
        /// <param name="nameSpace">The name space.</param>
        /// <returns>An IObservable of NamespaceInfo.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>NamespaceInfo</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// NamespaceInfo namespaceInfo = await new NamespaceMosaicHttp("<!--insert host like: http://0.0.0.0:7890-->").NamespaceInfo("test");
        /// </code>
        /// </example>
        public IObservable<NamespaceInfo> NamespaceInfo(string nameSpace)
        {
            return Observable.FromAsync(async ar => await NamespaceMosaicRoutesApi.NamespaceInfoAsync(nameSpace))
                .Select(e => 
                    new NamespaceInfo(
                        e["fqn"].ToString(), 
                        ulong.Parse(e["height"].ToString()), 
                        new Address(e["owner"].ToString()))
                    );
        }

        /// <summary>
        /// Gets newest root namespaces registered.
        /// </summary>
        /// <returns>An IObservable List of NamespaceRootInfo.</returns>
        ///  <example> 
        /// This sample shows how to use the <see>
        ///         <cref>NamespaceRootInfoPage</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// List&lt;NamespaceRootInfo&gt; namespaceInfo = await new NamespaceMosaicHttp("<!--insert host like: http://0.0.0.0:7890-->").NamespaceRootInfoPage();
        /// </code>
        /// </example>
        public IObservable<List<NamespaceRootInfo>> NamespaceRootInfoPage()
        {
            return NamespaceRootInfoPage(null, 25);
        }

        /// <summary>
        /// Gets root namespaces registered up to the provided database id.
        /// </summary>
        /// <param name="id">The database id for the namespace upto which namespaces should be returned.</param>
        /// <returns>An IObservable List of NamespaceRootInfo.</returns>
        ///  <example> 
        /// This sample shows how to use the <see>
        ///         <cref>NamespaceRootInfoPage</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// List&lt;NamespaceRootInfo&gt; namespaceInfo = await new NamespaceMosaicHttp("<!--insert host like: http://0.0.0.0:7890-->").NamespaceInfo("test");
        /// </code>
        /// </example>
        public IObservable<List<NamespaceRootInfo>> NamespaceRootInfoPage(string id)
        {
            return NamespaceRootInfoPage(id, 25);
        }

        /// <summary>
        /// Gets root namespaces registered up to the provided database id limited to the page size.
        /// </summary>
        /// <param name="id">The database id for the namespace upto which namespaces should be returned.</param>
        /// <param name="pageSize">The number of namespaces to return.</param>
        /// <returns>An IObservable List of NamespaceRootInfo.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>NamespaceRootInfoPage</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// List&lt;NamespaceRootInfo&gt; namespaceInfo = await new NamespaceMosaicHttp("<!--insert host like: http://0.0.0.0:7890-->").NamespaceInfo("test", 10);
        /// </code>
        /// </example>
        public IObservable<List<NamespaceRootInfo>> NamespaceRootInfoPage(string id, int pageSize)
        {
            return Observable.FromAsync(async ar => await NamespaceMosaicRoutesApi.NamespaceRootInfoAsync(id, pageSize))
                .Select(e =>  e["data"].ToObject<List<JObject>>()
                        .Select(i =>
                            new NamespaceRootInfo(
                                int.Parse(i["meta"]["id"].ToString()),
                                i["namespace"]["fqn"].ToString(),
                                ulong.Parse(i["namespace"]["height"].ToString()),
                                new Address(i["namespace"]["owner"].ToString()))
                        ).ToList());
        }

        /// <summary>
        /// Gets mosaic info for a given namespace.
        /// </summary>
        /// <param name="nameSpace">The name space.</param>
        /// <returns>An IObservable List of MosaicInfo.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>GetNamespaceMosaics</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// List&lt;MosaicInfo&gt; namespaceInfo = await new NamespaceMosaicHttp("<!--insert host like: http://0.0.0.0:7890-->").GetNamespaceMosaics("test");
        /// </code>
        /// </example>
        public IObservable<List<MosaicInfo>> GetNamespaceMosaics(string nameSpace)
        {
            return GetNamespaceMosaics(nameSpace, null, 25);
        }

        /// <summary>
        /// Gets mosaic info for a given namespace up to the provided database id.
        /// </summary>
        /// <param name="nameSpace">The name space.</param>
        /// <param name="id">The database id of the namespace..</param>
        /// <returns>An IObservable List of MosaicInfo.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>GetNamespaceMosaics</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// List&lt;MosaicInfo&gt; namespaceInfo = await new NamespaceMosaicHttp("<!--insert host like: http://0.0.0.0:7890-->").GetNamespaceMosaics("test", "xyz");
        /// </code>
        /// </example>
        public IObservable<List<MosaicInfo>> GetNamespaceMosaics(string nameSpace, string id)
        {
            return GetNamespaceMosaics(nameSpace, id, 25);
        }

        /// <summary>
        /// Gets mosaic info for a given namespace up to the provided database id with a given page size.
        /// </summary>
        /// <param name="nameSpace">The name space.</param>
        /// <param name="id">The database id of the namespace.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>An IObservable List of MosaicInfo.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>GetNamespaceMosaics</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// LList&lt;MosaicInfo&gt; namespaceInfo = await new NamespaceMosaicHttp("<!--insert host like: http://0.0.0.0:7890-->").GetNamespaceMosaics("test", "xyz", 10);
        /// </code>
        /// </example>
        public IObservable<List<MosaicInfo>> GetNamespaceMosaics(string nameSpace, string id, int pageSize)
        {
            return Observable.FromAsync(async ar => await NamespaceMosaicRoutesApi.NamespaceMosaicInfoAsync(nameSpace, id, pageSize))
               
                .Select(e =>
                    e.Select(i => new MosaicInfo(
                        int.Parse(i["meta"]["id"].ToString()),
                        new PublicAccount(i["mosaic"]["creator"].ToString(), GetNetworkTypeObservable().Wait()),
                        i["mosaic"]["description"].ToString(),
                        MosaicId.CreateFromMosaicIdentifier(i["mosaic"]["id"]["namespaceId"].ToString() + ":" + i["mosaic"]["id"]["name"].ToString()),
                        new MosaicProperties(
                            int.Parse(i["mosaic"]["properties"].ToList()[0]["value"].ToString()), 
                            ulong.Parse(i["mosaic"]["properties"].ToList()[1]["value"].ToString()),
                            bool.Parse(i["mosaic"]["properties"].ToList()[2]["value"].ToString()),
                            bool.Parse(i["mosaic"]["properties"].ToList()[3]["value"].ToString())),
                        null)).ToList()                
                );
        }
    } 
}
