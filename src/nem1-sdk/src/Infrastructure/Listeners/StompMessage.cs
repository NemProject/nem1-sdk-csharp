using io.nem1.sdk.Model.Accounts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace io.nem1.sdk.src.Infrastructure.Listener
{
    /// <summary>
    /// Message to be used in both directions of a WebSocket connection with NIS, following the Simple Text Orientated Messaging Protocol 
    /// </summary>
    public class StompMessage
    {
        const string ID = "id";
        const string DESTINATION = "destination";

        public struct ClientCommands
        {
            public const string CONNECT = "CONNECT";
            public const string DISCONNECT = "DISCONNECT";
            public const string SUBSCRIBE = "SUBSCRIBE";
            public const string UNSUBSCRIBE = "UNSUBSCRIBE";
            public const string SEND = "SEND";
        }

        public struct ServerResponses
        {
            public const string CONNECTED = "CONNECTED";
            public const string MESSAGE = "MESSAGE";
            public const string ERROR = "ERROR";
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        public string Command { get; private set; }

        /// <summary>
        /// local Dictionary of message headers.
        /// </summary>
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();

        /// <summary>
        /// Gets the Body.
        /// </summary>
        public string Body { get; private set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "StompMessage" /> class.
        /// </summary>
        /// <param name = "command">The command.</param>
        public StompMessage(string command) : this(command, string.Empty) { }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "StompMessage" /> class.
        /// </summary>
        /// <param name = "command">The command.</param>
        /// <param name = "body">The body.</param>
        public StompMessage(string command, string body) : this(command, new Dictionary<string, string>(), body) {}

        /// <summary>
        ///   Initializes a new instance of the <see cref = "StompMessage" /> class.
        /// </summary>
        /// <param name = "command">The command.</param>
        /// <param name = "headers">The headers.</param>
        /// <param name = "body">The body.</param>
        internal StompMessage(string command, Dictionary<string, string> headers, string body)
        {
            Command = command;
            _headers = headers;
            Body = body;
            this["content-length"] = body.Length.ToString();
        }

        /// <summary>
        /// Gets the message headers
        /// </summary>
        public Dictionary<string, string> Headers
        {
            get { return _headers; }
        }

        /// <summary>
        /// Gets or sets the specified header attribute.
        /// </summary>
        private string this[string header]
        {
            get { return _headers.ContainsKey(header) ? _headers[header] : string.Empty; }
            set { _headers[header] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void SetSubscriptionId(uint id)
        {
            this[ID] = "sub-" + id.ToString();
        }

        /// <summary>
        /// Sets the destination header
        /// </summary>
        /// <param name="destinationPath"></param>
        /// <param name="destinationAddress"></param>
        public void SetDestination(string destinationPath, Address destinationAddress = null)
        {
            this[DESTINATION] = destinationPath + destinationAddress?.Plain;
        }

        /// <summary>
        /// Gets the destination header
        /// </summary>
        /// <returns></returns>
        public string GetDestination() { return this[DESTINATION]; }

        /// <summary>
        ///   Serializes the message to be sent.
        /// </summary>
        /// <returns>A serialized version of the <see cref="StompMessage"/></returns>
        public string Serialize()
        {
            var buffer = new StringBuilder();

            buffer.Append(Command + "\n");
            if (Headers != null)
            {
                foreach (var header in Headers)
                {
                    buffer.Append(header.Key + ":" + header.Value + "\n");
                }
            }
            buffer.Append("\n");
            buffer.Append(Body);
            buffer.Append('\0');
            return buffer.ToString();
        }

        /// <summary>
        /// Deserializes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="StompMessage"/> instance</returns>
        public static StompMessage Deserialize(string message)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            StringReader msgReader = new StringReader(message);            
            // determine the command
            string command = msgReader.ReadLine();
            // determine the headers
            string header = msgReader.ReadLine();
            while (!string.IsNullOrEmpty(header))
            {
                var split = header.Split(':');
                if (split.Length == 2) headers[split[0].Trim()] = split[1].Trim();
                header = msgReader.ReadLine() ?? string.Empty;
            }
            // determine the body
            string body = msgReader.ReadToEnd() ?? string.Empty;
            body = body.TrimEnd('\r', '\n', '\0');
            // Construct the StompMessage to be returned
            return new StompMessage(command, headers, body);
        }

        public override string ToString()
        {
            string txt = "StompMessage: ";
            txt += Command;
            txt += ", " + string.Join(";", Headers);
            txt += ", " + Body;
            return txt;
        }
    }
}
