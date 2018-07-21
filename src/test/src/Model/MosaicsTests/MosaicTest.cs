﻿//
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

using System;
using io.nem1.sdk.Model.Mosaics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Model.MosaicsTests
{
    [TestClass]
    public class MosaicTest
    {

        [TestMethod]
        public void CreateANewMosaicViaConstructor()
        {
            Mosaic mosaic = new Mosaic("nem", "xem", 24);
            Assert.IsTrue(mosaic.MosaicName.Equals("xem"));
            Assert.IsTrue(mosaic.Amount.Equals(24));
        }

        [TestMethod]
        public void CreateANewMosaicWithIdentifier()
        {
            Mosaic mosaic = Mosaic.CreateFromIdentifier("nem:xem", 24);
            Assert.IsTrue(mosaic.MosaicName == "xem");
            Assert.IsTrue(mosaic.NamespaceName == "nem");
            Assert.IsTrue(mosaic.Amount.Equals(24));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [DataTestMethod]
        [DataRow("nem")]
        [DataRow("xem")]
        [DataRow("nem:xem:nem")]
        [DataRow("")]
        [DataRow(":nem")]
        public void CreateANewMosaicWithIdentifierThrowsExeption(string data)
        {
            Mosaic.CreateFromIdentifier(data, 24);       
        }
    }
}
