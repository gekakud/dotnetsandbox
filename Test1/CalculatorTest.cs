using CalculatorProject;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test1
{
    [TestClass]
    public class CalculatorTest
    {
        [TestMethod]
        public void TestAdd()
        {
            double addResult = 10;
            double actualAdd;
            
            //arrange
            var calc = new Calculator();
            //act
            actualAdd = calc.Add(3, 7);
            //assert
            Assert.AreEqual(addResult,actualAdd);
        }

        [TestMethod]
        public void TestDivide()
        {
            double divResult = 11;
            double actualDiv;

            //arrange
            var calc = new Calculator();
            //act
            actualDiv = calc.Divide(33, 3);
            //assert
            Assert.AreEqual(divResult, actualDiv);
        }
    }
}
