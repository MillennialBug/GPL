using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    /// <summary>
    /// Class to help control and maintain variable values in a GPL program.
    /// </summary>
    public class Variable
    {
        private int value;
        private Expression expression;

        /// <summary>
        /// Returns the current value of the variable.
        /// </summary>
        /// <returns>Integer value of the variable.</returns>
        public int GetValue()
        {
            return this.value;
        }

        /// <summary>
        /// Sets the value of this variable by evaluating its expression. Calls Expression.EvaluateValue.
        /// </summary>
        public void SetValue()
        {
            this.value = this.expression.EvaluateValue();
        }

        /// <summary>
        /// Sets the expression of this object.
        /// </summary>
        /// <param name="expression">String[] that makes up an expression. E.G. {"1","+","2"}</param>
        public void SetExpression(String[] expression)
        {
            this.expression = new Expression(expression);
        }

    }
}
