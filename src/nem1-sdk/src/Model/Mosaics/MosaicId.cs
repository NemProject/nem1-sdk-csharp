// ***********************************************************************
// Assembly         : nem1-sdk
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 11-07-2018
// ***********************************************************************
// <copyright file="MosaicId.cs" company="Nem.io">   
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
using io.nem1.sdk.Model.Namespace;

namespace io.nem1.sdk.Model.Mosaics
{
    /// <summary>
    /// A mosaicId describes an instance of a mosaic definition.
    /// Mosaics can be transferred by means of a transfer transaction.
    /// </summary>
    public class MosaicId
    {
        ///// <summary>
        ///// The NEM namespace name
        ///// </summary>
        public const string NEM = "nem";

        ///// <summary>
        ///// The XEM mosaic name
        ///// </summary>
        public const string XEM = "xem";

        ///// <summary>
        ///// The XEM mosaic FullName
        ///// </summary>
        public const string XEMfullName = NEM + ":" + XEM;

        /// <summary>
        /// Gets the namespace identifier.
        /// </summary>
        /// <value>The namespace identifier.</value>
        public string NamespaceId { get; private set; }

        /// <summary>
        /// The mosaic name.
        /// </summary>
        /// <value>The name of the mosaic.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName()
        {
            if (IsFullNamePresent) return NamespaceId + ":" + Name;
            else return "";
        }

        /// <summary>
        /// Describes if the namespace name is present.
        /// </summary>
        /// <returns><c>true</c> if MosaicName is present, <c>false</c> otherwise.</returns>
        public bool IsNamePresent => Name != null;

        /// <summary>
        /// Describes if the full name is present.
        /// </summary>
        /// <returns><c>true</c> if FullName is present, <c>false</c> otherwise.</returns>
        public bool IsFullNamePresent => (NamespaceId != null && IsNamePresent);

        /// <summary>
        /// Constructs a new instance of the <see cref="MosaicId"/> class.
        /// </summary>
        /// <param name="namespaceId"></param>
        /// <param name="name"></param>
        private void ConstructMosaicId(string namespaceId, string name)
        {
            NamespaceId = namespaceId ?? throw new ArgumentNullException(nameof(namespaceId));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            if (namespaceId == "") throw new ArgumentException(namespaceId + " is not valid");
            if (name == "") throw new ArgumentException(name + " is not valid");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MosaicId"/> class.
        /// </summary>
        /// <param name="namespaceId"></param>
        /// <param name="name"></param>
        public MosaicId(string namespaceId, string name)
        {
            ConstructMosaicId(namespaceId, name);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MosaicId"/> class.
        /// </summary>
        /// <param name="fullName">The identifier.</param>
        /// <exception cref="System.ArgumentException">
        /// </exception>
        public MosaicId(string fullName)
        {
            if (string.IsNullOrEmpty(fullName)) throw new ArgumentException(fullName + " is not valid");
            if (!fullName.Contains(":")) throw new ArgumentException(fullName + " is not valid");
            var parts = fullName.Split(':');
            if (parts.Length != 2) throw new ArgumentException(fullName + " is not valid");
            ConstructMosaicId(parts[0], parts[1]);
        }
    }
}
