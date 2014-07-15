using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Ninject;

namespace Airstrip.Simulator
{
    class ScenarioFactory
    {
        private static IKernel kernel;

        static ScenarioFactory()
        {
            kernel = new StandardKernel();

            Add<Scenarios.GroceryStore.Scenario>();
            // Add more scenarios here.
        }

        private static void Add<T>() where T : class, IScenario
        {
            string[] namespaceLevels = typeof(T).Namespace.Split('.');
            kernel.Bind<IScenario>().To<T>().Named(namespaceLevels.Last());
        }


        public static IScenario Make(string name, NameValueCollection parameters)
        {
            return kernel.Get<IScenario>(name, new Ninject.Parameters.ConstructorArgument("parameters", parameters));
        }
    }
}
