using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    internal class ParamMethod : Method
    {
        Dictionary<String, Variable> methodVariables;

        public ParamMethod() : base() 
        {
            methodVariables = new Dictionary<string, Variable>();
        }

        public void CreateVariables(String[] vars)
        {
            foreach(String s in vars)
            {
                methodVariables.Add(s.Trim(), new Variable());
            }
        }
    }
}
