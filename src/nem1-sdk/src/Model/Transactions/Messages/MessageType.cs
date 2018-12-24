// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 01-26-2018
//
// Last Modified By : kailin
// Last Modified On : 11-07-2018
// ***********************************************************************
// <copyright file="MessageType.cs" company="Nem.io">
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

namespace io.nem1.sdk.Model.Network.Messages
{
    /// <summary>
    /// Class MessageType.
    /// </summary>
    public static class MessageType
    {
        /// <summary>
        /// Enum Type
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// The encrypted
            /// </summary>
            ENCRYPTED = 2,

            /// <summary>
            /// The unencrypted
            /// </summary>
            UNENCRYPTED = 1
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Byte.</returns>
        public static int GetValue(this Type type)
        {
            return (int)type;
        }

        /// <summary>
        /// Gets the raw value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Type.</returns>
        public static Type GetRawValue(int value)
        {
            return value == 0x02 ? Type.ENCRYPTED : Type.UNENCRYPTED;
        }
    }
}
