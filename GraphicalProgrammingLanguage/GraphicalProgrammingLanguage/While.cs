using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    public class While : Loop
    {
        public While() : base()
        {
        }

        new public bool EvaluateExecution()
        {
            return this.expression.EvaluateTruth();
        }

        public override void Execute()
        {
            Parser parser = Parser.GetParser();

            while (this.EvaluateExecution()) //Will evaluate the while condition before execution.
            {
                parser.ParseLines(this.GetBodyAsArray(), true, true);
            }
        }
    }
}
