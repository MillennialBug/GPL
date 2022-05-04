using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    internal class ExceptionsList : List<String>
    {
        public void AddAll(String[] strings)
        {
            foreach(string s in strings)
            {
                this.Add(s);
            }
        }
    }
}
