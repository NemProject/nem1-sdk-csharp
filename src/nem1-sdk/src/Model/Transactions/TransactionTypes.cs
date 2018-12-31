// ***********************************************************************
// Assembly         : nem1-sdk
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 11-07-2018
// ***********************************************************************
// <copyright file="TransactionTypes.cs" company="Nem.io">
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
    /// Class TransactionTypes.
    /// </summary>
    public static class TransactionTypes
    {
        /// <summary>
        /// Enum Types
        /// </summary>
        public enum Types
        {
            /// <summary>
            /// Transfer transaction (257)
            /// </summary>
            Transfer = 0x0101,

            /// <summary>
            /// Importance transfer transaction (2049)
            /// </summary>
            ImportanceTransfer = 0x0801,

            /// <summary>
            /// Multisig aggregate modification transaction (4097)
            /// </summary>
            MultisigAggregateModification = 0x1001,

            /// <summary>
            /// Signature transaction (4098)
            /// </summary>
            SignatureTransaction = 0x1002,

            /// <summary>
            /// Multisig transaction (4100)
            /// </summary>
            Multisig = 0x1004,

            /// <summary>
            /// Provision namespace transaction (8193)
            /// </summary>
            ProvisionNamespace = 0x2001,

            /// <summary>
            /// Mosaic definition transaction (16385)
            /// </summary>
            MosaicDefinition = 0x4001,

            /// <summary>
            /// Supply change transaction (16386)
            /// </summary>
            SupplyChange = 0x4002
        }

        /// <summary>
        /// Gets the integer value of the transaction type.
        /// </summary>
        /// <param name="transactionType">The TransactionTypes.Types type.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="InvalidEnumArgumentException">type</exception>
        public static int GetValue(this Types transactionType)
        {
            if (!Enum.IsDefined(typeof(Types), transactionType))
                throw new InvalidEnumArgumentException(nameof(transactionType), (int) transactionType, typeof(Types));

            return (int)transactionType;
        }

        /// <summary>
        /// Gets the transaction Type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>TransactionTypes.Types</returns>
        /// <exception cref="System.ArgumentException">invalid transaction type.</exception>
        public static TransactionTypes.Types GetType(int value)
        {
            switch (value)
            {
                case 0x0101:
                    return Types.Transfer;
                case 0x1004:
                    return Types.Multisig;
                case 0x1001:
                    return Types.MultisigAggregateModification;
                case 0x1002:
                    return Types.SignatureTransaction;
                case 0x0801:
                    return Types.ImportanceTransfer;
                case 0x2001:
                    return Types.ProvisionNamespace;
                case 0x4001:
                    return Types.MosaicDefinition;
                case 0x4002:
                    return Types.SupplyChange;
                default:
                    throw new ArgumentException("invalid transaction type value.");
            }
        }
    }
}
