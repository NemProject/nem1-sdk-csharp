using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using io.nem1.sdk.Core.Crypto.Chaso.NaCl;
using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Network;
using io.nem1.sdk.Model.Mosaics;
using io.nem1.sdk.Model.Transactions.Messages;
using io.nem1.sdk.Model.Transactions;
using Newtonsoft.Json.Linq;

namespace io.nem1.sdk.Infrastructure.Mapping
{
    internal class TransactionMapping   {
        
        internal virtual Transaction Apply(string input)
        {
            var transaction = JObject.Parse(input)["transaction"] ?? JObject.Parse(input);
            
            var type = transaction["type"].ToObject<int>();

            if (type == TransactionTypes.Types.Transfer.GetValue())
            {
                return new TransferTransactionMapping().Apply(input);
            }
            if (type == TransactionTypes.Types.Multisig.GetValue())
            {
                return new MultisigTransactionMapping().Apply(input);
            }
            if (type == TransactionTypes.Types.ImportanceTransfer.GetValue())
            {
                return new ImportanceTransferTransactionMapping().Apply(input);
            }
            if (type == TransactionTypes.Types.SignatureTransaction.GetValue())
            {
                return new SignatureTransactionMapping().Apply(input);
            }
            if (type == TransactionTypes.Types.MultisigAggregateModification.GetValue())
            {
                return new MultisigAggregateModificatonTransactionMapping().Apply(input);
            }
            if (type == TransactionTypes.Types.MosaicDefinition.GetValue())
            {
                return new MosaicDefinitionCreationTransactionMapping().Apply(input);
            }
            if (type == TransactionTypes.Types.SupplyChange.GetValue())
            {
                return new SupplyChangeTransactionMapping().Apply(input);
            }
            if (type == TransactionTypes.Types.ProvisionNamespace.GetValue())
            {
                return new ProvisionNamespaceTransactionMapping().Apply(input);
            }
            throw new Exception("Unimplemented Transaction type");
        }

        protected NetworkType.Types ExtractNetworkType(int version)
        {
            var netBytes = BitConverter.GetBytes(version).ToHexLower();
            return NetworkType.GetRawValue(Convert.ToInt32(netBytes.Substring(4, 4), 16));
        }

        protected int ExtractVersion(int version)
        {
            var netBytes = BitConverter.GetBytes(version).ToHexLower();
            return Convert.ToInt32(netBytes.Substring(0, 2), 16);
        }

        protected TransactionInfo CreateTransactionInfo(JObject metaJsonObject)
        {   
            if (metaJsonObject["innerHash"].ToString() == "{}") // Not a Multisig
            {
                return TransactionInfo.Create(ulong.Parse(metaJsonObject["height"].ToString()),
                        int.Parse(metaJsonObject["id"].ToString()),
                        metaJsonObject["hash"]["data"].ToString());
            }
            return TransactionInfo.CreateMultisig(ulong.Parse(metaJsonObject["height"].ToString()),
                        int.Parse(metaJsonObject["id"].ToString()),
                        metaJsonObject["hash"]["data"].ToString(),
                        metaJsonObject["innerHash"]["data"].ToString());
        }

        protected IMessage RetrieveMessage(JObject msg)
        {
            if (msg["payload"].ToString().Substring(0,2) == "fe" && Regex.IsMatch(msg["payload"].ToString(), @"\A\b[0-9a-fA-F]+\b\Z"))
            {
                return new HexMessage(msg["payload"].ToString().FromHex());
            }
            if (int.Parse(msg["type"].ToString()) == 1)
            {
                return PlainMessage.Create(Encoding.UTF8.GetString(msg["payload"].ToString().FromHex()));
            }
            if (int.Parse(msg["type"].ToString()) == 2)
            {
                return new SecureMessage(msg["payload"].ToString().FromHex());
            }
            throw new Exception("invalid message type");
        }

