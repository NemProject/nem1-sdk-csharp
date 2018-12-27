// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 01-30-2018
// ***********************************************************************
// <copyright file="Transaction.cs" company="Nem.io">
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
using io.nem1.sdk.Core.Crypto.Chaso.NaCl;
using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Blockchain;
using io.nem1.sdk.Model.Network;

namespace io.nem1.sdk.Model.Transactions
{
    /// <summary>
    /// An abstract transaction class that serves as the base class of all NEM transactions.
    /// </summary>
    public abstract class Transaction
    {
        /// <summary>
        /// Gets the NetworkTime.
        /// </summary>
        /// <value>The NetworkTime.</value>
        public NetworkTime NetworkTime { get; internal set; }

        /// <summary>
        /// Gets the fee.
        /// </summary>
        /// <value>The fee.</value>
        public ulong Fee { get; internal set; }

        /// <summary>
        /// Gets the deadline.
        /// </summary>
        /// <value>The deadline.</value>
        public Deadline Deadline { get; internal set; }

        /// <summary>
        /// Gets the type of the network.
        /// </summary>
        /// <value>The type of the network.</value>
        public NetworkType.Types NetworkType { get; internal set; }

        /// <summary>
        /// Gets the Version.
        /// </summary>
        /// <value>The Version.</value>
        public int Version { get; set; }

        /// <summary>
        /// Gets the type of the transaction.
        /// </summary>
        /// <value>The type of the transaction.</value>
        public TransactionTypes.Types TransactionType { get; internal set; }

        /// <summary>
        /// Gets the signer.
        /// </summary>
        /// <value>The signer.</value>
        public PublicAccount Signer { get; internal set; }

        /// <summary>
        /// Gets the signature.
        /// </summary>
        /// <value>The signature.</value>
        public string Signature { get; internal set; }

        /// <summary>
        /// Gets the transaction information.
        /// </summary>
        /// <value>The transaction information.</value>
        public TransactionInfo TransactionInfo { get; internal set; }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <value>The bytes.</value>
        protected byte[] Bytes { get; set; }

        /// <summary>
        /// Gets the signer.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        internal byte[] GetSigner()
        {
            return Signer == null ? new byte[32] : Signer.PublicKey.FromHex();
        }

        /// <summary>
        /// Signs the transaction with the given <see cref="KeyPair"/>.
        /// </summary>
        /// <param name="keyPair">The <see cref="KeyPair"/>.</param>
        /// <returns><see cref="SignedTransaction"/>.</returns>
        /// <exception cref="ArgumentNullException">keyPair</exception>
        public SignedTransaction SignWith(KeyPair keyPair)
        {
            if (keyPair == null) throw new ArgumentNullException(nameof(keyPair));

            Signer = new PublicAccount(keyPair.PublicKeyString, NetworkType);

            Bytes = GenerateBytes();

            var sig = TransactionExtensions.SignTransaction(keyPair, Bytes);

            return SignedTransaction.Create(Bytes, sig, TransactionExtensions.Hasher(Bytes), keyPair.PublicKey, TransactionType);
        }

        /// <summary>
        /// Generates the hash of the serialized transaction payload.
        /// </summary>
        /// <returns>The hash in hex format.</returns>
        public string CreateTransactionHash()
        {
            return TransactionExtensions.Hasher(GenerateBytes()).ToHexUpper();
        }

        /// <summary>
        /// Generates the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        internal abstract byte[] GenerateBytes();

        protected byte ExtractVersion(int version)
        {
            var hex = BitConverter.GetBytes(version).ToHexLower();

            return Convert.ToByte(hex.Substring(0, 2), 16);
        }
    }
}
