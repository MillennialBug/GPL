using System;
using System.Collections.Generic;

namespace GraphicalProgrammingLanguage
{
    /// <summary>
    /// Object to hold details of a user defined method.
    /// </summary>
    public class Method
    {
        private List<String> body;
        protected bool requiresVariables = false;

        public Method()
        {
            body = new List<string>();
        }

        /// <summary>
        /// Adds a line of code to the body (String List) of the method.
        /// </summary>
        /// <param name="line">String. Line of code.</param>
        public void AddLine(String line)
        {
            body.Add(line);
        }

        /// <summary>
        /// Gets the body list as an array so it can be parsed by the Parser.
        /// </summary>
        /// <returns>String[] of member varaible 'body'.</returns>
        public String[] GetBodyAsArray()
        {
            return body.ToArray();
        }

        /// <summary>
        /// Boolean which differentiates a regular Method from a ParamMethod. Is set in the constructor of either object.
        /// </summary>
        /// <returns>bool</returns>
        public bool RequiresVariables()
        {
            return this.requiresVariables;
        }
    }
}
