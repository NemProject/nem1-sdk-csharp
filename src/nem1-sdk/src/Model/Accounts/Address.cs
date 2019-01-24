// ***********************************************************************
// Assembly         : nem1-sdk
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 11-07-2018
// ***********************************************************************
// <copyright file="Address.cs" company="Nem.io">   
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
using System.Linq;
using System.Text.RegularExpressions;
using io.nem1.sdk.Core.Crypto.Chaso.NaCl;
using io.nem1.sdk.Core.Utils;
using io.nem1.sdk.Model.Network;
using Org.BouncyCastle.Crypto.Digests;

namespace io.nem1.sdk.Model.Accounts
{
    /// <summary>
    /// The address structure describes an address with its network.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Struct Constants
        /// </summary>
        internal struct Constants
        {
            /// <summary>
            /// The ripemd160
            /// </summary>
            internal static int Ripemd160 = 20;
            /// <summary>
            /// The address decoded
            /// </summary>
            internal static int AddressDecoded = 25;
            /// <summary>
            /// The address encoded
            /// </summary>
            internal static int AddressEncoded = 40;
            /// <summary>
            /// The key
            /// </summary>
            internal static int Key = 32;
            /// <summary>
            /// The long key
            /// </summary>
            internal static int LongKey = 64;
            /// <summary>
            /// The checksum
            /// </summary>
            internal static int Checksum = 4;
        }

        /// <summary>
        /// Gets the address in plain format ex: SB3KUBHATFCPV7UZQLWAQ2EUR6SIHBSBEOEDDDF3.
        /// </summary>
        public string Plain { get; private set; }

        /// <summary>
        /// Create an Address from a given public key and network type.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="networkType">The network type</param>
        /// <returns>Address.</returns>
        public Address(string publicKey, NetworkType.Types networkType)
        {
            // step 1) sha-3(256) public key
            var digestSha3 = new KeccakDigest(256);
            var stepOne = new byte[Constants.Key];

            digestSha3.BlockUpdate(publicKey.FromHex(), 0, Constants.Key);
            digestSha3.DoFinal(stepOne, 0);

            // step 2) perform ripemd160 on previous step
            var digestRipeMd160 = new RipeMD160Digest();
            var stepTwo = new byte[Constants.Ripemd160];
            digestRipeMd160.BlockUpdate(stepOne, 0, Constants.Key);
            digestRipeMd160.DoFinal(stepTwo, 0);

            // step3) prepend network byte    
            var stepThree = new[] { networkType.GetNetwork() }.Concat(stepTwo).ToArray();

            // step 4) perform sha3 on previous step
            var stepFour = new byte[Constants.Key];
            digestSha3.BlockUpdate(stepThree, 0, Constants.Ripemd160 + 1);
            digestSha3.DoFinal(stepFour, 0);

            // step 5) retrieve checksum
            var stepFive = new byte[Constants.Checksum];
            Array.Copy(stepFour, 0, stepFive, 0, Constants.Checksum);

            // step 6) append stepFive to resulst of stepThree
            var stepSix = new byte[Constants.AddressDecoded];
            Array.Copy(stepThree, 0, stepSix, 0, Constants.Ripemd160 + 1);
            Array.Copy(stepFive, 0, stepSix, Constants.Ripemd160 + 1, Constants.Checksum);

            // step 7) return base 32 encode address byte array
            Initialize(stepSix.ToBase32String());
        }

        // Private method to be used in two constructors 
        private void Initialize(string addressPlainOrPretty)
        {
            addressPlainOrPretty = Regex.Replace(addressPlainOrPretty.Replace("-", ""), @"\s+", "").ToUpper();
            if (addressPlainOrPretty.Length != 40)
                throw new Exception("Address " + addressPlainOrPretty + " has to be 40 characters long");

            switch (addressPlainOrPretty.ToCharArray()[0])
            {
                case 'S': case 'M': case 'T': case 'N': break;
                default: throw new Exception("Address " + addressPlainOrPretty + " Network unsupported");
            }
            Plain = addressPlainOrPretty;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="addressPlainOrPretty">The address (plain or pretty) of the account.</param>
        /// <exception cref="System.Exception">
        /// Address has to be 40 characters long 
        /// or
        /// Address Network unsupported
        /// </exception>
        public Address(string addressPlainOrPretty)
        {
            Initialize(addressPlainOrPretty);
        }

        /// <summary>
        /// Get the address in pretty format ex: SB3KUB-HATFCP-V7UZQL-WAQ2EU-R6SIHB-SBEOED-DDF3.
        /// </summary>
        /// <returns>The address in pretty format</returns>
        public string Pretty => Regex.Replace(Plain, ".{6}", "$0-");

        /// <summary>
        /// The network type of the address
        /// </summary>
        /// <returns>The NetworkType.</returns>
        public NetworkType.Types GetNetworktype()
        {
            switch (Plain.ToCharArray()[0])
            {
                case 'S': return NetworkType.Types.MIJIN_TEST;
                case 'M': return NetworkType.Types.MIJIN;
                case 'T': return NetworkType.Types.TEST_NET;
                case 'N': return NetworkType.Types.MAIN_NET;
                default: return NetworkType.Types.UNDETERMINED_NET;
            }
        }

        /// <summary>
        /// Create an Address from a given hexadecimal address.
        /// </summary>
        /// <param name="addressHex">The hexadecimal Address</param>
        /// <returns>Address.</returns>
        /// <exception cref="System.Exception">
        /// Address has to be 40 characters long 
        /// or
        /// Address Network unsupported
        /// </exception>
        public static Address CreateFromHex(string addressHex)
        {
            return new Address(addressHex.FromHex().ToBase32String());
        }

    }
}
