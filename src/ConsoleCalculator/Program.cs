﻿using System;
using Domain;
using Domain.Abstractions;
using Domain.Exceptions;
using Logic.Services;
using Logic.Validators;

namespace ConsoleCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var expression = Console.ReadLine()?.Trim();
                if (!expression.ExpressionIsValid())
                {
                    Console.WriteLine("Введено некорректное выражение. Попробуйте снова.");
                    continue;
                }
                Calculate(new DefaultCalculateService(new BaseCalculator()), expression);
            }
        }

        private static void Calculate(ICalculationService calculationService, string expression)
        {
            try
            {
                Console.WriteLine(calculationService.Calculation(expression));
            }
            catch (CalculateExсeption calculateExeption)
            {
                Console.WriteLine(calculateExeption.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Неизвестная ошибка.{Environment.NewLine}{e.Message}");
                throw;
            }
        }
    }
}
