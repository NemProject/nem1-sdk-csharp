// ***********************************************************************
// Assembly         : nem1-sdk-csharp
// Author           : realgarp
// Created          : 24-01-2019
//
// Last Modified By : realgarp
// Last Modified On : 24-01-2019
// ***********************************************************************
// <copyright file="Listener.cs" company="Nem.io">
// Copyright 2019 NEM
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
// <summary>Websocket Listener</summary>
// ***********************************************************************
using System;
using System.Diagnostics;
using System.IO;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Mosaics;
using io.nem1.sdk.Model.Namespace;
using Newtonsoft.Json.Linq;

namespace io.nem1.sdk.src.Infrastructure.Listener
{
    /// <summary>
    /// Websocket Listener
    /// </summary>    
    public class Listener
    {
        const string DONE = "Done";

        public struct ChannelPaths
        {
            public const string ERRORS = "/errors";
            public const string BLOCKS = "/blocks/new";
            public const string ACCOUNT = "/account/";                                      // path to be completed with a plain account number
            public const string OWNEDNAMESPACES = "/account/namespace/owned/";              // path to be completed with a plain account number
            public const string OWNEDMOSAICS = "/account/mosaic/owned/";                    // path to be completed with a plain account number
            public const string OWNEDMOSAICDEFINITION = "/account/mosaic/owned/definition/";// path to be completed with a plain account number
            public const string RECENTTRANSACTIONS = "/recenttransactions/";                // path to be completed with a plain account number
            public const string UNCONFIRMED = "/unconfirmed/";                              // path to be completed with a plain account number
            public const string TRANSACTIONS = "/transactions/";                            // path to be completed with a plain account number
        }

        public struct ApiPaths  // these paths MAY NOT be completed with a plain account number
        {
            public const string ACCOUNT = "/w/api/account/get";
            public const string OWNEDNAMESPACES = "/w/api/account/namespace/owned";
            public const string OWNEDMOSAICS = "/w/api/account/mosaic/owned";
            public const string OWNEDMOSAICDEFINITIONS = "/w/api/account/mosaic/owned/definition";
            //public const string RECENTTRANSACTIONS = "/w/api/recenttransactions";
            //public const string UNCONFIRMED = "/w/api/unconfirmed";
            //public const string TRANSACTIONS = "/w/api/transactions";
        }

        /// <summary>
        /// Gets the client socket.
        /// </summary>
        /// <value>The client socket.</value>
        private ClientWebSocket ClientWs { get; }

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>The domain.</value>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        public int Port { get; set; }

        /// <summary>
        /// Callback function to execute when an Error message is received
        /// </summary>
        private Action<string> OnErrorEventHandler { get; set; }

        /// <summary>
        /// Callback function to execute when a Block message is received
        /// </summary>
        private Action<ulong> OnBlockEventHandler { get; set; }

        /// <summary>
        /// Callback function to execute when an Account message is received
        /// </summary>
        private Action<AccountInfo> OnAccountEventHandler { get; set; }

        /// <summary>
        /// Callback function to execute when an owned Namespaces message is received
        /// </summary>
        private Action<Address, NamespaceInfo> OnNamespaceEventHandler { get; set; }

        /// <summary>
        /// Callback function to execute when an owned Mosaics message is received
        /// </summary>
        private Action<Address, MosaicAmount> OnMosaicEventHandler { get; set; }

        /// <summary>
        /// Callback function to execute when an owned Mosaics Definitions message is received
        /// </summary>
        private Action<Address, MosaicInfo> OnMosaicDefinitionEventHandler { get; set; }

        /// <summary>
        /// Callback function to execute when a Recent Transactions message is received
        /// </summary>
        private Action<Address, string> OnRecentTransactionsEventHandler { get; set; }

        /// <summary>
        /// Callback function to execute when an Unconfirmed Transactions message is received
        /// </summary>
        private Action<Address, string> OnUnconfirmedTransactionsEventHandler { get; set; }

        /// <summary>
        /// Callback function to execute when a (confirmed) Transactions message is received
        /// </summary>
        private Action<Address, string> OnConfirmedTransactionsEventHandler { get; set; }

