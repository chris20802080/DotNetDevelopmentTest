using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airstrip.Simulator.Scenarios.GroceryStore.Components
{
    class CustomerB : Customer
    {
        private static LineComparerB comparer = new LineComparerB();

        protected CustomerB() { }

        public CustomerB(DataLayer.Models.Customer customer)
            : base(customer)
        {
            
        }


        public override int GetLineChoice(GroceryStore.LineOverview overview)
        {
            overview.Lines.Sort(comparer);
            var line = overview.Lines.First();

            return line.Number;
        }


        private class LineComparerB : IComparer<GroceryStore.LineOverview.Line>
        {
            public int Compare(GroceryStore.LineOverview.Line x, GroceryStore.LineOverview.Line y)
            {
                if (x.Customers.Count == 0 || y.Customers.Count == 0)
                {
                    if (y.Customers.Count > 0)
                        return -1;

                    if (x.Customers.Count > 0)
                        return 1;

                    return x.Number - y.Number;
                }

                int xLastItems = x.Customers.Last().ItemsRemaining;
                int yLastItems = y.Customers.Last().ItemsRemaining;

                if (xLastItems != yLastItems)
                    return xLastItems - yLastItems;

                return x.Number - y.Number;
            }
        }

        public override Customer.PersonalityType Type
        {
            get { return PersonalityType.B; }
        }
    }
}
