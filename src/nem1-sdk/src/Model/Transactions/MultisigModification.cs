// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="MultisigModification.cs" company="Nem.io">
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

using io.nem1.sdk.Model.Accounts;

namespace io.nem1.sdk.Model.Transactions
{
    /// <summary>
    /// Class MultisigModification.
    /// </summary>
    public class MultisigModification
    {
        /// <summary>
        /// Gets the cosignatory public key.
        /// </summary>
        /// <value>The cosignatory public key.</value>
        public PublicAccount CosignatoryPublicKey { get; }

        /// <summary>
        /// Gets the type of the modification.
        /// </summary>
        /// <value>The type of the modification.</value>
        public CosignatoryModificationType.Types ModificationType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultisigModification"/> class.
        /// </summary>
        /// <param name="cosignatoryPublicKey">The cosignatory public key.</param>
        /// <param name="modificationType">Type of the modification.</param>
        public MultisigModification(PublicAccount cosignatoryPublicKey, CosignatoryModificationType.Types modificationType)
        {
            CosignatoryPublicKey = cosignatoryPublicKey;
            ModificationType = modificationType;
        }
    }
}
