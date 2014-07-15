using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airstrip.Simulator.Scenarios.GroceryStore.Components
{
    internal abstract class Customer : ISimulatableComponent, IEquatable<Customer>
    {
        public enum PersonalityType
        {
            A,
            B
        }

        public enum CustomerState
        {
            IDLE,
            SHOPPING,
            REQUEST_LINE_UP,
            IN_LINE,
            CHECKING_OUT,
            DONE
        }

        public int Number { get; private set; }
        public abstract PersonalityType Type { get; }
        public int TotalItems { get; private set; }

        private int lineUpTime { get; set; }
        private GroceryStore store {get;set;}
        public CustomerState State { get; private set; }


        public static Customer New(DataLayer.Models.Customer customer)
        {
            switch (customer.Type)
            {
                case DataLayer.Models.Customer.PersonalityType.A:
                    return new CustomerA(customer);

                case DataLayer.Models.Customer.PersonalityType.B:
                    return new CustomerB(customer);

                default:
                    throw new Exception("Unknown customer type");
            }
        }

        protected Customer() { }

        protected Customer(DataLayer.Models.Customer customer)
        {
            this.Number = customer.Number;
            this.State = CustomerState.IDLE;

            this.TotalItems = customer.TotalItems;
            this.lineUpTime = customer.LineUpTime;
        }

        internal void Enter(GroceryStore store)
        {
            this.store = store;
            this.State = CustomerState.SHOPPING;
        }
    
        public string Status
        {
	        get 
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format("Customer #{0}-{1} {2}", this.Number, this.Type, this.State));
                return sb.ToString();
            }
        }


        public void OnTick(long time)
        {
            switch (this.State)
            {
                case CustomerState.SHOPPING:
                    if (time >= this.lineUpTime)
                    {
                        this.State = CustomerState.REQUEST_LINE_UP;
                        store.RequestLineUp(this);
                    }
                    break;

                case CustomerState.REQUEST_LINE_UP:
                    throw new Exception("Should never be in this state here.");
            }
        }
        
        public void OnPostTick(long time)
        {

        }

        public void OnLineUp()
        {
            this.State = CustomerState.IN_LINE; 
        }

        public void OnStartCheckOut()
        {
            this.State = CustomerState.CHECKING_OUT;
        }

        public void OnDone()
        {
            this.State = CustomerState.DONE;
        }

        public abstract int GetLineChoice(GroceryStore.LineOverview overview);




        public override bool Equals(object obj)
        {
            return this.Equals(obj as Customer);
        }

        public override int GetHashCode()
        {
            return this.Number;
        }

        public bool Equals(Customer other)
        {
            return this.Number == other.Number;
        }
    }
}
