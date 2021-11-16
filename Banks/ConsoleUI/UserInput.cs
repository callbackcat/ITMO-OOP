using System;
using System.Collections.Generic;
using System.Linq;

namespace Banks.ConsoleUI
{
    public static class UserInput
    {
        public static List<string> GetLine()
        {
            string input = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input must not be empty!");
                input = Console.ReadLine();
            }

            return input.Split().ToList();
        }

        public static List<decimal> GetDecimalArgs()
        {
            List<decimal> decimalArgs;

            while (true)
            {
                try
                {
                    List<string> input = GetLine();
                    decimalArgs = input.Select(Convert.ToDecimal).ToList();
                }
                catch (Exception)
                {
                    Console.WriteLine("Wrong decimal arguments input! Try again: ");
                    continue;
                }

                break;
            }

            return decimalArgs;
        }
    }
}