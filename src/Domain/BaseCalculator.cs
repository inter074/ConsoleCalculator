using System.Collections.Generic;
using Domain.Abstractions;
using Domain.Operations;

namespace Domain
{
    public class BaseCalculator : ICalculator
    {
        public BaseCalculator()
        {
            if (Operations == null)
                Operations = new List<IOperation>();

            Operations.Add(new AdditionOperation());
            Operations.Add(new SubtractionOperation());
            Operations.Add(new DivisionOperation());
            Operations.Add(new MultiplyOperation());
        }

        public ICollection<IOperation> Operations { get; }

    }
}
