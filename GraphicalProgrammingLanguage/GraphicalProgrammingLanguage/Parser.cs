using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data;

namespace GraphicalProgrammingLanguage
{
    /// <summary>
    /// Parses a String, or String[] one line at a time, to determine what commands and parameters have been entered, and executes them if they are correct and execution is required.
    /// </summary>
    internal class Parser
    {
        Validator validator;
        ShapeFactory shapeFactory;
        Canvas c;
        List<String> exceptions;
        List<int> args;
        String[] parts;
        String strArguments;
        String command;
        Color color;
        Dictionary<String, Variable> variables;

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
            exceptions = new List<String>();
            variables = new Dictionary<String, Variable>();

            foreach (String line in lines)
            {
                line.Trim(' ');
                if (line.Equals(String.Empty))
                {
                    exceptions.Add(String.Empty);
                    continue;
                }

                parts = line.Split(' ');
                command = parts[0];
                strArguments = "";
                if (parts.Length > 1) strArguments = parts[1];
                try
                {
                    validator.ValidateCommand(parts, variables.Keys);

                    if (parts[1].Equals("=") && (execute))
                    {
                        SetVariable(parts[0], new ArraySegment<string>(parts, 2, parts.GetLength(0) - 2).ToArray<String>());
                        
                    }

                    if (validator.IsShape(command))
                    {
                        args = this.GetIntArgs(strArguments);
                        if (command.Equals("polygon") || command.Equals("star")) validator.ValidatePolygon(command, args[0]);
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
                                    color = ColorTranslator.FromHtml(strArguments);
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
                            case "var":
                                variables.Add(strArguments, new Variable());
                                break;
                            default:
                                if (!VariableExists(parts[0]))
                                    throw new GPLException("Unknown command '" + command + "' found.");
                                break;
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

        /// <summary>
        /// Sets a Variable object's attributes. Evaluates expression if required.
        /// </summary>
        /// <param name="variable">Variable name to be assigned a value.</param> 
        /// <param name="expression">Variable value or expression that results in the value.</param> 
        private void SetVariable(String variable, String[] expression)
        {
            DataTable dt = new DataTable();
            String parsed = "";
            int j = 0;
            // If length of expression is 1 then it's either an int value or another variable.
            // Set this variable value as such.
            if (variables.TryGetValue(variable, out Variable v))
            {
                if (expression.GetLength(0) == 1)
                {
                    if (Int32.TryParse(expression[0], out int i))
                        v.setValue(i);
                    else
                    {
                        // Already checked that the variable exists.
                        v.setValue(GetVariableValue(expression[0]));
                    }
                }
                else
                {
                    if (Validator.validArgs.TryGetValue("var", out Regex var))
                    {
                        //Multi element expression.
                        //First replace any variables with their value
                        foreach (String s in expression)
                        {
                            parsed += (var.IsMatch(s)) ? GetVariableValue(s).ToString() : s;
                        }

                        v.setValue(Int32.Parse(dt.Compute(parsed, "").ToString()));
                    }
                }
            }
            // Doing this right at the end when we know it's valid. Belts and Braces!
            v.setExpression(expression);
        }

        /// <summary>
        /// Simply returns the value of a variable when given a valid variable name.
        /// </summary>
        /// <param name="variable">Name of the variable to return the value of.</param>
        /// <returns>Value of the named variable.</returns>
        private int GetVariableValue(String variable)
        {
            return GetVariable(variable).getValue();
        }

        /// <summary>
        /// Returns a variable object when given a valid variable name.
        /// </summary>
        /// <param name="variable">Name of the variable to return.</param>
        /// <returns>Variable object.</returns>
        /// <exception cref="GPLException">Variable does not exist.</exception>
        private Variable GetVariable(String variable)
        {
            if (variables.TryGetValue(variable, out Variable v))
                return v;
            else
                throw new GPLException("Problem getting variable " + variable);
        }

        /// <summary>
        /// Checks if a variable exists.
        /// </summary>
        /// <param name="variable">Name of variable to check for.</param>
        /// <returns>Boolean result.</returns>
        private bool VariableExists(String variable)
        {
            return variables.ContainsKey(variable);
        }

        
    }
}
