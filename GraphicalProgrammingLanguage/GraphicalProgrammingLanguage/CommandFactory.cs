using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    /// <summary>
    /// Factory Design Pattern.
    /// Singleton Design Pattern.
    /// This Class is used as a single point of creation for Shapes and Loops.
    /// </summary>
    public class CommandFactory
    {
        private static CommandFactory commandFactory = new CommandFactory();

        private CommandFactory() { }

        /// <summary>
        /// Returns the CommandFactory instance.
        /// </summary>
        /// <returns>CommandFactory</returns>
        public static CommandFactory GetShapeFactory() { return commandFactory; }

        /// <summary>
        /// Takes in a String containing the name of a Shape or Command and returns the requested Object, if it exists.
        /// </summary>
        /// <param name="name">Name of Shape or Command.</param>
        /// <returns>Command object.</returns>
        /// <exception cref="GPLException">Thrown when an invalid Shape or Command is requested.</exception>
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
