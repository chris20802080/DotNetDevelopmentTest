using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airstrip.Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Program prog = new Program();

            try
            {
                prog.Run(args);

                System.Console.Out.WriteLine("Press Enter to exit.");
                System.Console.In.Read();
            }
            catch (Exception ex)
            {
                prog.Error(ex);
            }
        }


        private void Run(string[] args)
        {
            InputParams inputParams = new InputParams(args);

            IScenario scenario = ScenarioFactory.Make(inputParams.ScenarioName, inputParams.Params);
            Simulator sim = new Simulator(scenario, System.Console.Out, this.isDebug, inputParams.IsVerbose);
            sim.Run();
        }


        private bool isDebug
        {
            get
            {
                return System.Diagnostics.Debugger.IsAttached;
            }
        }


        private void Error(Exception ex)
        {
            System.Console.Out.WriteLine();
            System.Console.Out.WriteLine("*** Error ***");
            System.Console.Out.WriteLine(this.isDebug? ex.ToString() : ex.Message);
            System.Console.Out.WriteLine("*************");
            System.Console.Out.WriteLine();
        }
    }
}
