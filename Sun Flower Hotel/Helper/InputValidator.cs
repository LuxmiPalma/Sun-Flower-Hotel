using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sun_Flower_Hotel.Helper
{
    public class InputValidator
    {
        /// <summary>
        /// Gets a valid integer input from the user.
        /// </summary>
        public int GetValidIntInput(string prompt)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (int.TryParse(input, out value))
                {
                    return value;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                }
            }
        }

        /// <summary>
        /// Gets a valid decimal input from the user.
        /// </summary>
        public decimal GetValidDecimalInput(string prompt)
        {
            decimal value;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (decimal.TryParse(input, out value))
                {
                    return value;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid decimal number.");
                }
            }
        }

        /// <summary>
        /// Gets a non-empty string input from the user.
        /// </summary>
        public string GetValidStringInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a non-empty string.");
                }
            }
        }
        public decimal? GetNullableDecimalInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    return null;

                if (decimal.TryParse(input, out var result))
                    return result;

                Console.WriteLine("Invalid input. Please enter a valid decimal number or leave blank to skip.");
            }
        }
    }

}

