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

        /// <summary>
        /// Tests that GetIntArgs returns Integer values when given simple input.
        /// </summary>
        /// <param name="s">Two integers in a string, seperated by a comma.</param>
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

        /// <summary>
        /// Tests that GetIntArgs returns Integer values from variable names.
        /// </summary>
        [Test]
        public void ReturnsValidVarArgsAsInts()
        {
            //Arrange
            parser.ParseLines("var two\ntwo = 2".Split('\n'), true);

            //Act
            List<int> list = parser.GetIntArgs("two,two");

            //Assert
            Assert.That(list[0], Is.EqualTo(2));
            Assert.That(list[1], Is.EqualTo(2));
        }

        /// <summary>
        /// Tests the various methods in Parser that realte to Variables.
        /// </summary>
        /// <param name="s">A program in a single string. Commands are seperated by newline characters.</param>
        [TestCase("var one\nvar two\none = 1\ntwo = one + one")]
        public void ParserVariableMethods(String s) 
        {
            //Arrange //Act
            parser.ParseLines(s.Split('\n'), true);
            Variable two = parser.GetVariable("two");

            //Assert
            Assert.That(parser.GetVariableValue("one"), Is.EqualTo(1));
            Assert.That(parser.VariableExists("two"), Is.True);
            Assert.That(two.GetValue(), Is.EqualTo(2));
        }

        /// <summary>
        /// Tests the various methods in Parser that realte to Methods.
        /// </summary>
        [Test]
        public void ParserMethodMethods()
        {
            //Arrange //Act
            parser.ParseLines("method mymethod\ncircle 50\ncircle 75\nendmethod".Split('\n'), true);

            //Assert
            Assert.That(parser.MethodExists("mymethod"), Is.True);
            Method m = parser.GetMethod("mymethod");
            Assert.That(m.GetBodyAsArray().Length, Is.EqualTo(2));
            Assert.That(m.GetBodyAsArray()[0], Is.EqualTo("circle 50"));
            Assert.That(m.GetBodyAsArray()[1], Is.EqualTo("circle 75"));
        }

        /// <summary>
        /// Tests that GetParsedExpression returns a translated experssion. I.E. Variable names are swapped for their respective values.
        /// </summary>
        [Test]
        public void GetParsedExpression()
        {
            //Arrage
            parser.ParseLines("var one\nvar two\none = 1".Split('\n'), true);

            //Act
            String s = parser.GetParsedExpression("one + one".Split(' '));

            //Assert
            Assert.That(s, Is.EqualTo("1 + 1 "));
        }

        /// <summary>
        /// Tests that an If's body executes when the condition is True.
        /// </summary>
        [Test]
        public void IfExecutesWhenConditionTrue()
        {
            //Arrange
            parser.ParseLines("var a\nvar b\na = 1\nif a == 1\nb = 2\nendif".Split('\n'), true);

            //Act
            Variable b = parser.GetVariable("b");

            //Assert
            Assert.That(b.GetValue(), Is.EqualTo(2));
        }

        /// <summary>
        /// Tests that an If's body does not execute when the condition is False.
        /// </summary>
        [Test]
        public void IfDoesNotExecuteWhenConditionFalse()
        {
            //Arrange
            parser.ParseLines("var a\nvar b\na = 1\nif a == 0\nb = 2\nendif".Split('\n'), true);

            //Act
            Variable b = parser.GetVariable("b");

            //Assert
            Assert.That(b.GetValue(), Is.EqualTo(0));
        }

        [Test]
        public void IncrementAddsOneToVariable()
        {
            //Arrange
            parser.ParseLines("var a\na = 1\na++".Split('\n'), true);

            //Act
            Variable a = parser.GetVariable("a");

            //Assert
            Assert.That(a.GetValue, Is.EqualTo(2));
        }

        [Test]
        public void DecrementTakesOneFromVariable()
        {
            //Arrange
            parser.ParseLines("var a\na = 2\na--".Split('\n'), true);

            //Act
            Variable a = parser.GetVariable("a");

            //Assert
            Assert.That(a.GetValue, Is.EqualTo(1));
        }

        [Test]
        public void ParsesParamMethod()
        {
            //Arrange
            String[] program = "method mymethod(one, two, three)\ncircle 50\ncircle 75\nendmethod".Split('\n');

            //Act
            parser.ParseLines(program, true);

            //Assert
            Assert.That(parser.MethodExists("mymethod"), Is.True);
            ParamMethod m = (ParamMethod) parser.GetMethod("mymethod");
            Assert.That(m.GetBodyAsArray().Length, Is.EqualTo(2));
            Assert.That(m.GetBodyAsArray()[0], Is.EqualTo("circle 50"));
            Assert.That(m.GetBodyAsArray()[1], Is.EqualTo("circle 75"));
            Assert.That(m.HasVariable("one"), Is.True);
            Assert.That(m.HasVariable("two"), Is.True);
            Assert.That(m.HasVariable("three"), Is.True);
        }

        [Test]
        public void ExecutesParamMethodNoParamsUsed()
        {
            //Arrange
            String[] program = "method mymethod(one, two, three)\ncircle 50\ncircle 75\nendmethod\nmymethod(1, 2, 3)".Split('\n');

            //Act
            String[] exceptions = parser.ParseLines(program, false);

            //Assert
            foreach(String s in exceptions)
            {
                Assert.That(s, Is.EqualTo(String.Empty));
            }
        }
    }
}
