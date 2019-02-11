using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Abstractions;
using Domain.Exceptions;
using Domain.Extensions;

namespace Logic.Services
{
    public class DefaultCalculateService : ICalculationService
    {
        private readonly ICalculator _calculator;
        readonly Dictionary<int, double> _digits = new Dictionary<int, double>();
        readonly Dictionary<int, IOperation> _operations = new Dictionary<int, IOperation>();
        readonly Dictionary<int, char> _brackets = new Dictionary<int, char>();

        private string _parseExeptionMessage(int indexOfExpressionElement, object simbol) =>
            $"Ошибка распознавания строки. {Environment.NewLine} Index: {indexOfExpressionElement};" +
            $"{Environment.NewLine}Parsed element: {simbol}";

        public DefaultCalculateService(ICalculator calculator)
        {
            _calculator = calculator;
        }

        public double Calculation(string expression)
        {
            ParseExpressionAndFillingDictionaries(expression);

            while (_brackets.Any())
            {
                var deepOfSubExpression = 0;
                CalculateExpressionInBrackets(ref deepOfSubExpression, _brackets.OrderBy(x => x.Key).First().Key);
            }

            return _digits.Count == 1 ? Math.Round(_digits.First().Value, 2) : Math.Round(CalculateByPriority(_operations), 2);
        }

        /// <summary>
        /// Парсинг элементов строки входящего математического выражения и заполнения этими значениями соотвествующих словарей 
        /// </summary>
        /// <param name="expression">выходящее математическое выражение</param>
        private void ParseExpressionAndFillingDictionaries(string expression)
        {
            var indexOfExpressionElement = 0;
            var num = "";

            for (int indexOfSymbol = 0; indexOfSymbol < expression.Length; indexOfSymbol++)
            {
                var simbol = expression[indexOfSymbol];

                if (char.IsDigit(simbol))
                    num = $"{num}{simbol}";
                else if (_calculator.OperationTryParse(simbol, out var operation))
                {
                    if (double.TryParse(num, out var parsedDigit))
                    {
                        _digits.Add(++indexOfExpressionElement, parsedDigit);
                        num = "";
                    }

                    _operations.Add(++indexOfExpressionElement, operation);
                }
                else if (simbol == '(' || simbol == ')')
                {
                    if (simbol == ')')
                        if (expression[indexOfSymbol-1] == ')')
                        {
                            _brackets.Add(++indexOfExpressionElement, simbol);
                            continue;
                        }
                        else if (double.TryParse(num, out var digit))
                        {
                            _digits.Add(++indexOfExpressionElement, digit);
                            num = "";
                        }
                        else
                            throw new CalculateExсeption(_parseExeptionMessage(indexOfExpressionElement, digit));

                    _brackets.Add(++indexOfExpressionElement, simbol);
                }
                else
                    throw new CalculateExсeption(_parseExeptionMessage(indexOfExpressionElement, simbol));
            }
        }

        /// <summary>
        /// Вычисление результата выражения, находящегося внутри скобок
        /// </summary>
        /// <param name="deepOfSubExpression">Глубина погружения рекурсивного метода, с целью выявления конечного выражения для вычисления</param>
        /// <param name="indexOfOpenBracket"></param>
        private void CalculateExpressionInBrackets(ref int deepOfSubExpression, int indexOfOpenBracket)
        {
            while (_brackets.Any())
            {
                var bracket = _brackets.First(x => x.Key > indexOfOpenBracket);

                if (bracket.Value == '(')
                {
                    deepOfSubExpression++;
                    CalculateExpressionInBrackets(ref deepOfSubExpression, bracket.Key);
                    continue;
                }

                if (bracket.Value == ')' && deepOfSubExpression > 0)
                {
                    deepOfSubExpression--;
                    continue;
                }
                    
                if (bracket.Value == ')' && deepOfSubExpression == 0)
                {
                    CalculateSubExpression(indexOfOpenBracket, bracket.Key);
                    RemoveProcessedBrackets(indexOfOpenBracket, bracket.Key);
                    return;
                }
            }
        }

        /// <summary>
        /// Вычисление подвыражения или всего выражения, если оно не имеет скобок
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        private void CalculateSubExpression(int startIndex, int endIndex)
        {
            var allDigitsOfSubExpresssionQuery = _digits.Where(x => x.Key >= startIndex && x.Key <= endIndex).AsQueryable();
            var maxIndexOfDigitFromSubExpression = allDigitsOfSubExpresssionQuery.Max(x => x.Key);
            var minIndexOfDigitFromSubExpression = allDigitsOfSubExpresssionQuery.Min(x => x.Key);
            CalculateByPriority(_operations.Where(x => maxIndexOfDigitFromSubExpression > x.Key && minIndexOfDigitFromSubExpression < x.Key));
        }


        /// <summary>
        /// Вычисление выражения по приоритету операций
        /// </summary>
        /// <param name="operations"></param>
        private double CalculateByPriority(IEnumerable<KeyValuePair<int, IOperation>> operations)
        {
            double result = 0;
            operations = operations as KeyValuePair<int, IOperation>[] ?? operations.ToArray();
            if (!operations.Any())
                return result;
            try
            {
                //итерация по операциям в порядке приоритета, начиная с высокого
                foreach (var operation in operations.OrderByDescending(x => x.Value.Priority))
                {
                    //в метод compute передаются значения, находящиеся по обе стороны от знака операции
                    result = Compute(_digits.OrderByDescending(x => x.Key).First(x => x.Key < operation.Key),
                        _digits.OrderBy(x => x.Key).First(x => x.Key > operation.Key),
                                 operation.Value);
                }
            }
            catch (Exception e)
            {
                throw new CalculateExсeption($"Ошибка вычисления.{Environment.NewLine}" +
                                            $"Message: {e.Message}{Environment.NewLine}{e.StackTrace}");
            }
            return result;
        }

        /// <summary>
        /// Вычисление 2х операндов по алгоритму конкретной операции вычисления - <see cref="operation"/>
        /// </summary>
        /// <param name="firstTerm"></param>
        /// <param name="secondTerm"></param>
        /// <param name="operation">Операция вычисления</param>
        private double Compute(KeyValuePair<int, double> firstTerm, KeyValuePair<int, double> secondTerm, IOperation operation)
        {
            var result = operation.Calculate(firstTerm.Value, secondTerm.Value);
            RemoveProcessedSymbolsAndSetResult(firstTerm.Key, secondTerm.Key, result);
            return result;
        }

        /// <summary>
        /// Замена отработаных числовых значений и операций на результирующее значение выражения, 
        /// находящегося в диапозоне <see cref="startIndex"/> и <see cref="endIndex"/>
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="result">Результат выражения, находящегося в диапозоне <see cref="startIndex"/> и <see cref="endIndex"/>, 
        /// который будет добавлен в массив числовых элементов, с соотвествующим порядковым индексом</param>
        private void RemoveProcessedSymbolsAndSetResult(int startIndex, int endIndex, double result)
        {
            _operations.Remove(_operations.First(x => x.Key > startIndex && x.Key < endIndex).Key);
            _digits.Remove(startIndex);
            _digits.Remove(endIndex);
            _digits.Add(startIndex, result);
        }

        private void RemoveProcessedBrackets(int startIndex, int endIndex)
        {
            if (_brackets.Any(x => x.Key == startIndex) && _brackets.Any(x => x.Key == endIndex))
            {
                _brackets.Remove(startIndex);
                _brackets.Remove(endIndex);
            }
        }
    }
}
