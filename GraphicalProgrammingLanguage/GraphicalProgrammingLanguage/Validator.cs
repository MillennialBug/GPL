using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GraphicalProgrammingLanguage
{
    internal class Validator
    {
        private Dictionary<String, Regex> validCommands = new Dictionary<String, Regex>() { { "circle", new Regex("^\\d*$") }, { "rectangle", new Regex("^\\d*,\\d*$")} };
        private List<String> shapes = new List<String>() { "circle", "star", "rectangle"};

        public void validateCommand(String[] cmd)
        {
            if (!validCommands.ContainsKey(cmd[0]))
            {
                throw new GPLException("Bad command found: " + cmd[0]);
            }
            if(!(cmd.GetLength(0) > 1))
            {
                throw new GPLException("No arguments provided: " + cmd.ToString());
            }
            String[] args = new ArraySegment<string>(cmd, 1, cmd.GetLength(0) - 1).ToArray<String>();
            validateArgs(cmd[0], args);

        }

        private void validateArgs(String cmd, String[] args)
        {
            Regex pattern;
            if (validCommands.TryGetValue(cmd, out pattern))
            {
                if (!pattern.IsMatch(args[0]))
                {
                    throw new GPLException("Bad arguments found: " + args[0]);
                }
            }
        }

        public bool isShape(String cmd)
        {
            return shapes.Contains(cmd);
        }
    }
}
