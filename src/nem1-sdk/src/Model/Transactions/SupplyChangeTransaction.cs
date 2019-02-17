// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="SupplyChangeTransaction.cs" company="Nem.io">
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
using io.nem1.sdk.Infrastructure.Buffers;
using io.nem1.sdk.Infrastructure.Buffers.Schema;
using io.nem1.sdk.Infrastructure.Imported.FlatBuffers;
using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Network;
using io.nem1.sdk.Model.Mosaics;

namespace io.nem1.sdk.Model.Transactions
{
    public class SupplyChangeTransaction : Transaction
    {
        /// <summary>
        /// Gets the amount by which the suppply should change.
        /// </summary>
        /// <value>The delta.</value>
        public ulong Delta { get; }
        /// <summary>
        /// Gets the identifier of the mosaic to modify.
        /// </summary>
        /// <value>The mosaic identifier.</value>
        public MosaicId MosaicId { get; }
        /// <summary>
        /// Gets the direction of the supply modification.
        /// </summary>
        /// <value>The type of the supply.</value>
        public int SupplyType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SupplyChangeTransaction"/> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The version.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="delta">The delta.</param>
        /// <param name="mosaicId">The mosaic identifier.</param>
        /// <param name="supplyType">Type of the supply.</param>
        public SupplyChangeTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, ulong delta, MosaicId mosaicId, int supplyType)
        {
            TransactionType = TransactionTypes.Types.SupplyChange;
            Version = version;
            Deadline = deadline;
            NetworkType = networkType;
            Fee = fee == 0 ? 150000 : fee;
            Delta = delta;
            MosaicId = mosaicId;
            SupplyType = supplyType;
        }

        /// <summary>
        /// Initializes a new signed instance of the <see cref="SupplyChangeTransaction"/> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The version.</param>
        /// <param name="networkTime">The networkTime.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="delta">The delta.</param>
        /// <param name="mosaicId">The mosaic identifier.</param>
        /// <param name="supplyType">Type of the supply.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="signer">The signer.</param>
        /// <param name="transactionInfo">The transaction information.</param>
        public SupplyChangeTransaction(NetworkType.Types networkType, int version, NetworkTime networkTime, Deadline deadline, ulong fee, ulong delta, MosaicId mosaicId, int supplyType, string signature, PublicAccount signer, TransactionInfo transactionInfo)
        {
            TransactionType = TransactionTypes.Types.SupplyChange;
            Version = version;
            NetworkTime = networkTime;
            Deadline = deadline;
            NetworkType = networkType;
            Signature = signature;
            Signer = signer;
            TransactionInfo = transactionInfo;
            Fee = fee == 0 ? 150000 : fee;
            Delta = delta;
            MosaicId = mosaicId;
            SupplyType = supplyType;
        }

        /// <summary>
        /// Create a supply increase instance of SupplyChangeTransaction.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="delta">The delta.</param>
        /// <param name="mosaicId">The mosaic identifier.</param>
        /// <returns>SupplyChangeTransaction.</returns>
        public static SupplyChangeTransaction CreateIncrease(NetworkType.Types network, Deadline deadline, ulong fee, ulong delta, MosaicId mosaicId)
        {
            return new SupplyChangeTransaction(network, 1, deadline, fee, delta, mosaicId, 1);
        }

        /// <summary>
        ///  Creates a supply decrease instance of SupplyChangeTransaction.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="delta">The delta.</param>
        /// <param name="mosaicId">The mosaic identifier.</param>
        /// <returns>SupplyChangeTransaction.</returns>
        public static SupplyChangeTransaction CreateDecrease(NetworkType.Types network, Deadline deadline, ulong fee, ulong delta, MosaicId mosaicId)
        {
            return new SupplyChangeTransaction(network, 1, deadline, fee, delta, mosaicId, 2);
        }

        /// <summary>
        ///  Creates a supply increase instance of SupplyChangeTransaction.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="delta">The delta.</param>
        /// <param name="mosaicId">The mosaic identifier.</param>
        /// <returns>SupplyChangeTransaction.</returns>
        public static SupplyChangeTransaction CreateIncrease(NetworkType.Types network, Deadline deadline, ulong delta, MosaicId mosaicId)
        {
            return new SupplyChangeTransaction(network, 1, deadline, 150000, delta, mosaicId, 1);
        }

        /// <summary>
        ///  Creates a supply decrease instance of SupplyChangeTransaction.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="delta">The delta.</param>
        /// <param name="mosaicId">The mosaic identifier.</param>
        /// <returns>SupplyChangeTransaction.</returns>
        public static SupplyChangeTransaction CreateDecrease(NetworkType.Types network, Deadline deadline, ulong delta, MosaicId mosaicId)
        {
            return new SupplyChangeTransaction(network, 1, deadline, 150000, delta, mosaicId, 2);
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);
            var mosaicName = Encoding.UTF8.GetBytes(MosaicId.Name);
            var namespaceName = Encoding.UTF8.GetBytes(MosaicId.NamespaceId);
            var signerVector = SupplyChangeBuffer.CreatePublicKeyVector(builder, GetSigner());
            var namespaceVector = SupplyChangeBuffer.CreateNamespaceIdstringVector(builder, namespaceName);
            var mosaicNameVector = SupplyChangeBuffer.CreateMosaicNameStringVector(builder, mosaicName);

            SupplyChangeBuffer.StartSupplyChangeBuffer(builder);

            SupplyChangeBuffer.AddTransactionType(builder, TransactionType.GetValue()); 
            SupplyChangeBuffer.AddVersion(builder, BitConverter.ToInt16(new byte[] { ExtractVersion(Version), 0 }, 0)); 
            SupplyChangeBuffer.AddNetwork(builder, BitConverter.ToInt16(new byte[] { 0, NetworkType.GetNetwork() }, 0));
            SupplyChangeBuffer.AddTimestamp(builder, NetworkTime.EpochTimeInMilliSeconds());
            SupplyChangeBuffer.AddPublicKeyLen(builder, 32); 
            SupplyChangeBuffer.AddPublicKey(builder, signerVector);
            SupplyChangeBuffer.AddFee(builder, Fee);
            SupplyChangeBuffer.AddDeadline(builder, Deadline.TimeStamp); 
            SupplyChangeBuffer.AddLengthOfMosaicIdStructure(builder, 8 + namespaceName.Length + mosaicName.Length);
            SupplyChangeBuffer.AddLengthOfNamespaceIdString(builder, namespaceName.Length);
            SupplyChangeBuffer.AddNamespaceIdstring(builder, namespaceVector);
            SupplyChangeBuffer.AddLengthOfMosaicNameString(builder, mosaicName.Length);
            SupplyChangeBuffer.AddMosaicNameString(builder, mosaicNameVector);
            SupplyChangeBuffer.AddSupplyType(builder, SupplyType);
            SupplyChangeBuffer.AddDelta(builder, Delta);

            var codedTransfer = ImportanceTransferBuffer.EndImportanceTransferBuffer(builder);
            builder.Finish(codedTransfer.Value);

            return new SupplyChangeTransactionSchema().Serialize(builder.SizedByteArray());
        }
    }
}