        protected TransferTransaction ToTransferTransaction(JObject tx, TransactionInfo txInfo)
        {
            return new TransferTransaction(
                ExtractNetworkType(int.Parse(tx["version"].ToString())),
                ExtractVersion(int.Parse(tx["version"].ToString())),
                new NetworkTime(int.Parse(tx["timeStamp"].ToString())),
                new Deadline(int.Parse(tx["deadline"].ToString())),
                ulong.Parse(tx["fee"].ToString()),
                new Address(tx["recipient"].ToString()),
                tx["mosaics"] == null
                    ? new List<Mosaic>() { new Mosaic("nem", "xem", ulong.Parse(tx["amount"].ToString())) }
                    : tx["mosaics"]?.Select(m => new Mosaic(m["mosaicId"]["namespaceId"].ToString(), m["mosaicId"]["name"].ToString(), ulong.Parse(m["quantity"].ToString()))).ToList(),
                tx["message"].ToString() == "{}" ? EmptyMessage.Create() : RetrieveMessage(tx["message"].ToObject<JObject>()),
                tx["signature"]?.ToString(),
                new PublicAccount(tx["signer"].ToString(), ExtractNetworkType(int.Parse(tx["version"].ToString()))),
                txInfo
            );
        }

        protected ImportanceTransferTransaction ToImportanceTransfer(JObject tx, TransactionInfo txInfo)
        {
            return new ImportanceTransferTransaction(
                ExtractNetworkType(int.Parse(tx["version"].ToString())),
                ExtractVersion(int.Parse(tx["version"].ToString())),
                new NetworkTime(int.Parse(tx["timeStamp"].ToString())),
                new Deadline(int.Parse(tx["deadline"].ToString())),
                ulong.Parse(tx["fee"].ToString()),
                ImportanceTransferMode.GetMode(int.Parse(tx["mode"].ToString())),
                new PublicAccount(tx["remoteAccount"].ToString(), ExtractNetworkType(int.Parse(tx["version"].ToString()))),
                tx["signature"]?.ToString(),
                new PublicAccount(tx["signer"].ToString(), ExtractNetworkType(int.Parse(tx["version"].ToString()))),
                txInfo
            );
        }

        protected MultisigTransaction ToMultisigTransaction(JObject tx, TransactionInfo txInfo)
        {
            return new MultisigTransaction(
                ExtractNetworkType(int.Parse(tx["version"].ToString())),
                ExtractVersion(int.Parse(tx["version"].ToString())),
                new NetworkTime(int.Parse(tx["timeStamp"].ToString())),
                new Deadline(int.Parse(tx["deadline"].ToString())),
                ulong.Parse(tx["fee"].ToString()),
                new TransactionMapping().Apply(tx["otherTrans"].ToString()),
                tx["signatures"]?.Select(e => new SignatureTransactionMapping().Apply(e.ToString())).ToList(),
                tx["signature"].ToString(),
                new PublicAccount(tx["signer"].ToString(), ExtractNetworkType(int.Parse(tx["version"].ToString()))),
                txInfo
            );
        }

        protected CosignatureTransaction ToSignatureTransaction(JObject tx, TransactionInfo txInfo)
        {
            return new CosignatureTransaction(
                ExtractNetworkType(int.Parse(tx["version"].ToString())),
                ExtractVersion(int.Parse(tx["version"].ToString())),
                new NetworkTime(int.Parse(tx["timeStamp"].ToString())),
                new Deadline(int.Parse(tx["deadline"].ToString())),
                ulong.Parse(tx["fee"].ToString()),
                tx["otherHash"]["data"].ToString(),
                new Address(tx["otherAccount"].ToString()),
                tx["signature"].ToString(),
                new PublicAccount(tx["signer"].ToString(), ExtractNetworkType(int.Parse(tx["version"].ToString()))),
                txInfo
            );
        }

