// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 01-31-2018
// ***********************************************************************
// <copyright file="NetworkTime.cs" company="Nem.io">
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

namespace io.nem1.sdk.Model.Network
{
    /// <summary>
    /// NetworkTime
    /// </summary>
    public class NetworkTime
    {
        public static readonly DateTime Epoch = new DateTime(2015, 03, 29, 0, 6, 25, 0, DateTimeKind.Utc); // Time of NEMesis block

        /// <summary>
        /// Gets the time since genesis block in miliseconds.
        /// </summary>
        /// <returns>The network time.</returns>
        public static int EpochTimeInMilliSeconds()
        {
            var timespan = DateTime.UtcNow - Epoch;
            return (int)timespan.TotalMilliseconds;
        }

        /// <summary>
        /// Gets the TimeStamp in seconds since the NEMesis block.
        /// </summary>
        /// <value>The TimeStamp.</value>
        internal int TimeStamp { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkTime"/> class.
        /// </summary>
        /// <param name="timestamp">The timestamp in seconds since the NEMesis block.</param>
        public NetworkTime(int timestamp)
        {
            TimeStamp = timestamp;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkTime"/> class.
        /// </summary>
        /// <param name="timespan">The timespan.</param>
        public NetworkTime(TimeSpan timespan)
        {
            var timespanNow = DateTime.UtcNow - NetworkTime.Epoch;
            TimeStamp = (int)timespanNow.Add(timespan).TotalSeconds;
        }

        /// <summary>
        /// Gets the UTC date time.
        /// </summary>
        /// <returns>DateTime.</returns>
        public DateTime GetUtcDateTime()
        {
            const long TICKSPERSECOND = 10000000;   // .NET library DateTime ticks definition
            return new DateTime((long)TimeStamp * TICKSPERSECOND + NetworkTime.Epoch.Ticks, DateTimeKind.Utc);  // DateTime constructor with Ticks as parameter
        }

        /// <summary>
        /// Gets the local date time.
        /// </summary>
        /// <param name="timeZoneInfo">The time zone information.</param>
        /// <returns>DateTime.</returns>
        public DateTime GetLocalDateTime(TimeZoneInfo timeZoneInfo)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(GetUtcDateTime(), timeZoneInfo);
        }

        /// <summary>
        /// Gets the local date time.
        /// </summary>
        /// <returns>DateTime.</returns>
        public DateTime GetLocalDateTime()
        {
            return GetLocalDateTime(TimeZoneInfo.Local);
        }

    }
}
