// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="TransactionHttp.cs" company="Nem.io">
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
using io.nem1.sdk.Model.Network;
using io.nem1.sdk.Model.Transactions;

namespace io.nem1.sdk.Infrastructure.HttpRepositories
{
    /// <summary>
    /// Class TransactionHttp.
    /// </summary>
    /// <seealso cref="HttpRouter" />
    /// <seealso cref="ITransactionRepository" />
    /// <inheritdoc />
    /// <seealso cref="T:io.nem1.sdk.Infrastructure.HttpRepositories.HttpRouter" />
    /// <seealso cref="T:io.nem1.sdk.Infrastructure.HttpRepositories.ITransactionRepository" />
    public class TransactionHttp : HttpRouter, ITransactionRepository
    {
        /// <summary>
        /// Gets the transaction routes API.
        /// </summary>
        /// <value>The transaction routes API.</value>
        private TransactionRoutesApi TransactionRoutesApi { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionHttp" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <exception cref="ArgumentException">Value cannot be null or empty. - host</exception>
        public TransactionHttp(string host) 
            : this(host, new NodeHttp(host)) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionHttp" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="networkHttp">The network HTTP.</param>
        /// <exception cref="ArgumentNullException">networkHttp</exception>
        /// <exception cref="ArgumentException">Value cannot be null or empty. - host</exception>
        public TransactionHttp(string host, NodeHttp networkHttp) : base(host, networkHttp)
        {
            TransactionRoutesApi = new TransactionRoutesApi(host);
        }

        /// <summary>
        /// Announces the transaction.
        /// </summary>
        /// <param name="signedTransaction">The signed transaction.</param>
        /// <returns>An IObservableof TransactionAnnounceResponse.</returns>
        /// <exception cref="ArgumentNullException">signedTransaction</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>Announce</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// 
        ///  var account = new Account("<!--insert private key-->", NetworkType.Types.TEST_NET);
        /// 
        ///  var transaction = TransferTransaction.Create(
        ///      NetworkType.Types.TEST_NET,
        ///      Deadline.CreateHours(2),
        ///      Address.CreateFromEncoded(""<!--insert address-->""),
        ///      new List<Mosaic> { Mosaic.CreateFromIdentifier("nem:xem", 1000000)},
        ///      PlainMessage.Create("hello")
        ///  ).SignWith(keyPair);
        /// 
        /// TransactionResponse response = await new TransactionHttp("<!--insert host like: http://0.0.0.0:7890-->").Announce(transaction);
        /// </code>
        /// </example>
        public IObservable<TransactionResponse> Announce(SignedTransaction signedTransaction)
        {
            if (signedTransaction == null) throw new ArgumentNullException(nameof(signedTransaction));

            return Observable.FromAsync(async ar => await TransactionRoutesApi.SendTransactionAsync(signedTransaction.TransactionPacket));
        }

    }
}