        protected MosaicDefinitionTransaction ToMosaicDefinitionTransaction(JObject tx, TransactionInfo txInfo)
        {
            return new MosaicDefinitionTransaction(
                ExtractNetworkType(int.Parse(tx["version"].ToString())),
                ExtractVersion(int.Parse(tx["version"].ToString())),
                new NetworkTime(int.Parse(tx["timeStamp"].ToString())),
                new Deadline(int.Parse(tx["deadline"].ToString())),
                ulong.Parse(tx["fee"].ToString()),
                new MosaicProperties(
                    int.Parse(tx["mosaicDefinition"]["properties"].ToList()[0]["value"].ToString()),
                    ulong.Parse(tx["mosaicDefinition"]["properties"].ToList()[1]["value"].ToString()),
                    bool.Parse(tx["mosaicDefinition"]["properties"].ToList()[2]["value"].ToString()),
                    bool.Parse(tx["mosaicDefinition"]["properties"].ToList()[3]["value"].ToString())),
                MosaicId.CreateFromMosaicIdentifier(tx["mosaicDefinition"]["id"]["namespaceId"] + ":" + tx["mosaicDefinition"]["id"]["name"]),
                tx["mosaicDefinition"]["levy"].ToString() == "{}" 
                    ? null 
                    : new MosaicLevy(
                        Mosaic.CreateFromIdentifier(
                            (tx["mosaicDefinition"]["levy"]["mosaicId"]["namespaceId"] + ":" + tx["mosaicDefinition"]["levy"]["mosaicId"]["name"]), 
                            ulong.Parse(tx["mosaicDefinition"]["levy"]["fee"].ToString())), 
                        int.Parse(tx["mosaicDefinition"]["levy"]["type"].ToString()),
                        new Address(tx["mosaicDefinition"]["levy"]["recipient"].ToString())),
                new PublicAccount(tx["mosaicDefinition"]["creator"].ToString(), ExtractNetworkType(int.Parse(tx["version"].ToString()))),
                tx["mosaicDefinition"]["description"].ToString(),
                tx["signature"].ToString(),
                new PublicAccount(tx["signer"].ToString(), ExtractNetworkType(int.Parse(tx["version"].ToString()))),
                txInfo
            );
        }

        protected MultisigAggregateModificationTransaction ToMultisigModificationTransaction(JObject tx, TransactionInfo txInfo)
        {
            return new MultisigAggregateModificationTransaction(
                ExtractNetworkType(int.Parse(tx["version"].ToString())),
                ExtractVersion(int.Parse(tx["version"].ToString())),
                new NetworkTime(int.Parse(tx["timeStamp"].ToString())),
                new Deadline(int.Parse(tx["deadline"].ToString())),
                ulong.Parse(tx["fee"].ToString()),
                tx["minCosignatories"].ToString() != "{}" ? int.Parse(tx["minCosignatories"]["relativeChange"].ToString()) : 0, // missing from transaction data 
                tx["modifications"]?.Select(
                    i => new MultisigModification(
                        new PublicAccount(i["cosignatoryAccount"].ToString(), NetworkType.Types.TEST_NET ), 
                        CosignatoryModificationType.GetRawValue(
                            int.Parse(i["modificationType"].ToString())))).ToList(), // list of modifications               
                tx["signature"]?.ToString(),
                new PublicAccount(tx["signer"].ToString(), ExtractNetworkType(int.Parse(tx["version"].ToString()))),
                txInfo
            );
        }

        protected SupplyChangeTransaction ToSupplyChangeTransaction(JObject tx, TransactionInfo txInfo)
        {
            return new SupplyChangeTransaction(
                ExtractNetworkType(int.Parse(tx["version"].ToString())),
                ExtractVersion(int.Parse(tx["version"].ToString())),
                new NetworkTime(int.Parse(tx["timeStamp"].ToString())),
                new Deadline(int.Parse(tx["deadline"].ToString())),
                ulong.Parse(tx["fee"].ToString()),
                ulong.Parse(tx["delta"].ToString()),
                MosaicId.CreateFromMosaicIdentifier(tx["mosaicId"]["namespaceId"] + ":" + tx["mosaicId"]["name"]),
                int.Parse(tx["supplyType"].ToString()),
                tx["signature"].ToString(),
                new PublicAccount(tx["signer"].ToString(), ExtractNetworkType(int.Parse(tx["version"].ToString()))),
                txInfo
            );
        }

        protected ProvisionNamespaceTransaction ToProvisionNamespaceTransaction(JObject tx, TransactionInfo txInfo)
        {
            return new ProvisionNamespaceTransaction(
                ExtractNetworkType(int.Parse(tx["version"].ToString())),
                ExtractVersion(int.Parse(tx["version"].ToString())),
                new NetworkTime(int.Parse(tx["timeStamp"].ToString())),
                new Deadline(int.Parse(tx["deadline"].ToString())),
                ulong.Parse(tx["fee"].ToString()),
                tx["newPart"].ToString(),
                tx["parent"].ToString(),
                tx["signature"].ToString(),
                new PublicAccount(tx["signer"].ToString(), ExtractNetworkType(int.Parse(tx["version"].ToString()))),
                txInfo
            );
        }
    }
    
