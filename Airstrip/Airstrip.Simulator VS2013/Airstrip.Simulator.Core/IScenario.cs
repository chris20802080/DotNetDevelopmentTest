using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airstrip.Simulator
{
    public interface IScenario
    {
        //void Init();
        void OnTick(long time);
        bool IsDone { get; }
        string Status { get; }
    }
}
