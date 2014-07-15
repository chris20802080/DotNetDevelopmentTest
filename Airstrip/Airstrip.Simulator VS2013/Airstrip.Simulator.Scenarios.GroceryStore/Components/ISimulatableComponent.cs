using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airstrip.Simulator.Scenarios.GroceryStore.Components
{
    interface ISimulatableComponent
    {
        string Status { get; }
        void OnTick(long time);
        void OnPostTick(long time);
    }
}
