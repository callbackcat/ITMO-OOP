using System;

namespace Banks.ConsoleUI
{
    public static class UserInput
    {
        public static string[] Get()
        {
            string input = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Wrong input!");
                input = Console.ReadLine();
            }

            return input.Split();
        }
    }
}