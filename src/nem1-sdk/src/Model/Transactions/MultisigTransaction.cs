// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="MultisigTransaction.cs" company="Nem.io">
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
using io.nem1.sdk.Core.Crypto.Chaso.NaCl;
using io.nem1.sdk.Infrastructure.Buffers;
using io.nem1.sdk.Infrastructure.Buffers.Schema;
using io.nem1.sdk.Infrastructure.Imported.FlatBuffers;
using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Blockchain;
using io.nem1.sdk.Model.Network;

namespace io.nem1.sdk.Model.Transactions
{

    public class MultisigTransaction : Transaction
    {
        /// <summary>
        /// Gets the inner transaction.
        /// </summary>
        /// <value>The inner transaction.</value>
        public Transaction InnerTransaction { get; }

        /// <summary>
        /// Gets the cosignatures.
        /// </summary>
        /// <value>The cosignatures.</value>
        public List<CosignatureTransaction> Cosignatures { get; }

        /// <summary>
        /// Gets the signed inner transaction.
        /// </summary>
        /// <value>The signed inner transaction.</value>
        internal SignedTransaction SignedInnerTransaction { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultisigTransaction"/> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The version.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="innerTransaction">The inner transaction.</param>
        public MultisigTransaction(NetworkType.Types networkType, byte version, Deadline deadline, ulong fee, Transaction innerTransaction)
        {
            TransactionType = TransactionTypes.Types.Multisig;
            NetworkType = networkType;
            Version = version;
            Deadline = deadline;
            Fee = fee == 0 ? 150000 : fee;
            InnerTransaction = innerTransaction;
        }

        /// <summary>
        /// Initializes a new signed instance of the <see cref="MultisigTransaction"/> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The version.</param>
        /// <param name="networkTime">The networkTime.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="innerTransaction">The inner transaction.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="signer">The signer.</param>
        /// <param name="transactionInfo">The transaction information.</param>
        public MultisigTransaction(NetworkType.Types networkType, int version, NetworkTime networkTime, Deadline deadline, ulong fee, Transaction innerTransaction, string signature, PublicAccount signer, TransactionInfo transactionInfo)
        {
            TransactionType = TransactionTypes.Types.Multisig;
            NetworkType = networkType;
            Version = version;
            NetworkTime = networkTime;
            Deadline = deadline;
            Fee = fee  == 0 ? 150000 : fee;
            InnerTransaction = innerTransaction;
            Signer = signer;
            Signature = signature;
            TransactionInfo = transactionInfo;
        }

        /// <summary>
        /// Initializes a new sigend instance of the <see cref="MultisigTransaction"/> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The version.</param>
        /// <param name="networkTime">The networkTime.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="innerTransaction">The inner transaction.</param>
        /// <param name="cosignatures">The cosignatures.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="signer">The signer.</param>
        /// <param name="transactionInfo">The transaction information.</param>
        public MultisigTransaction(NetworkType.Types networkType, int version, NetworkTime networkTime, Deadline deadline, ulong fee, Transaction innerTransaction, List<CosignatureTransaction> cosignatures, string signature, PublicAccount signer, TransactionInfo transactionInfo)
        {
            TransactionType = TransactionTypes.Types.Multisig;
            NetworkType = networkType;
            Version = version;
            NetworkTime = networkTime;
            Deadline = deadline;
            Fee = fee == 0 ? 150000 : fee;
            InnerTransaction = innerTransaction;
            Signer = signer;
            Signature = signature;
            TransactionInfo = transactionInfo;
            Cosignatures = cosignatures;
        }

        /// <summary>
        /// Creates an instance of MultisigTransaction.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>MultisigTransaction.</returns>
        public static MultisigTransaction Create(NetworkType.Types network, Deadline deadline, ulong fee, Transaction transaction)
        {
            return new MultisigTransaction(network, 1, deadline, fee, transaction);
        }

        /// <summary>
        /// Creates an instance of MultisigTransaction.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>MultisigTransaction.</returns>
        public static MultisigTransaction Create(NetworkType.Types network, Deadline deadline, Transaction transaction)
        {
            return new MultisigTransaction(network, 1, deadline, 150000, transaction);
        }

        /// <summary>
        /// Signs the MultisigTransaction with a KeyPair and sets the inner transaction signer.
        /// </summary>
        /// <param name="cosignatory">The cosignatory.</param>
        /// <param name="multisigAccount">The multisig account.</param>
        /// <returns>SignedMultisigTransaction.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// cosignatory
        /// or
        /// multisigAccount
        /// </exception>
        public SignedMultisigTransaction SignWith(KeyPair cosignatory, PublicAccount multisigAccount)
        {
            if (cosignatory == null) throw new ArgumentNullException(nameof(cosignatory));
            if (multisigAccount == null) throw new ArgumentNullException(nameof(multisigAccount));

            Signer = new PublicAccount(cosignatory.PublicKeyString, NetworkType);
            InnerTransaction.Signer = multisigAccount;
            Bytes = GenerateBytes();
            var sig = TransactionExtensions.SignTransaction(cosignatory, Bytes);
            return SignedMultisigTransaction.Create(Bytes, sig, TransactionExtensions.Hasher(Bytes), InnerTransaction.CreateTransactionHash().FromHex(), cosignatory.PublicKey, TransactionType);
        }

        internal override byte[] GenerateBytes()
        {
            var innerTransactionBytes = InnerTransaction.GenerateBytes();
            var builder = new FlatBufferBuilder(1);
            var signer = MultisigTransactionBuffer.CreatePublicKeyVector(builder, GetSigner());         
            var innerTransaction = MultisigTransactionBuffer.CreateInnerTransactionBytesVector(builder, innerTransactionBytes);
           
            MultisigTransactionBuffer.StartMultisigTransactionBuffer(builder);

            MultisigTransactionBuffer.AddTransactionType(builder, TransactionType.GetValue());
            MultisigTransactionBuffer.AddVersion(builder, BitConverter.ToInt16(new byte[] { ExtractVersion(Version), 0 }, 0));
            MultisigTransactionBuffer.AddNetwork(builder, BitConverter.ToInt16(new byte[] { 0, NetworkType.GetNetwork() }, 0));
            MultisigTransactionBuffer.AddTimestamp(builder, NetworkTime.EpochTimeInMilliSeconds()); 
            MultisigTransactionBuffer.AddPublicKeyLen(builder, 32); 
            MultisigTransactionBuffer.AddPublicKey(builder, signer);
            MultisigTransactionBuffer.AddFee(builder, Fee);
            MultisigTransactionBuffer.AddDeadline(builder, Deadline.TimeStamp); 
            MultisigTransactionBuffer.AddInnerTransactionLength(builder, innerTransactionBytes.Length); 
            MultisigTransactionBuffer.AddInnerTransactionBytes(builder, innerTransaction);

            var codedTransfer = MultisigTransactionBuffer.EndMultisigTransactionBuffer(builder);
            builder.Finish(codedTransfer.Value);

            return new MultisigTransactionSchema().Serialize(builder.SizedByteArray());
        }
    }
}
