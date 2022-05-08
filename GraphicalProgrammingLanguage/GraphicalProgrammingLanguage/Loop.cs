using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    public class Loop
    {
        List<String> body;
        Expression expression;
        int numberOfLoops;

        public Loop(String[] expression)
        {
            this.expression = new Expression(expression);
            numberOfLoops = this.expression.EvaluateValue();
            body = new List<String>();
        }

        public void AddLine(String line)
        {
            body.Add(line);
        }

        public String[] GetBodyAsArray()
        {
            return body.ToArray();
        }

        public int GetNumberOfLoops()
        {
            return this.numberOfLoops;
        }
    }
}