        /// <summary>
        ///     Counter holding the number of active subscriptions, used to determine the id of the next subscription.
        /// </summary>
        private uint SubscriptionCounter { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="domain">The host domain to listen to.</param>
        /// <param name="port">The port to listen on.</param>
        public Listener(string domain, int port = 7778)     // Default Websocket port in nem1 (used to be 7777 and changed to) 7778. In Nem2 3000 is used.
        {
            SubscriptionCounter = 0;
            ClientWs = new ClientWebSocket();
            Domain = domain;
            Port = port;
        }

        /// <summary>
        /// Opens the websocket connection and sends the CONNECT message.
        /// </summary>
        /// <returns>&lt;System.Boolean&gt; true if successful.</returns>
        public async Task<bool> ConnectAsync(Action<string> errorsEventHandler)
        {
            const string WEBSOCKETPATH = "/w/messages/websocket";    // Based on https://docs.nem.io/en/nem-dev-basics-docker/blockchain-monitoring 

            await ClientWs.ConnectAsync(new Uri(string.Concat("ws://", Domain, ":", Port, WEBSOCKETPATH)), CancellationToken.None);
            if (ClientWs.State != WebSocketState.Open) return false;
            // Send CONNECT
            StompMessage connect = new StompMessage(StompMessage.ClientCommands.CONNECT);
            // Set message headers, might not be needed
            //connect["accept-version"] = "1.1,1.0";
            //connect["heart-beat"] = "10000, 10000";  // out, in
            await SendAsync(connect);
            // Read the answer from the server
            StompMessage connectedMsg = StompMessage.Deserialize(await ReadSocketAsync());
            if (connectedMsg.Command != StompMessage.ServerResponses.CONNECTED) return false;
            // Subscribe to errors channel
            await SubscribeToErrorsAsync(errorsEventHandler);
            // Start the Loop to read answers
            Task loop = LoopReadStompMsgsAsync(); // Explicitely not using await, to allow the Loop to run asynchronously without waiting for it to complete!
            return true;
        }

        /// <summary>
        /// Sends the DISCONNECT message and Closes the websocket connection.
        /// </summary>
        /// <returns>&lt;System.Boolean&gt;.</returns>
        public async Task<bool> DisconnectAsync()
        {
            //// Send DISCONNECT, does not seem to be needed
            //StompMessage disconnect = new StompMessage(StompMessage.ClientCommands.DISCONNECT);
            //await SendAsync(disconnect);

            // Close the Websocket
            await ClientWs.CloseAsync(WebSocketCloseStatus.NormalClosure, DONE, CancellationToken.None);
            if (ClientWs.State != WebSocketState.Open) return true;
            return false;
        }

        /// <summary>
        /// Sends the StompMessage Asynchronously. Call LoopReadStompMsgsAsync to process answers from the server.
        /// </summary>
        /// <param name="stompMsg"></param>
        async Task SendAsync(StompMessage stompMsg)
        {
            byte[] encoded = Encoding.UTF8.GetBytes(stompMsg.Serialize());
            ArraySegment<byte> segment = new ArraySegment<byte>(encoded, 0, encoded.Length);
            await ClientWs.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        /// Subscribes to the channel on the destination path.
        /// </summary>
        /// <param name="destinationPath">The channel.</param>
        async Task SendSubscribeStompMsgAsync(string destinationPath, Address destinationAddress = null)
        {
            StompMessage subscribe = new StompMessage(StompMessage.ClientCommands.SUBSCRIBE);
            subscribe.SetSubscriptionId(SubscriptionCounter++);
            subscribe.SetDestination(destinationPath, destinationAddress);
            await SendAsync(subscribe);  // The LoopReadStompMsgsAsync will process the answer from the server
            Debug.WriteLine("Subscription requested on path " + subscribe.GetDestination());
        }

        /// <summary>
        /// Subscribes to channel for Errors. The Eventhandler will be invoked when a message is received on the channel.
        /// </summary>
        /// <param name="errorsEventHandler">The channel.</param>
        public async Task SubscribeToErrorsAsync(Action<string> errorsEventHandler)
        {
            this.OnErrorEventHandler = errorsEventHandler;
            await SendSubscribeStompMsgAsync(ChannelPaths.ERRORS);
        }

        /// <summary>
        /// Subscribes to the channel for new Blocks. The Eventhandler will be invoked when a message is received on the channel.
        /// </summary>
        /// <param name="blocksEventHandler">The channel.</param>
        public async Task SubscribeToBlocksAsync(Action<ulong> blocksEventHandler)
        {
            this.OnBlockEventHandler = blocksEventHandler;
            await SendSubscribeStompMsgAsync(ChannelPaths.BLOCKS);
        }

        /// <summary>
        /// Subscribes to the channel for an Account. The Eventhandler will be invoked when a message is received on the channel.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="accountEventHandler"></param>
        public async Task SubscribeToAccountAsync(string account, Action<AccountInfo> accountEventHandler)
        {
            Address destinationAddress = new Address(account);
            this.OnAccountEventHandler = accountEventHandler; 
            await SendSubscribeStompMsgAsync(ChannelPaths.ACCOUNT, destinationAddress);
            // Send an explicit message to request an immediate answer on the channel
            StompMessage send = new StompMessage(StompMessage.ClientCommands.SEND, "{'account':'" + destinationAddress.Plain + "'}");
            send.SetDestination(ApiPaths.ACCOUNT);
            await SendAsync(send);  // The LoopReadStompMsgsAsync will process the answer from the server
        }

        /// <summary>
        /// Subscribes to the channel for Namespaces owned by the given Account. The Eventhandler will be invoked when a message is received on the channel.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="namespaceEventHandler"></param>
        public async Task SubscribeToOwnedNamespacesAsync(string account, Action<Address, NamespaceInfo> namespaceEventHandler)
        {
            Address destinationAddress = new Address(account);
            this.OnNamespaceEventHandler = namespaceEventHandler;     // The callback will be invoked for each msg received per Namespace owned
            await SendSubscribeStompMsgAsync(ChannelPaths.OWNEDNAMESPACES, destinationAddress);
            // Send an explicit message to request an immediate answer on the channel
            StompMessage send = new StompMessage(StompMessage.ClientCommands.SEND, "{'account':'" + destinationAddress.Plain + "'}");
            send.SetDestination(ApiPaths.OWNEDNAMESPACES);
            await SendAsync(send);  // The LoopReadStompMsgsAsync will process the answer from the server
        }

        /// <summary>
        /// Subscribes to the channel for Mosaics owned by the given Account. The Eventhandler will be invoked when a message is received on the channel.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="mosaicEventHandler"></param>
        public async Task SubscribeToOwnedMosaicsAsync(string account, Action<Address, MosaicAmount> mosaicEventHandler)
        {
            Address destinationAddress = new Address(account);
            this.OnMosaicEventHandler = mosaicEventHandler;   // The callback will be invoked for each msg received per Mosaic owned
            await SendSubscribeStompMsgAsync(ChannelPaths.OWNEDMOSAICS, destinationAddress);
            // Send an explicit message to request an immediate answer on the channel
            StompMessage send = new StompMessage(StompMessage.ClientCommands.SEND, "{'account':'" + destinationAddress.Plain + "'}");
            send.SetDestination(ApiPaths.OWNEDMOSAICS);
            await SendAsync(send);  // The LoopReadStompMsgsAsync will process the answer from the server
        }

        /// <summary>
        /// Subscribes to the channel for Definitions of Mosaics owned by the given Account. The Eventhandler will be invoked when a message is received on the channel.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="mosaicDefinitionEventHandler"></param>
        /// <returns></returns>
        public async Task SubscribeToOwnedMosaicDefinitionsAsync(string account, Action<Address, MosaicInfo> mosaicDefinitionEventHandler)
        {
            Address destinationAddress = new Address(account);
            this.OnMosaicDefinitionEventHandler = mosaicDefinitionEventHandler;
            await SendSubscribeStompMsgAsync(ChannelPaths.OWNEDMOSAICDEFINITION, destinationAddress);
            // Send an explicit message to request an immediate answer on the channel
            StompMessage send = new StompMessage(StompMessage.ClientCommands.SEND, "{'account':'" + destinationAddress.Plain + "'}");
            send.SetDestination(ApiPaths.OWNEDMOSAICDEFINITIONS);
            await SendAsync(send);  // The LoopReadStompMsgsAsync will process the answer from the server
        }

        /// <summary>
        /// Subscribes to the channel for Recent Transactions of the given Account. The Eventhandler will be invoked when a message is received on the channel.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="recentTransactionsEventHandler"></param>
        /// <returns></returns>
        public async Task SubscribeToRecentTransactionsAsync(string account, Action<Address, string> recentTransactionsEventHandler)
        {
            Address destinationAddress = new Address(account);
            this.OnRecentTransactionsEventHandler = recentTransactionsEventHandler;
            await SendSubscribeStompMsgAsync(ChannelPaths.RECENTTRANSACTIONS, destinationAddress);
        }

        /// <summary>
        /// Subscribes to the channel for Unconfirmed Transactions of the given Account. The Eventhandler will be invoked when a message is received on the channel.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="unconfirmedTransactionsEventHandler"></param>
        public async Task SubscribeToUnconfirmedTransactionsAsync(string account, Action<Address, string> unconfirmedTransactionsEventHandler)
        {
            Address destinationAddress = new Address(account);
            this.OnUnconfirmedTransactionsEventHandler = unconfirmedTransactionsEventHandler;
            await SendSubscribeStompMsgAsync(ChannelPaths.UNCONFIRMED, destinationAddress);
        }

        /// <summary>
        /// Subscribes to the channel for Confirmed Transactions of the given Account. The Eventhandler will be invoked when a message is received on the channel.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="confirmedTransactionsEventHandler"></param>
        public async Task SubscribeToConfirmedTransactionsAsync(string account, Action<Address, string> confirmedTransactionsEventHandler)
        {
            Address destinationAddress = new Address(account);
            this.OnConfirmedTransactionsEventHandler = confirmedTransactionsEventHandler;
            await SendSubscribeStompMsgAsync(ChannelPaths.TRANSACTIONS, destinationAddress);
        }

        /// <summary>
        /// Reads the socket.
        /// </summary>
        /// <returns>Task&lt;System.String&gt;.</returns>
        private async Task<string> ReadSocketAsync()
        {
            var segment = new ArraySegment<byte>(new byte[8192]);    // 8Kb
            using (var stream = new MemoryStream())
            {
                WebSocketReceiveResult result;
                do
                {
                    result = await ClientWs.ReceiveAsync(segment, CancellationToken.None);
                    switch (result.MessageType)
                    {
                        case WebSocketMessageType.Text:
                            stream.Write(segment.Array, segment.Offset, result.Count);
                            break;
                        case WebSocketMessageType.Close:
                            await ClientWs.CloseAsync(WebSocketCloseStatus.NormalClosure, DONE, CancellationToken.None);
                            break;
                        case WebSocketMessageType.Binary:
                            Debug.WriteLine("Received Unsupported Binary Websocket message: " + segment.ToString());
                            break;
                    }
                }
                while (ClientWs.State == WebSocketState.Open && !result.EndOfMessage);
                stream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(stream)) return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Loop to read Stomp messages from the websocket and call appropriate actions.
        /// </summary>
        private async Task LoopReadStompMsgsAsync()
        {
            while (ClientWs.State == WebSocketState.Open)   // Infinite loop for as long as we are running with an open socket
            {
                var msg = await ReadSocketAsync();
                StompMessage stompMsg = StompMessage.Deserialize(msg);
                switch (stompMsg.Command)
                {
                    case StompMessage.ServerResponses.ERROR:
                        this.OnErrorEventHandler?.Invoke(stompMsg.Body);
                        break;
                    case StompMessage.ServerResponses.MESSAGE:
                        ProcessReceivedMessage(stompMsg.GetDestination(), stompMsg.Body);
                        break;
                    default:
                        // throw exception?
                        Debug.WriteLine("Received STOMP Message with an unsupported Command: " + stompMsg);
                        break;
                }
            }
        }

        private void ProcessReceivedMessage(string msgDestination, string msgBody)
        {
            Address addr = null;    // When the address of the account is included in the destination, it can be extracted as the last part of it.
            if (msgDestination.Substring(0, ChannelPaths.ERRORS.Length).Equals(ChannelPaths.ERRORS))
            {
                // ERROR
                this.OnErrorEventHandler?.Invoke(msgBody);
            }
            else if (msgDestination.Substring(0, ChannelPaths.BLOCKS.Length).Equals(ChannelPaths.BLOCKS))
            {
                // BLOCK
                var jobject = JObject.Parse(msgBody);  // body is a JSON object with only one field containing the height of the last block
                this.OnBlockEventHandler?.Invoke(ulong.Parse(jobject["height"].ToString()));
            }
            else if (msgDestination.Substring(0, ChannelPaths.OWNEDMOSAICDEFINITION.Length).Equals(ChannelPaths.OWNEDMOSAICDEFINITION))
            {
                // MOSAIC DEFINITION
                addr = new Address(msgDestination.Substring(ChannelPaths.OWNEDMOSAICDEFINITION.Length));
                JObject oMosaicDefinitionSupply = JObject.Parse(msgBody);
                this.OnMosaicDefinitionEventHandler?.Invoke(addr, new MosaicInfo((JObject)oMosaicDefinitionSupply["mosaicDefinition"], ulong.Parse(oMosaicDefinitionSupply["supply"].ToString())));
            }
            else if (msgDestination.Substring(0, ChannelPaths.OWNEDMOSAICS.Length).Equals(ChannelPaths.OWNEDMOSAICS))
            {
                // MOSAIC
                addr = new Address(msgDestination.Substring(ChannelPaths.OWNEDMOSAICS.Length));
                this.OnMosaicEventHandler?.Invoke(addr, new MosaicAmount(JObject.Parse(msgBody)));
            }
            else if (msgDestination.Substring(0, ChannelPaths.OWNEDNAMESPACES.Length).Equals(ChannelPaths.OWNEDNAMESPACES))
            {
                // NAMESPACE
                addr = new Address(msgDestination.Substring(ChannelPaths.OWNEDNAMESPACES.Length));
                this.OnNamespaceEventHandler?.Invoke(addr, new NamespaceInfo(JObject.Parse(msgBody)));
            }
            else if (msgDestination.Substring(0, ChannelPaths.ACCOUNT.Length).Equals(ChannelPaths.ACCOUNT))
            {
                // ACCOUNT
                addr = new Address(msgDestination.Substring(ChannelPaths.ACCOUNT.Length));
                this.OnAccountEventHandler?.Invoke(new AccountInfo(JObject.Parse(msgBody)));
            }
            else if (msgDestination.Substring(0, ChannelPaths.RECENTTRANSACTIONS.Length).Equals(ChannelPaths.RECENTTRANSACTIONS))
            {
                // TRANSACTIONS
                addr = new Address(msgDestination.Substring(ChannelPaths.RECENTTRANSACTIONS.Length));
                Debug.WriteLine("Recent transactions for account " + addr.Plain + " message received, processing to be implemented: " + msgBody);
                this.OnRecentTransactionsEventHandler?.Invoke(addr, msgBody);
            }
            else if (msgDestination.Substring(0, ChannelPaths.UNCONFIRMED.Length).Equals(ChannelPaths.UNCONFIRMED))
            {
                // UNCONFIRMED
                addr = new Address(msgDestination.Substring(ChannelPaths.UNCONFIRMED.Length));
                Debug.WriteLine("Unconfirmed transaction message for account " + addr.Plain + " received, processing to be implemented: " + msgBody);
                this.OnRecentTransactionsEventHandler?.Invoke(addr, msgBody);
            }
            else if (msgDestination.Substring(0, ChannelPaths.TRANSACTIONS.Length).Equals(ChannelPaths.TRANSACTIONS))
            {
                // CONFIRMED
                addr = new Address(msgDestination.Substring(ChannelPaths.TRANSACTIONS.Length));
                Debug.WriteLine("Confirmed transaction message for account " + addr.Plain + " received, processing to be implemented: " + msgBody);
                this.OnConfirmedTransactionsEventHandler?.Invoke(addr, msgBody);
            }
            else Debug.WriteLine("Received a Message for an unsupported Channel: " + msgDestination);
        }
    }
}

