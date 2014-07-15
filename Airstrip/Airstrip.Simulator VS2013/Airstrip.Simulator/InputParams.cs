using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Airstrip.Simulator
{
    class InputParams
    {
        private static readonly string USAGE = "Airstrip.Simulator -scenario <scenario name> (-verbose) ... ";
        private static Regex commandLinePattern = new Regex(@"""-(?<switch>\w+)""( ""(?<value>[^-].*?)"")?");

        //public bool IsValid { get { return this.ScenarioName != null; } }
        public string ScenarioName { get; private set; }
        public bool IsVerbose { get; private set; }
        public NameValueCollection Params { get; private set; }

        protected InputParams()
        {
            this.Params = new NameValueCollection();
        }

        public InputParams(string[] commandLineTokens) : this()
        {
            string commandLine = '"' + string.Join("\" \"", commandLineTokens) + '"';
            var matches = commandLinePattern.Matches(commandLine);

            foreach (Match match in matches)
                this.Params.Add(match.Groups["switch"].Value.ToLower(), match.Groups["value"].Value);

            this.ScenarioName = this.Params["scenario"];
            this.IsVerbose = (this.Params["verbose"] != null);

            if (this.ScenarioName == null)
                throw new Exception("Input params invalid.\n" + USAGE);
        }

    }
}
