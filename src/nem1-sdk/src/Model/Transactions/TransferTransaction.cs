// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="TransferTransaction.cs" company="Nem.io">
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
using System.Collections.Generic;
using System.Text;
using io.nem1.sdk.Infrastructure.Buffers;
using io.nem1.sdk.Infrastructure.Buffers.Schema;
using io.nem1.sdk.Infrastructure.Imported.FlatBuffers;
using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Blockchain;
using io.nem1.sdk.Model.Mosaics;
using io.nem1.sdk.Model.Network;
using io.nem1.sdk.Model.Transactions.Messages;

namespace io.nem1.sdk.Model.Transactions
{
    /// <summary>
    /// Class TransferTransaction.
    /// </summary>
    /// <seealso cref="Transaction" />
    public class TransferTransaction : Transaction
    {
        /// <summary>
        /// Gets the address.
        /// </summary>
        /// <value>The address.</value>
        public Address Address { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public IMessage Message { get; private set; }

        /// <summary>
        /// Gets the mosaics.
        /// </summary>
        /// <value>The mosaics.</value>
        public List<Mosaic> Mosaics { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferTransaction"/> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The version.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="mosaics">The mosaics.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentNullException">recipient</exception>
        internal TransferTransaction(NetworkType.Types networkType, byte version, Deadline deadline, ulong fee, Address recipient, List<Mosaic> mosaics, IMessage message)
        {
            Address = recipient ?? throw new ArgumentNullException(nameof(recipient));
            TransactionType = TransactionTypes.Types.Transfer;
            Version = version;
            Deadline = deadline;
            Message = message ?? EmptyMessage.Create();
            Mosaics = mosaics ?? new List<Mosaic>(); ;
            NetworkType = networkType;
            Fee = fee == 0 ? CalculateFee() : fee;          
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferTransaction"/> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The version.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="mosaics">The mosaics.</param>
        /// <param name="message">The message.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="signer">The signer.</param>
        /// <param name="transactionInfo">The transaction information.</param>
        /// <exception cref="System.ArgumentNullException">recipient</exception>
        internal TransferTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, Address recipient, List<Mosaic> mosaics, IMessage message, string signature, PublicAccount signer, TransactionInfo transactionInfo)
        {
            Address = recipient ?? throw new ArgumentNullException(nameof(recipient));
            Mosaics = mosaics ?? new List<Mosaic>();
            TransactionType = TransactionTypes.Types.Transfer;
            Version = version;
            Deadline = deadline;
            Message = message ?? EmptyMessage.Create();
            NetworkType = networkType;    
            Signature = signature;
            Signer = signer;
            TransactionInfo = transactionInfo;
            Fee = fee == 0 ? CalculateFee() : fee;
        }

        /// <summary>
        /// Creates the specified netowrk type.
        /// </summary>
        /// <param name="netowrkType">Type of the netowrk.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="address">The address.</param>
        /// <param name="mosaics">The mosaics.</param>
        /// <param name="message">The message.</param>
        /// <returns>TransferTransaction.</returns>
        public static TransferTransaction Create(NetworkType.Types netowrkType, Deadline deadline, ulong fee, Address address, List<Mosaic> mosaics, IMessage message)
        {
            return new TransferTransaction(netowrkType, 2, deadline, fee, address, mosaics, message);
        }
        /// <summary>
        /// Creates the specified netowrk type.
        /// </summary>
        /// <param name="netowrkType">Type of the netowrk.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="address">The address.</param>
        /// <param name="mosaics">The mosaics.</param>
        /// <param name="message">The message.</param>
        /// <returns>TransferTransaction.</returns>
        public static TransferTransaction Create(NetworkType.Types netowrkType, Deadline deadline, Address address, List<Mosaic> mosaics, IMessage message)
        {
            return new TransferTransaction(netowrkType, 2, deadline, 0, address, mosaics, message);
        }

        /// <summary>
        /// Generates the bytes.
        /// </summary>
        /// <returns>The transaction bytes.</returns>
        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);

            var signer = TransferTransactionBuffer.CreatePublicKeyVector(builder, GetSigner());
            var recipientVector = TransferTransactionBuffer.CreateRecipientVector(builder, Encoding.UTF8.GetBytes(Address.Plain));

            if (Message == null) Message = EmptyMessage.Create();
            var messageBytePayload = Message.GetPayload();
            var payload = MessageBuffer.CreatePayloadVector(builder, messageBytePayload);

            var messageBuff = new Offset<MessageBuffer>[1];
            MessageBuffer.StartMessageBuffer(builder);
            
            if (messageBytePayload.Length > 0) MessageBuffer.AddType(builder, Message.GetMessageType().GetValue());
            if (messageBytePayload.Length > 0) MessageBuffer.AddPayloadLen(builder, messageBytePayload.Length);
            if (messageBytePayload.Length > 0) MessageBuffer.AddPayload(builder, payload);

            messageBuff[0] = MessageBuffer.EndMessageBuffer(builder);

            var messageVector = TransferTransactionBuffer.CreateMessageVector(builder, messageBuff);
            
            var mosaics = new Offset<MosaicBuffer>[Mosaics.Count];
            for (var index = 0; index < Mosaics.Count; index++)
            {
                var mosaic = Mosaics[index];
                var namespaceVector = MosaicBuffer.CreateNamespaceStringVector(builder, Encoding.UTF8.GetBytes(mosaic.NamespaceName));
                var mosaicNameVector = MosaicBuffer.CreateMosaicIdStringVector(builder, Encoding.UTF8.GetBytes(mosaic.MosaicName));

                MosaicBuffer.StartMosaicBuffer(builder);

                var nsLen = Encoding.Default.GetBytes(mosaic.NamespaceName).Length;
                var msLen = Encoding.Default.GetBytes(mosaic.MosaicName).Length;

                MosaicBuffer.AddMosaicStructureLen(builder, 20 + nsLen + msLen);
                MosaicBuffer.AddMosaicIdStructureLen(builder, nsLen + msLen + 8);
                MosaicBuffer.AddNamespaceIdLen(builder, nsLen);
                MosaicBuffer.AddNamespaceString(builder, namespaceVector);
                MosaicBuffer.AddMosaicIdLen(builder, msLen);
                MosaicBuffer.AddMosaicIdString(builder, mosaicNameVector);
                MosaicBuffer.AddQuantity(builder, mosaic.Amount);

                mosaics[index] = MosaicBuffer.EndMosaicBuffer(builder);
            }

            var mosaicsVector = TransferTransactionBuffer.CreateMosaicsVector(builder, mosaics);

            TransferTransactionBuffer.StartTransferTransactionBuffer(builder);

            TransferTransactionBuffer.AddTransactionType(builder, TransactionType.GetValue());
            TransferTransactionBuffer.AddVersion(builder, BitConverter.ToInt16(new byte[] { ExtractVersion(Version), 0 }, 0 ));
            TransferTransactionBuffer.AddNetwork(builder, BitConverter.ToInt16(new byte[] { 0, NetworkType.GetNetwork() }, 0 ));           
            TransferTransactionBuffer.AddTimestamp(builder, NetworkTime.EpochTimeInMilliSeconds());
            TransferTransactionBuffer.AddPublicKeyLen(builder, 32);
            TransferTransactionBuffer.AddPublicKey(builder, signer);
            TransferTransactionBuffer.AddFee(builder, Fee);
            TransferTransactionBuffer.AddDeadline(builder, Deadline.Ticks);        
            TransferTransactionBuffer.AddRecipientLen(builder, 40);
            TransferTransactionBuffer.AddRecipient(builder, recipientVector);
            TransferTransactionBuffer.AddAmount(builder, 1000000);          
            TransferTransactionBuffer.AddMessageFieldLen(builder, Message.GetLength() == 0 ? 0 : Message.GetLength() + 8);
            TransferTransactionBuffer.AddMessage(builder, messageVector);
            TransferTransactionBuffer.AddNoOfMosaics(builder, Mosaics.Count);
            TransferTransactionBuffer.AddMosaics(builder, mosaicsVector);

            var codedTransfer = TransferTransactionBuffer.EndTransferTransactionBuffer(builder);
            builder.Finish(codedTransfer.Value);

            return new TransferTransactionSchema().Serialize(builder.SizedByteArray());
        }

