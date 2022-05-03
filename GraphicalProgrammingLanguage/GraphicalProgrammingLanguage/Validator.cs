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
        private static Regex oneInt = new Regex("^\\d*$");
        private static Regex twoInts = new Regex("^\\d*,\\d*$");
        private Dictionary<String, Regex> validCommands = new Dictionary<String, Regex>() { { "circle",  oneInt }, 
                                                                                            { "rectangle", twoInts }, 
                                                                                            { "fill", new Regex("^on|off$") }, 
                                                                                            { "pen", new Regex("^#(([\\da-f]{3}){1,2})$|^([a-zA-Z]{3,})$") },
                                                                                            { "triangle", oneInt },
                                                                                            { "star", twoInts },
                                                                                            { "square", oneInt },
                                                                                            { "polygon", twoInts },
                                                                                            { "moveto", twoInts },
                                                                                            { "drawto", twoInts },
                                                                                            { "var", new Regex("^([a-z]|[A-Z])+$")}
                                                                                          };
        private List<String> shapes = new List<String>() { "circle", "star", "rectangle", "triangle", "square", "polygon"};
        private List<String> commands = new List<String>() { "moveto", "drawto", "reset", "clear", "pen", "fill", "var"};

        /// <summary>
        /// Takes a String array, expected to hold 2 values, and validates them to the expected format of acceptable commands for the program using Regex and Lists.
        /// </summary>
        /// <param name="cmd">String array containing a potential command word in [0] and a set of potential arguments in [1].</param>
        /// <exception cref="GPLException">Command not found in a List of valid commands or String array length is not 2.</exception>
        public void ValidateCommand(String[] cmd)
        {
            // If the command isn't in one of these lists, it doesn't exist.
            
            if (!shapes.Contains(cmd[0]) && !commands.Contains(cmd[0]))
            {
                throw new GPLException("Bad command found: " + cmd[0]);
            }

            // Only single word lines should be reset and clear.
            if(!(String.Equals(cmd[0],"reset") || String.Equals(cmd[0],"clear")) && !(cmd.GetLength(0) == 2))
            {
                throw new GPLException("No arguments provided: " + cmd.ToString());
            }

            // Seperates the arguments from the command.
            String[] args = new ArraySegment<string>(cmd, 1, cmd.GetLength(0) - 1).ToArray<String>();
            ValidateArgs(cmd[0], args);

        }

        /// <summary>
        /// Validates potential arguments for a command.
        /// </summary>
        /// <param name="cmd">Command the arguments are for.</param>
        /// <param name="args">Potential arguments.</param>
        /// <exception cref="GPLException">Arguments provided do not match the expected format for the command.</exception>
        private void ValidateArgs(String cmd, String[] args)
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
