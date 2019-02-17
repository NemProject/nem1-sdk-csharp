// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="MosaicDefinitionTransaction.cs" company="Nem.io">
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
using io.nem1.sdk.Core.Crypto.Chaso.NaCl;
using io.nem1.sdk.Infrastructure.Buffers;
using io.nem1.sdk.Infrastructure.Buffers.Schema;
using io.nem1.sdk.Infrastructure.Imported.FlatBuffers;
using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Network;
using io.nem1.sdk.Model.Mosaics;

namespace io.nem1.sdk.Model.Transactions
{
    public class MosaicDefinitionTransaction : Transaction
    {
        /// <summary>
        /// Gets the mosaic levy.
        /// </summary>
        /// <value>The mosaic levy.</value>
        public MosaicLevy MosaicLevy { get; }
        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>The properties.</value>
        public MosaicProperties Properties { get; }
        /// <summary>
        /// Gets the mosaic.
        /// </summary>
        /// <value>The mosaic.</value>
        public MosaicId Mosaic { get; }
        /// <summary>
        /// Gets the creator.
        /// </summary>
        /// <value>The creator.</value>
        public PublicAccount Creator { get; }
        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MosaicDefinitionTransaction"/> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The version.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="mosaic">The mosaic.</param>
        /// <param name="mosaicLevy">The mosaic levy.</param>
        /// <param name="creator">The creator.</param>
        /// <param name="description">The description.</param>
        /// <exception cref="System.ArgumentNullException">
        /// properties
        /// or
        /// mosaic
        /// or
        /// creator
        /// </exception>
        public MosaicDefinitionTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, MosaicProperties properties, MosaicId mosaic, MosaicLevy mosaicLevy, PublicAccount creator, string description)
        {
            TransactionType = TransactionTypes.Types.MosaicDefinition;
            Version = version;
            Deadline = deadline;
            NetworkType = networkType;
            Fee = fee == 0 ? 150000 : fee;
            Properties = properties ?? throw new ArgumentNullException(nameof(properties));
            Mosaic = mosaic ?? throw new ArgumentNullException(nameof(mosaic)); 
            Creator = creator ?? throw new ArgumentNullException(nameof(creator));
            Description = description;
            MosaicLevy = mosaicLevy;
        }

        /// <summary>
        /// Initializes a new signed instance of the <see cref="MosaicDefinitionTransaction"/> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The version.</param>
        /// <param name="networkTime">The networkTime.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="mosaic">The mosaic.</param>
        /// <param name="mosaicLevy">The mosaic levy.</param>
        /// <param name="creator">The creator.</param>
        /// <param name="description">The description.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="signer">The signer.</param>
        /// <param name="transactionInfo">The transaction information.</param>
        /// <exception cref="System.ArgumentNullException">
        /// properties
        /// or
        /// mosaic
        /// or
        /// creator
        /// </exception>
        public MosaicDefinitionTransaction(NetworkType.Types networkType, int version, NetworkTime networkTime, Deadline deadline, ulong fee, MosaicProperties properties, MosaicId mosaic, MosaicLevy mosaicLevy, PublicAccount creator, string description, string signature, PublicAccount signer, TransactionInfo transactionInfo)
        {
            TransactionType = TransactionTypes.Types.MosaicDefinition;
            Version = version;
            NetworkTime = networkTime;
            Deadline = deadline;
            NetworkType = networkType;
            Fee = fee == 0 ? 150000 : fee;
            Properties = properties ?? throw new ArgumentNullException(nameof(properties));
            Mosaic = mosaic ?? throw new ArgumentNullException(nameof(mosaic)); 
            Creator = creator ?? throw new ArgumentNullException(nameof(creator));
            Description = description;
            MosaicLevy = mosaicLevy;
            Signature = signature;
            Signer = signer;
            TransactionInfo = transactionInfo;
            
        }

