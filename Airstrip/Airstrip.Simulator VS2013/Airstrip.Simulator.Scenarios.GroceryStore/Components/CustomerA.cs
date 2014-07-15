using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airstrip.Simulator.Scenarios.GroceryStore.Components
{
    class CustomerA : Customer
    {
        private static LineComparerA comparer = new LineComparerA();

        protected CustomerA() { }
        public CustomerA(DataLayer.Models.Customer customer) : base(customer)
        {
            
        }


        public override int GetLineChoice(GroceryStore.LineOverview overview)
        {
            overview.Lines.Sort(comparer);
            var line = overview.Lines.First();

            return line.Number;
        }


        private class LineComparerA : IComparer<GroceryStore.LineOverview.Line>
        {
            public int Compare(GroceryStore.LineOverview.Line x, GroceryStore.LineOverview.Line y)
            {
                if (x.Customers.Count != y.Customers.Count)
                    return x.Customers.Count - y.Customers.Count;

                return x.Number - y.Number;
            }
        }


        public override Customer.PersonalityType Type
        {
            get { return PersonalityType.A; }
        }
    }
}
