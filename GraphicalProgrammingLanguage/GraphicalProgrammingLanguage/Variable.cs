using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    internal class Variable
    {
        private int value;
        private String[] expression;

        public int getValue()
        {
            return this.value;
        }

        public void setValue(int value)
        {
            this.value = value;
        }

        public String[] getExpression()
        {
            return this.expression;
        }

        public void setExpression(String[] expression)
        {
            this.expression = expression;
        }

    }
}
