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
            else if (name.Equals("rectangle"))
            {
                return new Rectangle();
            }
            else 
            {
                throw new GPLException("Shape '" + name + "' not found.");
            }
        }
        
    }
}
