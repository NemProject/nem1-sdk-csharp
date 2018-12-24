// ***********************************************************************
// Assembly         : nem1-sdk
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 11-07-2018
// ***********************************************************************
// <copyright file="ImportanceTransferMode.cs" company="Nem.io">
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

namespace io.nem1.sdk.Model.Transactions
{
    /// <summary>
    /// Class ImportanceTransferMode.
    /// </summary>
    public static class ImportanceTransferMode
    {
        /// <summary>
        /// Enum Mode
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// Add an account as a delegated account
            /// </summary>
            Add = 0x01,

            /// <summary>
            /// Remove an account as a delegated account
            /// </summary>
            Remove = 0x02
        }

        /// <summary>
        /// Gets the integer value of the transfer mode type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="InvalidEnumArgumentException">type</exception>
        public static int GetValue(this Mode type)
        {
            if (!Enum.IsDefined(typeof(Mode), type))
                throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(Mode));

            return (int)type;
        }

        /// <summary>
        /// Gets the raw value of the transfer mode type integer.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Mode.</returns>
        /// <exception cref="System.ArgumentException">invalid importance transfer mode.</exception>
        public static Mode GetRawValue(this int type)
        {
            switch (type)
            {
                case 0x01:
                    return Mode.Add;
                case 0x02:
                    return Mode.Remove;
                default:
                    throw new ArgumentException("invalid importance transfer mode.");
            }
        }
    }
}
