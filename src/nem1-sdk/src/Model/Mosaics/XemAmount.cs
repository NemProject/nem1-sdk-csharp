// ***********************************************************************
// Assembly         : nem1-sdk
// Author           : kailin
// Created          : 01-25-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="XEM.cs" company="Nem.io">
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

namespace io.nem1.sdk.Model.Mosaics
{
    /// <summary>
    /// Describes the XEM mosaic definition
    /// see http://csharpindepth.com/Articles/General/Singleton.aspx
    /// </summary>
    public class XemAmount : MosaicAmount
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XemAmount"/> class.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public XemAmount(ulong amount) : base(MosaicInfo.Xem, amount) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="XemAmount"/> class.
        /// </summary>
        /// <param name="decimalAmount">The amount.</param>
        public XemAmount(double decimalAmount) : base(MosaicInfo.Xem, decimalAmount) { }
    }
}
