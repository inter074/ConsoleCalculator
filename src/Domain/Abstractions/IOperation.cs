using Domain.Enums;

namespace Domain.Abstractions
{
    public interface IOperation
    {
        /// <summary>
        /// Знак конкретной операции
        /// </summary>
        char OperationSign { get; }

        /// <summary>
        /// Приоритет операции
        /// </summary>
        OperationPriority Priority { get; }

        /// <summary>
        /// Вычисление 2х операндов специальным образом, в зависимости от конкретной операции
        /// </summary>
        /// <param name="firstNum"></param>
        /// <param name="secondNum"></param>
        double Calculate(double firstNum, double secondNum);
    }
}