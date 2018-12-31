// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 11-07-2018
// ***********************************************************************
// <copyright file="Message.cs" company="Nem.io">
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
using System.Text;

namespace io.nem1.sdk.Model.Transactions.Messages
{
    /// <summary>
    /// Class PlainMessage.
    /// </summary>
    /// <seealso cref="IMessage" />
    public class PlainMessage : IMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        private int Type { get; }

        /// <summary>
        /// Gets the payload.
        /// </summary>
        /// <value>The payload.</value>
        private byte[] Payload { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlainMessage"/> class.
        /// </summary>
        /// <param name="payload">The payload.</param>
        protected PlainMessage(byte[] payload)
        {
            Type = MessageType.Type.UNENCRYPTED.GetValue();
            Payload = payload;
        }

        /// <summary>
        /// Creates the specified payload.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <returns>PlainMessage.</returns>
        /// <exception cref="ArgumentNullException">payload</exception>
        public static PlainMessage Create(string payload)
        {
            if (payload == null) throw new ArgumentNullException(nameof(payload));

            return new PlainMessage(Encoding.UTF8.GetBytes(payload));
        }

        /// <summary>
        /// Gets the payload.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public override byte[] GetPayload()
        {
            return Payload;
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <returns>System.UInt32.</returns>
        public override int GetLength()
        {
            return Payload.Length;
        }

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        /// <returns>System.Byte.</returns>
        internal override MessageType.Type GetMessageType()
        {
            return MessageType.GetRawValue(Type);
        }

        /// <summary>
        /// Gets the String payload.
        /// </summary>
        /// <returns>System.String.</returns>
        public virtual string GetStringPayload()
        {
            return Encoding.UTF8.GetString(Payload);
        }
    }
}
