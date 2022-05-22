using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    /// <summary>
    /// Extends functionality of a Loop to add conditional execution.
    /// </summary>
    public class While : Loop
    {
        public While() : base()
        {
        }

        /// <summary>
        /// Overrides base method so that the expression of this object is evaluated for truth rather than int value.
        /// </summary>
        /// <returns></returns>
        new public bool EvaluateExecution()
        {
            return this.expression.EvaluateTruth();
        }

        /// <summary>
        /// Controls execution of the loop. Uses a C# while loop.
        /// </summary>
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
