using System.Collections.Generic;

namespace Domain.Abstractions
{
    public interface ICalculator
    {
        /// <summary>
        /// Доступные математические операции
        /// </summary>
        ICollection<IOperation> Operations { get; }
    }
}