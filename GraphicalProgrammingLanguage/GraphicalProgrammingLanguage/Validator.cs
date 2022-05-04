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
        public static Regex oneInt = new Regex("^\\d*$");
        public static Regex twoInts = new Regex("^\\d*,\\d*$");
        public static Dictionary<String, Regex> validArgs = new Dictionary<String, Regex>() { 
            { "circle",  oneInt }, 
            { "rectangle", twoInts }, 
            { "fill", new Regex("^on|off$") }, 
            { "pen", new Regex("^#(([\\da-f]{3}){1,2})$|^([a-zA-Z]{3,})$") },
            { "triangle", oneInt },
            { "star", twoInts },
            { "square", oneInt },
            { "polygon", twoInts },
            { "moveto", twoInts },
            { "drawto", twoInts },
            { "var", new Regex("^([a-z]|[A-Z])+$") },
            { "math", new Regex("^(\\+|\\*|\\\\|\\-|=)$") } 
        };
        private List<String> shapes = new List<String>() { "circle", "star", "rectangle", "triangle", "square", "polygon"};
        private List<String> commands = new List<String>() { "moveto", "drawto", "reset", "clear", "pen", "fill", "var"};

        /// <summary>
        /// Takes a String array, expected to hold 2 values, and validates them to the expected format of acceptable commands for the program using Regex and Lists.
        /// </summary>
        /// <param name="cmd">String array containing a potential command word in [0] and a set of potential arguments in [1].</param>
        /// <exception cref="GPLException">Command not found in a List of valid commands or String array length is not 2.</exception>
        public void ValidateCommand(String[] cmd, Dictionary<String, Variable>.KeyCollection variables)
        {
            if (!shapes.Contains(cmd[0]) && !commands.Contains(cmd[0]) && cmd.Length < 2)
                throw new GPLException("Bad command found: " + cmd[0]);

            // If the command isn't in one of these lists, and this isn't an assignment statement, it doesn't exist.
            if (!shapes.Contains(cmd[0]) && !commands.Contains(cmd[0]) && !cmd[1].Equals("="))
                throw new GPLException("Bad command found: " + cmd[0]);

            // If creating variable, check it does not exist. Variables should also be a single word only.
            if (cmd[0].Equals("var") && variables.Contains(cmd[1]) && cmd.Length == 2)
                throw new GPLException("Variable " + cmd[1] + " already exists");

            // Check if variable exists for assingment
            if (!shapes.Contains(cmd[0]) && !commands.Contains(cmd[0]) && cmd[1].Equals("=") && !variables.Contains(cmd[0]))
                    throw new GPLException("Declaration: Variable " + cmd[0] + " does not exist.");

            // Only single word lines should be reset and clear.
            if (!(String.Equals(cmd[0], "reset") || String.Equals(cmd[0], "clear")) && !(cmd.Length >= 2))
                throw new GPLException("No arguments provided: " + cmd.ToString());

            // Seperates the arguments from the command.
            String[] args = new ArraySegment<string>(cmd, 1, cmd.GetLength(0) - 1).ToArray<String>();

            if (cmd[1].Equals("="))
                ValidateVariableAssignment(args, variables);
            else
                ValidateArgs(cmd[0], args);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="variables"></param>
        /// <exception cref="GPLException"></exception>
        private void ValidateVariableAssignment(String[] args, Dictionary<String, Variable>.KeyCollection variables)
        {
            if(validArgs.TryGetValue("math", out Regex math) && validArgs.TryGetValue("var", out Regex var))
            {
                //TODO: Could add validation for order of items i.e. should start with int or variable, then one math operator, then one int/var etc.
                //Each element should be an existing variable, and integer or a math operator.
                foreach (String arg in args)
                {
                    if (!(math.IsMatch(arg) || var.IsMatch(arg) || oneInt.IsMatch(arg)))
                        throw new GPLException("Bad assignment statement.");

                    if (var.IsMatch(arg))
                        if (!variables.Contains(arg))
                            throw new GPLException("Assignment: Variable " + arg + " does not exist.");
                }
            }
        }

        /// <summary>
        /// Validates potential arguments for a command.
        /// </summary>
        /// <param name="cmd">Command the arguments are for.</param>
        /// <param name="args">Potential arguments.</param>
        /// <exception cref="GPLException">Arguments provided do not match the expected format for the command.</exception>
        private void ValidateArgs(String cmd, String[] args)
        {
            if (validArgs.TryGetValue(cmd, out Regex pattern))
                if (!pattern.IsMatch(args[0]))
                    throw new GPLException("Bad arguments found: " + args[0]);
        }

        /// <summary>
        /// Checks to ensure a Polygon/Star has at least 5 sides/points.
        /// </summary>
        /// <param name="shape">String input. Either "Star" or "Polygon".</param>
        /// <param name="arg">Integer. Number of sides/points for Shape to be checked.</param>
        /// <exception cref="GPLException">arg less than 5</exception>
        public void ValidatePolygon(String shape, int arg)
        {
            if (arg < 5) throw new GPLException(shape + " must have more than 4 points/sides.");
        }

        /// <summary>
        /// Checks if a command is for a valid shape.
        /// </summary>
        /// <param name="cmd">Command to be checked.</param>
        /// <returns>Boolean. True if cmd is in the list of valid Shapes, otherwise False.</returns>
        public bool IsShape(String cmd)
        {
            return shapes.Contains(cmd);
        }
    }
}
