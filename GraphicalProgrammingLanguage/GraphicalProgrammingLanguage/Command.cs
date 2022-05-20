using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalProgrammingLanguage
{
    public abstract class Command : CommandInterface
    {
        abstract public void Execute();
    }
}
