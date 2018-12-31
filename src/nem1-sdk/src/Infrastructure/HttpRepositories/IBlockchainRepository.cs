// ***********************************************************************
// Assembly         : nem1-sdk
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 01-31-2018
// ***********************************************************************
// <copyright file="IBlockchainRepository.cs" company="Nem.io">
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
using io.nem1.sdk.Model.Network;

namespace io.nem1.sdk.Infrastructure.HttpRepositories
{
    /// <summary>
    /// Interface IBlockchainRepository
    /// </summary>
    interface IBlockchainRepository
    {

        /// <summary>
        /// Gets the blockchain score.
        /// </summary>
        /// <returns>IObservable&lt;BlockchainScore&gt;.</returns>
        IObservable<string> GetBlockchainScore();

        /// <summary>
        /// Gets the blockchain score.
        /// </summary>
        /// <returns>IObservable&lt;BlockchainScore&gt;.</returns>
        IObservable<BlockInfo> GetLastBlock();
        /// <summary>
        /// Gets the chain height.
        /// </summary>
        /// <returns>An IObservable of ChainHeightDTO</returns>
        IObservable<ulong> GetBlockchainHeight();

        /// <summary>
        /// Gets a BlockInfo for a given block height.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <returns>An IObservable of BlockInfoDTO</returns>
        IObservable<BlockInfo> GetBlockByHeight(ulong height);

    }
}