        /// <summary>
        /// Creates the mosaic without a levy.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="mosaic">The mosaic.</param>
        /// <param name="creator">The creator.</param>
        /// <param name="description">The description.</param>
        /// <returns>MosaicDefinitionTransaction.</returns>
        public static MosaicDefinitionTransaction CreateWithoutLevy(NetworkType.Types networkType, Deadline deadline, ulong fee, MosaicProperties properties, MosaicId mosaic, PublicAccount creator, string description)
        {
            return new MosaicDefinitionTransaction(networkType, 1, deadline, fee, properties, mosaic, null, creator, description);
        }
        /// <summary>
        /// Creates the mosaic with a levy.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="mosaic">The mosaic.</param>
        /// <param name="levy">The levy.</param>
        /// <param name="creator">The creator.</param>
        /// <param name="description">The description.</param>
        /// <returns>MosaicDefinitionTransaction.</returns>
        public static MosaicDefinitionTransaction CreateWithLevy(NetworkType.Types networkType, Deadline deadline, ulong fee, MosaicProperties properties, MosaicId mosaic, MosaicLevy levy, PublicAccount creator, string description)
        {
            return new MosaicDefinitionTransaction(networkType, 1, deadline, fee, properties, mosaic, levy, creator, description);
        }
        /// <summary>
        /// Creates a mosaic without levy.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="mosaic">The mosaic.</param>
        /// <param name="creator">The creator.</param>
        /// <param name="description">The description.</param>
        /// <returns>MosaicDefinitionTransaction.</returns>
        public static MosaicDefinitionTransaction CreateWithoutLevy(NetworkType.Types networkType, Deadline deadline, MosaicProperties properties, MosaicId mosaic, PublicAccount creator, string description)
        {
            return new MosaicDefinitionTransaction(networkType, 1, deadline, 150000, properties, mosaic, null, creator, description);
        }
        /// <summary>
        /// Creates a mosaic with a levy.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="mosaic">The mosaic.</param>
        /// <param name="levy">The levy.</param>
        /// <param name="creator">The creator.</param>
        /// <param name="description">The description.</param>
        /// <returns>MosaicDefinitionTransaction.</returns>
        public static MosaicDefinitionTransaction CreateWithLevy(NetworkType.Types networkType, Deadline deadline, MosaicProperties properties, MosaicId mosaic, MosaicLevy levy, PublicAccount creator, string description)
        {
            return new MosaicDefinitionTransaction(networkType, 1, deadline, 150000, properties, mosaic, levy, creator, description);
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);

            var signer = MosaicDefinitionTransactionBuffer.CreatePublicKeyVector(builder, GetSigner());
            var creatorVector = MosaicDefinitionTransactionBuffer.CreateCreatorPublicKeyVector(builder, Creator.PublicKey.FromHex());
            var descriptionVector = MosaicDefinitionTransactionBuffer.CreateDescriptionStringVector(builder, Encoding.UTF8.GetBytes(Description));
            var nammespaceVector = MosaicDefinitionTransactionBuffer.CreateNamespaceIdStringVector(builder, Encoding.UTF8.GetBytes(Mosaic.NamespaceId));
            var mosaicNameVector = MosaicDefinitionTransactionBuffer.CreateMosaicNameStringVector(builder, Encoding.UTF8.GetBytes(Mosaic.Name));
            var feeSinkVector = MosaicDefinitionTransactionBuffer.CreateFeeSinkAddressVector(builder, Encoding.UTF8.GetBytes(new Address("53e140b5947f104cabc2d6fe8baedbc30ef9a0609c717d9613de593ec2a266d3", NetworkType).Plain));
            var definitionStructureLength = 60 + Encoding.UTF8.GetBytes(Mosaic.NamespaceId).Length + Encoding.UTF8.GetBytes(Mosaic.Name).Length + Encoding.UTF8.GetBytes(Description).Length;

            var propertiesOffset = new Offset<MosaicPropertyBuffer>[4];

            foreach (var property in Properties.Properties)
            {             
                var name = Encoding.Default.GetBytes(property.Item1);
                var value = Encoding.Default.GetBytes(property.Item2.ToLower());

                definitionStructureLength += 12 + name.Length + value.Length;

                var nameVector = MosaicPropertyBuffer.CreatePropertyNameVector(builder, name);
                var valueVector = MosaicPropertyBuffer.CreatePropertyValueVector(builder, value);

                MosaicPropertyBuffer.StartMosaicPropertyBuffer(builder);

                MosaicPropertyBuffer.AddLengthOfPropertyStructure(builder, 8 + name.Length  + value.Length);
                MosaicPropertyBuffer.AddLengthOfPropertyName(builder, name.Length);
                MosaicPropertyBuffer.AddPropertyName(builder, nameVector);
                MosaicPropertyBuffer.AddLengthOfPropertyValue(builder, value.Length);
                MosaicPropertyBuffer.AddPropertyValue(builder, valueVector);

                propertiesOffset[Properties.Properties.IndexOf(property)] = MosaicPropertyBuffer.EndMosaicPropertyBuffer(builder);
            }

            var mosaicPropertyVector = MosaicDefinitionTransactionBuffer.CreatePropertiesVector(builder, propertiesOffset);

            var levyVector = new Offset<MosaicLevyBuffer>[1];

