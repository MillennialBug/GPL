using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    /// <summary>
    /// Created this class as a way to allow String[]s to be added to a list wholesale, but didn't end up using it for that.
    /// </summary>
    public class ExceptionsList : List<String>
    {
        /// <summary>
        /// Adds the entire contents of a String array to a this object.
        /// </summary>
        /// <param name="strings">String[] with contents to be added.</param>
        public void AddAll(String[] strings)
        {
            foreach(string s in strings)
            {
                this.Add(s);
            }
        }
    }
}
