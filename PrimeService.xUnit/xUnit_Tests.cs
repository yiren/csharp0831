using System;
using Xunit;


namespace PrimeService.xUnit
{
    public class xUnitTests
    {
        [Fact]
        public void IsPrime_Input1_ReturnFalse()
        {
            // arrange
            PrimeService ps = new PrimeService();

            // act
            bool result = ps.IsPrime(1);
            
            //assert
            Assert.False(result, "1 不是質數");
        }

        [Theory]
        [InlineData(3)]
        [InlineData(113)]
        [InlineData(45)]
        public void IsPrime_InlineData(int n)
        {
            // arrange
            PrimeService ps = new PrimeService();

            // act
            bool result = ps.IsPrime(n);
            
            //assert
            Assert.False(result, $"{n} 不是質數");
        }
    }
}
