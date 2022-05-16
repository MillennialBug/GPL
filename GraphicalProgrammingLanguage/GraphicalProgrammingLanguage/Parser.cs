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
        String strArgument;
        String command;
        Color color;
        Dictionary<String, Variable> variables;
        Dictionary<String, Method> methods;
        Dictionary<Int32, Loop> loops;
        Dictionary<Int32, While> whiles;
        int loopCount;
        bool methodFlag;
        String methodName;
        bool loopFlag;
        String trimmed;
        bool ifFlag;
        bool executeIf;
        bool whileFlag;
        int whileCount;
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
        public String[] ParseLines(String[] lines, Boolean execute, Boolean nestedExec = false, ParamMethod methodExecuting = null)
        {
            if (!nestedExec)
            {
                variables = new Dictionary<string, Variable>();
                methods = new Dictionary<string, Method>();
                loops = new Dictionary<Int32, Loop>();
                whiles = new Dictionary<int, While>();
                loopCount = 0;
                whileCount = 0;
                exceptionsList = new ExceptionsList();
            }
            
            methodFlag = false;
            loopFlag = false;
            whileFlag = false;
            ifFlag = false;
            executeIf = false;

            foreach (String line in lines)
            {
                command = String.Empty;
                strArgument = String.Empty;

                //Trim unnecessary whitespace.
                trimmed = line.Trim(' ');

                //If the string is empty now, it's a blank line and should be ignored.
                if (trimmed.Equals(String.Empty))
                {
                    exceptionsList.Add(String.Empty);
                    continue;
                }

                //Split the remaining string at each space.
                parts = trimmed.Split(' ');

                //If there's a bracket in the first part, user should be calling a paramMethod
                if (parts[0].IndexOf('(') >= 0)
                {
                    //Grab the method name.
                    command = parts[0].Substring(0, parts[0].IndexOf('('));
                    //Grab the arguments.
                    strArgument = ExtractMethodCallParameters(parts);
                }
                else
                {
                    //Otherwise, first part should be the command name as it is.
                    command = parts[0];
                    if (command.Equals("method"))
                        //If we're defining a method, the remaining part of the line should be the name and parameters.
                        strArgument = concatArgument(new ArraySegment<String>(parts, 1, parts.Length - 1).ToArray());
                    else
                        //Second part of a 2 part command should be the arguments. There should be no spaces provided.
                        strArgument = parts.Length == 2 ? parts[1] : String.Empty;
                }
                    

                try
                {                    
                    if (!methods.ContainsKey(command))
                    {
                        if (command.Equals("method"))
                            methodName = validator.ValidateMethod(strArgument, variables.Keys, methods.Keys);
                        else
                            validator.ValidateCommand(parts, variables.Keys, methods.Keys);
                    }
                        
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

                        if (command.Equals("method") || command.Equals("var") || command.Equals("loop") || command.Equals("while"))
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
                            if (loops.TryGetValue(loopCount, out Loop loop) && execute)
                            {
                                loop.EvaluateExecution();
                                for (int i = 0; i < loop.GetNumberOfLoops(); i++)
                                {
                                    ParseLines(loop.GetBodyAsArray(), execute, true);
                                }
                            }
                            loopCount++;
                            continue;
                        }

                        if (command.Equals("loop") || command.Equals("method") || command.Equals("var") || command.Equals("while"))
                            throw new GPLException("Command '" + command + "' cannot be used within a loop.");
                        else if(loops.TryGetValue(loopCount, out Loop loop))
                        {
                            loop.AddLine(trimmed);
                            exceptionsList.Add(String.Empty);
                            continue;
                        }
                    }

                    if (whileFlag)
                    {
                        if (command.Equals("endwhile"))
                        {
                            whileFlag = false;
                            exceptionsList.Add(String.Empty);
                            if (whiles.TryGetValue(whileCount, out While whileloop) && execute)
                            {
                                while (whileloop.EvaluateExecution())
                                {
                                    parser.ParseLines(whileloop.GetBodyAsArray(), execute, true);
                                }
                            }
                            whileCount++;
                            continue;
                        }

                        if (command.Equals("loop") || command.Equals("method") || command.Equals("var") || command.Equals("while"))
                            throw new GPLException("Command '" + command + "' cannot be used within a while loop.");
                        else if (whiles.TryGetValue(whileCount, out While whileloop))
                        {
                            whileloop.AddLine(trimmed);
                            exceptionsList.Add(String.Empty);
                            continue;
                        }
                    }

                    if (MethodExists(command))
                    {
                        Method m = GetMethod(command);
                        if (m.RequiresVariables())
                        {
                            CheckVariablesSuppliedToMethod((ParamMethod) m, strArgument);
                            SetMethodVariables((ParamMethod)m, strArgument);
                            ParseLines(m.GetBodyAsArray(), execute, true, (ParamMethod)m);
                        }
                        else
                            ParseLines(m.GetBodyAsArray(), execute, true, (ParamMethod) m);

                        if (!nestedExec) exceptionsList.Add(String.Empty);
                        continue;
                    }

                    if (VariableExists(command))
                    {
                        if (execute) SetVariableValue(command, new ArraySegment<string>(parts, 2, parts.GetLength(0) - 2).ToArray<String>(), methodExecuting);
                        if (!nestedExec) exceptionsList.Add(String.Empty);
                        continue;
                    }

                    if (VariableExists(command.Substring(0,command.Length-2)) || methodExecuting != null)
                    {
                        String name = command.Substring(0, command.Length - 2);

                        if (command.Substring(command.Length - 2, 2).Equals("++"))
                        {
                            if (execute) { SetVariableValue(name, (name + " + 1").Split(' '), methodExecuting); continue; }
                        }
                        else if (command.Substring(command.Length - 2, 2).Equals("--"))
                        {
                            if (execute) { SetVariableValue(name, (name + " - 1").Split(' '), methodExecuting); continue; }
                        }
                    }

                    if (validator.IsShape(command))
                    {
                        args = this.GetIntArgs(strArgument, methodExecuting);
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
                                args = this.GetIntArgs(strArgument);
                                if (execute) canvas.MoveCursor(args);
                                break;
                            case "drawto":
                                args = this.GetIntArgs(strArgument);
                                if (execute) canvas.DrawLine(args);
                                break;
                            case "pen":
                                try
                                {
                                    color = ColorTranslator.FromHtml(strArgument);
                                    if (execute) canvas.SetColor(color);
                                }
                                catch(Exception e)
                                {
                                    if(e.Message.Equals(strArgument + " is not a valid value for Int32.") || e.Message.Equals("Could not find any recognizable digits."))
                                    {
                                        throw new GPLException(strArgument + " is not a valid color name or hex.");
                                    }
                                    else
                                    {
                                        throw e;
                                    }
                                }
                                break;
                            case "fill":
                                if (execute) canvas.SetFill(strArgument);
                                break;
                            case "var":
                                variables.Add(strArgument, new Variable());
                                break;
                            case "method":
                                CreateMethod(strArgument);
                                methodFlag = true;
                                break;
                            case "loop":
                                loops.Add(loopCount, new Loop(new ArraySegment<String>(parts, 1, parts.Length - 1).ToArray()));
                                loopFlag = true;
                                break;
                            case "while":
                                whiles.Add(whileCount, new While((new ArraySegment<String>(parts, 1, parts.Length - 1).ToArray())));
                                whileFlag = true;
                                break;
                            case "if":
                                ifFlag = true;
                                if (CheckCondition(new ArraySegment<String>(parts, 1, parts.Length - 1).ToArray()))
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
        /// Sets the value of variables held within a method.
        /// </summary>
        /// <param name="m">Method to set variables in.</param>
        /// <param name="strArgument">String containing comma separated variable values/expressions.</param>
        private void SetMethodVariables(ParamMethod m, string strArgument)
        {
            int i = 0;
            String[] variableAssignments = ExtractParameterNamesFromCSVString(strArgument);
            foreach(String s in variableAssignments)
            {
                m.SetMethodVariableValue(m.GetVariableNameFromPosition(i++), s.Split(' '));
            }
        }

        /// <summary>
        /// Seperates the parameters from a method call.
        /// </summary>
        /// <param name="parts">Program line split by ' ' into an array. </param>
        /// <returns>String</returns>
        private string ExtractMethodCallParameters(string[] parts)
        {
            foreach (String s in parts)
            {
                if (s.IndexOf('(') >= 0)
                    strArgument += s.Substring(s.IndexOf('('));
                else
                    strArgument += s + " ";
            }
            return strArgument.Trim();
        }

        /// <summary>
        /// Checks that the number of parameters supplied to a ParamMethod call is correct.
        /// </summary>
        /// <param name="m">The method being called.</param>
        /// <param name="strArgument">The parameters passed.</param>
        /// <exception cref="GPLException">Exception thrown if number does not match the expected number of parameters for the given method.</exception>
        private void CheckVariablesSuppliedToMethod(ParamMethod m, string strArgument)
        {
            if (!(m.GetVariableCount() == ExtractParameterNamesFromCSVString(strArgument).Length))
                throw new GPLException("Incorrect number of parameters passed to Method");
        }

        /// <summary>
        /// Rebuilds a String array into a single string.
        /// </summary>
        /// <param name="parts">String array to be concatenated</param>
        /// <returns>String</returns>
        private string concatArgument(string[] parts)
        {
            //Compile the remaining arguments into a single string
            foreach (String s in parts)
            {
                strArgument += s + " ";
            }
            return strArgument.Trim();
        }

        /// <summary>
        /// Takes in a String and extracts values as Integers, then returns them in a List.
        /// </summary>
        /// <param name="argsIn"></param>
        /// <returns>Integer List containing the converted values.</returns>
        public List<int> GetIntArgs(String argsIn, ParamMethod method = null)
        {
            Validator.validArgs.TryGetValue("var", out Regex var);
            String[] strArgs = argsIn.Split(',');
            List<int> args = new List<int>();
            foreach (String s in strArgs)
            {
                int i;
                if (var.IsMatch(s))
                    i = GetVariableValue(s, method);
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
        public void SetVariableValue(String variable, String[] expression, ParamMethod method = null)
        {
            if (method != null)
                method.SetMethodVariableValue(variable, expression);
            else
            {
                Variable v = GetVariable(variable);
                v.SetExpression(expression);
                v.SetValue();
            }
        }

        /// <summary>
        /// Simply returns the value of a variable when given a valid variable name.
        /// </summary>
        /// <param name="variable">Name of the variable to return the value of.</param>
        /// <returns>Value of the named variable.</returns>
        public int GetVariableValue(String variable, ParamMethod method = null)
        {
            if (method != null)
                return method.GetVariableValue(variable);
            else
                return GetVariable(variable).GetValue();
        }

        /// <summary>
        /// Returns a variable object when given a valid variable name.
        /// </summary>
        /// <param name="variable">Name of the variable to return.</param>
        /// <returns>Variable object.</returns>
        /// <exception cref="GPLException">Variable does not exist.</exception>
        public Variable GetVariable(String variable, ParamMethod method = null)
        {
            if (method != null)
                return method.GetVariable(variable);
            else if (variables.TryGetValue(variable, out Variable v))
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

        /// <summary>
        /// Takes in a string array containing parts of an expression and replaces any variable names with their value.
        /// </summary>
        /// <param name="expression">String array containing expression, e.g. {"one", "+", "one"}</param>
        /// <returns>String with variable names replaced by values, e.g. "1 + 1"</returns>
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

        /// <summary>
        /// Checks the condition given to an IF statment.
        /// </summary>
        /// <param name="condition">The condition to check, e.g. "1 > 2"</param>
        /// <returns>Boolean result</returns>
        public bool CheckCondition(String[] condition)
        {
            Expression e = new Expression(condition);
            return e.EvaluateTruth();
        }

        /// <summary>
        /// Creates a Method or ParamMethod object depending on needs.
        /// </summary>
        /// <param name="arg">Argument passed to the 'method' command.</param>
        public void CreateMethod(String arg)
        {
            //If no parameters provided, create Method and return.
            if (arg.IndexOf('(') < 0)
            {
                methods.Add(methodName, new Method());
                return;
            }

            //Otherwise create ParamMethod and pass variables for creation.
            ParamMethod m = new ParamMethod();
            m.CreateVariables(ExtractParameterNamesFromCSVString(arg));
            methods.Add(methodName, m);

        }

        /// <summary>
        /// Removes brackets from an argument and splits the values inside at each comma.
        /// </summary>
        /// <param name="arg">String containing arguments and brackets e.g. "(one, two, three)"</param>
        /// <returns>String array of seperated arguments e.g. {"one", " two", " three"}. Note, the strings 
        /// have preceeding whitespace so will need to be trimmed</returns>
        public String[] ExtractParameterNamesFromCSVString(String arg)
        {
            if (arg.Equals(String.Empty))
                return new string[0];

            return arg.Substring(arg.IndexOf('(') + 1, arg.IndexOf(')') - (arg.IndexOf('(') + 1)).Split(',');
        }
    }
}
