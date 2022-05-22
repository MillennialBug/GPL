using System;
using System.Collections.Generic;

namespace GraphicalProgrammingLanguage
{
    /// <summary>
    /// Class used to hold and evaluate the value of an expression for either value or truth.
    /// </summary>
    public class Expression
    {
        String[] exp;

        public Expression(String[] exp)
        {
            this.exp = exp;
        }

        /// <summary>
        /// Evaluates an expression and returns an int value.
        /// </summary>
        /// <returns>Integer result of the expression.</returns>
        public int EvaluateValue()
        {
            Parser parser = Parser.GetParser();
            System.Data.DataTable dt = new System.Data.DataTable(); 

            // If length of expression is 1 then it's either an int value or another variable.
            // If it's an int set variable value as such.
            if (exp.GetLength(0) == 1)
            {
                if (Int32.TryParse(exp[0], out int i))
                    return i;
            }
            
            return Int32.Parse(dt.Compute(parser.GetParsedExpression(exp), "").ToString());
        }

        /// <summary>
        /// Evaluates an expression and returns a bool value.
        /// </summary>
        /// <returns>Boolean result of the expression.</returns>
        public bool EvaluateTruth()
        {
            Parser parser = Parser.GetParser();

            String[] parsed = parser.GetParsedExpression(exp).TrimEnd().Split(' ');

            int a = int.Parse(parsed[0]);
            int b = int.Parse(parsed[2]);

            switch(parsed[1])
            {
                case "==":
                    return a == b;
                case "<":
                    return a < b;
                case ">":
                    return a > b;
                case "<=":
                    return a <= b;
                case ">=":
                    return a >= b;
                default:
                    throw new GPLException("Cannot evaluate expression truth.");
            }
        }
    }
}
