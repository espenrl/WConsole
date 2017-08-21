using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SodaMachine sodaMachine = new SodaMachine();
            sodaMachine.Start();
        }
    }

    public class SodaMachine
    {
        private static int money;

        /// <summary>
        /// This is the starter method for the machine
        /// </summary>
        public void Start()
        {
            var inventory = new[] { new Soda { Name = "coke", Nr = 5 }, new Soda { Name = "sprite", Nr = 3 }, new Soda { Name = "fanta", Nr = 3 } };

            while (true)
            {
                Console.WriteLine("\n\nAvailable commands:");
                Console.WriteLine("insert (money) - Money put into money slot");
                Console.WriteLine("order (coke, sprite, fanta) - Order from machines buttons");
                Console.WriteLine("sms order (coke, sprite, fanta) - Order sent by sms");
                Console.WriteLine("recall - gives money back");
                Console.WriteLine("-------");
                Console.WriteLine("Inserted money: " + money);
                Console.WriteLine("-------\n\n");

                var input = Console.ReadLine();

                if (input.StartsWith("insert"))
                {
                    //Add to credit
                    money += int.Parse(input.Split(' ')[1]);
                    Console.WriteLine("Adding " + int.Parse(input.Split(' ')[1]) + " to credit");
                }
                if (input.StartsWith("order"))
                {
                    // split string on space
                    var csoda = input.Split(' ')[1];
                    //Find out witch kind
                    switch (csoda)
                    {
                        case "coke":
                            var coke = inventory[0];
                            if (coke.Name == csoda && money > 19 && coke.Nr > 0)
                            {
                                Console.WriteLine("Giving coke out");
                                money -= 20;
                                Console.WriteLine("Giving " + money + " out in change");
                                money = 0;
                                coke.Nr--;
                            }
                            else if (coke.Name == csoda && coke.Nr == 0)
                            {
                                Console.WriteLine("No coke left");
                            }
                            else if (coke.Name == csoda)
                            {
                                Console.WriteLine("Need " + (20 - money) + " more");
                            }

                            break;
                        case "fanta":
                            var fanta = inventory[2];
                            if (fanta.Name == csoda && money > 14 && fanta.Nr >= 0)
                            {
                                Console.WriteLine("Giving fanta out");
                                money -= 15;
                                Console.WriteLine("Giving " + money + " out in change");
                                money = 0;
                                fanta.Nr--;
                            }
                            else if (fanta.Name == csoda && fanta.Nr == 0)
                            {
                                Console.WriteLine("No fanta left");
                            }
                            else if (fanta.Name == csoda)
                            {
                                Console.WriteLine("Need " + (15 - money) + " more");
                            }

                            break;
                        case "sprite":
                            var sprite = inventory[1];
                            if (sprite.Name == csoda && money > 14 && sprite.Nr > 0)
                            {
                                Console.WriteLine("Giving sprite out");
                                money -= 15;
                                Console.WriteLine("Giving " + money + " out in change");
                                money = 0;
                                sprite.Nr--;
                            }
                            else if (sprite.Name == csoda && sprite.Nr == 0)
                            {
                                Console.WriteLine("No sprite left");
                            }
                            else if (sprite.Name == csoda)
                            {
                                Console.WriteLine("Need " + (15 - money) + " more");
                            }
                            break;
                        default:
                            Console.WriteLine("No such soda");
                            break;
                    }
                }
                if (input.StartsWith("sms order"))
                {
                    var csoda = input.Split(' ')[2];
                    //Find out witch kind
                    switch (csoda)
                    {
                        case "coke":
                            if (inventory[0].Nr > 0)
                            {
                                Console.WriteLine("Giving coke out");
                                inventory[0].Nr--;
                            }
                            break;
                        case "sprite":
                            if (inventory[1].Nr > 0)
                            {
                                Console.WriteLine("Giving sprite out");
                                inventory[1].Nr--;
                            }
                            break;
                        case "fanta":
                            if (inventory[2].Nr > 0)
                            {
                                Console.WriteLine("Giving fanta out");
                                inventory[2].Nr--;
                            }
                            break;
                    }

                }

                if (input.Equals("recall"))
                {
                    //Give money back
                    Console.WriteLine("Returning " + money + " to customer");
                    money = 0;
                }

            }
        }
    }
    public class Soda
    {
        public string Name { get; set; }
        public int Nr { get; set; }

    }
}
