using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Ninject;

namespace Airstrip.Simulator.Scenarios.GroceryStore
{
    class Setup
    {
        public Components.GroceryStore Store;
        public List<Components.Customer> Customers = new List<Components.Customer>();


        public Setup(DataLayer.Models.Test test)
        {
            this.Store = new Components.GroceryStore(test.GroceryStore);
            test.Customers.ForEach(c => this.Customers.Add(Components.Customer.New(c)));
        }
    }
}