        private ulong CalculateFee()
        {
            switch (NetworkType)
            {
                case Blockchain.NetworkType.Types.MIJIN:
                    return 0;
                case Blockchain.NetworkType.Types.MIJIN_TEST:
                    return 0;
            }

             Mosaics.ForEach(m =>
             {
                 foreach (MosaicConfigElement Config in new ConfigDiscovery().GetConfig("MosaicConfigElement"))
                 {
                     if (Config.MosaicID == m.NamespaceName + ":" +m.MosaicName)
                     {
                         var q = m.Amount;
                         var d = Convert.ToUInt32(Config.Divisibility);
                         var s = Convert.ToUInt64(Config.InitialSupply);
                         
                         if (s <= 10000 && d == 0)
                         {
                             Fee += 50000;
                         }
                         else
                         {
                             // get xem equivilent
                             var xemEquivalent = 8999999999 * (q / Math.Pow(10, d)) / (s * 10 ^ d) * 1000000;

                             // apply xem transfer fee formula 
                             var xemFee = Math.Max(1, Math.Min((long)Math.Ceiling((decimal)xemEquivalent / 1000000000), 25)) * 1000000;

                             // Adjust fee based on supply
                             const long maxMosaicQuantity = 9000000000000000;

                             // get total mosaic quantity
                             var totalMosaicQuantity = s * Math.Pow(10, d);

                             // get supply related adjustment
                             var supplyRelatedAdjustment = Math.Floor(0.8 * Math.Log(maxMosaicQuantity / totalMosaicQuantity)) * 1000000;

                             // get final individual mosaic fee
                             var individualMosaicfee = (ulong)Math.Max(1000000, xemFee - supplyRelatedAdjustment);

                             // add individual fee to total fee for all mosaics to be sent 
                             Fee += individualMosaicfee / 20;
                         }
                     }
                 }
             });
            
            return Fee += Message.GetLength() > 0
                ? 50000 * (ulong)Math.Floor((double)Message.GetLength() / 32 + 1)
                : 0;
        }
    }  
}
