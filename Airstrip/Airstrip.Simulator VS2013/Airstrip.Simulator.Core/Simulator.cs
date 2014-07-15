using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Airstrip.Simulator
{
    public class Simulator
    {
        private long ticksElapsed = -1;
        private IScenario scenario;
        private TextWriter output;
        private bool bDebugMode = false;
        private bool bVerbose = false;

        public event Action<long> Tick;


        public Simulator(IScenario scenario, TextWriter output = null, bool bDebugMode = false, bool bVerbose = false)
        {
            this.scenario = scenario;
            this.output = output;
            this.bDebugMode = bDebugMode;
            this.bVerbose = bVerbose;

            this.Tick += scenario.OnTick;
        }



        public void Run()
        {
            while (!scenario.IsDone)
            {
                this.FireTick();

                if (this.bVerbose)
                    this.OutputStatus();
            }

            this.OutputSummary();
        }


        protected virtual void FireTick()
        {
            this.Tick(++this.ticksElapsed);
        }


        protected virtual void OutputStatus()
        {
            this.output.WriteLine();
            this.output.WriteLine(string.Format("*** Time {0} ***", this.ticksElapsed));
            this.output.WriteLine(this.scenario.Status);
            this.output.WriteLine();
        }

        protected virtual void OutputSummary()
        {
            this.output.WriteLine();
            this.output.WriteLine(string.Format("*** Time {0} ***", this.ticksElapsed));
            this.output.WriteLine("Finished");
            this.output.WriteLine();
        }
    }
}
