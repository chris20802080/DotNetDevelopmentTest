using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Airstrip.Simulator.Scenarios.GroceryStore.DataLayer.Repository.File
{
    public class FileTestRepository : ITestRepository
    {
        private static DirectoryInfo _testFileDirectory;
        private static DirectoryInfo TestFileDirectory
        {
            get
            {
                if(_testFileDirectory == null)
                    _testFileDirectory = new DirectoryInfo(Properties.Settings.Default.TestFileDirectory);

                return _testFileDirectory;
            }
        }


        public DataLayer.Models.Test GetByName(string testName)
        {
            var files = TestFileDirectory.GetFiles(testName + ".config");
            FileInfo file = files.FirstOrDefault();

            if (file == null)
                throw new Exception("No file found: " + testName + ".config");

            string configText = System.IO.File.ReadAllText(file.FullName);
            return new Repository.File.Models.Test(configText);
        }
    }
}
