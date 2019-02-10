using System.Linq;
using Domain.Abstractions;

namespace Domain.Extensions
{
    public static class OperationExtension
    {
        public static bool OperationTryParse(this ICalculator calculator, char sign, out IOperation operation)
        {
            operation = calculator.Operations.FirstOrDefault(x => x.OperationSign == sign);
            return operation != null;
        }
    }
}
