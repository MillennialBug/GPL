using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    internal class Method
    {
        private List<String> body;

        public Method()
        {
            body = new List<string>();
        }

        public void AddLine(String line)
        {
            body.Add(line);
        }

        public String[] GetBodyAsArray()
        {
            return body.ToArray();
        }
    }
}
