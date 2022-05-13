using System;
using System.Collections.Generic;

namespace GraphicalProgrammingLanguage
{
    public class Method
    {
        private List<String> body;
        protected bool requiresVariables = false;

        public Method()
        {
            body = new List<string>();
        }

        public void AddLine(String line)
        {
            body.Add(line);
        }

        public String[] GetBodyAsArray()
        {
            return body.ToArray();
        }

        public bool RequiresVariables()
        {
            return this.requiresVariables;
        }
    }
}
