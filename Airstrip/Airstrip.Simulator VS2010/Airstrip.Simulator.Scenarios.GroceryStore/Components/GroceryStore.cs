using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airstrip.Simulator.Scenarios.GroceryStore.Components
{
    internal class GroceryStore : ISimulatableComponent
    {
        private Dictionary<int, Register> registers = new Dictionary<int, Register>();
        private LineArbitrator arbitrator;

        private event Action<long> Tick, PostTick;


        public GroceryStore(DataLayer.Models.GroceryStore store)
        {
            // setup registers

            if (store.TotalRegisters < 1)
                throw new Exception("No registers.");

            for (int n = 1, nNormalRegs = store.TotalRegisters - 1; n <= nNormalRegs; ++n)
                this.registers[n] = new RegisterNormal(n);

            int nTrainingRegister = store.TotalRegisters;

            this.registers[nTrainingRegister] = new RegisterTraining(nTrainingRegister);

            foreach (var reg in this.registers.Values)
            {
                this.Tick += reg.OnTick;
                this.PostTick += reg.OnPostTick;
            }

            // setup LineArbitrator

            arbitrator = new LineArbitrator(this.GetLineOverview, this.LineUp);

            this.Tick += this.arbitrator.OnTick;
            this.PostTick += this.arbitrator.OnPostTick;
        }


        public string Status
        {
            get 
            {
                StringBuilder sb = new StringBuilder();

                foreach (var reg in this.registers.Values)
                    sb.AppendLine(reg.Status);

                return sb.ToString();
            }
        }

        public void OnTick(long time)
        {
            this.Tick(time);
        }

        public void OnPostTick(long time)
        {
            this.PostTick(time);
        }



        public void RequestLineUp(Customer cust)
        {
            this.arbitrator.RequestLineUp(cust);
        }

        private void LineUp(Customer cust, int nLine)
        {
            var reg = this.registers[nLine];
            if (reg == null)
                throw new Exception("Register doesn't exist.");

            reg.LineUp(cust);
        }

        private LineOverview GetLineOverview()
        {
            return new LineOverview(this.registers.Values.ToList());
        }


        private class LineArbitrator : ISimulatableComponent
        {
            private CustomerLineupPriorityComparer priorityComparer = new CustomerLineupPriorityComparer();
            private HashSet<Customer> customersRequestingLineUp = new HashSet<Customer>();

            private Func<LineOverview> GetLineOverview;
            private Action<Customer, int> LineUp;

            public LineArbitrator(Func<LineOverview> GetLineOverview, Action<Customer, int> LineUp)
            {
                this.GetLineOverview = GetLineOverview;
                this.LineUp = LineUp;
            }

            public string Status
            {
                get { throw new NotImplementedException(); }
            }

            public void RequestLineUp(Customer cust)
            {
                this.customersRequestingLineUp.Add(cust);
            }

            public void OnTick(long time)
            {

            }

            public void OnPostTick(long time)
            {
                // Process pending requests to line up.

                // Determine the order in which customers get to choose to line up.
                List<Customer> customersInPriorityOrder = this.customersRequestingLineUp.ToList();
                customersInPriorityOrder.Sort(this.priorityComparer);

                // Get each customer's line choice and line them up.
                foreach (Customer cust in customersInPriorityOrder)
                {
                    LineOverview overview = this.GetLineOverview(); // Get a new line overview for each customer, because as the customers are lining up, the line overview will change.
                    int nLine = cust.GetLineChoice(overview);
                    this.LineUp(cust, nLine);
                }

                this.customersRequestingLineUp.Clear();
            }

            private class CustomerLineupPriorityComparer : IComparer<Customer>
            {
                public int Compare(Customer x, Customer y)
                {
                    if (x.TotalItems != y.TotalItems)
                        return x.TotalItems - y.TotalItems;

                    if(x.Type != y.Type)
                    {
                        if (x.Type == Customer.PersonalityType.A)
                            return -1;
                        else
                            return 1;
                    }

                    return (x.Number - y.Number);
                }
            }
        }



        // This class is mainly a data object. It is used to share the state of the lines with the customers (without giving them too much info)
        // so that they can make a decision about what line to choose. 
        public class LineOverview
        {
            public List<Line> Lines  = new List<Line>();

            protected LineOverview() { }

            public LineOverview(List<Register> regs)
            {
                regs.ForEach(r => this.Lines.Add(new Line(r)));
                this.Lines.Sort((l1, l2) => l1.Number - l2.Number); // Shouldn't be necessary, but doesn't hurt
            }


            public class Line
            {
                public int Number;
                public List<Customer> Customers = new List<Customer>();

                protected Line() { }

                public Line(Register reg)
                {
                    this.Number = reg.Number;

                    if(reg.CustCheckingOut == null)
                        return;

                    // For the customer at the front of the line
                    // we figure out how many items are remaining by asking the register.
                    this.Customers.Add(new Customer(reg.ItemsRemaining)); 

                    // For the other customers in line
                    // we figure out how many items are remaining by asking the customers.
                    foreach(var cust in reg.Customers.Skip(1))
                        this.Customers.Add(new Customer(cust.TotalItems));
                }
            }


            public class Customer
            {
                protected Customer() { }

                public Customer(int itemsRemaining)
                {
                    this.ItemsRemaining = itemsRemaining;
                }

                public int ItemsRemaining;
            }
        }
    }
}
