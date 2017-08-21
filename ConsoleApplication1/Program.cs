using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

// NOTE: case insenstive parsing of command

namespace ConsoleApplication1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var inventory = new[]
            {
                new Soda("coke", count:5, cost:20),
                new Soda("sprite", count:3, cost:15),
                new Soda("fanta", count:3, cost:15)
            };
            var sodaMachine = new SodaMachine(inventory);
            sodaMachine.Start();
        }
    }

    public class SodaMachine
    {
        private readonly Dictionary<string, Soda> _inventory; // OrdinalIgnoreCase

        public SodaMachine(IEnumerable<Soda> inventory)
        {
            _inventory = inventory.ToDictionary(s => s.Name, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// This is the starter method for the machine
        /// </summary>
        public void Start()
        {
            // func for clarity further down
            bool StringEquals(string a, string b)
                => string.Equals(a, b, StringComparison.OrdinalIgnoreCase);

            // dummy log function
            void LogError(string str) {};

            int money = 0;

            while (true)
            {
                Console.WriteLine("\n\nAvailable commands:");
                Console.WriteLine("insert (money) - Money put into money slot");
                Console.WriteLine("order (coke, sprite, fanta) - Order from machines buttons");
                Console.WriteLine("sms order (coke, sprite, fanta) - Order sent by sms");
                Console.WriteLine("recall - gives money back");
                Console.WriteLine("-------");
                Console.WriteLine($"Inserted money: {money}");
                Console.WriteLine("-------\n\n");

                // get input from user, parse command and execute it
                var input = Console.ReadLine();
                var inputParts = input?.Split(' ') ?? new string[0];

                if (inputParts.Length < 1)
                {
                    continue;
                }

                var commandName = inputParts[0];

                if (StringEquals("insert", commandName)
                    && inputParts.Length >= 2)
                {
                    //Add to credit
                    var moneyStr = inputParts[1];

                    if (!int.TryParse(moneyStr, NumberStyles.None, CultureInfo.InvariantCulture, out var insertedMoney))
                    {
                        Console.WriteLine("Enter a valid amount");
                    }
                    else
                    {
                        money += insertedMoney;
                        Console.WriteLine($"Adding {insertedMoney} to credit");
                    }
                }
                else if (StringEquals("order", commandName)
                    && inputParts.Length >= 2)
                {
                    var sodaName = inputParts[1];

                    if (!_inventory.TryGetValue(sodaName, out var soda))
                    {
                        Console.WriteLine($"No such soda '{sodaName}'");
                    }
                    else if (money < soda.Cost)
                    {
                        var diff = soda.Cost - money;
                        Console.WriteLine($"Need {diff} more");
                    }
                    else if (!soda.TryReserveSoda())
                    {
                        Console.WriteLine($"No {soda.Name} left");
                    }
                    else
                    {
                        Console.WriteLine($"Giving {soda.Name} out");
                        money -= soda.Cost;
                        Console.WriteLine($"Giving {money} out in change");
                        money = 0;
                    }
                }
                else if (StringEquals("sms", commandName)
                         && inputParts.Length >= 3)
                {
                    var sodaName = inputParts[2];

                    // NOTE: sms order is confirmed to be correct, no additional checks on count, inventory count etc.

                    if (!_inventory.TryGetValue(sodaName, out var soda))
                    {
                        // NOTE: should not occur
                        LogError($"sms order: no soda by name '{sodaName}'");
                    }
                    else if (!soda.TryReserveSoda())
                    {
                        // NOTE: should not occur
                        LogError($"sms order: no {soda.Name} left");
                    }
                    else
                    {
                        Console.WriteLine($"Giving {soda.Name} out");
                    }
                }
                else if (StringEquals("recall", commandName))
                {
                    //Give money back
                    Console.WriteLine($"Returning {money} to customer");
                    money = 0;
                }
            }
        }
    }
    public class Soda
    {
        public Soda(string name, int cost, int count)
        {
            Name = name;
            Cost = cost;
            Count = count;
        }

        public string Name { get; }
        public int Cost { get; }
        public int Count { get; private set; }

        public bool TryReserveSoda()
        {
            if (Count < 1)
            {
                return false;
            }

            Count--;
            return true;
        }
    }
}