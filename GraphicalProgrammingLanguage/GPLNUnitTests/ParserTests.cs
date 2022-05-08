using System;
using System.Collections.Generic;
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

        [TestCase("var one\nvar two\none = 1\ntwo = one + one")]
        public void ParserVariableMethods(String s) 
        {
            //Arrange //Act
            parser.parseLines(s.Split('\n'), true);
            Variable two = parser.GetVariable("two");

            //Assert
            Assert.That(parser.GetVariableValue("one"), Is.EqualTo(1));
            Assert.That(parser.VariableExists("two"), Is.True);
            Assert.That(two.GetValue(), Is.EqualTo(2));
        }

        [Test]
        public void ParserMethodMethods()
        {
            //Arrange //Act
            parser.parseLines("method mymethod\ncircle 50\ncircle 75\nendmethod".Split('\n'), true);

            //Assert
            Assert.That(parser.MethodExists("mymethod"), Is.True);
            Method m = parser.GetMethod("mymethod");
            Assert.That(m.GetBodyAsArray().Length, Is.EqualTo(2));
            Assert.That(m.GetBodyAsArray()[0], Is.EqualTo("circle 50"));
            Assert.That(m.GetBodyAsArray()[1], Is.EqualTo("circle 75"));
        }

        //public String GetParsedExpression(String[] expression)
        [Test]
        public void GetParsedExpression()
        {
            //Arrage
            parser.parseLines("var one\nvar two\none = 1".Split('\n'), true);

            //Act
            String s = parser.GetParsedExpression("one + one".Split(' '));

            //Assert
            Assert.That(s, Is.EqualTo("1+1"));
        }
    }
}
