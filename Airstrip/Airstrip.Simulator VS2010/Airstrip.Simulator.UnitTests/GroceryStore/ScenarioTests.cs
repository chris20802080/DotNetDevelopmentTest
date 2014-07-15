using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Airstrip.Simulator.UnitTests.GroceryStore
{
    // Comprehensive tests on 5 setups. Simulates a given GroceryStore test and checks that the finish time is correct.
    [TestClass]
    public class ScenarioTests
    {
        private static Dictionary<int, int> ExpectedResults = new Dictionary<int, int>(){{1, 7}, {2, 13}, {3, 6}, {4, 9}, {5, 11}};
        private Scenarios.GroceryStore.DataLayer.Repository.ITestRepository testRepo;


        [TestInitialize]
        public void TestInitialize()
        {
            this.testRepo = new MockTestRepository();
        }

        [TestMethod]
        public void Test1()
        {
            this.RunTest(1);
        }

        [TestMethod]
        public void Test2()
        {
            this.RunTest(2);
        }

        [TestMethod]
        public void Test3()
        {
            this.RunTest(3);
        }

        [TestMethod]
        public void Test4()
        {
            this.RunTest(4);
        }

        [TestMethod]
        public void Test5()
        {
            this.RunTest(5);
        }


        private void RunTest(int nTest)
        {
            var test = this.testRepo.GetByName(nTest.ToString());
            var setup = new Scenarios.GroceryStore.Setup(test);
            var scenario = new Scenarios.GroceryStore.Scenario(setup);

            // Mini simulator
            int nTime = -1, nMaxTime = 999;
            while(!scenario.IsDone && nTime < nMaxTime)
                scenario.OnTick(++nTime);

            if(!scenario.IsDone)
                throw new Exception(string.Format("Scenario did not finish in {0} minutes.", nTime));

            Assert.AreEqual(nTime, ExpectedResults[nTest]);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }


        private class MockTestRepository : Scenarios.GroceryStore.DataLayer.Repository.ITestRepository
        {
            private static readonly string
                TEST1 = @"
                            1
                            A 1 2
                            A 2 1
                        ",
                TEST2 = @"
                            2
                            A 1 5
                            B 2 1
                            A 3 5
                            B 5 3
                            A 8 2
                        ",
                TEST3 = @"
                            2
                            A 1 2
                            A 1 2
                            A 2 1
                            A 3 2
                        ",
                TEST4 = @"
                            2
                            A 1 2
                            A 1 3
                            A 2 1
                            A 2 1
                        ",
                TEST5 = @"
                            2
                            A 1 3
                            A 1 5
                            A 3 1
                            B 4 1
                            A 4 1
                        ";

            private Dictionary<int, string> RawTests = new Dictionary<int, string>(){{ 1, TEST1 }, { 2, TEST2 }, { 3, TEST3 }, { 4, TEST4 }, { 5, TEST5 }};

            public MockTestRepository()
            {

            }

            public Scenarios.GroceryStore.DataLayer.Models.Test GetByName(string testName)
            {
                int nTest = Int32.Parse(testName);
                string rawTest = this.RawTests[nTest];
                return new Scenarios.GroceryStore.DataLayer.Repository.File.Models.Test(rawTest);
            }
        }
    }
}
