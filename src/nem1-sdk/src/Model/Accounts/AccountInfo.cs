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
        public struct StatusValue
        {
            /// <summary>
            /// LOCKED accounts cannot harvest.
            /// </summary>
            public const string LOCKED = "LOCKED";
            /// <summary>
            /// UNLOCKED accounts can harvest.
            /// </summary>
            public const string UNLOCKED = "UNLOCKED";
        }

        public struct RemoteStatusValue
        {
            /// <summary>
            /// Remote harvesting is not active.
            /// </summary>
            public const string INACTIVE = "INACTIVE";
            /// <summary>
            /// Remote harvesting is active.
            /// </summary>
            public const string ACTIVE = "ACTIVE";
        }

        /// <summary>
        /// Gets the public account.
        /// </summary>
        /// <value>The public account.</value>
        private PublicAccount _PublicAccount { get; }

        /// <summary>
        /// Gets the balance.
        /// </summary>
        /// <value>The balance.</value>
        public ulong Balance { get; }
        /// <summary>
        /// Gets the vested balance.
        /// </summary>
        /// <value>The vested balance.</value>
        public ulong VestedBalance { get; }
        /// <summary>
        /// Gets the importance.
        /// </summary>
        /// <value>The importance.</value>
        public ulong Importance { get; }
        /// <summary>
        /// Gets the harvested blocks.
        /// </summary>
        /// <value>The harvested blocks.</value>
        public ulong HarvestedBlocks { get; }
        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>The Status.</value>
        public string Status { get; }
        /// <summary>
        /// Gets the remote status.
        /// </summary>
        /// <value>The Remote Status.</value>
        public string RemoteStatus { get; }

        /// <summary>
        /// Gets the minCosigners.
        /// </summary>
        /// <value>The minCosigners.</value>
        public int MinCosigners { get; }
        /// <summary>
        /// Returns multisig account cosigners.
        /// </summary>
        /// <value>The cosigners.</value>
        public List<AccountInfo> Cosigners { get; }
        /// <summary>
        /// Returns multisig accounts this account is cosigner of.
        /// </summary>
        /// <value>The multisig accounts.</value>
        public List<AccountInfo> CosginatoryOf { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountInfo"/> class.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="address">The address.</param>
        /// <param name="balance">The vbalance.</param>
        /// <param name="vestedBalance">The vested balance.</param>
        /// <param name="importance">The importance.</param>
        /// <param name="harvestedBlocks">The harvested blocks.</param>
        /// <param name="status">The status.</param>
        /// <param name="remoteStatus">The remoteStatus.</param>
        /// <param name="minCosigners">The minimum number of Cosigners (less or eqaul than cosigners.Count).</param>
        /// <param name="cosigners">The cosigners of this multisig account .</param>
        /// <param name="cosginatoryOf">The account is cosginatoryOf these multisig Accounts.</param>
        public AccountInfo(string publicKey, Address address, ulong balance, ulong vestedBalance, ulong importance, ulong harvestedBlocks,
                            string status = "", string remoteStatus = "", 
                            int minCosigners = 0, List<AccountInfo> cosigners = null, List<AccountInfo> cosginatoryOf = null)
        {
            _PublicAccount = new PublicAccount(publicKey, address.Networktype());
            Balance = balance;
            VestedBalance = vestedBalance;
            Importance = importance;
            HarvestedBlocks = harvestedBlocks;
            Status = status;
            RemoteStatus = remoteStatus;
            MinCosigners = minCosigners;
            Cosigners = cosigners;
            CosginatoryOf = cosginatoryOf;
        }

        /// <summary>
        /// Gets the public key.
        /// </summary>
        /// <value>The public key.</value>
        public string PublicKey => _PublicAccount.PublicKey;

        /// <summary>
        /// Gets the address.
        /// </summary>
        /// <value>The address.</value>
        public Address Address => _PublicAccount.Address;

        /// <summary>
        /// Checks if an account is cosignatory of the multisig account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns><c>true</c> if the specified account has cosigners; otherwise, <c>false</c>.</returns>
        public bool HasCosigner(AccountInfo account) => Cosigners.Contains(account);
    }
}
