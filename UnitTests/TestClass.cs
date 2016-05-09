using System.Collections;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class TestClass
    {
        public Sut ClassUnderTest;

        [SetUp]
        public void SetUp()
        {
            ClassUnderTest = new Sut();
        }

        [Test, TestCaseSource(typeof (TestsFactory), "TestCase")]
        public void RunTest(TestObjectInstance p_objectToTest)
        {
            
        }

        [TearDown]
        public void TearDown()
        {
            ClassUnderTest = null;
        }
    }

    public class Sut
    {
        
    }

    public class TestObjectInstance
    {
        
    }

    public class TestsFactory
    {
        public static IEnumerable TestCase
        {
            get
            {
                yield return new TestObjectInstance
                {

                };

                yield return new TestObjectInstance
                {
                    
                };
            }
        }
    }
}
