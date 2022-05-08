using System;
using System.Collections.Generic;
using GraphicalProgrammingLanguage;
using NUnit.Framework;

namespace GPLNUnitTests
{
    internal class ValidatorTests
    {
        Validator validator;
        Dictionary<string, Variable> variablesDict;
        Dictionary<string, Variable>.KeyCollection variables;
        Dictionary<string, Method> methodsDict;
        Dictionary<string, Method>.KeyCollection methods;
        String[] singleWordCommand;
        String[] multiPartProgramline;

        [SetUp]
        public void SetUp()
        {
            validator = Validator.GetValidator();
            variablesDict = new Dictionary<string, Variable>();
            methodsDict = new Dictionary<string, Method>();
            singleWordCommand = new String[1];
            multiPartProgramline = new String[10];

            Variable two = new Variable();
            variablesDict.Add("two", two);
            Variable three = new Variable();
            variablesDict.Add("three", three);

            Method myMethod = new Method();
            myMethod.AddLine("moveto 100,100");
            myMethod.AddLine("circle 50");
            methodsDict.Add("mymethod", myMethod);

            methods = methodsDict.Keys;
            variables = variablesDict.Keys;
        }

        [TestCaseSource(nameof(SingleWordCommandCases))]
        public void ValidOneWordCommands(String s)
        {
            //Arrange
            singleWordCommand = s.Split(' ');

            //Act
            validator.ValidateCommand(singleWordCommand, variables, methods);
        }

        [TestCaseSource(nameof(VariableAssignmentCommandCases))]
        public void ValidVariableAssignmentCommand(String s)
        {
            //Arrange
            multiPartProgramline = s.Split(' ');

            //Act
            validator.ValidateCommand(multiPartProgramline, variables, methods);
        }

        [Test]
        public void AssignmentVariableDoesNotExist()
        {
            //Arrange
            multiPartProgramline = "two = 1 + one".Split(' ');

            //Act
            try
            {
                validator.ValidateCommand(multiPartProgramline, variables, methods);
            }
            catch(GPLException ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Variable 'one' does not exist."));
                return;
            }

            Assert.Fail();
        }

        [Test]
        public void CommandWithInvalidChar()
        {
            //Arrange
            multiPartProgramline = "circle !50".Split(' ');

            //Act
            try
            {
                validator.ValidateCommand(multiPartProgramline, variables, methods);
            }
            catch(GPLException ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Invalid character found in command."));
                return;
            }

            Assert.Fail();
        }

        [TestCaseSource(nameof(InvalidSingleWordCommandCases))]
        [TestCaseSource(nameof(ShapeCommandWordCases))]
        public void ValidCommandRequiringArgsNoArgs(String s)
        {
            //Arrange
            singleWordCommand = s.Split(' ');

            //Act
            try
            {
                validator.ValidateCommand(singleWordCommand, variables, methods);
                return;
            }
            catch(GPLException ex)
            {
                Assert.That(ex.Message, Is.EqualTo("No arguments provided."));
                return;
            }

            //Assert
            Assert.Fail();

        }

        [Test]
        public void VariableDeclaredDoesNotExist()
        {
            //Arrange
            multiPartProgramline = "var newvariable".Split(' ');

            //Act
            validator.ValidateCommand(multiPartProgramline, variables, methods);
        }

        [Test]
        public void VariableDeclareAlreadyExists()
        {
            //Arrange
            multiPartProgramline = "var two".Split(' ');

            //Act
            try
            {
                validator.ValidateCommand(multiPartProgramline, variables, methods);
                return;
            }
            catch(GPLException ex)
            {
                //Assert
                Assert.That(ex.Message, Is.EqualTo("Variable " + multiPartProgramline[1] + " already exists"));
                return;
            }

            Assert.Fail();
        }

        [TestCaseSource(nameof(SingleIntegerCases))]
        public void ValidatePolygonNumberOfSides(int i)
        {
            //Arrange
            multiPartProgramline = ("polygon " + i + ",50").Split(' ');

            //Act
            try
            {
                validator.ValidateCommand(multiPartProgramline, variables, methods);
                return;
            }
            catch (GPLException ex)
            {
                //Assert
                Assert.That(ex.Message, Is.EqualTo("polygon must have more than 4 points/sides."));
                return;
            }
        }

        [TestCaseSource(nameof(SingleIntegerCases))]
        public void ValidateStarNumberOfPoints(int i)
        {
            //Arrange
            multiPartProgramline = ("star " + i + ",50").Split(' ');

            //Act
            try
            {
                validator.ValidateCommand(multiPartProgramline, variables, methods);
                return;
            }
            catch (GPLException ex)
            {
                //Assert
                Assert.That(ex.Message, Is.EqualTo("star must have more than 4 points/sides."));
                return;
            }
        }

        [TestCaseSource(nameof(ValidCommands))]
        public void ValidArgs(String s)
        {
            //Arrange
            multiPartProgramline = s.Split(' ');

            //Act
            try
            {
                validator.ValidateCommand(multiPartProgramline, variables, methods);
            }
            catch
            {
                Assert.Fail();
                return;
            }
            
        }

        [Test]
        public void VariableNameNotCommand()
        {
            //Arrange
            multiPartProgramline = "var var".Split(' ');

            //Act
            try
            {
                validator.ValidateCommand(multiPartProgramline, variables, methods);
            }
            catch (GPLException ex)
            {
                //Assert
                Assert.That(ex.Message, Is.EqualTo("Variable name cannot be the same as an existing command or method."));
                return;
            }

            Assert.Fail();
        }

        [Test]
        public void MethodNameNotCommand()
        {
            //Arrange
            multiPartProgramline = "method method".Split(' ');

            //Act
            try
            {
                validator.ValidateCommand(multiPartProgramline, variables, methods);
            }
            catch (GPLException ex)
            {
                //Assert
                Assert.That(ex.Message, Is.EqualTo("Mathod name cannot be the same as an existing command or variable."));
                return;
            }

            Assert.Fail();
        }

        [TestCaseSource(nameof(ShapeCommandWordCases))]
        public void IsShapeReturnsTrueForValidShape(String s)
        {
            if (!validator.IsShape(s))
                Assert.Fail();
        }

        static string[] VariableAssignmentCommandCases =
        {
            "two = 2",
            "three = three"
        };

        static string[] ValidCommands =
        {
            "circle two",
            "star 5,three",
            "rectangle two,three",
            "triangle 50",
            "polygon 7,50",
            "loop 2",
            "loop two"
        };

        static string[] SingleWordCommandCases = Validator.singleWordCommands.ToArray();
        
        //These commands require arguments so should not be single word lines.
        static string[] InvalidSingleWordCommandCases = Validator.commands.ToArray();
        static string[] ShapeCommandWordCases = Validator.shapes.ToArray();

        static int[] SingleIntegerCases =
        {
            1,2,3,4,5,6,7,8,9
        };

    }
}
