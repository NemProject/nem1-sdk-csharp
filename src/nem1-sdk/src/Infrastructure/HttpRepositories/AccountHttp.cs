// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="AccountHttp.cs" company="Nem.io">
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
using io.nem1.sdk.Model.Transactions;

namespace io.nem1.sdk.Infrastructure.HttpRepositories
{
    /// <summary>
    /// Account Http Repository.
    /// </summary>
    /// <seealso cref="HttpRouter" />
    /// <seealso cref="IAccountRepository" />
    public class AccountHttp : HttpRouter, IAccountRepository
    {
       
        private AccountRoutesApi AccountRoutesApi { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountHttp" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <exception cref="ArgumentException">Value cannot be null or empty. - host</exception>
        public AccountHttp(string host) 
            : this(host, new NodeHttp(host)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountHttp" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="networkHttp">The network HTTP.</param>
        /// <exception cref="ArgumentException">Value cannot be null or empty. - host</exception>
        public AccountHttp(string host, NodeHttp networkHttp) : base(host, networkHttp)
        {     
            AccountRoutesApi = new AccountRoutesApi(host);
        }

        /// <summary>
        /// Get account infomration.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>An  <see cref="IObservable"/> of <see cref="AccountInfo "/></returns>
        /// <exception cref="System.ArgumentNullException">address</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>GetAccountInfo</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// var accountHttp = new AccountHttp("<!--insert host like: http://0.0.0.0:7890-->");
        /// 
        /// var address = Address.CreateFromEncoded("TCTUIF-557ZCQ-OQPW2M-6GH4TC-DPM2ZY-BBL54K-GNHR");
        /// 
        /// var accountInfo = await accountHttp.GetAccountInfo(address);
        /// </code>
        /// </example>
        public IObservable<AccountInfo> GetAccountInfo(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            return Observable.FromAsync(async ar => await AccountRoutesApi.GetAccountInfoAsync(address.Plain))
                .Select(accountInfo =>
                {
                    return new AccountInfo(
                        Address.CreateFromEncoded(accountInfo["account"]["address"].ToString()),
                        accountInfo["account"]["publicKey"].ToString(),
                        ulong.Parse(accountInfo["account"]["importance"].ToString()),
                        ulong.Parse(accountInfo["account"]["harvestedBlocks"].ToString()),
                        ulong.Parse(accountInfo["account"]["vestedBalance"].ToString()),
                        ulong.Parse(accountInfo["account"]["balance"].ToString()),
                        new MultisigAccountInfo(
                            PublicAccount.CreateFromPublicKey(
                                accountInfo["account"]["publicKey"].ToString(), new NodeHttp(Url).GetNetworkType().Wait()),
                            accountInfo["meta"]["cosignatories"]?.Select(accountInfo2 => new AccountInfo(
                                Address.CreateFromEncoded(accountInfo["address"].ToString()),
                                accountInfo["publicKey"].ToString(),
                                ulong.Parse(accountInfo["importance"].ToString()),
                                ulong.Parse(accountInfo["harvestedBlocks"].ToString()),
                                ulong.Parse(accountInfo["vestedBalance"].ToString()),
                                ulong.Parse(accountInfo["balance"].ToString()),
                                null)).ToList(),
                            accountInfo["meta"]["cosignatoryOf"]?.Select(accountInfo2 => new AccountInfo(
                                Address.CreateFromEncoded(accountInfo["address"].ToString()),
                                accountInfo["publicKey"].ToString(),
                                ulong.Parse(accountInfo["importance"].ToString()),
                                ulong.Parse(accountInfo["harvestedBlocks"].ToString()),
                                ulong.Parse(accountInfo["vestedBalance"].ToString()),
                                ulong.Parse(accountInfo["balance"].ToString()),
                                null)).ToList()
                        ));
                });
        }

        /// <summary>
        /// Get account information
        /// </summary>
        /// <param name="account">The public account.</param>
        /// <returns>An  <see cref="IObservable"/> of <see cref="AccountInfo "/></returns>
        /// <exception cref="System.ArgumentNullException">address</exception>
        /// /// /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>GetAccountInfo</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// var accountHttp = new AccountHttp("<!--insert host like: http://0.0.0.0:7890-->");
        /// 
        /// var publicAccount = new PublicAccount("856f39436e33129afff95b89aca998fa23cd751a6f4d79ce4fb9da9641ecb59c", NetworkType.Types.TEST_NET);
        /// 
        /// var accountInfo = await accountHttp.GetAccountInfo(publicAccount);
        /// </code>
        /// </example>
        public IObservable<AccountInfo> GetAccountInfo(PublicAccount account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));

            return Observable.FromAsync(async ar => await AccountRoutesApi.GetAccountInfoFromPublicKeyAsync(account.PublicKey))
                .Select(accountInfo =>
                {
                    return new AccountInfo(
                        Address.CreateFromEncoded(accountInfo["account"]["address"].ToString()),
                        accountInfo["account"]["publicKey"].ToString(),
                        ulong.Parse(accountInfo["account"]["importance"].ToString()),
                        ulong.Parse(accountInfo["account"]["harvestedBlocks"].ToString()),
                        ulong.Parse(accountInfo["account"]["vestedBalance"].ToString()),
                        ulong.Parse(accountInfo["account"]["balance"].ToString()),
                        new MultisigAccountInfo(
                            PublicAccount.CreateFromPublicKey(
                                accountInfo["account"]["publicKey"].ToString(), new NodeHttp(Url).GetNetworkType().Wait()),
                            accountInfo["meta"]["cosignatories"]?.Select(accountInfo2 => new AccountInfo(
                                Address.CreateFromEncoded(accountInfo["address"].ToString()),
                                accountInfo["publicKey"].ToString(),
                                ulong.Parse(accountInfo["importance"].ToString()),
                                ulong.Parse(accountInfo["harvestedBlocks"].ToString()),
                                ulong.Parse(accountInfo["vestedBalance"].ToString()),
                                ulong.Parse(accountInfo["balance"].ToString()),
                                null)).ToList(),
                            accountInfo["meta"]["cosignatoryOf"]?.Select(accountInfo2 => new AccountInfo(
                                Address.CreateFromEncoded(accountInfo["address"].ToString()),
                                accountInfo["publicKey"].ToString(),
                                ulong.Parse(accountInfo["importance"].ToString()),
                                ulong.Parse(accountInfo["harvestedBlocks"].ToString()),
                                ulong.Parse(accountInfo["vestedBalance"].ToString()),
                                ulong.Parse(accountInfo["balance"].ToString()),
                                null)).ToList()
                        ));
                });
        }

