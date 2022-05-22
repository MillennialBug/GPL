using System;
using System.Collections.Generic;

namespace GraphicalProgrammingLanguage
{
    /// <summary>
    /// Object to hold details of a user defined loop.
    /// </summary>
    public class Loop : Command
    {
        protected List<String> body;
        protected Expression expression;
        int numberOfLoops;

        /// <summary>
        /// Sets the expression of the loop. Used to determine how many times the loop should execute.
        /// </summary>
        /// <param name="expression">String[] that makes up an expression. E.G. {"1","+","2"}</param>
        public void set(String[] expression)
        {
            this.expression = new Expression(expression);
            body = new List<String>();
        }

        /// <summary>
        /// Adds a line to the body of the loop. The body is held as an internal String List.
        /// </summary>
        /// <param name="line"></param>
        public void AddLine(String line)
        {
            body.Add(line);
        }

        /// <summary>
        /// Returns internal variable body as an array.
        /// </summary>
        /// <returns>String array</returns>
        public String[] GetBodyAsArray()
        {
            return body.ToArray();
        }

        /// <summary>
        /// Returns the number of times the loop should execute.
        /// </summary>
        /// <returns>Integer</returns>
        public int GetNumberOfLoops()
        {
            return this.numberOfLoops;
        }

        /// <summary>
        /// Evaluates the Loops expression to calculate the number of loops required.
        /// </summary>
        public void EvaluateExecution()
        {
            numberOfLoops = this.expression.EvaluateValue();
        }

        /// <summary>
        /// Controls execution of the loop using a C# for loop.
        /// </summary>
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
