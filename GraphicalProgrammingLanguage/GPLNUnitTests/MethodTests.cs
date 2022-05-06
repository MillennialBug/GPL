using NUnit.Framework;
using GraphicalProgrammingLanguage;

namespace GPLNUnitTests
{
    public class MethodTests
    {
        [Test]
        public void MethodObject_ReturnsBodyAsArray()
        {
            //Arrange
            string line1 = "moveto 100,100";
            string line2 = "fill on";
            string line3 = "circle 50";
            Method method = new Method();
            method.AddLine(line1);
            method.AddLine(line2);
            method.AddLine(line3);
            string[] returnArray = new string[3];

            //Act
            returnArray = method.GetBodyAsArray();

            //Assert
            Assert.AreEqual(line1, returnArray[0]);
            Assert.AreEqual(line2, returnArray[1]);
            Assert.AreEqual(line3, returnArray[2]);
        }
    }
}