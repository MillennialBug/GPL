using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicalProgrammingLanguage;
using NUnit.Framework;

namespace GPLNUnitTests
{
    internal class ParserTests
    {
        private Parser parser;
        String[] singleWordCommand;
        String[] multiPartProgramline;
        string[] exceptions;

        [SetUp]
        public void SetUp()
        {
            parser = Parser.GetParser();

            singleWordCommand = new String[1];
            multiPartProgramline = new String[10];
            exceptions = new string[10];
        }

        [Test]
        [TestCase("2,3")]
        public void ReturnsValidIntArgs(String s)
        {
            //Act
            List<int> list = parser.GetIntArgs(s);

            //Assert
            Assert.That(list[0], Is.EqualTo(2));
            Assert.That(list[1], Is.EqualTo(3));
        }

        [Test]
        public void ReturnsValidVarArgsAsInts()
        {
            //Arrange
            parser.parseLines("var two\ntwo = 2".Split('\n'), true);

            //Act
            List<int> list = parser.GetIntArgs("two,two");

            //Assert
            Assert.That(list[0], Is.EqualTo(2));
            Assert.That(list[1], Is.EqualTo(2));
        }

        //private void SetVariableValue(String variable, String[] expression)
        //private int GetVariableValue(String variable)
        //private Variable GetVariable(String variable)
        //private Method GetMethod(String method)
        //private bool VariableExists(String variable)
        //private bool MethodExists(String method)
        //public String GetParsedExpression(String[] expression)

    }
}
