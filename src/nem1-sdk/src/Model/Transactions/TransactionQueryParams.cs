// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="TransactionQueryParams.cs" company="Nem.io">
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

namespace io.nem1.sdk.Model.Transactions
{
    /// <summary>
    /// Class QueryParams.
    /// </summary>
    public class TransactionQueryParams
    {
        /// <summary>
        /// The hash of the transaction upto which transactions should be returned.
        /// </summary>
        private readonly string _hash;
        /// <summary>
        /// The database identifier upto which transactions should be returned.
        /// </summary>
        private readonly string _id;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionQueryParams"/> class.
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <param name="id">The identifier.</param>
        public TransactionQueryParams(string hash = null, string id = null)
        {
            _hash = hash;
            _id = id;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <returns>String.</returns>
        public string GetId() { return _id; }

        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public string GetHash() { return _hash; }
    }
}
