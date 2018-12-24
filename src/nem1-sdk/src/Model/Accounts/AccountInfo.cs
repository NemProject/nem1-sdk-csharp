// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 11-07-2018
// ***********************************************************************
// <copyright file="AccountInfo.cs" company="Nem.io">   
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

using System.Collections.Generic;

namespace io.nem1.sdk.Model.Accounts
{
    /// <summary>
    /// The account info structure describes basic information for an account.
    /// </summary>
    public class AccountInfo
    {

        public List<AccountInfo> Cosignatories { get; }

        /// <summary>
        /// Gets the address.
        /// </summary>
        /// <value>The address.</value>
        public Address Address { get; }

        /// <summary>
        /// Gets the public key.
        /// </summary>
        /// <value>The public key.</value>
        public string PublicKey { get; }

        /// <summary>
        /// Gets the harvested blocks.
        /// </summary>
        /// <value>The harvested blocks.</value>
        public ulong HarvestedBlocks { get; }

        /// <summary>
        /// Gets the vested balance.
        /// </summary>
        /// <value>The vested balance.</value>
        public ulong VestedBalance { get; }

        /// <summary>
        /// Gets the balance.
        /// </summary>
        /// <value>The balance.</value>
        public ulong Balance { get; }

        /// <summary>
        /// Gets the multisig account information.
        /// </summary>
        /// <value>The multisig account information.</value>
        public MultisigAccountInfo MultisigAccountInfo { get; }

        /// <summary>
        /// Gets the importance.
        /// </summary>
        /// <value>The importance.</value>
        public ulong Importance { get; }

        /// <summary>
        /// Gets the public account.
        /// </summary>
        /// <value>The public account.</value>
        public PublicAccount PublicAccount => new PublicAccount(
            PublicKey,
            Address.Networktype
        );

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountInfo"/> class.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="publicKey">The public key.</param>
        /// <param name="importance">The importance.</param>
        /// <param name="harvestedBlocks">The harvested blocks.</param>
        /// <param name="vestedBalance">The vested balance.</param>
        /// <param name="multisigAccountInfo">The multisig account information.</param>
        public AccountInfo(Address address,  string publicKey, ulong importance, ulong harvestedBlocks, ulong vestedBalance, ulong balance, MultisigAccountInfo multisigAccountInfo)
        {
            Address = address;
            HarvestedBlocks = harvestedBlocks;
            PublicKey = publicKey;
            VestedBalance = vestedBalance;
            Balance = balance;
            Importance = importance;
            MultisigAccountInfo = multisigAccountInfo;          
        }
    }
}
