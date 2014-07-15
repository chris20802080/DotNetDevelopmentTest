using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airstrip.Simulator.Scenarios.GroceryStore.Components
{
    class RegisterNormal : Register
    {
        public RegisterNormal(int number) : base(number) 
        { 

        }

        public override int ItemProcessingTime
        {
            get { return 1; }
        }
    }
}
