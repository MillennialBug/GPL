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
                String command = parts[0];
                String strArguments = "";
                if (parts.Length > 1) strArguments = parts[1];
                try
                {
                    List<int> args;
                    validator.ValidateCommand(parts);

                    if (validator.IsShape(command))
                    {
                        args = this.GetIntArgs(strArguments);
                        if (command.Equals("polygon") || command.Equals("star")) validator.ValidatePolygon(command, args);
                        if (command.Equals("square")) args.Add(args[0]); //Adds size of square again so Rectangle can be re-used.
                        c.DrawShape(shapeFactory.getShape(command), args);
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
                                args = this.GetIntArgs(strArguments);
                                c.MoveCursor(args);
                                break;
                            case "drawto":
                                args = this.GetIntArgs(strArguments);
                                c.DrawLine(args);
                                break;
                            case "pen":
                                try
                                {
                                    c.SetColor(ColorTranslator.FromHtml(strArguments));
                                }
                                catch(Exception e)
                                {
                                    if(e.Message.Equals(strArguments + " is not a valid value for Int32.") || e.Message.Equals("Could not find any recognizable digits."))
                                    {
                                        throw new GPLException(strArguments + " is not a valid color name or hex.");
                                    }
                                    else
                                    {
                                        throw e;
                                    }
                                }
                                break;
                            case "fill":
                                c.SetFill(strArguments);
                                break;
                            default:
                                throw new GPLException("Unknown command '" + command + "' found.");
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
                int i;
                int.TryParse(s, out i);
                args.Add(i);
            }
            return args;
        }

        
    }
}
