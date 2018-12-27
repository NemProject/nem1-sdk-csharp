// ***********************************************************************
// Assembly         : nem1-sdk
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 11-07-2018
// ***********************************************************************
// <copyright file="PublicAccount.cs" company="Nem.io">   
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
using System.ComponentModel;
using System.Text.RegularExpressions;
using io.nem1.sdk.Model.Blockchain;

namespace io.nem1.sdk.Model.Accounts
{
    /// <summary>
    /// The public account structure contains account's address and public key.
    /// </summary>
    public class PublicAccount
    {
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
        /// Initializes a new instance of the <see cref="PublicAccount"/> class.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="networkType">Type of the network.</param>
        /// <exception cref="ArgumentException">
        /// invalid public key length
        /// or
        /// invalid public key not hex
        /// </exception>
        /// <exception cref="ArgumentNullException">publicKey</exception>
        /// <exception cref="InvalidEnumArgumentException">networkType</exception>
        public PublicAccount(string publicKey, NetworkType.Types networkType)
        {
            if (publicKey == null) throw new ArgumentNullException(nameof(publicKey));
            if (!Regex.IsMatch(publicKey, @"\A\b[0-9a-fA-F]+\b\Z")) throw  new ArgumentException("invalid public key length");
            if (publicKey.Length != 64) throw new ArgumentException("invalid public key not hex");           
            if (!Enum.IsDefined(typeof(NetworkType.Types), networkType))
                throw new InvalidEnumArgumentException(nameof(networkType), (int) networkType,
                    typeof(NetworkType.Types));

            Address = new Address(publicKey, networkType);
            PublicKey = publicKey;
        }

        /// <summary>
        /// Compares public accounts for equality.
        /// </summary>
        /// <param name="other">The other public account to compare with.</param>
        /// <returns>True if they are they are equal, else false.</returns>
        public bool Equals(PublicAccount other)
        {
            return Equals(Address, other.Address) && string.Equals(PublicKey, other.PublicKey);
        }

        /// <summary>
        /// Gets the hashcode of PublicAccount.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            unchecked  // to suppress overflow-checking for integral-type arithmetic operations and conversions.
            {
                return ((Address != null ? Address.GetHashCode() : 0) * 397) ^ (PublicKey != null ? PublicKey.GetHashCode() : 0);
            }
        }
    }
}
