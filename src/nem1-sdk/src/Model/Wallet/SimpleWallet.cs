using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using io.nem1.sdk.Core.Crypto;
using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Blockchain;
using Newtonsoft.Json;

namespace io.nem1.sdk.Model.Wallet
{
    public class SimpleWallet
    {
        /// <summary>
        /// Gets the encrypted private key.
        /// </summary>
        /// <value>The encrypted private key.</value>
        public EncryptedPrivateKey EncryptedPrivateKey { get; }

        /// <summary>
        /// Gets or sets the wallet object.
        /// </summary>
        /// <value>The wallet object.</value>
        public WalletObject WalletObj { get; set; }

        /// <summary>
        /// Gets the wallet network.
        /// </summary>
        /// <value>The network.</value>
        public NetworkType.Types Network { get; }

        /// <summary>
        /// Gets the wallet name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleWallet"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="network">The network.</param>
        /// <param name="wlt">The WLT.</param>
        /// <param name="encryptedPrivateKey">The encrypted private key.</param>
        internal SimpleWallet(string name, NetworkType.Types network, WalletObject wlt, EncryptedPrivateKey encryptedPrivateKey)
        {
            Name = name;
            EncryptedPrivateKey = encryptedPrivateKey;
            WalletObj = wlt;
            Network = network;
        }

        /// <summary>
        /// Import from nano wallet file.
        /// </summary>
        /// <param name="base64WltText">The base64 WLT text.</param>
        /// <param name="network">The network.</param>
        /// <returns>SimpleWallet.</returns>
        public static SimpleWallet ImportFromNanoWalletFile(string base64WltText, NetworkType.Types network)
        {
            var wlt = WalletAdapter(Encoding.UTF8.GetString(Convert.FromBase64String(base64WltText)));

            return new SimpleWallet(wlt.Name, network, wlt, new EncryptedPrivateKey(wlt.Accounts.Account[0].Iv + wlt.Accounts.Account[0].Encrypted));
        }

        /// <summary>
        /// Creates a new simple wallet.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="password">The password.</param>
        /// <param name="privateKey">The private key.</param>
        /// <param name="network">The network.</param>
        /// <returns>SimpleWallet.</returns>
        public static SimpleWallet CreateNewSimpleWallet(string name, Password password, string privateKey, NetworkType.Types network)
        {
            var account = PrivateAccount.CreateFromPrivateKey(privateKey, network);

            return CreateNewSimpleWallet(name, password, account, network);
        }

        /// <summary>
        /// Creates a new simple wallet.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="password">The password.</param>
        /// <param name="network">The network.</param>
        /// <returns>SimpleWallet.</returns>
        public static SimpleWallet CreateNewSimpleWallet(string name, Password password, NetworkType.Types network)
        {
            var Acc = PrivateAccount.GenerateNewAccount(network);

            return CreateNewSimpleWallet(name, password, Acc, network);
        }

        internal static SimpleWallet CreateNewSimpleWallet(string name, Password password, PrivateAccount account, NetworkType.Types network)
        {
            var encKey = new EncryptedPrivateKey(CryptoUtils.EncodePrivateKey(account.PrivateKey, password.Value));

            var wlt = new WalletObject()
            {
                Accounts = new WalletAccounts()
                {
                    Account = new List<WalletAccount>
                    {
                        new WalletAccount
                        {
                            Address = account.Address.Plain,
                            Algo = "pass:bip32",
                            Brain = false,
                            Child = "",
                            Encrypted = encKey.EncryptedKey,
                            Iv = encKey.Iv,
                            Label = "Primary",
                            Network = network.GetNetwork()
                        }
                    }
                },
                Name = name,
                PrivateKey = ""
            };

            return new SimpleWallet(name, network, wlt, encKey);
        }

        /// <summary>
        /// Writes the wallet to base64 to be saved in file.
        /// </summary>
        /// <returns>System.String.</returns>
        public string WriteFile()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(WalletAdapter(WalletObj)));
        }

        /// <summary>
        /// Unlocks the private key.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>System.String.</returns>
        public string UnlockPrivateKey(Password password)
        {
            return PrivateAccount.CreateFromPrivateKey(EncryptedPrivateKey.Decrypt(password), Network)
                .PrivateKey;
        }

        /// <summary>
        /// Opens the wallet with a specified password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>Account.</returns>
        public PrivateAccount Open(Password password)
        {
            return PrivateAccount.CreateFromPrivateKey(EncryptedPrivateKey.Decrypt(password), Network);
        }

        private static WalletObject WalletAdapter(string json)
        {
            var newString = Regex.Replace(json, "{\"\\d\":{", "{\"account\":[{");

            newString = newString.Insert(newString.Length - 2, "]");

            return JsonConvert.DeserializeObject<WalletObject>(newString);
        }

        private static string WalletAdapter(WalletObject wlt)
        {
            var newString = JsonConvert.SerializeObject(wlt);

            return FindAndReplace(newString);
        }

        private static string FindAndReplace(string toSearchInside)
        {
            toSearchInside = toSearchInside.Replace("[", "");
            toSearchInside = toSearchInside.Replace("]", "");
            var toMatch = "{\"account\":{";

            var matches = Regex.Matches(toSearchInside, toMatch);

            var result = toSearchInside;

            for (var i = 0; i < matches.Count; i++)
                result = Replace(result, matches[i].Index, matches[i].Length, "{\"" + i + "\":{");
            return result;
        }

        private static string Replace(string s, int index, int length, string replacement)
        {
            var builder = new StringBuilder();
            builder.Append(s.Substring(0, index));
            builder.Append(replacement);
            builder.Append(s.Substring(index + length));
            return builder.ToString();
        }
    }
}
