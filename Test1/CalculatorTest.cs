using System;
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
            int addResult = 10;
            int actualAdd;
            
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
            int divResult = 11;
            int actualDiv;

            //arrange
            var calc = new Calculator();
            //act
            actualDiv = calc.Divide(33, 3);
            //assert
            Assert.AreEqual(divResult, actualDiv);
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void DivideByZero()
        {
            var calc = new Calculator();
            var vvv = calc.Divide(5, 0);
        }
    }
}