    internal class SupplyChangeTransactionMapping : TransactionMapping
    {
        internal new SupplyChangeTransaction Apply(string input)
        {
            var tx = JObject.Parse(input)["transaction"] ?? JObject.Parse(input);
            var txInfo = JObject.Parse(input)["meta"] != null ? CreateTransactionInfo(JObject.Parse(input)["meta"].ToObject<JObject>()) : null;
            return ToSupplyChangeTransaction(tx.ToObject<JObject>(), txInfo);
        }
    }

    internal class TransferTransactionMapping : TransactionMapping
    {
        internal new TransferTransaction Apply(string input)
        {
             var tx = JObject.Parse(input)["transaction"] ?? JObject.Parse(input);           
             var txInfo = JObject.Parse(input)["meta"] != null ? CreateTransactionInfo(JObject.Parse(input)["meta"].ToObject<JObject>()) : null;
             return ToTransferTransaction(tx.ToObject<JObject>(), txInfo);
        }
    }

    internal class ImportanceTransferTransactionMapping : TransactionMapping
    {
        internal new ImportanceTransferTransaction Apply(string input)
        {
            var tx = JObject.Parse(input)["transaction"] ?? JObject.Parse(input);
            var txInfo = JObject.Parse(input)["meta"] != null ? CreateTransactionInfo(JObject.Parse(input)["meta"].ToObject<JObject>()) : null;
            return ToImportanceTransfer(tx.ToObject<JObject>(), txInfo);
        }
    }

    internal class MultisigTransactionMapping : TransactionMapping
    {
        internal new MultisigTransaction Apply(string input)
        {
            var tx = JObject.Parse(input)["transaction"] ?? JObject.Parse(input);
            var txInfo = JObject.Parse(input)["meta"] != null ? CreateTransactionInfo(JObject.Parse(input)["meta"].ToObject<JObject>()) : null;
            return ToMultisigTransaction(tx.ToObject<JObject>(), txInfo);
        }
    }

    internal class SignatureTransactionMapping : TransactionMapping
    {
        internal new CosignatureTransaction Apply(string input)
        {
            var tx = JObject.Parse(input)["transaction"] ?? JObject.Parse(input);
            var txInfo = JObject.Parse(input)["meta"] != null ? CreateTransactionInfo(JObject.Parse(input)["meta"].ToObject<JObject>()) : null;
            return ToSignatureTransaction(tx.ToObject<JObject>(), txInfo);
        }
    }

    internal class MultisigAggregateModificatonTransactionMapping : TransactionMapping
    {
        internal new MultisigAggregateModificationTransaction Apply(string input)
        {
            var tx = JObject.Parse(input)["transaction"] ?? JObject.Parse(input);
            var txInfo = JObject.Parse(input)["meta"] != null ? CreateTransactionInfo(JObject.Parse(input)["meta"].ToObject<JObject>()) : null;
            return ToMultisigModificationTransaction(tx.ToObject<JObject>(), txInfo);
        }
    }

    internal class MosaicDefinitionCreationTransactionMapping : TransactionMapping
    {
        internal new MosaicDefinitionTransaction Apply(string input)
        {
            var tx = JObject.Parse(input)["transaction"] ?? JObject.Parse(input);
            var txInfo = JObject.Parse(input)["meta"] != null ? CreateTransactionInfo(JObject.Parse(input)["meta"].ToObject<JObject>()) : null;
            return ToMosaicDefinitionTransaction(tx.ToObject<JObject>(), txInfo);
        }
    }

    internal class ProvisionNamespaceTransactionMapping : TransactionMapping
    {
        internal new ProvisionNamespaceTransaction Apply(string input)
        {
            var tx = JObject.Parse(input)["transaction"] ?? JObject.Parse(input);
            var txInfo = JObject.Parse(input)["meta"] != null ? CreateTransactionInfo(JObject.Parse(input)["meta"].ToObject<JObject>()) : null;
            return ToProvisionNamespaceTransaction(tx.ToObject<JObject>(), txInfo);
        }
    }
}
