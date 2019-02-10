using System;

namespace Domain.Exceptions
{
    public class CalculateExeption : Exception
    {
        public CalculateExeption(string message) : base(message)
        {
            
        }
    }
}
