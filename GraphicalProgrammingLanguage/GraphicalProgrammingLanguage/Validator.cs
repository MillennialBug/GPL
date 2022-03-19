using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Drawing;

namespace GraphicalProgrammingLanguage
{
    internal class Validator
    {
        private Dictionary<String, Regex> validCommands = new Dictionary<String, Regex>() { { "circle", new Regex("^\\d*$") }, { "rectangle", new Regex("^\\d*,\\d*$")}, { "fill", new Regex("^on|off$") } };
        private List<String> shapes = new List<String>() { "circle", "star", "rectangle", "triangle"};
        private List<String> commands = new List<String>() { "moveto", "drawto", "reset", "clear", "pen", "fill"};

        public void validateCommand(String[] cmd)
        {
            if (!shapes.Contains(cmd[0]) && !commands.Contains(cmd[0]))
            {
                throw new GPLException("Bad command found: " + cmd[0]);
            }

            if(!(String.Equals(cmd[0],"reset") || String.Equals(cmd[0],"clear")) && !(cmd.GetLength(0) > 1))
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

        public bool isCommand(String cmd) 
        {
            return commands.Contains(cmd);
        }
    }
}
