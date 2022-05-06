using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    public class Variable
    {
        private int value;
        private Expression expression;

        public int GetValue()
        {
            return this.value;
        }

        public void SetValue(Dictionary<String, Variable> d)
        {
            this.value = this.expression.Evaluate(d);
        }

        public void SetExpression(String[] expression)
        {
            this.expression = new Expression(expression);
        }

    }
}
