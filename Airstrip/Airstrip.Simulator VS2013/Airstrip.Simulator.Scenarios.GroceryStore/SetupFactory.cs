using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Ninject;

namespace Airstrip.Simulator.Scenarios.GroceryStore
{
    class SetupFactory
    {
        private static readonly string USAGE = "Required: -testsource [file|db] -testname <name>";

        private static IKernel kernel = new StandardKernel();

        static SetupFactory()
        {
            kernel.Bind<DataLayer.Repository.ITestRepository>().To<DataLayer.Repository.File.FileTestRepository>().Named("file");
            kernel.Bind<DataLayer.Repository.ITestRepository>().To<DataLayer.Repository.Db.DbTestRepository>().Named("db");
        }
        
        public static Setup Get(NameValueCollection parameters)
        {
            try
            {
                string testsource = parameters["testsource"];
                string testname = parameters["testname"];

                if (testsource == null)
                    throw new Exception("No test source specified.");

                if (testname == null)
                    throw new Exception("No test name specified." + USAGE);

                DataLayer.Repository.ITestRepository testRepo = kernel.Get<DataLayer.Repository.ITestRepository>(testsource);

                if (testRepo == null)
                    throw new Exception("No test source of type '" + testsource + "' found.");

                DataLayer.Models.Test test = testRepo.GetByName(testname);

                return new Setup(test);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + USAGE);
            }
        }
    }
}
