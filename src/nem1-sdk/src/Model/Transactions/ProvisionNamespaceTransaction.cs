// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="ProvisionNamespaceTransaction.cs" company="Nem.io">
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
using System.Linq;
using System.Text;
using io.nem1.sdk.Infrastructure.Buffers;
using io.nem1.sdk.Infrastructure.Buffers.Schema;
using io.nem1.sdk.Infrastructure.Imported.FlatBuffers;
using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Network;

namespace io.nem1.sdk.Model.Transactions
{
    public class ProvisionNamespaceTransaction : Transaction
    {
        /// <summary>
        /// Gets the new part of the namespace.
        /// </summary>
        /// <remarks>
        /// This is the root if Parent is absent. Otherwise, its the sub-nammespace name.
        /// </remarks>
        /// <value>The new part.</value>
        public string NewPart { get; }
        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public string Parent { get; }

        /// <summary>
        /// Initializes a new signed instance of the <see cref="ProvisionNamespaceTransaction"/> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The version.</param>
        /// <param name="networkTime">The networkTime.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="newPart">The new part.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="signer">The signer.</param>
        /// <param name="transactionInfo">The transaction information.</param>
        public ProvisionNamespaceTransaction(NetworkType.Types networkType, int version, NetworkTime networkTime, Deadline deadline, ulong fee, string newPart, string parent, string signature, PublicAccount signer, TransactionInfo transactionInfo)
        {
            TransactionType = TransactionTypes.Types.ProvisionNamespace;
            Version = version;
            NetworkTime = networkTime;
            Deadline = deadline;
            NetworkType = networkType;
            Signature = signature;
            Signer = signer;
            TransactionInfo = transactionInfo;
            Fee = fee == 0 ? 150000 : fee;
            Parent = parent;
            NewPart = newPart;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvisionNamespaceTransaction"/> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The version.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="newPart">The new part.</param>
        /// <param name="parent">The parent.</param>
        public ProvisionNamespaceTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, string newPart, string parent)
        {
            TransactionType = TransactionTypes.Types.ProvisionNamespace;
            Version = version;
            Deadline = deadline;
            NetworkType = networkType;
            Fee = fee == 0 ? 150000 : fee;
            Parent = parent;
            NewPart = newPart;
        }

        /// <summary>
        /// Creates a sub namespace instance of ProvisionNamespaceTransaction 
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="newPart">The new part.</param>
        /// <param name="parent">The parent.</param>
        /// <returns>ProvisionNamespaceTransaction.</returns>
        public static ProvisionNamespaceTransaction CreateSubNamespace(NetworkType.Types network, Deadline deadline, ulong fee, string newPart, string parent)
        {
            return new ProvisionNamespaceTransaction(network, 1, deadline, fee, newPart, parent);
        }

        /// <summary>
        /// Creates a root namespace instance of ProvisionNamespaceTransaction 
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="newPart">The new part.</param>
        /// <returns>ProvisionNamespaceTransaction.</returns>
        public static ProvisionNamespaceTransaction CreateRootNamespace(NetworkType.Types network, Deadline deadline, ulong fee, string newPart)
        {
            return new ProvisionNamespaceTransaction(network, 1, deadline, fee, newPart, null);
        }

        /// <summary>
        /// Creates a sub namespace instance of ProvisionNamespaceTransaction 
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="newPart">The new part.</param>
        /// <param name="parent">The parent.</param>
        /// <returns>ProvisionNamespaceTransaction.</returns>
        public static ProvisionNamespaceTransaction CreateSubNamespace(NetworkType.Types network, Deadline deadline, string newPart, string parent)
        {
            return new ProvisionNamespaceTransaction(network, 1, deadline, 150000, newPart, parent);
        }

        /// <summary>
        /// Creates a root namespace instance of ProvisionNamespaceTransaction 
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="newPart">The new part.</param>
        /// <returns>ProvisionNamespaceTransaction.</returns>
        public static ProvisionNamespaceTransaction CreateRootnamespace(NetworkType.Types network, Deadline deadline, string newPart)
        {
            return new ProvisionNamespaceTransaction(network, 1, deadline, 150000, newPart, null);
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);
            var signer = ProvisionNamespaceTransactionBuffer.CreatePublicKeyVector(builder, GetSigner());
            var sink = ProvisionNamespaceTransactionBuffer.CreateSinkAddressStringVector(builder, GetFeeSinkAddress());
            var newPart = ProvisionNamespaceTransactionBuffer.CreateNewPartStringVector(builder, Encoding.UTF8.GetBytes(NewPart));

            var parent = new VectorOffset();
            if (Parent != null){
                parent = ProvisionNamespaceTransactionBuffer.CreateParentStringVector(builder, Encoding.UTF8.GetBytes(Parent));
            }
           
            ProvisionNamespaceTransactionBuffer.StartProvisionNamespaceTransaction(builder);

            ProvisionNamespaceTransactionBuffer.AddTransactionType(builder, TransactionType.GetValue()); 
            ProvisionNamespaceTransactionBuffer.AddVersion(builder, BitConverter.ToInt16(new byte[] { ExtractVersion(Version), 0 }, 0)); 
            ProvisionNamespaceTransactionBuffer.AddNetwork(builder, BitConverter.ToInt16(new byte[] { 0, NetworkType.GetNetwork() }, 0)); 
            ProvisionNamespaceTransactionBuffer.AddTimestamp(builder, NetworkTime.EpochTimeInMilliSeconds()); 
            ProvisionNamespaceTransactionBuffer.AddPublicKeyLen(builder, 32); 
            ProvisionNamespaceTransactionBuffer.AddPublicKey(builder, signer); 
            ProvisionNamespaceTransactionBuffer.AddFee(builder, Fee); 
            ProvisionNamespaceTransactionBuffer.AddDeadline(builder, Deadline.TimeStamp); 
            ProvisionNamespaceTransactionBuffer.AddSinkAddressLength(builder, 40); 
            ProvisionNamespaceTransactionBuffer.AddSinkAddressString(builder, sink); 
            ProvisionNamespaceTransactionBuffer.AddRentalFee(builder, Parent == null ? 0x5F5E100 : 0x989680); 
            ProvisionNamespaceTransactionBuffer.AddNewPartLength(builder, Encoding.UTF8.GetBytes(NewPart).Length);
            ProvisionNamespaceTransactionBuffer.AddNewPartString(builder, newPart); 
            ProvisionNamespaceTransactionBuffer.AddParentLength(builder, Parent == null ? 4294967295 : (uint)Encoding.UTF8.GetBytes(Parent).Length); // 14
            if(Parent != null) ProvisionNamespaceTransactionBuffer.AddParentString(builder, parent);

            var codedTransfer = ProvisionNamespaceTransactionBuffer.EndProvisionNamespaceTransaction(builder);
            builder.Finish(codedTransfer.Value);

            var tempBytes = new ProvisionNamespaceSchema().Serialize(builder.SizedByteArray());     
            return Parent == null ? tempBytes.Take(tempBytes.Length - 1).ToArray() : tempBytes;
        }

        private byte[] GetFeeSinkAddress()
        {
            return Encoding.UTF8.GetBytes(new Address("3e82e1c1e4a75adaa3cba8c101c3cd31d9817a2eb966eb3b511fb2ed45b8e262", NetworkType).Plain);
        }
    }
}
