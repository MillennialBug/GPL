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
            else 
            {
                Console.WriteLine("Dropped into else.");
                return new Circle();
            }
        }
        
    }
}
