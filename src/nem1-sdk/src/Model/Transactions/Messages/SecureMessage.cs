// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 11-07-2018
// ***********************************************************************
// <copyright file="SecureMessage.cs" company="Nem.io">
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

using io.nem1.sdk.Core.Crypto;
using io.nem1.sdk.Core.Crypto.Chaso.NaCl;

namespace io.nem1.sdk.Model.Network.Messages
{
    /// <summary>
    /// Class SecureMessage.
    /// </summary>
    /// <seealso cref="IMessage" />
    public class SecureMessage : IMessage
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        private int Type { get; }
        /// <summary>
        /// Gets the payload.
        /// </summary>
        /// <value>The payload.</value>
        private byte[] Payload { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureMessage"/> class.
        /// </summary>
        /// <param name="payload">The payload.</param>
        public SecureMessage(byte[] payload)
        {
            Type = MessageType.Type.ENCRYPTED.GetValue();
            Payload = payload;
        }

        /// <summary>
        /// Creates the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="senderPrivateKey">The sender private key.</param>
        /// <param name="receiverPublicKey">The receiver public key.</param>
        /// <returns>SecureMessage.</returns>
        public static SecureMessage Create(string msg, string senderPrivateKey, string receiverPublicKey)
        {
            return new SecureMessage(CryptoUtils.AsymetricEncode(msg, senderPrivateKey, receiverPublicKey).FromHex());
        }

        /// <summary>
        /// Gets the decoded payload.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="publicKey">The public key.</param>
        /// <returns>System.String.</returns>
        public string GetDecodedPayload(string privateKey, string publicKey)
        {
            return CryptoUtils.AsymetricDecode(Payload.ToHexLower(), privateKey, publicKey);
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
    }
}
