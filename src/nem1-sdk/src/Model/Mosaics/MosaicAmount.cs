// ***********************************************************************
// Assembly         : nem1-sdk
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 11-07-2018
// ***********************************************************************
// <copyright file="Mosaic.cs" company="Nem.io">   
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

using Newtonsoft.Json.Linq;
using System;

namespace io.nem1.sdk.Model.Mosaics
{
    /// <summary>
    /// A MosaicAmount determines the Amount of a Mosaic, defined by its MosaicInfo. MosaicAmounts can be transferred with a transfer transaction.
    /// </summary>
    public class MosaicAmount
    {
        /// <summary>
        /// Gets the MosaicInfo.
        /// </summary>
        public MosaicInfo MosaicInfo { get; private set; }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public ulong Amount { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MosaicAmount"/> class.
        /// </summary>
        /// <param name="namespaceId"></param>
        /// <param name="mosaicName"></param>
        /// <param name="amount"></param>
        public MosaicAmount(string namespaceId, string mosaicName, ulong amount)
        {
            MosaicInfo = new MosaicInfo(namespaceId, mosaicName);
            Amount = amount;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MosaicAmount"/> class.
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="amount"></param>
        public MosaicAmount(string fullName, ulong amount)
        {
            MosaicInfo = new MosaicInfo(fullName);
            Amount = amount;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MosaicAmount"/> class.
        /// </summary>
        /// <param name="mosaicInfo"></param>
        /// <param name="amount"></param>
        public MosaicAmount(MosaicInfo mosaicInfo, ulong amount)
        {
            MosaicInfo = mosaicInfo;
            Amount = amount;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MosaicAmount"/> class.
        /// </summary>
        /// <param name="mosaicInfo"></param>
        /// <param name="decimalAmount"></param>
        public MosaicAmount(MosaicInfo mosaicInfo, double decimalAmount)
        {
            MosaicInfo = mosaicInfo;
            SetDecimalAmount(decimalAmount);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MosaicAmount"/> class.
        /// </summary>
        /// <param name="oAcctMosaic"></param>
        public MosaicAmount(JObject oAcctMosaic) 
            :this(oAcctMosaic["mosaicId"]["namespaceId"].ToString(), oAcctMosaic["mosaicId"]["name"].ToString(), ulong.Parse(oAcctMosaic["quantity"].ToString())) { }

        /// <summary>
        /// Set the MosaicInfo
        /// </summary>
        /// <param name="mosaicInfo"></param>
        public void SetMosaicInfo(MosaicInfo mosaicInfo)
        {
            if (!mosaicInfo.FullName().Equals(this.MosaicInfo.FullName())) throw new ArgumentException(mosaicInfo.FullName() + " is not valid");
            MosaicInfo = mosaicInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MosaicAmount"/> class, using a decimal amount with a number of decimals.
        ///     The number of possibkle decimals depend on the divisibility of the mosaic.
        /// </summary>
        /// <param name="decimalAmount"></param>
        public void SetDecimalAmount(double decimalAmount)
        {
            if (MosaicInfo.Properties == null) throw new ArgumentException(decimalAmount.ToString() + " is not valid since the MosiacInfo's Divisibility is unknown.");
            Amount = (ulong)(Math.Pow(10, MosaicInfo.Properties.Divisibility) * decimalAmount);
        }

        /// <summary>
        /// gets the Amount of the mosaic as a decimal number, using the divisibility of the MMosaic
        /// </summary>
        /// <returns></returns>
        public double GetDecimalAmount()
        {
            if (MosaicInfo.Properties == null) throw new Exception("The DecimalAmount cannot be determined since the MosiacInfo's Divisibility is unknown.");
            return Amount / Math.Pow(10, MosaicInfo.Properties.Divisibility);
        }
    }
}
