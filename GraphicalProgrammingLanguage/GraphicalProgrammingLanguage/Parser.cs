using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text.RegularExpressions;

namespace GraphicalProgrammingLanguage
{
    /// <summary>
    /// Parses a String, or String[] one line at a time, to determine what commands and parameters have been entered, and executes them if they are correct and execution is required.
    /// </summary>
    public class Parser
    {
        Validator validator;
        ShapeFactory shapeFactory;
        Canvas canvas;
        ExceptionsList exceptionsList;
        List<int> args;
        String[] parts;
        String strArguments;
        String command;
        Color color;
        Dictionary<String, Variable> variables;
        Dictionary<String, Method> methods;
        Dictionary<Int32, Loop> loops;
        int loopCount;
        bool methodFlag;
        String methodName;
        bool loopFlag;
        String trimmed;
        bool ifFlag;
        bool executeIf;
        private static Parser parser = new Parser();

        /// <summary>
        /// Constructor taking a single parameter, a reference to a Canvas object.
        /// </summary>
        /// <param name="c">Canvas object</param>
        private Parser()
        {
            validator = Validator.GetValidator();
            shapeFactory = ShapeFactory.GetShapeFactory();
        }

        public static Parser GetParser() { return parser; }

        public void SetCanvas(Canvas canvas) { this.canvas = canvas; }