        /// <summary>
        /// Get mosaics owned.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="query">The query.</param>
        /// <returns>An <see cref="IObservable"/> list of <see cref="Mosaic"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// account
        /// or
        /// query
        /// </exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>MosaicsOwned</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// var accountHttp = new AccountHttp("<!--insert host like: http://0.0.0.0:7890-->");
        /// 
        /// var publicAccount = new PublicAccount("856f39436e33129afff95b89aca998fa23cd751a6f4d79ce4fb9da9641ecb59c", NetworkType.Types.TEST_NET);
        /// 
        /// var accountInfo = await accountHttp.MosaicsOwned(publicAccount);
        /// </code>
        /// </example>
        public IObservable<List<Mosaic>> MosaicsOwned(PublicAccount account)
        {
            return MosaicsOwned(account.Address);
        }

        /// <summary>
        /// Get mosaics owned.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="query">The query.</param>
        /// <returns>An <see cref="IObservable"/> list of <see cref="Mosaic"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// account
        /// or
        /// query
        /// </exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>MosaicsOwned</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// var accountHttp = new AccountHttp("<!--insert host like: http://0.0.0.0:7890-->");
        /// 
        /// var address = Address.CreateFromEncoded("TCTUIF-557ZCQ-OQPW2M-6GH4TC-DPM2ZY-BBL54K-GNHR");
        /// 
        /// var accountInfo = await accountHttp.MosaicsOwned(address);
        /// </code>
        /// </example>
        public IObservable<List<Mosaic>> MosaicsOwned(Address account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));

