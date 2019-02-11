using Domain;
using Domain.Abstractions;
using Domain.Exceptions;
using Logic.Services;
using Xunit;

namespace Logic.Tests
{
    public class DefaultCalculateServiceTests
    {
        private readonly ICalculationService _calculationService = new DefaultCalculateService(new BaseCalculator());

        [Fact]
        public void LongExpressionWithNegativeResult()
        {
            var result = _calculationService.Calculation("(1+5*8)+(1+8+(8*5)+(5*6)+(10/2)+8)-15+" +
                                            "(5*2-8*6)+(1+5*8)+(1+8+(8*5)+(5*6)+(10/2+8)-" +
                                            "15+(5*2-8*6)/((5*6)+(10/2)+8)-15+" +
                                            "(5*2-8*6)*((1+5*8)+(1+8+(8*5)+(5*6)+(10/2)+8)" +
                                            "-15+(5*2-8*6)+(1+5*8)+(1+8+(8*5)+(5*6)+(10/2)+8)" +
                                            "-15+(5*2-8*6)/((5*6)+(10/2)+8))-15+(5*2-8*6))");
            Assert.Equal(-7361.3, result);
        }

        [Fact]
        public void LongExpressionWithPositiveResult()
        {
            var result = _calculationService.Calculation("(1+5*8)+(1+8+(8*5)+(5*6)+(10/2)+8)-15+" +
                                                         "(5*2-8*6)+(1+5*8)+(1+8+(8*5)+(5*6)+(10/2+8)-" +
                                                         "15+(5*2-8*6)*(((5*6)+(10/2)+8)-15+" +
                                                         "(5*2-8*6)*((1+5*8)+(1+8+(8*5)+(5*6)+(10/2)+8)" +
                                                         "-15+(5*2-8*6)+(1+5*8)+(1+8+(8*5)+(5*6)+(10/2)+8)" +
                                                         "-15+(5*2-8*6)/((5*6)+(10/2)+8))-15+(5*2-8*6))-100*10+(12*2)-4)");
            Assert.Equal(284803.91, result);
        }

        [Fact]
        public void IncorrectExpressionWithCalculateExñeption()
        {
            Assert.Throws<CalculateExñeption>(() => 
            _calculationService.Calculation("(1+5*8)+(1+8+(8*5)+(5*6)+(10/2)+â)-15"));
        }
    }
}
