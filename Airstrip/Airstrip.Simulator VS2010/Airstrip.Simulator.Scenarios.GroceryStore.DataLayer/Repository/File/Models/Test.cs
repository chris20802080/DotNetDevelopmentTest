using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airstrip.Simulator.Scenarios.GroceryStore.DataLayer.Repository.File.Models
{
    internal class Test : DataLayer.Models.Test
    {
        public Test(string configText)
        {
            this.Customers = new List<DataLayer.Models.Customer>();

            try
            {
                string[] lines = configText.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                lines = lines.Where(line => line.Trim() != String.Empty).ToArray();

                this.GroceryStore = new Repository.File.Models.GroceryStore(lines[0]);

                int nCustomer = 1;
                foreach (string line in lines.Skip(1))
                    this.Customers.Add(new Repository.File.Models.Customer(nCustomer++, line));
            }
            catch (Exception ex)
            {
                throw new Exception("Error parsing config file.", ex);
            }
        }
    }
}
