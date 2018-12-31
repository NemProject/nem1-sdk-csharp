// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="EmptyMessage.cs" company="Nem.io">
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

namespace io.nem1.sdk.Model.Transactions.Messages
{
    /// <summary>
    /// Class EmptyMessage.
    /// </summary>
    /// <seealso cref="IMessage" />
    public class EmptyMessage : IMessage
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        private int Type { get; }

        /// <summary>
        /// Prevents a default instance of the <see cref="EmptyMessage"/> class from being created.
        /// </summary>
        private EmptyMessage()
        {
            Type = MessageType.Type.UNENCRYPTED.GetValue();          
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>EmptyMessage.</returns>
        public static EmptyMessage Create()
        {
            return new EmptyMessage();
        }

        /// <summary>
        /// Gets the payload.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public override byte[] GetPayload()
        {
            return new byte[]{};
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <returns>System.UInt32.</returns>
        public override int GetLength()
        {
            return 0;
        }

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        /// <returns>System.Byte.</returns>
        internal override MessageType.Type GetMessageType()
        {
            return MessageType.GetRawValue(Type);
        }

    }
}
