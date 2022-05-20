using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    public class CommandFactory
    {
        private static CommandFactory commandFactory = new CommandFactory();

        private CommandFactory() { }

        public static CommandFactory GetShapeFactory() { return commandFactory; }
        public Command GetCommand(String name)
        {
            switch (name)
            {
                case "circle":
                    return new Circle();
                case "square":
                case "rectangle":
                    return new Rectangle();
                case "triangle":
                    return new Triangle();
                case "star":
                    return new Star();
                case "polygon":
                    return new Polygon();
                /*case "method":
                    return new Method();
                case "parammethod":
                    return new ParamMethod();
                case "variable":
                    return new Variable();*/
                case "loop":
                    return new Loop();
                case "while":
                    return new While();
                default:
                    throw new GPLException("Command '" + name + "' not found.");
            }
        }
    }
}
