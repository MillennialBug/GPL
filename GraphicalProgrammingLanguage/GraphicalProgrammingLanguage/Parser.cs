using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GraphicalProgrammingLanguage
{
    internal class Parser
    {
        private Validator validator;
        ShapeFactory shapeFactory;
        Canvas c;

        public Parser(Canvas c)
        {
            this.c = c;
            validator = new Validator();
            shapeFactory = new ShapeFactory();
        }

        public void parseLines(String[] lines)
        {
            foreach(String line in lines)
            {
                String[] parts = line.Trim(' ').Split(' ');
                try
                {
                    validator.validateCommand(parts);

                    if (validator.isShape(parts[0]))
                    {
                        String[] strArgs = parts[1].Split(',');
                        List<int> args = new List<int>();
                        foreach(String s in strArgs)
                        {
                            // For variables this will need changing.
                            int tmp;
                            int.TryParse(s, out tmp );
                            args.Add(tmp);
                        }
                        c.DrawShape(shapeFactory.getShape(parts[0]), args);
                    }

                } catch (GPLException e)
                {
                    Console.WriteLine(e.Message);
                }

            }
        }

        
    }
}
