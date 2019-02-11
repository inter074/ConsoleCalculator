using Logic.Validators;
using Xunit;

namespace Logic.Tests
{
    public class ValidatorTests
    {
        [Fact]
        public void ExpressionWithIncorrectNumberOfBracketsTest()
        {
            var expression = "(1+5*8)+(1+8+(8*5)+(5*6)+(10/2)+8-15";
            Assert.False(expression.ExpressionIsValid());
        }

        [Fact]
        public void ExpressionWithIncorrectSymbolsTest()
        {
            var expression = "ццц(1+5*8)+(1+8+(8*5)+(5*6)+(10/2)+8)-15";
            Assert.False(expression.ExpressionIsValid());
        }
    }
}
