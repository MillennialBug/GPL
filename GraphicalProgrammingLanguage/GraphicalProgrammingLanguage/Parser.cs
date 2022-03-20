﻿using System;
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
                    validator.ValidateCommand(parts);

                    if (validator.IsShape(parts[0]))
                    {
                        args = this.GetIntArgs(parts[1]);
                        if (parts[0].Equals("polygon") || parts[0].Equals("star")) validator.ValidatePolygon(parts[0], args);
                        if(parts[0].Equals("polygon")) args.Add(0); //Add upsideDown parameter which is not currently user defined.
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
                                    if(e.Message.Equals(parts[1] + " is not a valid value for Int32.") || e.Message.Equals("Could not find any recognizable digits."))
                                    {
                                        throw new GPLException(parts[1] + " is not a valid color name or hex.");
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
