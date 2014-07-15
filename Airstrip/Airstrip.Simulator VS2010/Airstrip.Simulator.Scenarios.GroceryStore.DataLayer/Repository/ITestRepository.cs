using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airstrip.Simulator.Scenarios.GroceryStore.DataLayer.Repository
{
    public interface ITestRepository
    {
        DataLayer.Models.Test GetByName(string testName);
    }
}
