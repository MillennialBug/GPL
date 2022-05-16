using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    public class While : Loop
    {
        public While(string[] expression) : base(expression)
        {
        }

        new public bool EvaluateExecution()
        {
            return this.expression.EvaluateTruth();
        }
    }
}
