// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="TransactionInfo.cs" company="Nem.io">
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

namespace io.nem1.sdk.Model.Transactions
{
    /// <summary>
    /// Class TransactionInfo.
    /// </summary>
    public class TransactionInfo
    {
        /// <summary>
        /// Gets the height at which the transaction was included in a block.
        /// </summary>
        /// <value>The height.</value>
        public ulong Height { get; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; }

        /// <summary>
        /// Gets the hash.
        /// </summary>
        /// <value>The hash.</value>
        public string Hash { get; }

        /// <summary>
        /// Gets the inner hash.
        /// </summary>
        /// <value>The inner hash.</value>
        public string InnerHash { get; }

        /// <summary>
        /// Gets a value indicating whether the transaction is a multisig transaction.
        /// </summary>
        /// <value><c>true</c> if this instance is multisig; otherwise, <c>false</c>.</value>
        public bool IsMultisig => InnerHash != null;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionInfo"/> class.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="hash">The hash.</param>
        /// <param name="innerHash">The inner hash.</param>
        internal TransactionInfo(ulong height, int id, string hash, string innerHash)
        {
            Height = height;
            Id = id;
            Hash = hash;
            InnerHash = innerHash;
        }

        /// <summary>
        /// Creates a new multisig instance of TransactionInfo.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="hash">The hash.</param>
        /// <param name="innerHash">The inner hash.</param>
        /// <returns>TransactionInfo.</returns>
        public static TransactionInfo CreateMultisig(ulong height, int id, string hash, string innerHash)
        {
            return new TransactionInfo(height, id, hash, innerHash);
        }

        /// <summary>
        /// Creates a new instance of TransactionInfo.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="hash">The hash.</param>
        /// <returns>TransactionInfo.</returns>
        public static TransactionInfo Create(ulong height, int id, string hash)
        {
            return new TransactionInfo(height, id, hash, null);
        }
    }
}
