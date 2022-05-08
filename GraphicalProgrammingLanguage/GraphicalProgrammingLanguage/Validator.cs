using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GraphicalProgrammingLanguage
{
    public class Validator
    {
        public static Regex oneArg = new Regex("^(\\d+|[a-zA-Z]+)$");
        public static Regex oneWord = new Regex("^([a-zA-Z])+$");
        public static Regex twoArgs = new Regex("^(\\d+|[a-zA-Z]+),(\\d+|[a-zA-Z]+)$");
        public static Regex invalidChars = new Regex("[^a-zA-Z\\d\\\\\\+\\*\\-=\\,#\\s]");
        public static Dictionary<String, Regex> validArgs = new Dictionary<String, Regex>() { 
            { "circle",  oneArg }, 
            { "rectangle", twoArgs }, 
            { "fill", new Regex("^on|off$") }, 
            { "pen", new Regex("^#(([\\da-f]{3}){1,2})$|^([a-zA-Z]{3,})$") },
            { "triangle", oneArg },
            { "star", twoArgs },
            { "square", oneArg },
            { "polygon", twoArgs },
            { "moveto", twoArgs },
            { "drawto", twoArgs },
            { "var", oneWord },
            { "math", new Regex("^(\\+|\\*|\\\\|\\-|=)$") },
            { "method", oneWord },
            { "loop", oneArg }
        };
        public static List<String> shapes = new List<String>() { "circle", "star", "rectangle", "triangle", "square", "polygon"};
        public static List<String> commands = new List<String>() { "moveto", "drawto", "pen", "fill", "var", "method", "loop"};
        public static List<String> singleWordCommands = new List<String>() { "reset", "clear", "endmethod", "endloop" };
        public static Validator validator = new Validator();

        private Validator() { }

        public static Validator GetValidator() { return validator; }

        /// <summary>
        /// Takes a String array and validates the contents against the expected format of acceptable commands for the program using Regex and Lists.
        /// </summary>
        /// <param name="cmd">String array containing a potential command word in [0] and a set of potential arguments in [1].</param>
        /// <param name="variables">A list of variable names.</param>
        /// <param name="methods">A list of method names.</param>
        /// <exception cref="GPLException">Command not found in a List of valid commands or String array length is not 2.</exception>
        public void ValidateCommand(String[] cmd, Dictionary<String, Variable>.KeyCollection variables, Dictionary<String, Method>.KeyCollection methods)
        {
            foreach(String s in cmd)
            {
                if (invalidChars.IsMatch(s))
                    throw new GPLException("Invalid character found in command.");
            }

            if (!shapes.Contains(cmd[0]) && !commands.Contains(cmd[0]) && !singleWordCommands.Contains(cmd[0]) && !methods.Contains(cmd[0]) && !variables.Contains(cmd[0]))
                throw new GPLException("Bad command found: " + cmd[0]);

            // Only single word lines should be reset, clear, endmethod or defined methods.
            if (!singleWordCommands.Contains(cmd[0]) && !methods.Contains(cmd[0]) && cmd.Length < 2)
                throw new GPLException("No arguments provided.");

            if(cmd.Length > 1)
            {
                // If creating variable, check it does not exist.
                if (cmd[0].Equals("var") && variables.Contains(cmd[1]))
                    throw new GPLException("Variable " + cmd[1] + " already exists");
                // Check that variable is not a command/method name.
                else if (cmd[0].Equals("var") &&  (shapes.Contains(cmd[1]) || commands.Contains(cmd[1]) || singleWordCommands.Contains(cmd[1]) || methods.Contains(cmd[1])))
                    throw new GPLException("Variable name cannot be the same as an existing command or method.");
                // Check if variable exists for assingment
                else if (cmd[1].Equals("="))
                    if (!variables.Contains(cmd[0]))
                        throw new GPLException("Variable " + cmd[0] + " does not exist and cannot be assigned a value.");
                    else
                        ValidateVariableAssignment(cmd, variables);

                // If creating method, check it does not exist.
                if (cmd[0].Equals("method") && methods.Contains(cmd[1]))
                    throw new GPLException("Method " + cmd[1] + " already exists");
                // Check that method is not a command/variable name.
                else if (cmd[0].Equals("method") && (shapes.Contains(cmd[1]) || commands.Contains(cmd[1]) || singleWordCommands.Contains(cmd[1]) || variables.Contains(cmd[1])))
                    throw new GPLException("Mathod name cannot be the same as an existing command or variable.");

                ValidateArgs(cmd[0], new ArraySegment<string>(cmd, 1, cmd.GetLength(0) - 1).ToArray<String>());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="variables"></param>
        /// <exception cref="GPLException"></exception>
        public void ValidateVariableAssignment(String[] cmd, Dictionary<String, Variable>.KeyCollection variables)
        {
            if(validArgs.TryGetValue("math", out Regex math) && validArgs.TryGetValue("var", out Regex var))
            {
                //TODO: Could add validation for order of items i.e. should start with int or variable, then one math operator, then one int/var etc.
                //Each element should be an existing variable, an integer or a math operator.
                foreach (String s in cmd)
                {
                    if (var.IsMatch(s))
                        if (!variables.Contains(s))
                            throw new GPLException("Variable '" + s + "' does not exist.");
                }
            }
        }

        /// <summary>
        /// Validates potential arguments for a command.
        /// </summary>
        /// <param name="cmd">Command the arguments are for.</param>
        /// <param name="args">Potential arguments.</param>
        /// <exception cref="GPLException">Arguments provided do not match the expected format for the command.</exception>
        public void ValidateArgs(String cmd, String[] args)
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
