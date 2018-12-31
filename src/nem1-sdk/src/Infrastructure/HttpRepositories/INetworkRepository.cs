// ***********************************************************************
// Assembly         : nem1-sdk
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 11-07-2018
// ***********************************************************************
// <copyright file="INetworkRepository.cs" company="Nem.io">   
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
    /// Interface INetworkRepository
    /// </summary>
    interface INetworkRepository
    {
        /// <summary>
        /// Gets the type of the network.
        /// </summary>
        /// <returns>IObservable&lt;NetworkTypeDTO&gt;.</returns>
        IObservable<NetworkType.Types> GetNetworkType();
    }
}
