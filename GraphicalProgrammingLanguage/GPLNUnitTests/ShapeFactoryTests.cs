using NUnit.Framework;
using GraphicalProgrammingLanguage;
using System;
using System.Collections.Generic;

namespace GPLNUnitTests
{
    public class ShapeFactoryTests
    {
        ShapeFactory shapeFactory = ShapeFactory.GetShapeFactory();

        [Test, TestCaseSource("GetShapeCreationTestCases")]
        public void ShapeFactory_CreatesCorrectShape(string name, Type type)
        {
            //Act
            Shape shape = shapeFactory.GetShape(name);

            //Assert
            Assert.That(shape, Is.TypeOf(type));
        }

        [Test]
        public void ShapeFactory_ThrowsException_IncorrectShapeName()
        {
            //Arrange
            String name = "invalid";
            //Act
            try
            {
                Shape shape = shapeFactory.GetShape(name);
            }
            catch (GPLException e)
            {
                Assert.That(e.Message, Is.EqualTo("Shape '" + name + "' not found."));
                return;
            }

            Assert.Fail();
        }

        private static IEnumerable<TestCaseData> GetShapeCreationTestCases()
        {
            yield return new TestCaseData("circle", typeof(Circle));
            yield return new TestCaseData("rectangle", typeof(Rectangle));
            yield return new TestCaseData("square", typeof(Rectangle));
            yield return new TestCaseData("triangle", typeof(Triangle));
            yield return new TestCaseData("polygon", typeof(Polygon));
            yield return new TestCaseData("star", typeof(Star));
        }
    }
}