            if (MosaicLevy != null)
            {
                var mosaicName = Encoding.UTF8.GetBytes(MosaicLevy.Name);
                var namespaceName = Encoding.UTF8.GetBytes(MosaicLevy.NamespaceId);
                var recipient = Encoding.UTF8.GetBytes(MosaicLevy.Recipient.Plain);

                definitionStructureLength += 68 + namespaceName.Length + mosaicName.Length;

                var mosaicLevyNameVector = MosaicLevyBuffer.CreateMosaicNameStringVector(builder, mosaicName);
                var namespaceLevyNameVector = MosaicLevyBuffer.CreateNamespaceIdStringVector(builder, namespaceName);
                var recipientVector = MosaicLevyBuffer.CreateRecipientAddressVector(builder, recipient);

                MosaicLevyBuffer.StartMosaicLevyBuffer(builder);

                MosaicLevyBuffer.AddLengthOfLevyStructure(builder, 68 + namespaceName.Length + mosaicName.Length);
                MosaicLevyBuffer.AddFeeType(builder, MosaicLevy.FeeType);
                MosaicLevyBuffer.AddLengthOfRecipientAddress(builder, 40);
                MosaicLevyBuffer.AddRecipientAddress(builder, recipientVector);
                MosaicLevyBuffer.AddLengthOfMosaicIdStructure(builder, 8 + namespaceName.Length + mosaicName.Length);
                MosaicLevyBuffer.AddLengthOfMosaicNamespaceId(builder, namespaceName.Length);
                MosaicLevyBuffer.AddNamespaceIdString(builder, namespaceLevyNameVector);
                MosaicLevyBuffer.AddLengthMosaicNameString(builder, mosaicName.Length);
                MosaicLevyBuffer.AddMosaicNameString(builder, mosaicLevyNameVector);
                MosaicLevyBuffer.AddFeeQuantity(builder, MosaicLevy.Fee);
                levyVector[0] = MosaicLevyBuffer.EndMosaicLevyBuffer(builder);
            }
            else {
                MosaicLevyBuffer.StartMosaicLevyBuffer(builder);
                MosaicLevyBuffer.AddLengthOfLevyStructure(builder, 0);
                levyVector[0] = MosaicLevyBuffer.EndMosaicLevyBuffer(builder);
            }

            var levyVectorOffset = MosaicDefinitionTransactionBuffer.CreateMosaicLevyVector(builder, levyVector);
           
            MosaicDefinitionTransactionBuffer.StartMosaicDefinitionTransactionBuffer(builder);

            var namespaceBytes = Encoding.UTF8.GetBytes(Mosaic.NamespaceId);
            var mosaicNameBytes = Encoding.UTF8.GetBytes(Mosaic.Name);
            var descriptionBytes = Encoding.UTF8.GetBytes(Description);

            MosaicDefinitionTransactionBuffer.AddTransactionType(builder, TransactionType.GetValue());
            MosaicDefinitionTransactionBuffer.AddVersion(builder, BitConverter.ToInt16(new byte[] { ExtractVersion(Version), 0 }, 0));
            MosaicDefinitionTransactionBuffer.AddNetwork(builder, BitConverter.ToInt16(new byte[] { 0, NetworkType.GetNetwork() }, 0));
            MosaicDefinitionTransactionBuffer.AddTimestamp(builder, NetworkTime.EpochTimeInMilliSeconds());
            MosaicDefinitionTransactionBuffer.AddPublicKeyLen(builder, 32);
            MosaicDefinitionTransactionBuffer.AddPublicKey(builder, signer);
            MosaicDefinitionTransactionBuffer.AddFee(builder, Fee);
            MosaicDefinitionTransactionBuffer.AddDeadline(builder, Deadline.TimeStamp);
            MosaicDefinitionTransactionBuffer.AddMosaicDefinitionStructureLength(builder, definitionStructureLength);
            MosaicDefinitionTransactionBuffer.AddLengthCreatorPublicKey(builder, 32);
            MosaicDefinitionTransactionBuffer.AddCreatorPublicKey(builder, creatorVector);
            MosaicDefinitionTransactionBuffer.AddLengthOfMosaicIdStructure(builder, 8 + namespaceBytes.Length + mosaicNameBytes.Length);
            MosaicDefinitionTransactionBuffer.AddLengthOfNamespaceIdString(builder, namespaceBytes.Length);
            MosaicDefinitionTransactionBuffer.AddNamespaceIdString(builder, nammespaceVector);
            MosaicDefinitionTransactionBuffer.AddLengthOfMosaicNameString(builder, mosaicNameBytes.Length);
            MosaicDefinitionTransactionBuffer.AddMosaicNameString(builder, mosaicNameVector);
            MosaicDefinitionTransactionBuffer.AddLengthOfDescription(builder, descriptionBytes.Length);
            MosaicDefinitionTransactionBuffer.AddDescriptionString(builder, descriptionVector);
            MosaicDefinitionTransactionBuffer.AddNoOfProperties(builder, 4);
            MosaicDefinitionTransactionBuffer.AddProperties(builder, mosaicPropertyVector);
            MosaicDefinitionTransactionBuffer.AddMosaicLevy(builder, levyVectorOffset);
            MosaicDefinitionTransactionBuffer.AddLenFeeSinkAddress(builder, 40);
            MosaicDefinitionTransactionBuffer.AddFeeSinkAddress(builder, feeSinkVector);
            MosaicDefinitionTransactionBuffer.AddFeeQuantity(builder, 10000000);

            var transction = MosaicDefinitionTransactionBuffer.EndMosaicDefinitionTransactionBuffer(builder);
            builder.Finish(transction.Value);
           
            var temp = new MosaicDefinitionTransactionSchema().Serialize(builder.SizedByteArray());

            return MosaicLevy == null ? temp.Take(temp.Length - 61).Concat(temp.Skip(temp.Length - 52)).ToArray() : temp; // hackery to remove default flatbuffers values when no levy is included.
        }
    }
}
