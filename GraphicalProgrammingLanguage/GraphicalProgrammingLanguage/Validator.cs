using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GraphicalProgrammingLanguage
{
    /// <summary>
    /// Singleton Pattern.
    /// Class used to validate (mostly) the format of a given command string.
    /// </summary>
    public class Validator
    {
        public static Regex oneArg = new Regex("^(\\d+|[a-zA-Z]+)$");
        public static Regex oneWord = new Regex("^[a-zA-Z]+$");
        public static Regex paramMethod = new Regex("^([a-zA-Z]+)(?:(?:\\([a-zA-Z]+\\))|(?:\\(([a-zA-Z]+)(?:, *[a-zA-Z]+)+\\)))$");
        public static Regex twoArgs = new Regex("^(\\d+|[a-zA-Z]+),(\\d+|[a-zA-Z]+)$");
        public static Regex invalidChars = new Regex("[^a-zA-Z\\d\\\\\\+\\*\\-=\\,#\\s<>\\(\\)]");
        public static Regex comparrison = new Regex("[==|>=|<=|>|<]{1}");
        public static Dictionary<String, Regex> validArgs = new Dictionary<String, Regex>() { 
            { "circle",  oneArg }, 
            { "rectangle", twoArgs }, 
            { "fill", new Regex("^on|off$") }, 
            { "pen", new Regex("^#(([\\da-f]{3}){1,2})$|^([a-zA-Z]{3,})$|^[a-zA-Z]+") },
            { "triangle", oneArg },
            { "star", twoArgs },
            { "square", oneArg },
            { "polygon", twoArgs },
            { "moveto", twoArgs },
            { "drawto", twoArgs },
            { "var", oneWord },
            { "math", new Regex("^(\\+|\\*|\\\\|\\-|=)$") },
            { "loop", oneArg },
            { "method", oneWord },
            { "paramMethod", paramMethod }
        };
        public static List<String> shapes = new List<String>() { "circle", "star", "rectangle", "triangle", "square", "polygon"};
        public static List<String> commands = new List<String>() { "moveto", "drawto", "pen", "fill", "var", "method", "loop", "if", "while"};
        public static List<String> singleWordCommands = new List<String>() { "reset", "clear", "endmethod", "endloop", "endif", "endwhile" };
        public static Validator validator = new Validator();

        private Validator() { }

        /// <summary>
        /// Returns the Validator instance.
        /// </summary>
        /// <returns></returns>
        public static Validator GetValidator() { return validator; }

        /// <summary>
        /// Takes a String array and validates the contents against the expected format of acceptable commands for the program using Regex and Lists.
        /// </summary>
        /// <param name="cmd">String array containing a potential command word in [0] and a set of potential arguments in [1].</param>
        /// <param name="variables">A list of variable names.</param>
        /// <param name="methods">A list of method names.</param>
        /// <param name="methodExecuting">ParamMethod being executed, or null by default. Used to check for the correct variables.</param>
        /// <exception cref="GPLException">Thows exceptions for invalid commands for a number of reasons.</exception>
        public void ValidateCommand(String[] cmd, Dictionary<String, Variable>.KeyCollection variables, Dictionary<String, Method>.KeyCollection methods, ParamMethod methodExecuting = null)
        {
            foreach(String s in cmd)
            {
                if (invalidChars.IsMatch(s))
                    throw new GPLException("Invalid character found in command.");
            }
                
            if (!shapes.Contains(cmd[0]) && !commands.Contains(cmd[0]) && !singleWordCommands.Contains(cmd[0]) && !methods.Contains(cmd[0]) && !variables.Contains(cmd[0]))
            {
                String variableName = cmd[0].Substring(0, cmd[0].Length - 2);
                String op = cmd[0].Substring(cmd[0].Length - 2, 2);
                if (!(variables.Contains(variableName) && (op.Equals("++") || op.Equals("--"))))
                    throw new GPLException("Bad command found: " + cmd[0]);
                else 
                    return;
            }

            // Only single word lines should be reset, clear, endmethod or defined methods.
            if (!singleWordCommands.Contains(cmd[0]) && !methods.Contains(cmd[0]) && cmd.Length < 2)
                throw new GPLException("No arguments provided.");

            if(cmd.Length > 1)
            {
                // If creating variable, check it does not exist.
                if (cmd[0].Equals("var") && variables.Contains(cmd[1]))
                    throw new GPLException("Variable " + cmd[1] + " already exists");
                // Check that variable is not a command/method name.
                else if (cmd[0].Equals("var") && (shapes.Contains(cmd[1]) || commands.Contains(cmd[1]) || singleWordCommands.Contains(cmd[1]) || methods.Contains(cmd[1])))
                    throw new GPLException("Variable name cannot be the same as an existing command or method.");
                else if (variables.Contains(cmd[0]) && !cmd[1].Equals("="))
                    throw new GPLException("Invalid variable assignment.");
                // Check if variable exists for assingment
                else if (cmd[1].Equals("="))
                    if (!variables.Contains(cmd[0]))
                        throw new GPLException("Variable " + cmd[0] + " does not exist and cannot be assigned a value.");
                    else
                        ValidateVariableAssignment(cmd, variables);

                ValidateArgs(cmd[0], new ArraySegment<string>(cmd, 1, cmd.GetLength(0) - 1).ToArray<String>());
            }
        }

        /// <summary>
        /// Validates a command for setting a variable value.
        /// </summary>
        /// <param name="cmd">Variable assingment command in a String array E.G. {"one","=","1","+","zero"}</param>
        /// <param name="variables">Dictionary collection of variables. Used to validate named variable exists.</param>
        /// <exception cref="GPLException">Throws exception if a named variable does not exist.</exception>
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
            if (cmd.Equals("if") || cmd.Equals("while"))
            {
                if (args.Length < 3)
                    throw new GPLException("'" + cmd + "' conditional incorrectly formatted.");

                if (!oneArg.IsMatch(args[0]) || !comparrison.IsMatch(args[1]) || !oneArg.IsMatch(args[2]))
                    throw new GPLException("'" + cmd + "' conditional incorrectly formatted.");
            }
            else
            {
                if (validArgs.TryGetValue(cmd, out Regex pattern))
                    if (!pattern.IsMatch(args[0]))
                        throw new GPLException("Bad arguments found: " + args[0]);
            }
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

        /// <summary>
        /// Validates method creation commands.
        /// </summary>
        /// <param name="cmd">Command line for creating a Method or ParamMethod.</param>
        /// <param name="variables">Dictionary collection of Variables.</param>
        /// <param name="methods">Dictionary collection of Methods.</param>
        /// <returns>String with the value of the method name being created.</returns>
        /// <exception cref="GPLException">Throws an exception if the method already exists or if the name is the same as an existing Variable or Command.</exception>
        public String ValidateMethod(String cmd, Dictionary<String, Variable>.KeyCollection variables, Dictionary<String, Method>.KeyCollection methods)
        {
            String methodName;
            bool paramMethod = false;

            //Check if parameters have been supplied by looking for an open bracket.
            //This will help extract the method name and decide what type of method to create.
            if (cmd.IndexOf('(') >= 0)
            {
                methodName = cmd.Substring(0, cmd.IndexOf('('));
                paramMethod = true;
            }
            else
                methodName = cmd;

            // Check method doesn't already exist.
            if (methods.Contains(methodName))
                throw new GPLException("Method " + cmd + " already exists");
            // Check that method is not a command/variable name.
            else if (shapes.Contains(methodName) || commands.Contains(methodName) || singleWordCommands.Contains(methodName) || variables.Contains(methodName))
                throw new GPLException("Method name cannot be the same as an existing command or variable.");

            if (paramMethod)
                ValidateArgs("paramMethod", cmd.Trim().Split('\n'));
            else
                ValidateArgs("method", cmd.Trim().Split('\n'));

            return methodName;
        }
    }
}
