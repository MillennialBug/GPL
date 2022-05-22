using System;
using System.Collections.Generic;
using GraphicalProgrammingLanguage;
using NUnit.Framework;

namespace GPLNUnitTests
{
    /// <summary>
    /// A Test Class to check funcionality within the Validator Class.
    /// </summary>
    internal class ValidatorTests
    {
        Validator validator;
        Dictionary<string, Variable> variablesDict;
        Dictionary<string, Variable>.KeyCollection variables;
        Dictionary<string, Method> methodsDict;
        Dictionary<string, Method>.KeyCollection methods;
        String[] singleWordCommand;
        String[] multiPartProgramline;

        /// <summary>
        /// Sets objects ready for use in the tests.
        /// </summary>
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

        /// <summary>
        /// Tests that valid single word commands pass validation.
        /// </summary>
        /// <param name="s">Valid single word command.</param>
        [TestCaseSource(nameof(SingleWordCommandCases))]
        public void ValidOneWordCommands(String s)
        {
            //Arrange
            singleWordCommand = s.Split(' ');

            //Act
            validator.ValidateCommand(singleWordCommand, variables, methods);
        }

        /// <summary>
        /// Tests that variables can be successfully assigned a value with a valid command.
        /// </summary>
        /// <param name="s">Variable assignment command E.G. "one = 1"</param>
        [TestCaseSource(nameof(VariableAssignmentCommandCases))]
        public void ValidVariableAssignmentCommand(String s)
        {
            //Arrange
            multiPartProgramline = s.Split(' ');

            //Act
            validator.ValidateCommand(multiPartProgramline, variables, methods);
        }

        /// <summary>
        /// Tests that exception is thrown when a variable used for assignment does not exist.
        /// </summary>
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

        /// <summary>
        /// Tests that commands containing invalid characters throw an exception.
        /// </summary>
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

        /// <summary>
        /// Tests that a command that requires arguments throws an exception when no args provided.
        /// </summary>
        /// <param name="s"></param>
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
            }
            catch(GPLException ex)
            {
                Assert.That(ex.Message, Is.EqualTo("No arguments provided."));
            }
        }

        /// <summary>
        /// Tests a variable cannot be declared twice.
        /// </summary>
        [Test]
        public void VariableDeclareAlreadyExists()
        {
            //Arrange
            multiPartProgramline = "var two".Split(' ');

            //Act
            try
            {
                validator.ValidateCommand(multiPartProgramline, variables, methods);
            }
            catch(GPLException ex)
            {
                //Assert
                Assert.That(ex.Message, Is.EqualTo("Variable " + multiPartProgramline[1] + " already exists"));
            }
        }

        /// <summary>
        /// Tests that Polygons must have at least 5 sides.
        /// </summary>
        /// <param name="i"></param>
        [TestCaseSource(nameof(SingleIntegerCases))]
        public void ValidatePolygonNumberOfSides(int i)
        {
            //Arrange
            multiPartProgramline = ("polygon " + i + ",50").Split(' ');

            //Act
            try
            {
                validator.ValidateCommand(multiPartProgramline, variables, methods);
            }
            catch (GPLException ex)
            {
                //Assert
                Assert.That(ex.Message, Is.EqualTo("polygon must have more than 4 points/sides."));
            }
        }

        /// <summary>
        /// Tests that Stars must have at least 5 points.
        /// </summary>
        /// <param name="i"></param>
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

        /// <summary>
        /// Tests a set of valid commands pass validation.
        /// </summary>
        /// <param name="s"></param>
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

        /// <summary>
        /// Tests that variables cannot be named the same as an existing command or method.
        /// </summary>
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

        /// <summary>
        /// Tests that methods cannot be named the same as an existing command or variable.
        /// </summary>
        [Test]
        public void MethodNameNotCommand()
        {
            //Arrange
            multiPartProgramline = "method method".Split(' ');

            //Act
            try
            {
                validator.ValidateMethod(multiPartProgramline[1], variables, methods);
            }
            catch (GPLException ex)
            {
                //Assert
                Assert.That(ex.Message, Is.EqualTo("Method name cannot be the same as an existing command or variable."));
                return;
            }

            Assert.Fail();
        }

        /// <summary>
        /// Checks that the Validator.ValidateMethod method is able to correctly grab a ParamMethod name.
        /// </summary>
        [Test]
        public void ValidatesParamMethodCommand()
        {
            //Arrange
            String program = "myparammethod(one, two, three)";

            //Act
            String methodName = validator.ValidateMethod(program, variables, methods);

            //Assert
            Assert.That(methodName, Is.EqualTo("myparammethod"));
        }

        /// <summary>
        /// Tests that IsShape returns true when passed the name of a valid shape.
        /// </summary>
        /// <param name="s"></param>
        [TestCaseSource(nameof(ShapeCommandWordCases))]
        public void IsShapeReturnsTrueForValidShape(String s)
        {
            if (!validator.IsShape(s))
                Assert.Fail();
        }

        /// <summary>
        /// Checks that Validator.ValidateCommand is able to correctly validate an increment or decrement command.
        /// </summary>
        /// <param name="s"></param>
        [TestCase("two++")]
        [TestCase("two--")]
        public void ValidatesIncrementOrDecrementCommand(String s)
        {
            //Arrange
            singleWordCommand = s.Split(' ');

            //Act
            validator.ValidateCommand(singleWordCommand, variables, methods);
        }

        /// <summary>
        /// Valid variable assignment comamnds.
        /// </summary>
        static string[] VariableAssignmentCommandCases =
        {
            "two = 2",
            "three = three"
        };

        /// <summary>
        /// A set of valid commands with valid arguments.
        /// </summary>
        static string[] ValidCommands =
        {
            "circle two",
            "star 5,three",
            "rectangle two,three",
            "triangle 50",
            "polygon 7,50",
            "loop 2",
            "loop two",
            "if two == 2"
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
