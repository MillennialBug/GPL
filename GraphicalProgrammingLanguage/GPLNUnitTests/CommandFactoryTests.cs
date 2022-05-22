using NUnit.Framework;
using GraphicalProgrammingLanguage;
using System;
using System.Collections.Generic;

namespace GPLNUnitTests
{
    /// <summary>
    /// Test Class containing tests that related to functionality within the CommandFactory class.
    /// </summary>
    public class CommandFactoryTests
    {
        CommandFactory commandFactory = CommandFactory.GetShapeFactory();

        /// <summary>
        /// Checks that when given a valid Command name as a parameter, the CommandFactroy.GetCommand method returns the correct Command object.
        /// </summary>
        /// <param name="name">Valid Command name.</param>
        /// <param name="type">Expected type of the returned Command object.</param>
        [Test, TestCaseSource("GetCommandCreationTestCases")]
        public void CommandFactory_CreatesCorrectCommand(string name, Type type)
        {
            //Act
            Command command = commandFactory.GetCommand(name);

            //Assert
            Assert.That(command, Is.TypeOf(type));
        }

        /// <summary>
        /// Checks that the CommandFactroy.GetCommand method throws an exception when passed an invalid Command name.
        /// </summary>
        [Test]
        public void CommandFactory_ThrowsException_IncorrectCommandName()
        {
            //Arrange
            String name = "invalid";
            //Act
            try
            {
                Command command = commandFactory.GetCommand(name);
            }
            catch (GPLException e)
            {
                Assert.That(e.Message, Is.EqualTo("Command '" + name + "' not found."));
                return;
            }

            Assert.Fail();
        }

        private static IEnumerable<TestCaseData> GetCommandCreationTestCases()
        {
            yield return new TestCaseData("circle", typeof(Circle));
            yield return new TestCaseData("rectangle", typeof(Rectangle));
            yield return new TestCaseData("square", typeof(Rectangle));
            yield return new TestCaseData("triangle", typeof(Triangle));
            yield return new TestCaseData("polygon", typeof(Polygon));
            yield return new TestCaseData("star", typeof(Star));
            yield return new TestCaseData("loop", typeof(Loop));
            yield return new TestCaseData("while", typeof(While));
        }
    }
}