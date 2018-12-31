// ***********************************************************************
// Assembly         : nem1-sdk
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 11-07-2018
// ***********************************************************************
// <copyright file="ITransactionRepository.cs" company="Nem.io">   
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
using io.nem1.sdk.Model.Transactions;

namespace io.nem1.sdk.Infrastructure.HttpRepositories
{
    /// <summary>
    /// Interface ITransactionRepository
    /// </summary>
    interface ITransactionRepository
    {
        /// <summary>
        /// Announces the transaction.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <returns>IObservable&lt;TransactionAnnounceResponse&gt;.</returns>
        IObservable<TransactionResponse> Announce(SignedTransaction payload);

        /// <summary>
        /// Gets a transaction for the provided hash.
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <returns>IObservable Transaction.</returns>
        IObservable<Transaction> GetTransaction(string hash);

    }
}
