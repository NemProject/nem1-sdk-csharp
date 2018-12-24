// ***********************************************************************
// Assembly         : nem1-sdk
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 11-07-2018
// ***********************************************************************
// <copyright file="CosignatoryModificationType.cs" company="Nem.io">
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
    /// Class CosignatoryModificationType.
    /// </summary>
    public static class CosignatoryModificationType
    {
        /// <summary>
        /// Enum Types
        /// </summary>
        public enum Types
        {
            /// <summary>
            /// Add cosignatory
            /// </summary>
            Add = 0x01,

            /// <summary>
            /// Remove Ccosignatory
            /// </summary>
            Remove = 0x02
        }

        /// <summary>
        /// Gets the integer value of the modification type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="InvalidEnumArgumentException">type</exception>
        public static int GetValue(this Types type)
        {
            if (!Enum.IsDefined(typeof(Types), type))
                throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(Types));

            return (int)type;
        }

        /// <summary>
        /// Gets the raw value of the modification type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Types.</returns>
        /// <exception cref="System.ArgumentException">invalid modification type.</exception>
        public static Types GetRawValue(this int type)
        {
            switch (type)
            {
                case 0x01:
                    return Types.Add;
                case 0x02:
                    return Types.Remove;
                default:
                    throw new ArgumentException("invalid modification type.");
            }
        }
    }
}
