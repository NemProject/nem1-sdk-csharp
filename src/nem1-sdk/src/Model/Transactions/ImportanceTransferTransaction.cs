// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="ImportanceTransferTransaction.cs" company="Nem.io">
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
using io.nem1.sdk.Core.Crypto.Chaso.NaCl;
using io.nem1.sdk.Infrastructure.Buffers;
using io.nem1.sdk.Infrastructure.Buffers.Schema;
using io.nem1.sdk.Infrastructure.Imported.FlatBuffers;
using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Blockchain;
using io.nem1.sdk.Model.Network;

namespace io.nem1.sdk.Model.Transactions
{
    /// <summary>
    /// Class ImportanceTransferTransaction.
    /// </summary>
    /// <seealso cref="Transaction" />
    public class ImportanceTransferTransaction : Transaction
    {
        /// <summary>
        /// Gets the remote account.
        /// </summary>
        /// <value>The remote account.</value>
        public PublicAccount RemoteAccount { get; }
        /// <summary>
        /// Gets the mode.
        /// </summary>
        /// <value>The mode.</value>
        public ImportanceTransferMode.Mode Mode { get; }

        /// <summary>
        /// Initializes a new signed instance of the <see cref="ImportanceTransferTransaction"/> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The version.</param>
        /// <param name="networkTime">The networkTime.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="remoteAccount">The remote account.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="signer">The signer.</param>
        /// <param name="transactionInfo">The transaction information.</param>
        public ImportanceTransferTransaction(NetworkType.Types networkType, int version, NetworkTime networkTime, Deadline deadline, ulong fee, ImportanceTransferMode.Mode mode, PublicAccount remoteAccount, string signature, PublicAccount signer, TransactionInfo transactionInfo)
        {
            TransactionType = TransactionTypes.Types.ImportanceTransfer;
            Version = version;
            NetworkTime = networkTime;
            Deadline = deadline;
            NetworkType = networkType;
            Signature = signature;
            Signer = signer;
            TransactionInfo = transactionInfo;
            Fee = fee == 0 ? 150000 : fee;
            RemoteAccount = remoteAccount;
            Mode = mode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportanceTransferTransaction"/> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The version.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="remoteAccount">The remote account.</param>
        public ImportanceTransferTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, ImportanceTransferMode.Mode mode, PublicAccount remoteAccount)
        {
            TransactionType = TransactionTypes.Types.ImportanceTransfer;
            Version = version;
            Deadline = deadline;
            NetworkType = networkType;
            Fee = fee == 0 ? 150000 : fee;
            RemoteAccount = remoteAccount;
            Mode = mode;
        }

        /// <summary>
        /// Creates an instance of ImportanceTransferTransaction.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="remoteAccount">The remote account.</param>
        /// <returns>ImportanceTransferTransaction.</returns>
        public static ImportanceTransferTransaction Create(NetworkType.Types network, Deadline deadline, ulong fee, ImportanceTransferMode.Mode mode, PublicAccount remoteAccount)
        {
            return new ImportanceTransferTransaction(network, 1, deadline, fee, mode, remoteAccount);
        }

        /// <summary>
        /// Creates an instance of ImportanceTransferTransaction.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="remoteAccount">The remote account.</param>
        /// <returns>ImportanceTransferTransaction.</returns>
        public static ImportanceTransferTransaction Create(NetworkType.Types network, Deadline deadline, ImportanceTransferMode.Mode mode, PublicAccount remoteAccount)
        {
            return new ImportanceTransferTransaction(network, 1, deadline, 150000, mode, remoteAccount);
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);
            var signer = ImportanceTransferBuffer.CreatePublicKeyVector(builder, GetSigner());
            var remote = ImportanceTransferBuffer.CreateRemotePublicKeyVector(builder, RemoteAccount.PublicKey.FromHex());
           
            ImportanceTransferBuffer.StartImportanceTransferBuffer(builder);

            ImportanceTransferBuffer.AddTransactionType(builder, TransactionType.GetValue()); 
            ImportanceTransferBuffer.AddVersion(builder, BitConverter.ToInt16(new byte[] { ExtractVersion(Version), 0 }, 0));
            ImportanceTransferBuffer.AddNetwork(builder, BitConverter.ToInt16(new byte[] { 0, NetworkType.GetNetwork() }, 0)); 
            ImportanceTransferBuffer.AddTimestamp(builder, NetworkTime.EpochTimeInMilliSeconds()); 
            ImportanceTransferBuffer.AddPublicKeyLen(builder, 32); 
            ImportanceTransferBuffer.AddPublicKey(builder, signer);
            ImportanceTransferBuffer.AddFee(builder, Fee);
            ImportanceTransferBuffer.AddDeadline(builder, Deadline.TimeStamp);
            ImportanceTransferBuffer.AddMode(builder, Mode.GetValue());
            ImportanceTransferBuffer.AddRemotePublicKeyLen(builder, 32);
            ImportanceTransferBuffer.AddRemotePublicKey(builder, remote);

            var codedTransfer = ImportanceTransferBuffer.EndImportanceTransferBuffer(builder);
            builder.Finish(codedTransfer.Value);

            return new ImportanceTransferSchema().Serialize(builder.SizedByteArray());
        }
    }
}
