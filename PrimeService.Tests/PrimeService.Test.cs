using NUnit.Framework;
using PrimeService;

namespace PrimeService.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IsPrime_Input1_ShouldTrue()
        {
            // Arrange 
            PrimeService ps = new PrimeService();

            //Act

            var result = ps.IsPrime(2);

            //Assert
            
            Assert.IsTrue(result, "2 is not prime number?");
            
        }
        [Test]
        [TestCase(7)]
        [TestCase(10)]
        [TestCase(113)]
        public void IsPrime_WithTestCase(int n){
            // Arrange 
            PrimeService ps = new PrimeService();

            //Act

            var result = ps.IsPrime(n);

            //Assert
            
            Assert.IsTrue(result, $"{n} is not prime number?");

        }
    }
}