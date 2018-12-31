//
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
// 

using io.nem1.sdk.Model.Transactions.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Model.Transactions
{
    [TestClass]
    public class MessageTests
    {
        [TestMethod]
        public void CanCreateMessage()
        {
            var plainMessage = PlainMessage.Create("Hello");

            Assert.AreEqual("Hello", plainMessage.GetStringPayload());

        }

        [TestMethod]
        public void CanCreateSecureMessage()
        {
            var secureMessage = SecureMessage.Create("Hello", "5949fc564c90ac186cd4f9d2b8298b677bca300b9d8f926ca04e1739e4ed0cba", "2ecf1decef6818bd9c38985afd6efc1c981e64e9a1ecc1e7b6b25eb30454cce0");
            
            var decoded = secureMessage.GetDecodedPayload(
                "5949fc564c90ac186cd4f9d2b8298b677bca300b9d8f926ca04e1739e4ed0cba",
                "2ecf1decef6818bd9c38985afd6efc1c981e64e9a1ecc1e7b6b25eb30454cce0");
            
           Assert.AreEqual("Hello", decoded);
        }
    }
}
