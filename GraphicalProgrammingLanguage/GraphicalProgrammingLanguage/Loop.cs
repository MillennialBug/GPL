using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    public class Loop : Command
    {
        protected List<String> body;
        protected Expression expression;
        int numberOfLoops;

        public void set(String[] expression)
        {
            this.expression = new Expression(expression);
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

        public void EvaluateExecution()
        {
            numberOfLoops = this.expression.EvaluateValue();
        }

        public override void Execute()
        {
            Parser parser = Parser.GetParser();

            this.EvaluateExecution();
            for (int i = 0; i < this.numberOfLoops; i++) //Loops for the given number of loops.
            {
                parser.ParseLines(this.GetBodyAsArray(), true, true);
            }
        }
    }
}
