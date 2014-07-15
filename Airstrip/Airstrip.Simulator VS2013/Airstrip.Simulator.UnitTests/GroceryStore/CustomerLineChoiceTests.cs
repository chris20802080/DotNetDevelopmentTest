using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Airstrip.Simulator.UnitTests.GroceryStore
{
    [TestClass]
    public class CustomerLineChoiceTests
    {
        private Scenarios.GroceryStore.Components.GroceryStore.LineOverview overview;

        [TestInitialize]
        public void TestInitialize()
        {

        }

        private void SetupLineOverview(int[][] lines)
        {
            this.overview = new MockLineOverview(lines);
        }


        private void RunTest(Scenarios.GroceryStore.Components.Customer.PersonalityType type, int nExpectedChoice)
        {
            Mock mockCust = null;

            switch(type)
            {
                case Scenarios.GroceryStore.Components.Customer.PersonalityType.A:
                    mockCust = new Mock<Scenarios.GroceryStore.Components.CustomerA>();
                    break;

                case Scenarios.GroceryStore.Components.Customer.PersonalityType.B:
                    mockCust = new Mock<Scenarios.GroceryStore.Components.CustomerB>();
                    break;
            }

            mockCust.CallBase = true;
            Scenarios.GroceryStore.Components.Customer cust = mockCust.Object as Scenarios.GroceryStore.Components.Customer;

            int nChoice = cust.GetLineChoice(this.overview);

            Assert.AreEqual(nChoice, nExpectedChoice);
        }

        [TestMethod]
        public void A_DiffLineLengths()
        {
             // 1: 4,2,7
             // 2: 1,1,1
             // 3: 9
             // 4: 3,1

            this.SetupLineOverview(new int[][]
                {
                     new int[]{4,2,7},
                     new int[]{1,1,1},
                     new int[]{9},
                     new int[]{3,1}
                });

            this.RunTest(Scenarios.GroceryStore.Components.Customer.PersonalityType.A, 3);
        }

        [TestMethod]
        public void A_SameLineLengths()
        {
            // 1: 4,2,7
            // 2: 1,8
            // 3: 1,1
            // 4: 2,1

            this.SetupLineOverview(new int[][]
                {
                     new int[]{4,2,7},
                     new int[]{1,8},
                     new int[]{1,1},
                     new int[]{2,1}
                });

            this.RunTest(Scenarios.GroceryStore.Components.Customer.PersonalityType.A, 2);
        }


        [TestMethod]
        public void B_OneLeast()
        {
            // 1: 1,4
            // 2: 4
            // 3: 2
            // 4: 5,3,1

            this.SetupLineOverview(new int[][]
                {
                     new int[]{1,4},
                     new int[]{4},
                     new int[]{2},
                     new int[]{5,3,1}
                });

            this.RunTest(Scenarios.GroceryStore.Components.Customer.PersonalityType.B, 4);
        }


        [TestMethod]
        public void B_MultipleLeast()
        {
            // 1: 1,4
            // 2: 4
            // 3: 5,3,2
            // 4: 2

            this.SetupLineOverview(new int[][]
                {
                     new int[]{1,4},
                     new int[]{4},
                     new int[]{5,3,2},
                     new int[]{2}
                });

            this.RunTest(Scenarios.GroceryStore.Components.Customer.PersonalityType.B, 3);
        }


        [TestMethod]
        public void B_OneEmptyLine()
        {
            // 1: 1,4
            // 2: 
            // 3: 5,3,2
            // 4: 2

            this.SetupLineOverview(new int[][]
                {
                     new int[]{1,4},
                     new int[]{},
                     new int[]{5,3,2},
                     new int[]{2}
                });

            this.RunTest(Scenarios.GroceryStore.Components.Customer.PersonalityType.B, 2);
        }

        [TestMethod]
        public void B_MultipleEmptyLines()
        {
            // 1: 1,4
            // 2: 3,1
            // 3: 
            // 4: 

            this.SetupLineOverview(new int[][]
                {
                     new int[]{1,4},
                     new int[]{3,1},
                     new int[]{},
                     new int[]{}
                });

            this.RunTest(Scenarios.GroceryStore.Components.Customer.PersonalityType.B, 3);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }


        private class MockLineOverview : Scenarios.GroceryStore.Components.GroceryStore.LineOverview
        {
            public MockLineOverview(int[][] lines)
            {
                this.Lines = new List<Scenarios.GroceryStore.Components.GroceryStore.LineOverview.Line>();

                for (int i = 0; i < lines.Length; ++i)
                    this.Lines.Add(new MockLine(i + 1, lines[i]));
            }

            private class MockLine : Scenarios.GroceryStore.Components.GroceryStore.LineOverview.Line
            {
                public MockLine(int nLine, int[] customers)
                {
                    this.Number = nLine;
                    this.Customers = new List<Scenarios.GroceryStore.Components.GroceryStore.LineOverview.Customer>();

                    for (int i = 0; i < customers.Length; ++i)
                        this.Customers.Add(new MockCustomer(customers[i]));
                }
            }


            private class MockCustomer : Scenarios.GroceryStore.Components.GroceryStore.LineOverview.Customer
            {
                public MockCustomer(int nItemsRemaining)
                {
                    this.ItemsRemaining = nItemsRemaining;
                }
            }
        }
    }
}
