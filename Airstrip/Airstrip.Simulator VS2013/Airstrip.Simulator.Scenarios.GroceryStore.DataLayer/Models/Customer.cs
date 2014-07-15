using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airstrip.Simulator.Scenarios.GroceryStore.DataLayer.Models
{
    public class Customer
    {
        public enum PersonalityType
        {
            A,
            B
        }

        public virtual int Id { get; set; }
        public virtual int Number { get; set; }
        public virtual PersonalityType Type { get; set; }
        public virtual int TotalItems { get; set; }
        public virtual int LineUpTime { get; set; }
    }
}
