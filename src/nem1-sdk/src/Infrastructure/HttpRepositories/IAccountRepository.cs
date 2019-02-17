// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 11-07-2018
// ***********************************************************************
// <copyright file="IAccountRepository.cs" company="Nem.io">   
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
using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Mosaics;
using io.nem1.sdk.Model.Transactions;

namespace io.nem1.sdk.Infrastructure.HttpRepositories
{
    /// <summary>
    /// Interface IAccountRepository
    /// </summary>
    interface IAccountRepository
    {
        /// <summary>
        /// Gets the account information.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>IObservable&lt;AccountInfoDTO&gt;.</returns>
        IObservable<AccountInfo> GetAccountInfo(PublicAccount account);

        /// <summary>
        /// Gets the account information.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>IObservable&lt;AccountInfoDTO&gt;.</returns>
        IObservable<AccountInfo> GetAccountInfo(Address account);

        /// <summary>
        /// Get incoming transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <returns>IObservable list of type Transaction.</returns>
        IObservable<List<Transaction>> IncomingTransactions(Address account);

        /// <summary>
        /// Get incoming transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>IObservable list of type Transaction.</returns>
        IObservable<List<Transaction>> IncomingTransactions(Address account, TransactionQueryParams query);

        /// <summary>
        /// Get incoming transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <returns>IObservable list of type Transaction.</returns>
        IObservable<List<Transaction>> OutgoingTransactions(Address account);

        /// <summary>
        /// Get incoming transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>IObservable list of type Transaction.</returns>
        IObservable<List<Transaction>> OutgoingTransactions(Address account, TransactionQueryParams query);

        /// <summary>
        /// Get unconfirmed transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <returns>IObservable list of type Transaction.</returns>
        IObservable<List<Transaction>> UnconfirmedTransactions(Address account);

        /// <summary>
        /// Get all transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <returns>IObservable list of type Transaction.</returns>
        IObservable<List<Transaction>> Transactions(Address account);

        /// <summary>
        /// Get all transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>IObservable list of type Transaction.</returns>
        IObservable<List<Transaction>> Transactions(Address account, TransactionQueryParams query);

        /// <summary>
        /// Mosaicses the owned.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>IObservable&lt;List&lt;Mosaic&gt;&gt;.</returns>
        IObservable<List<MosaicAmount>> MosaicsOwned(PublicAccount account);

        /// <summary>
        /// Mosaicses the owned.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>IObservable&lt;List&lt;Mosaic&gt;&gt;.</returns>
        IObservable<List<MosaicAmount>> MosaicsOwned(Address account);
    }
}
