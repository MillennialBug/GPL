using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    internal class ShapeFactory
    {
        public Shape getShape(String name)
        {
            if (name.Equals("circle"))
            {
                return new Circle();
            }
            else if (name.Equals("rectangle") || name.Equals("square"))
            {
                return new Rectangle();
            }
            else if (name.Equals("triangle"))
            {
                return new Triangle();
            }
            else if (name.Equals("star"))
            {
                return new Star();
            }
            else if (name.Equals("polygon"))
            {
                return new Polygon();
            }
            else 
            {
                throw new GPLException("Shape '" + name + "' not found.");
            }
        }
        
    }
}
