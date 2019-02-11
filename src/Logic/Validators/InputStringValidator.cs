using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Logic.Validators
{
    public static class InputStringValidator
    {
        public static bool ExpressionIsValid(this string expression) => 
            CheckFirstSymbolOfExpression(expression) && CheckBrackets(expression) && CheckOnlyForAllowedCharactersAndTypes(expression.Trim());
        
        /// <summary>
        /// Проверяет строку выражения на корректность
        /// </summary>
        /// <param name="expression"></param>
        /// <remarks>По дефолту может принимать числа типа int или double и дефолтные знаки операций: +,-,/,* </remarks>
        /// <returns></returns>
        private static bool CheckOnlyForAllowedCharactersAndTypes(string expression)
        {
            return Regex.IsMatch(expression.Replace("(","").Replace(")", ""), @"^-?\d+(?:\.\d+)+?$|\d+(?:\+\d+)?$|^-?\d+(?:\.\d+)(?:\+\d+)(?:\.\d+)+?$|^-?\d+(?:\.\d+)(?:\-\d+)(?:\.\d+)+?$|^-?\d+(?:\.\d+)(?:\*\d+)(?:\.\d+)+?$|^-?\d+(?:\.\d+)(?:\/\d+)(?:\.\d+)+?$");
        }

        private static bool CheckFirstSymbolOfExpression(string expression)
        {
            if (expression == null)
            {
                Console.WriteLine("Неверный формат строки. Строка не может быть пустой.");
                return false;
            }

            if (!char.IsDigit(expression.First()) && expression.First() != '(')
            {
                Console.WriteLine("Неверный формат строки. Строка начинается некорректно.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Проверка на кол-во открывающих скобок относительно закрывающих
        /// </summary>
        private static bool CheckBrackets(string expression)
        {
            if (expression.Count(x => x == '(') != expression.Count(x => x == ')'))
            {
                Console.WriteLine("Неверный формат строки. Кол-во знаков \"(\" не соотвествует кол-ву знаков \")\".");
                return false;
            }
            return true;
        }
    }
}
