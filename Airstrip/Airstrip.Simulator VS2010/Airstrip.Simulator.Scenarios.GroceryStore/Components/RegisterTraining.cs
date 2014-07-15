using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airstrip.Simulator.Scenarios.GroceryStore.Components
{
    class RegisterTraining : Register
    {

        public RegisterTraining(int number) : base(number) 
        { 

        }

        public override int ItemProcessingTime
        {
            get { return 2; }
        }
    }
}