            return Observable.FromAsync(async ar => await AccountRoutesApi.MosaicsOwnedAsync(account.Plain));
        }

        /// <summary>
        /// Get incoming transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <returns>An <see cref="IObservable"/> list of <see cref="Transaction"/>.</returns>
        /// <exception cref="ArgumentNullException">account</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>IncomingTransactions</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// var accountHttp = new AccountHttp("<!--insert host like: http://0.0.0.0:7890-->");
        /// 
        /// var address = Address.CreateFromEncoded("TCTUIF-557ZCQ-OQPW2M-6GH4TC-DPM2ZY-BBL54K-GNHR");
        ///
        /// var transactions = await accountHttp().IncomingTransactions(address);
        /// </code>
        /// </example>
        public IObservable<List<Transaction>> IncomingTransactions(Address account)
        {
            return IncomingTransactions(account, new TransactionQueryParams(null, null));
        }

        /// <summary>
        /// Get incoming transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <returns>An <see cref="IObservable"/> list of <see cref="Transaction"/>.</returns>
        /// <exception cref="ArgumentNullException">account</exception>
        /// // <example> 
        /// This sample shows how to use the <see>
        ///         <cref>IncomingTransactions</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// var accountHttp = new AccountHttp("<!--insert host like: http://0.0.0.0:7890-->");
        /// 
        /// var publicAccount = new PublicAccount("856f39436e33129afff95b89aca998fa23cd751a6f4d79ce4fb9da9641ecb59c", NetworkType.Types.TEST_NET);
        ///
        /// var transactions = await accountHttp().IncomingTransactions(publicAccount);
        /// </code>
        /// </example>
        public IObservable<List<Transaction>> IncomingTransactions(PublicAccount account)
        {
            return IncomingTransactions(account.Address, new TransactionQueryParams(null, null));
        }

        /// <summary>
        /// Get incoming transactions.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="query">The query. Includes the Transaction of the transaction up to which transactions should be returned and the data base id.</param>
        /// <returns>An <see cref="IObservable"/> list of <see cref="Transaction"/>.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>IncomingTransactions</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// var accountHttp = new AccountHttp("<!--insert host like: http://0.0.0.0:7890-->");
        /// 
        /// var publicAccount = new PublicAccount("856f39436e33129afff95b89aca998fa23cd751a6f4d79ce4fb9da9641ecb59c", NetworkType.Types.TEST_NET);
        ///
        /// var transactions = await accountHttp().IncomingTransactions(publicAccount, new TransactionQueryParams("hash","id"));
        /// </code>
        /// </example>
        public IObservable<List<Transaction>> IncomingTransactions(PublicAccount account, TransactionQueryParams query)
        {
            return IncomingTransactions(account.Address, query);
        }

        /// <summary>
        /// Get incoming transactions.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="query">The query. Includes the Transaction of the transaction up to which transactions should be returned and the data base id.</param>
        /// <returns>An <see cref="IObservable"/> list of <see cref="Transaction"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// account
        /// or
        /// query
        /// </exception>
        /// /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>IncomingTransactions</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// var accountHttp = new AccountHttp("<!--insert host like: http://0.0.0.0:7890-->");
        /// 
        /// var address = Address.CreateFromEncoded("TCTUIF-557ZCQ-OQPW2M-6GH4TC-DPM2ZY-BBL54K-GNHR");
        ///
        /// var transactions = await accountHttp().IncomingTransactions(address, new TransactionQueryParams("hash","id"));
        /// </code>
        /// </example>
        public IObservable<List<Transaction>> IncomingTransactions(Address account, TransactionQueryParams query)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));
            if (query == null) throw new ArgumentNullException(nameof(query));
           
            return Observable.FromAsync(async ar => await AccountRoutesApi.TransactionsIncomingAsync(account.Plain, query.GetHash(), query.GetId()));
        }

        /// <summary>
        /// Get outgoing transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <returns>An <see cref="IObservable"/> list of <see cref="Transaction"/>.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>OutgoingTransactions</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// var accountHttp = new AccountHttp("<!--insert host like: http://0.0.0.0:7890-->");
        /// 
        /// var address = Address.CreateFromEncoded("TCTUIF-557ZCQ-OQPW2M-6GH4TC-DPM2ZY-BBL54K-GNHR");
        ///
        /// var transactions = await accountHttp().OutgoingTransactions(address);
        /// </code>
        /// </example>
        public IObservable<List<Transaction>> OutgoingTransactions(Address account)
        {
            return OutgoingTransactions(account, new TransactionQueryParams(null, null));
        }

        /// <summary>
        /// Get outgoing transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <returns>An <see cref="IObservable"/> list of <see cref="Transaction"/>.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>OutgoingTransactions</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// var accountHttp = new AccountHttp("<!--insert host like: http://0.0.0.0:7890-->");
        /// 
        /// var publicAccount = new PublicAccount("856f39436e33129afff95b89aca998fa23cd751a6f4d79ce4fb9da9641ecb59c", NetworkType.Types.TEST_NET);
        ///
        /// var transactions = await accountHttp().OutgoingTransactions(publicAccount, new TransactionQueryParams("hash","id"));
        /// </code>
        /// </example>
        public IObservable<List<Transaction>> OutgoingTransactions(PublicAccount account)
        {
            return OutgoingTransactions(account.Address, new TransactionQueryParams(null, null));
        }

        /// <summary>
        /// Get outgoing transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <param name="query">The query parameters. Includes the Transaction of the transaction up to which transactions should be returned and the data base id.</param>
        /// <returns>An <see cref="IObservable"/> list of <see cref="Transaction"/>.</returns>
        /// <exception cref="ArgumentNullException">account
        /// or
        /// query</exception>
        /// /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>OutgoingTransactions</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// var accountHttp = new AccountHttp("<!--insert host like: http://0.0.0.0:7890-->");
        /// 
        /// var address = Address.CreateFromEncoded("TCTUIF-557ZCQ-OQPW2M-6GH4TC-DPM2ZY-BBL54K-GNHR");
        ///
        /// var transactions = await accountHttp().OutgoingTransactions(address, new TransactionQueryParams("hash","id"));
        /// </code>
        /// </example>
        public IObservable<List<Transaction>> OutgoingTransactions(Address account, TransactionQueryParams query)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));
            if (query == null) throw new ArgumentNullException(nameof(query));

            return Observable.FromAsync(async ar => await AccountRoutesApi.TransactionsOutgoingAsync(account.Plain, query.GetHash(), query.GetId()));
        }

        /// <summary>
        /// Get unconfirmed transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>An <see cref="IObservable"/> list of <see cref="Transaction"/>.</returns>
        /// <exception cref="ArgumentNullException">account
        /// or
        /// query</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>UnconfirmedTransactions</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// var accountHttp = new AccountHttp("<!--insert host like: http://0.0.0.0:7890-->");
        /// 
        /// var address = Address.CreateFromEncoded("TCTUIF-557ZCQ-OQPW2M-6GH4TC-DPM2ZY-BBL54K-GNHR");
        ///
        /// var transactions = await accountHttp().UnconfirmedTransactions(address);
        /// </code>
        /// </example>
        public IObservable<List<Transaction>> UnconfirmedTransactions(Address account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));

            return Observable.FromAsync(async ar => await AccountRoutesApi.TransactionsUnconfirmedAsync(account.Plain));
        }

        /// <summary>
        /// Get unconfirmed transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>An <see cref="IObservable"/> list of <see cref="Transaction"/>.</returns>
        /// <exception cref="ArgumentNullException">account
        /// or
        /// query</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>UnconfirmedTransactions</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// var accountHttp = new AccountHttp("<!--insert host like: http://0.0.0.0:7890-->");
        /// 
        /// var publicAccount = new PublicAccount("856f39436e33129afff95b89aca998fa23cd751a6f4d79ce4fb9da9641ecb59c", NetworkType.Types.TEST_NET);
        ///
        /// var transactions = await accountHttp().UnconfimedTransaction(publicAccount);
        /// </code>
        /// </example>
        public IObservable<List<Transaction>> UnconfirmedTransactions(PublicAccount account)
        {
            return UnconfirmedTransactions(account.Address);
        }

        /// <summary>
        /// Get all transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <returns>An <see cref="IObservable"/> list of <see cref="Transaction"/>.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>Transactions</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// var accountHttp = new AccountHttp("<!--insert host like: http://0.0.0.0:7890-->");
        /// 
        /// var address = Address.CreateFromEncoded("TCTUIF-557ZCQ-OQPW2M-6GH4TC-DPM2ZY-BBL54K-GNHR");
        ///
        /// var transactions = await accountHttp().Transactions(address);
        /// </code>
        /// </example>
        public IObservable<List<Transaction>> Transactions(Address account)
        {            
            return Transactions(account, new TransactionQueryParams(null, null));
        }

        /// <summary>
        /// Get all transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <returns>An <see cref="IObservable"/> list of <see cref="Transaction"/>.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>Transactions</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// var accountHttp = new AccountHttp("<!--insert host like: http://0.0.0.0:7890-->");
        /// 
        /// var publicAccount = new PublicAccount("856f39436e33129afff95b89aca998fa23cd751a6f4d79ce4fb9da9641ecb59c", NetworkType.Types.TEST_NET);
        ///
        /// var transactions = await accountHttp().Transactions(publicAccount);
        /// </code>
        /// </example>
        public IObservable<List<Transaction>> Transactions(PublicAccount account)
        {
            return Transactions(account.Address, new TransactionQueryParams(null, null));
        }

        /// <summary>
        /// Get all transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>An <see cref="IObservable"/> list of <see cref="Transaction"/>.</returns>
        /// <exception cref="ArgumentNullException">account
        /// or
        /// query</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>Transactions</cref>
        ///     </see>
        ///     method. 
        /// <code>
        /// var accountHttp = new AccountHttp("<!--insert host like: http://0.0.0.0:7890-->");
        /// 
        /// var address = Address.CreateFromEncoded("TCTUIF-557ZCQ-OQPW2M-6GH4TC-DPM2ZY-BBL54K-GNHR");
        ///
        /// var transactions = await accountHttp().Transactions(address, new TransactionQueryParams("hash","id"));
        /// </code>
        /// </example>
        public IObservable<List<Transaction>> Transactions(Address account, TransactionQueryParams query)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));
            if (query == null) throw new ArgumentNullException(nameof(query));

            return Observable.FromAsync(async ar => await AccountRoutesApi.TransactionsAllAsync(account.Plain, query.GetHash(), query.GetId()));
        }
    }
}
