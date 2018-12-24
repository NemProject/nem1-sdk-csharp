// ***********************************************************************
// Assembly         : nem1-sdk
// Author           : kailin
// Created          : 06-01-2018
//
// Last Modified By : kailin
// Last Modified On : 11-07-2018
// ***********************************************************************
// <copyright file="TransactionInfoTests.cs" company="Nem.io">
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

using io.nem1.sdk.Model.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Model.Transactions
{
    [TestClass]
    public class TransactionInfoTests
    {
        [TestMethod]
        public void CreateATransactionInfoWithStaticConstructorCreateForStandaloneTransactions()
        {
            TransactionInfo transactionInfo = TransactionInfo.Create(121855, 1, "B6C7648A3DDF71415650805E9E7801424FE03BBEE7D21F9C57B60220D3E95B2F");

            Assert.AreEqual((ulong)121855, transactionInfo.Height);
            Assert.IsTrue(transactionInfo.Hash == "B6C7648A3DDF71415650805E9E7801424FE03BBEE7D21F9C57B60220D3E95B2F");
            Assert.IsTrue(transactionInfo.Id == 1);
            Assert.IsNull(transactionInfo.InnerHash);
        }


        [TestMethod]
        public void CreateATransactionInfoWithStaticConstructorCreateForAggregateInnerTransactions()
        {
            TransactionInfo transactionInfo = TransactionInfo.CreateMultisig(121855, 1, "3D28C804EDD07D5A728E5C5FFEC01AB07AFA5766AE6997B38526D36015A4D006",
                    "3D28C804EDD07D5A728E5C5FFEC01AB07AFA5766AE6997B38526D36015A4D006");

            Assert.AreEqual((ulong)121855, transactionInfo.Height);
            Assert.IsTrue(transactionInfo.InnerHash == "3D28C804EDD07D5A728E5C5FFEC01AB07AFA5766AE6997B38526D36015A4D006");
            Assert.IsNotNull(transactionInfo.Id);
            Assert.IsTrue(transactionInfo.Hash == "3D28C804EDD07D5A728E5C5FFEC01AB07AFA5766AE6997B38526D36015A4D006");
        }
    }
}

