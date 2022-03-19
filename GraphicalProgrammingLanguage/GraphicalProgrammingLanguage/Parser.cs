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
                    List<int> args;
                    validator.validateCommand(parts);

                    if (validator.isShape(parts[0]))
                    {
                        args = this.GetIntArgs(parts[1]);
                        c.DrawShape(shapeFactory.getShape(parts[0]), args);
                    }
                    else
                    {
                        switch(parts[0]) {
                            case "clear":
                                c.Clear();
                                break;
                            case "reset":
                                c.Reset();
                                break;
                            case "moveto":
                                args = this.GetIntArgs(parts[1]);
                                c.MoveCursor(args);
                                break;
                            case "drawto":
                                args = this.GetIntArgs(parts[1]);
                                c.DrawLine(args);
                                break;
                            case "pen":
                                try
                                {
                                    c.SetColor(ColorTranslator.FromHtml(parts[1]));
                                }
                                catch(Exception e)
                                {
                                    if(e.Message.Equals("htmlColor is not a valid HTML color name."))
                                    {
                                        throw new GPLException(e.Message);
                                    }
                                    else
                                    {
                                        throw e;
                                    }
                                }
                                break;
                            case "fill":
                                c.SetFill(parts[1]);
                                break;
                            default:
                                throw new GPLException("Unknown command '" + parts[0] + "' found.");
                        }
                    }

                } catch (GPLException e)
                {
                    Console.WriteLine(e.Message);
                }

            }
        }

        public List<int> GetIntArgs(String argsIn)
        {
            String[] strArgs = argsIn.Split(',');
            List<int> args = new List<int>();
            foreach (String s in strArgs)
            {
                // For variables this will need changing.
                int tmp;
                int.TryParse(s, out tmp);
                args.Add(tmp);
            }
            return args;
        }

        
    }
}
