// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 11-07-2018
// ***********************************************************************
// <copyright file="NetworkType.cs" company="Nem.io">   
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

namespace io.nem1.sdk.Model.Blockchain
{
    /// <summary>
    /// Static class containing network type constants enum and helper functions.
    /// </summary>
    public static class NetworkType
    {
        /// <summary>
        /// Contains the valid network types.
        /// </summary>
        public enum Types
        {
            /// <summary>
            /// Unsupported net
            /// </summary>
            UNDETERMINED_NET = 0x00, // To detect networkId 0, occasionaly received in the TESTNET NisInfo
            /// <summary>
            /// The main net
            /// </summary>
            MAIN_NET = 0x68,
            /// <summary>
            /// The test net
            /// </summary>
            TEST_NET = 0x98,
            /// <summary>
            /// The mijin
            /// </summary>
            MIJIN = 0x60,
            /// <summary>
            /// The mijin test
            /// </summary>
            MIJIN_TEST = 0x90
        }


        /// <summary>
        /// Gets the network identifier byte.
        /// </summary>
        /// <param name="type">The network byte.</param>
        /// <returns>System.Byte.</returns>
        public static byte GetNetwork(this Types type)
        {
            return (byte)type;
        }

        /// <summary>
        /// Gets the network enum type.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Types.</returns>
        public static Types GetNetwork(string name)
        {
            switch (name)
            {
                case "":
                    return Types.UNDETERMINED_NET;
                case "mijinTest":
                    return Types.MIJIN_TEST;
                case "mijin":
                    return Types.MIJIN;
                case "testnet":
                    return Types.TEST_NET;
                case "mainnet":
                    return Types.MAIN_NET;
                default:
                    throw  new ArgumentException("invalid network name.");
            }
        }

        /// Gets the network enum type.
        /// <param name="value">Value</param>
        /// <returns>Types.</returns>
        public static Types GetRawValue(int value)
        {
            switch (value)
            {
                case 0x00:
                    return Types.UNDETERMINED_NET;
                case 0x90:
                    return Types.MIJIN_TEST;
                case 0x60:
                    return Types.MIJIN;
                case 0x98:
                    return Types.TEST_NET;
                case 0x68:
                    return Types.MAIN_NET;
                default:
                    throw new ArgumentException("invalid network value.");
            }
        }
    }  
}
