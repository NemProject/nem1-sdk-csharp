// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 11-07-2018
// ***********************************************************************
// <copyright file="BlockchainHttp.cs" company="Nem.io">   
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
using System.Reactive.Linq;
using io.nem1.sdk.Infrastructure.Imported.Api;
using io.nem1.sdk.Model.Blockchain;

namespace io.nem1.sdk.Infrastructure.HttpRepositories
{
    /// <summary>
    /// Blockchain Http Repository.
    /// </summary>
    /// <seealso cref="HttpRouter" />
    /// <seealso cref="IBlockchainRepository" />
    public class BlockchainHttp : HttpRouter, IBlockchainRepository
    {
        /// <summary>
        /// Gets the blockchain routes API.
        /// </summary>
        /// <value>The blockchain routes API.</value>
        private BlockchainRoutesApi BlockchainRoutesApi { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockchainHttp" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <exception cref="ArgumentException">Value cannot be null or whitespace. - host</exception>
        public BlockchainHttp(string host) 
            : this(host, new NodeHttp(host)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockchainHttp" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="networkHttp">The network HTTP.</param>
        /// <exception cref="ArgumentNullException">networkHttp</exception>
        /// <exception cref="ArgumentException">Value cannot be null or empty. - host</exception>
        public BlockchainHttp(string host, NodeHttp networkHttp) : base(host, networkHttp)
        {
            BlockchainRoutesApi = new BlockchainRoutesApi(Url);
        }


        /// <summary>
        /// Gets the blockchain score.
        /// </summary>
        /// <returns>IObservable&lt;BlockchainScore&gt;.</returns>
        /// /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>GetBlockchainScore</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// string score = await new BlockchainHttp("<!--insert host like: http://0.0.0.0:7890-->").GetBlockchainScore();
        /// </code>
        /// </example>
        public IObservable<string> GetBlockchainScore()
        {
            return Observable.FromAsync(async ar => await BlockchainRoutesApi.CurrentChainScoreAsync());
        }

        /// <summary>
        /// Gets a BlockInfo for the last block.
        /// </summary>
        /// <returns>An IObservable of BlockInfo.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>GetLastBlock</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// BlockInfo blockInfo = await new BlockchainHttp("<!--insert host like: http://0.0.0.0:7890-->").GetLastBlock();
        /// </code>
        /// </example>
        public IObservable<BlockInfo> GetLastBlock()
        {
            return Observable.FromAsync(async ar => await BlockchainRoutesApi.LastBlockAsync());
        }

        /// <summary>
        /// Gets the chain height.
        /// </summary>
        /// <returns>An IObservable of ulong</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>GetBlockchainHeight</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// ulong height = await new BlockchainHttp("<!--insert host like: http://0.0.0.0:7890-->").GetBlockchainHeight();
        /// </code>
        /// </example>
        public IObservable<ulong> GetBlockchainHeight()
        {
            return Observable.FromAsync(async ar => await BlockchainRoutesApi.CurrentChainHeightAsync());
        }

        /// <summary>
        /// Gets a BlockInfo for a given block height.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <returns>An IObservable of BlockInfo</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>GetBlockByHeight</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// BlockInfo blockInfo = await new BlockchainHttp("<!--insert host like: http://0.0.0.0:7890-->").GetBlockByHeight(1);
        /// </code>
        /// </example>
        public IObservable<BlockInfo> GetBlockByHeight(ulong height)
        {
            return Observable.FromAsync(async ar => await BlockchainRoutesApi.BlockAtHeightAsync(height));

        }
    }
}
