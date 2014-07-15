using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airstrip.Simulator.Scenarios.GroceryStore.Components
{
    internal abstract class Register : ISimulatableComponent
    {
        public int Number { get; private set; }
        public Queue<Customer> Customers = new Queue<Customer>();

        public Customer CustCheckingOut { get; private set; }
        public int ItemsRemaining { get; private set; }
        public int ItemProcessingTimeRemaining { get; private set; }

        public Register(int number)
        {
            this.Number = number;
        }

        public void LineUp(Customer cust)
        {
            this.Customers.Enqueue(cust);
            cust.OnLineUp();

            if (this.CustCheckingOut == null)
                this.custStart();
        }

        public string Status
        {
            get 
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format("Register #{0}: Customers = {1}", this.Number, this.Customers.Count));

                foreach(var cust in this.Customers)
                    sb.Append(string.Format(", #{0}/{1}/{2}", cust.Number, cust == this.CustCheckingOut? this.ItemsRemaining : cust.TotalItems, cust.State));

                return sb.ToString();
            }
        }

        public void OnTick(long time)
        {
            if (this.CustCheckingOut == null)
                return;

            if (--this.ItemProcessingTimeRemaining == 0)
            {
                this.ItemProcessingTimeRemaining = this.ItemProcessingTime;

                if (--this.ItemsRemaining == 0)
                    this.custDone();
            }
        }

        public void OnPostTick(long time)
        {
        }

        private void custStart()
        {
            if (this.Customers.Count == 0)
                return;     // Should never happen.

            this.CustCheckingOut = this.Customers.Peek();
            this.ItemsRemaining = this.CustCheckingOut.TotalItems;
            this.ItemProcessingTimeRemaining = this.ItemProcessingTime;

            this.CustCheckingOut.OnStartCheckOut();

            if (this.ItemsRemaining == 0)
                this.custDone();
        }

        private void custDone()
        {
            this.Customers.Dequeue();
            this.CustCheckingOut.OnDone();

            if (this.Customers.Count == 0)
            {
                this.CustCheckingOut = null;
            }
            else
            {
                this.custStart();
            }
        }

        public abstract int ItemProcessingTime { get; }
    }
}
