using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    public class ParamMethod : Method
    {
        Dictionary<String, Variable> methodVariables;

        public ParamMethod() : base() 
        {
            methodVariables = new Dictionary<string, Variable>();
            this.requiresVariables = true;
        }

        public void CreateVariables(String[] vars)
        {
            foreach(String s in vars)
            {
                methodVariables.Add(s.Trim(), new Variable());
            }
        }

        public bool HasVariable(String name)
        {
            return methodVariables.TryGetValue(name, out Variable v);
        }

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
    }
}
