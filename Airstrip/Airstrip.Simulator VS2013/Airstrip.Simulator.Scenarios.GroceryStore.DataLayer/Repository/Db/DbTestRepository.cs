using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Airstrip.Simulator.Scenarios.GroceryStore.DataLayer.Repository.Db
{
    public class DbTestRepository : ITestRepository
    {

        public DataLayer.Models.Test GetByName(string testName)
        {
            throw new NotImplementedException();
        }
    }
}
