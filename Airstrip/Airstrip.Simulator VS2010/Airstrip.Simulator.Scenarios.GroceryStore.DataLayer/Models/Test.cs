using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airstrip.Simulator.Scenarios.GroceryStore.DataLayer.Models
{
    public class Test
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual List<Models.Customer> Customers { get; set; }
        public virtual Models.GroceryStore GroceryStore { get; set; }
    }
}
