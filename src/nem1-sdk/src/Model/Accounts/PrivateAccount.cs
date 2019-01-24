// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 11-07-2018
// ***********************************************************************
// <copyright file="Account.cs" company="Nem.io">   
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

using System.Security.Cryptography;
using io.nem1.sdk.Core.Crypto.Chaso.NaCl;
using io.nem1.sdk.Model.Network;
using io.nem1.sdk.Model.Transactions;
using Org.BouncyCastle.Crypto.Digests;

namespace io.nem1.sdk.Model.Accounts
{
    /// <summary>
    /// Account class.
    /// </summary>
    public class PrivateAccount
    {
        /// <summary>
        /// Gets the key pair.
        /// </summary>
        /// <value>The key pair.</value>
        public KeyPair KeyPair { get; }

        /// <summary>
        /// Gets the public account.
        /// </summary>
        /// <value>The public account.</value>
        private PublicAccount _PublicAccount;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateAccount" /> class.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="networkType">Type of the network.</param>
        public PrivateAccount(string privateKey, NetworkType.Types networkType)
        {
            KeyPair = new KeyPair(privateKey);
            _PublicAccount = new PublicAccount(KeyPair.PublicKeyString, networkType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateAccount" /> class.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="keyPair">The key pair.</param>
        public PrivateAccount(Address address, KeyPair keyPair)
        {
            KeyPair = keyPair;
            _PublicAccount = new PublicAccount(keyPair.PublicKeyString, address.GetNetworktype());
        }

        /// <summary>
        /// Creates an Account from a private key.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="networkType">Type of the network.</param>
        /// <returns>Account.</returns>
        public static PrivateAccount CreateFromPrivateKey(string privateKey, NetworkType.Types networkType)
        {
            return new PrivateAccount(privateKey, networkType);
        }

        /// <summary>
        /// Generates a new account.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <returns>Account.</returns>
        public static PrivateAccount GenerateNewAccount(NetworkType.Types networkType)
        {
            using (var ng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[2048];
                ng.GetNonZeroBytes(bytes);

                var digestSha3 = new Sha3Digest(256);
                var stepOne = new byte[32];
                digestSha3.BlockUpdate(bytes, 0, 32);
                digestSha3.DoFinal(stepOne, 0);

                var keyPair = new KeyPair(stepOne.ToHexLower());

                return new PrivateAccount(keyPair.PrivateKeyString, networkType);
            }
        }

        /// <summary>
        /// Gets the private key string.
        /// </summary>
        /// <value>The private key.</value>
        public string PrivateKey => KeyPair.PrivateKeyString;

        /// <summary>
        /// Gets the public key.
        /// </summary>
        /// <value>The public key.</value>
        public string PublicKey => KeyPair.PublicKeyString; // == PublicAccount.PublicKey;

        /// <summary>
        /// Gets the address.
        /// </summary>
        /// <value>The address.</value>
        public Address Address => _PublicAccount.Address;

        /// <summary>
        /// Signs the specified transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <returns>SignedTransaction.</returns>
        public SignedTransaction Sign(Transaction transaction)
        {
            return transaction.SignWith(KeyPair);
        }

    }
}
