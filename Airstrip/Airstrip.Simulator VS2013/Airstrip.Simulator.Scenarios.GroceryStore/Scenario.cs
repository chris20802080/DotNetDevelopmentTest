using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Airstrip.Simulator.Scenarios.GroceryStore
{
    public class Scenario : IScenario
    {
        private List<Components.Customer> Customers;
        private Components.GroceryStore Store;

        private event Action<long> Tick, PostTick;

        //public void Init()
        public Scenario(NameValueCollection parameters) : this(SetupFactory.Get(parameters))
        {

        }

        internal Scenario(Setup setup)
        {
            this.Customers = setup.Customers;
            this.Store = setup.Store;

            foreach (var cust in this.Customers)
            {
                this.Tick += cust.OnTick;
                this.PostTick += cust.OnPostTick;
            }

            this.Tick += this.Store.OnTick;
            this.PostTick += this.Store.OnPostTick;

            this.Customers.ForEach(c => c.Enter(this.Store));
        }


        public void OnTick(long time)
        {
            this.Tick(time);
            this.PostTick(time);
        }

        public bool IsDone
        {
            get 
            {
                return this.Customers.All(c => c.State == Components.Customer.CustomerState.DONE);
            }
        }

        public string Status
        {
            get
            {
                return this.Store.Status;
            }
        }
    }
}