        /// <summary>
        /// Parses a String array and either executes a correctly entered command or returns any Exceptions in another String array so they can be shown to the user.
        /// </summary>
        /// <param name="lines">String array containing user entered commands.</param>
        /// <param name="execute">Boolean determining if the commands should be executed (True), or just validated (False).</param>
        /// <param name="nestedExec">Boolean to determine if execution is nested. This stops lists being destroyed. Default False.</param>
        /// <returns>String array holding any exceptions caused by the user inputted commands.</returns>
        public String[] parseLines(String[] lines, Boolean execute, Boolean nestedExec = false)
        {
            if (!nestedExec)
            {
                variables = new Dictionary<string, Variable>();
                methods = new Dictionary<string, Method>();
                loops = new Dictionary<Int32, Loop>();
                loopCount = 0;
                exceptionsList = new ExceptionsList();
            }
            
            methodFlag = false;
            loopFlag = false;
            ifFlag = false;
            executeIf = false;

            foreach (String line in lines)
            {
                trimmed = line.Trim(' ');
                if (trimmed.Equals(String.Empty))
                {
                    exceptionsList.Add(String.Empty);
                    continue;
                }

                parts = trimmed.Split(' ');
                command = parts[0];
                strArguments = "";


                if (parts.Length == 2) strArguments = parts[1];
                try
                {
                    if (!methods.ContainsKey(command))
                        validator.ValidateCommand(parts, variables.Keys, methods.Keys);

                    if (ifFlag)
                    {
                        if (command.Equals("method") || command.Equals("var"))
                        {
                            throw new GPLException("Command '" + command + "' cannot be used within an if.");
                        }

                        if (command.Equals("endif"))
                        {
                            ifFlag = false;
                            executeIf = false;
                            exceptionsList.Add(String.Empty);
                            continue;
                        }

                        if (!executeIf)
                        {
                            exceptionsList.Add(String.Empty);
                            continue;
                        }
                    }

                    if (methodFlag)
                    {
                        if (command.Equals("endmethod"))
                        {
                            methodName = String.Empty;
                            methodFlag = false;
                            exceptionsList.Add(String.Empty);
                            continue;
                        }

                        if (command.Equals("method") || command.Equals("var"))
                            throw new GPLException("Command '" + command + "' cannot be used within a method.");
                        else if (methods.TryGetValue(methodName, out Method method))
                        {
                            method.AddLine(trimmed);
                            exceptionsList.Add(String.Empty);
                            continue;
                        }
                    }

                    if (loopFlag)
                    {
                        if (command.Equals("endloop"))
                        {
                            loopFlag = false;
                            exceptionsList.Add(String.Empty);
                            if (loops.TryGetValue(loopCount, out Loop loop))
                            {
                                for (int i = 0; i < loop.GetNumberOfLoops(); i++)
                                {
                                    parseLines(loop.GetBodyAsArray(), execute, true);
                                }
                            }
                            loopCount++;
                            continue;
                        }

                        if (command.Equals("loop") || command.Equals("method") || command.Equals("var"))
                            throw new GPLException("Command '" + command + "' cannot be used within a loop.");
                        else if(loops.TryGetValue(loopCount, out Loop loop))
                        {
                            loop.AddLine(trimmed);
                            exceptionsList.Add(String.Empty);
                            continue;
                        }
                    }

                    if (MethodExists(command))
                    {
                        parseLines(GetMethod(command).GetBodyAsArray(), execute, true);
                        if (!nestedExec) exceptionsList.Add(String.Empty);
                        continue;
                    }

                    if (VariableExists(command))
                    {
                        if (execute) SetVariableValue(command, new ArraySegment<string>(parts, 2, parts.GetLength(0) - 2).ToArray<String>());
                        if (!nestedExec) exceptionsList.Add(String.Empty);
                        continue;
                    }

                    if (VariableExists(command.Substring(0,command.Length-2)))
                    {
                        String name = command.Substring(0, command.Length - 2);

                        if (command.Substring(command.Length - 2, 2).Equals("++"))
                        {
                            if (execute) { SetVariableValue(name, (name + " + 1").Split(' ')); continue; }
                        }
                        else if (command.Substring(command.Length - 2, 2).Equals("--"))
                        {
                            if (execute) { SetVariableValue(name, (name + " - 1").Split(' ')); continue; }
                        }
                    }

                    if (validator.IsShape(command))
                    {
                        args = this.GetIntArgs(strArguments);
                        if (command.Equals("polygon") || command.Equals("star")) validator.ValidatePolygon(command, args[0]);
                        if (command.Equals("square")) args.Add(args[0]); //Adds size of square again so Rectangle can be re-used.
                        if (execute) canvas.DrawShape(shapeFactory.GetShape(command), args);
                    }
                    else
                    {
                        switch(command) {
                            case "clear":
                                if (execute) canvas.Clear();
                                break;
                            case "reset":
                                if (execute) canvas.Reset();
                                break;
                            case "moveto":
                                args = this.GetIntArgs(strArguments);
                                if (execute) canvas.MoveCursor(args);
                                break;
                            case "drawto":
                                args = this.GetIntArgs(strArguments);
                                if (execute) canvas.DrawLine(args);
                                break;
                            case "pen":
                                try
                                {
                                    color = ColorTranslator.FromHtml(strArguments);
                                    if (execute) canvas.SetColor(color);
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
                                if (execute) canvas.SetFill(strArguments);
                                break;
                            case "var":
                                variables.Add(strArguments, new Variable());
                                break;
                            case "method":
                                methodName = strArguments;
                                methods.Add(methodName, new Method());
                                methodFlag = true;
                                break;
                            case "loop":
                                loops.Add(loopCount, new Loop(new ArraySegment<String>(parts, 1, parts.Length - 1).ToArray()));
                                loopFlag = true;
                                break;
                            case "if":
                                ifFlag = true;
                                if (checkCondition(new ArraySegment<String>(parts, 1, parts.Length - 1).ToArray()))
                                    executeIf = true;
                                break;
                            default:
                                throw new GPLException("Unknown command '" + command + "' found.");
                        }
                    }

                    if(!nestedExec) exceptionsList.Add(String.Empty);

                } catch (GPLException e)
                {
                    exceptionsList.Add(e.Message);
                }

            }

            return exceptionsList.ToArray();
        }

        /// <summary>
        /// Takes in a String and extracts values as Integers, then returns them in a List.
        /// </summary>
        /// <param name="argsIn"></param>
        /// <returns>Integer List containing the converted values.</returns>
        public List<int> GetIntArgs(String argsIn)
        {
            Validator.validArgs.TryGetValue("var", out Regex var);
            String[] strArgs = argsIn.Split(',');
            List<int> args = new List<int>();
            foreach (String s in strArgs)
            {
                int i;
                if (var.IsMatch(s))
                    i = GetVariableValue(s);
                else
                    i = Int32.Parse(s);

                args.Add(i);
            }
            return args;
        }

        /// <summary>
        /// Sets a Variable object's attributes. Evaluates expression if required.
        /// </summary>
        /// <param name="variable">Variable name to be assigned a value.</param> 
        /// <param name="expression">Variable value or expression that results in the value.</param> 
        public void SetVariableValue(String variable, String[] expression)
        {
            Variable v = GetVariable(variable);
            v.SetExpression(expression);
            v.SetValue();
        }

        /// <summary>
        /// Simply returns the value of a variable when given a valid variable name.
        /// </summary>
        /// <param name="variable">Name of the variable to return the value of.</param>
        /// <returns>Value of the named variable.</returns>
        public int GetVariableValue(String variable)
        {
            return GetVariable(variable).GetValue();
        }

        /// <summary>
        /// Returns a variable object when given a valid variable name.
        /// </summary>
        /// <param name="variable">Name of the variable to return.</param>
        /// <returns>Variable object.</returns>
        /// <exception cref="GPLException">Variable does not exist.</exception>
        public Variable GetVariable(String variable)
        {
            if (variables.TryGetValue(variable, out Variable v))
                return v;
            else
                throw new GPLException("Problem getting variable " + variable);
        }

        /// <summary>
        /// Returns a method object when given a valid method name.
        /// </summary>
        /// <param name="method">Name of the method to return.</param>
        /// <returns>Method object.</returns>
        /// <exception cref="GPLException">Method does not exist.</exception>
        public Method GetMethod(String method)
        {
            if (methods.TryGetValue(method, out Method m))
                return m;
            else
                throw new GPLException("Problem getting method " + method);
        }

        /// <summary>
        /// Checks if a variable exists.
        /// </summary>
        /// <param name="variable">Name of variable to check for.</param>
        /// <returns>Boolean result.</returns>
        public bool VariableExists(String variable)
        {
            return variables.ContainsKey(variable);
        }

        /// <summary>
        /// Checks if a method exists.
        /// </summary>
        /// <param name="method">Name of method to check for.</param>
        /// <returns>Boolean result</returns>
        public bool MethodExists(String method)
        {
            return methods.ContainsKey(method);
        }

        public String GetParsedExpression(String[] expression)
        {
            Validator.validArgs.TryGetValue("var", out Regex var);
            String parsed = "";

            //Multi element expression.
            //First replace any variables with their value
            foreach (String s in expression)
            {
                parsed += var.IsMatch(s) ? GetVariableValue(s).ToString() + " " : s + " ";
            }

            return parsed;
        }

        public bool checkCondition(String[] condition)
        {
            Expression e = new Expression(condition);
            return e.EvaluateTruth();
        }
    }
}
