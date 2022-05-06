using System;
using System.Collections.Generic;

namespace GraphicalProgrammingLanguage
{
    public class Method
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
