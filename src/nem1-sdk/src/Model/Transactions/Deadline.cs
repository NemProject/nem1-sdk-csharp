// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="Deadline.cs" company="Nem.io">
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

using io.nem1.sdk.Model.Network;
using System;

namespace io.nem1.sdk.Model.Transactions
{
    /// <summary>
    /// Class Deadline.
    /// </summary>
    public class Deadline : NetworkTime
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Deadline"/> class.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        public Deadline(int timestamp) : base(timestamp) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Deadline"/> class.
        /// </summary>
        /// <param name="time">The timespan.</param>
        public Deadline(TimeSpan timespan) : base(timespan) { }

        /// <summary>
        /// Creates a deadline in hours.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <returns>Deadline.</returns>
        public static Deadline CreateHours(int hours)
        {
            return new Deadline(TimeSpan.FromHours(hours));
        }

        /// <summary>
        /// Creates a deadline in minutes.
        /// </summary>
        /// <param name="mins">The mins.</param>
        /// <returns>Deadline.</returns>
        public static Deadline CreateMinutes(int mins)
        {
            return new Deadline(TimeSpan.FromMinutes(mins));
        }

    }
}
