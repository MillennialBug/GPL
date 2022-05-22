using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    /// <summary>
    /// Extends Method to allow for internal parameter variables.
    /// </summary>
    public class ParamMethod : Method
    {
        Dictionary<String, Variable> methodVariables;
        Dictionary<int, String> methodVariablePositions;

        public ParamMethod() : base() 
        {
            methodVariables = new Dictionary<string, Variable>();
            methodVariablePositions = new Dictionary<int, string>();
            this.requiresVariables = true;
        }

        /// <summary>
        /// Adds Variable objects to the internal Dictionary of Variables. 
        /// Uses a second dictionary to record the position of the given Variable for assingment when the ParamMethod is called.
        /// </summary>
        /// <param name="vars">String[] containing parameter names from ParamMethod declaration.</param>
        public void CreateVariables(String[] vars)
        {
            int i = 0;

            foreach(String s in vars)
            {
                methodVariables.Add(s.Trim(), new Variable());
                methodVariablePositions.Add(i++, s.Trim());
            }
        }

        /// <summary>
        /// Checks that the variable name passed in exists as a Variable within the ParamMethod.
        /// </summary>
        /// <param name="name">Name of the Variable to check for.</param>
        /// <returns>Boolean result.</returns>
        public bool HasVariable(String name)
        {
            return methodVariables.TryGetValue(name, out Variable v);
        }

        /// <summary>
        /// Returns the value of the requested Variable.
        /// </summary>
        /// <param name="name">Name of the variable to retrieve the value for.</param>
        /// <returns>Integer value of variable.</returns>
        /// <exception cref="GPLException">Throws an exception if the variable does not exist.</exception>
        public int GetVariableValue(String name)
        {
            if (HasVariable(name))
            {
                methodVariables.TryGetValue(name, out Variable v);
                return v.GetValue();
            }
            else
                throw new GPLException("Method Variable '" + name + "' does not exists'.");
        }

        /// <summary>
        /// Gets the number of variables held in the ParamMethod. Used to check correct number of parameters are passed when called.
        /// </summary>
        /// <returns>Int count of Variables.</returns>
        public int GetVariableCount()
        {
            return methodVariables.Count;
        }

        /// <summary>
        /// Returns the variable name for a given parameter position.
        /// </summary>
        /// <param name="pos">Integer Position</param>
        /// <returns>String name of Variable.</returns>
        public String GetVariableNameFromPosition(int pos)
        {
            methodVariablePositions.TryGetValue(pos, out string s);
            return s;
        }

        /// <summary>
        /// Sets the expression for a named variable.
        /// </summary>
        /// <param name="name">Name of Variable to set.</param>
        /// <param name="expression">String[] that makes up an expression. E.G. {"1","+","2"}</param>
        public void SetMethodVariableValue(String name, String[] expression)
        {
            methodVariables.TryGetValue(name, out Variable v);
            v.SetExpression(expression);
            v.SetValue();
        }

        /// <summary>
        /// Returns the Variable object of a given variable name.
        /// </summary>
        /// <param name="name">Name of the variable to get.</param>
        /// <returns>Variable object</returns>
        /// <exception cref="GPLException">Throws an exception when Variable is not found.</exception>
        public Variable GetVariable(String name)
        {
            if (methodVariables.TryGetValue(name, out Variable v))
                return v;
            else
                throw new GPLException("Problem getting method variable " + name + ".");
        }


    }
}
