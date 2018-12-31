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

using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Model.AccountTest
{
    [TestClass]
    public class AddressTest
    {

        [TestMethod]
        public void TestAddressCreation()
        {
            Address address = new Address("TCTUIF-557ZCQ-OQPW2M-6GH4TC-DPM2ZY-BBL54K-GNHR", NetworkType.Types.TEST_NET);
            Assert.AreEqual("TCTUIF557ZCQOQPW2M6GH4TCDPM2ZYBBL54KGNHR", address.Plain);
        }

        [TestMethod]
        public void TestAddressWithSpacesCreation()
        {
            Address address = new Address(" TCTUIF-557ZCQ-OQPW2M-6GH4TC-DPM2ZY-BBL54K-GNHR ", NetworkType.Types.TEST_NET);
            Assert.AreEqual("TCTUIF557ZCQOQPW2M6GH4TCDPM2ZYBBL54KGNHR", address.Plain);
        }

        [TestMethod]
        public void TestLowerCaseAddressCreation()
        {
            Address address = new Address("tctuif-557zcq-oqpw2m-6gh4tc-dpm2zy-bbl54k-gnhr", NetworkType.Types.TEST_NET);
            Assert.AreEqual("TCTUIF557ZCQOQPW2M6GH4TCDPM2ZYBBL54KGNHR", address.Plain);
        }

        [TestMethod]
        public void AddressInPrettyFormat()
        {
            Address address = new Address("TCTUIF-557ZCQ-OQPW2M-6GH4TC-DPM2ZY-BBL54K-GNHR", NetworkType.Types.TEST_NET);
            Assert.AreEqual("TCTUIF-557ZCQ-OQPW2M-6GH4TC-DPM2ZY-BBL54K-GNHR", address.Pretty);
        }

        [TestMethod]
        public void Equality()
        {
            Address address1 = new Address("TCTUIF-557ZCQ-OQPW2M-6GH4TC-DPM2ZY-BBL54K-GNHR", NetworkType.Types.TEST_NET);
            Address address2 = new Address("TCTUIF557ZCQOQPW2M6GH4TCDPM2ZYBBL54KGNHR", NetworkType.Types.TEST_NET);
            Assert.AreEqual(address1.Pretty, address2.Pretty);
            Assert.AreEqual(address1.Plain, address2.Plain);
        }

        [TestMethod]
        public void NoEquality()
        {
            Address address1 = new Address("TCTUIF557ZCQOQPW2M6GH4TCDPM2ZYBBL54KGNHR", NetworkType.Types.MIJIN_TEST);
            Address address2 = new Address("TACOPE-XRLZTU-WBQA3U-XV66R4-55L76E-NWK6OY-ITBJ", NetworkType.Types.MIJIN_TEST);
            Assert.AreNotEqual(address1.Pretty, address2.Pretty);
            Assert.AreNotEqual(address1.Plain, address2.Plain);
        }
    }
}