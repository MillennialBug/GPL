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
        Dictionary<int, String> methodVariablePositions;

        public ParamMethod() : base() 
        {
            methodVariables = new Dictionary<string, Variable>();
            methodVariablePositions = new Dictionary<int, string>();
            this.requiresVariables = true;
        }

        public void CreateVariables(String[] vars)
        {
            int i = 0;

            foreach(String s in vars)
            {
                methodVariables.Add(s.Trim(), new Variable());
                methodVariablePositions.Add(i++, s.Trim());
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

        public int GetVariableCount()
        {
            return methodVariables.Count;
        }

        public String GetVariableNameFromPosition(int pos)
        {
            methodVariablePositions.TryGetValue(pos, out string s);
            return s;
        }

        public void SetMethodVariableValue(String name, String[] expression)
        {
            methodVariables.TryGetValue(name, out Variable v);
            v.SetExpression(expression);
            v.SetValue();
        }

        public Variable GetVariable(String name)
        {
            if (methodVariables.TryGetValue(name, out Variable v))
                return v;
            else
                throw new GPLException("Problem getting method variable " + name + ".");
        }


    }
}
