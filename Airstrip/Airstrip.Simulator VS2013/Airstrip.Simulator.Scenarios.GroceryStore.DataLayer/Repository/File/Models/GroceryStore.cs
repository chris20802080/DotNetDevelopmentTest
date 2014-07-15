using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airstrip.Simulator.Scenarios.GroceryStore.DataLayer.Repository.File.Models
{
    internal class GroceryStore : DataLayer.Models.GroceryStore
    {
        public GroceryStore(string configText)
        {
            this.Id = 0;

            try
            {
                this.TotalRegisters = Int32.Parse(configText.Trim());

                if (this.TotalRegisters <= 0)
                    throw new Exception("TotalRegisters value must be > 0, was " + this.TotalRegisters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error parsing GroceryStore.", ex);
            }
        }
    }
}
