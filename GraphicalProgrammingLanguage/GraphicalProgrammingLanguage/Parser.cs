using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace GraphicalProgrammingLanguage
{
    /// <summary>
    /// Parses a String, or String[] one line at a time, to determine what commands and parameters have been entered, and executes them if they are correct and execution is required.
    /// </summary>
    internal class Parser
    {
        private Validator validator;
        ShapeFactory shapeFactory;
        Canvas c;

        /// <summary>
        /// Constructor taking a single parameter, a reference to a Canvas object.
        /// </summary>
        /// <param name="c">Canvas object</param>
        public Parser(Canvas c)
        {
            this.c = c;
            validator = new Validator();
            shapeFactory = new ShapeFactory();
        }

        /// <summary>
        /// Parses a String array and either executes a correctly entered command or returns any Exceptions in another String array so they can be shown to the user.
        /// </summary>
        /// <param name="lines">String array containing user entered commands.</param>
        /// <param name="execute">Boolean determining if the commands should be executed (True), or just validated (False).</param>
        /// <returns>String array holding any exceptions caused by the user inputted commands.</returns>
        public String[] parseLines(String[] lines, Boolean execute)
        {
            List<String> exceptions = new List<String>();

            foreach(String line in lines)
            {
                line.Trim(' ');
                if (line.Equals(String.Empty))
                {
                    exceptions.Add(String.Empty);
                    continue;
                }
                String[] parts = line.Trim(' ').Split(' ');
                String command = parts[0];
                String strArguments = "";
                if (parts.Length > 1) strArguments = parts[1];
                try
                {
                    List<int> args;
                    validator.ValidateCommand(parts);

                    if (validator.IsShape(command))
                    {
                        args = this.GetIntArgs(strArguments);
                        if (command.Equals("polygon") || command.Equals("star")) validator.ValidatePolygon(command, args);
                        if (command.Equals("square")) args.Add(args[0]); //Adds size of square again so Rectangle can be re-used.
                        if (execute) c.DrawShape(shapeFactory.getShape(command), args);
                    }
                    else
                    {
                        switch(parts[0]) {
                            case "clear":
                                if (execute) c.Clear();
                                break;
                            case "reset":
                                if (execute) c.Reset();
                                break;
                            case "moveto":
                                args = this.GetIntArgs(strArguments);
                                if (execute) c.MoveCursor(args);
                                break;
                            case "drawto":
                                args = this.GetIntArgs(strArguments);
                                if (execute) c.DrawLine(args);
                                break;
                            case "pen":
                                try
                                {
                                    Color color = ColorTranslator.FromHtml(strArguments);
                                    if (execute) c.SetColor(color);
                                }
                                catch(Exception e)
                                {
                                    if(e.Message.Equals(strArguments + " is not a valid value for Int32.") || e.Message.Equals("Could not find any recognizable digits."))
                                    {
                                        throw new GPLException(strArguments + " is not a valid color name or hex.");
                                    }
                                    else
                                    {
                                        throw e;
                                    }
                                }
                                break;
                            case "fill":
                                if (execute) c.SetFill(strArguments);
                                break;
                            default:
                                throw new GPLException("Unknown command '" + command + "' found.");
                        }
                    }

                    exceptions.Add(String.Empty);

                } catch (GPLException e)
                {
                    exceptions.Add(e.Message);
                    //Console.WriteLine(e.Message);
                }

            }

            return exceptions.ToArray();
        }

        /// <summary>
        /// Takes in a String and extracts values as Integers, then returns them in a List.
        /// </summary>
        /// <param name="argsIn"></param>
        /// <returns>Integer List containing the converted values.</returns>
        public List<int> GetIntArgs(String argsIn)
        {
            String[] strArgs = argsIn.Split(',');
            List<int> args = new List<int>();
            foreach (String s in strArgs)
            {
                // For variables this will need changing.
                int i;
                int.TryParse(s, out i);
                args.Add(i);
            }
            return args;
        }

        
    }
}
