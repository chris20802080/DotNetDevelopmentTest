using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Airstrip.Simulator.Scenarios.GroceryStore.DataLayer.Repository.File.Models
{
    internal class Customer : DataLayer.Models.Customer
    {
        private static Regex pattern = new Regex(@"^\s*(?<type>[AB])\s+(?<time>\d+)\s+(?<items>\d+)\s*$");


        public Customer(int nCustomer, string configText)
        {
            this.Id = 0;
            this.Number = nCustomer;

            try
            {
                Match m = pattern.Match(configText.Trim());

                if (!m.Success)
                    throw new Exception("Error parsing line: '" + configText + "'");

                var typeMatch = Enum.GetValues(typeof(PersonalityType)).Cast<PersonalityType>().Where(type => type.ToString() == m.Groups["type"].Value);
                if(!typeMatch.Any())
                    throw new Exception("Customer type parsing error.");

                this.Type = typeMatch.First();
                this.LineUpTime = Int32.Parse(m.Groups["time"].Value);
                this.TotalItems = Int32.Parse(m.Groups["items"].Value);
            }
            catch(Exception ex)
            {
                throw new Exception("Error parsing Customer. Invalid format: " + configText, ex);
            }
        }
    }
}
