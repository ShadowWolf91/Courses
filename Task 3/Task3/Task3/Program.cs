using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;
using ConsoleTables;
using System.Collections;

namespace Task3
{
internal class Program
    {
        static void Main(string[] args)
        {
            if (ParametersValidation(args) == -1)
            {
                return;
            }
            while (true)
            {
                
                byte[] keyForHMAC = HMACKeyGenerator();
                byte computerChoice = ComputerMove(args.Length);
                byte[] HMAC = HMACOutput(keyForHMAC, computerChoice);
                Console.WriteLine("HMAC: " + BitConverter.ToString(HMAC).Replace("-", ""));
                ConsoleMenu(args);
                var playerChoice = InputArgumentsCheck(args);
                if (playerChoice == 0)
                {
                    break;
                }
                Console.WriteLine("Your move: " + args[playerChoice - 1]);
                Console.WriteLine("Computer move: " + args[computerChoice - 1]);
                Console.WriteLine(CalculatingTheWinner(playerChoice, computerChoice, args.Length));
                Console.WriteLine("HMACKey: " + BitConverter.ToString(keyForHMAC).Replace("-", "") + "\n");
            }
        }

        public static int ParametersValidation(string[] args)
        {
            if (args.Count() < 3)
            {
                Console.WriteLine("Small amount of parameters, please, make them odd and more than 1.");
                return -1;
            }
            if (args.Count() % 2 == 0 || args.Count() < 3)
            {
                Console.WriteLine("Even amount of parameters, please, make them odd and more than 1.");
                return -1;
            }
            if (args.Distinct().Count() != args.Count())
            {
                Console.WriteLine("Repeated parameters, please, make them unique.");
                return -1;
            }
            return 0;
        }

        private static byte[] HMACKeyGenerator()
        {
            var generator = RandomNumberGenerator.Create();
            var key = new byte[32];
            generator.GetBytes(key);
            return key;
        }

        public static byte ComputerMove(int amount)
        {
            var random = new Random();
            int value = random.Next(0, amount) + 1;
            return (byte)value;
        }

        private static byte[] HMACOutput(byte[] keyBytes, byte message)
        {

            var temp = new HMACSHA256(keyBytes);

            var sha256 = SHA256.Create();

            var buffer2 = temp.ComputeHash(System.Text.Encoding.UTF8.GetBytes(message.ToString()));
            var str = BitConverter.ToString(buffer2).Replace("-", "");
            Console.WriteLine(Convert.ToString(str));

            return sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(message.ToString()));
        }

        private static void ConsoleMenu(string[] choice)
        {
            Console.WriteLine("Available moves: ");
            for (int i = 0; i < choice.Length; i++)
            {
                Console.WriteLine((i + 1) + " - " + choice[i]);
            }
            Console.WriteLine("0 - exit");
            Console.WriteLine("10 - help\n");
        }

        private static byte InputArgumentsCheck(string[] args)
        {
            int selection;
            while (true)
            {
                Console.Write("Enter your move: ");
                try
                {
                    selection = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine($"Wrong input data. Please, choose from 0 to {args.Length}. \n");
                    ConsoleMenu(args);
                    continue;
                }
                if (selection >= 0 && selection <= args.Length)
                {
                    break;
                }
                else if (selection == 10)
                {
                    HelpTable(args, args.Length);
                }
                else
                {
                    Console.WriteLine($"Incorrect amount of arguments. Please, choose from 0 to {args.Length}. \n");
                    ConsoleMenu(args);
                }
            }
            return (byte)selection;
        }

        private static string CalculatingTheWinner(int playerMove, int computerMove, int amountOfMoves)
        {
            if (playerMove - computerMove == 0)
            {
                return "Result: Draw";
            }
            if (((playerMove + amountOfMoves) - computerMove) % amountOfMoves > amountOfMoves / 2)
            {
                return "Result: Computer wins";
            }
            else 
            {
                return "Result: Player wins";
            }
        }

        private static void HelpTable(string[] args, int amountOfMoves)
        {
            var list = new List<string>() { };
            foreach (string arg in args)
            {
                list.Add(arg);
            }

            var a = new string[args.Length];
            var b = new string[args.Length];
            var c = new string[args.Length];
            var d = new string[args.Length];
            var e = new string[args.Length];
            var f = new string[args.Length];
            var g = new string[args.Length];
            var h = new string[args.Length];
            var k = new string[args.Length];
            var resultList = new List<string>() { };
            int[,] resultTable = new int[args.Length, args.Length];
            ConsoleTable table = new ConsoleTable(" v PC/User > ");
            table.AddColumn(list);
            for (int i = 0; i < args.Length; i++)
            {
                for (int j = 0; j < args.Length; j++)
                {
                    resultTable[i, j] += i;
                    resultTable[i, j] -= j;
                    if ((i - j + (amountOfMoves / 2) + amountOfMoves) % amountOfMoves - (amountOfMoves / 2) == 0)
                    {
                        resultTable[i, j] = 0;
                    }
                    else if ((i - j + (amountOfMoves / 2) + amountOfMoves) % amountOfMoves - (amountOfMoves / 2) > 0)
                    {
                        resultTable[i, j] = 1;
                    }
                    else if ((i - j + (amountOfMoves / 2) + amountOfMoves) % amountOfMoves - (amountOfMoves / 2) < 0)
                    {
                        resultTable[i, j] = -1;
                    }
                    //Console.Write($"{resultTable[i, j]}\t");
                }
                //Console.WriteLine();
            }
            int z = 0;
            for (int i = 0; i < args.Length; i++)
            {
                for (int j = 0; j < args.Length; j++)
                {
                    resultList.Add(resultTable[j, i].ToString());
                    //Console.WriteLine(resultList[z]);
                    z++;
                }
                //Console.WriteLine();
            }
            z = 0;
            for (int i = 0; i < args.Length; i++)
            {
                for (int j = 0; j < args.Length; j++)
                {
                    if (resultList[z].ToString() == "0")
                    {
                        resultList[z] = "Draw";
                    }
                    else if (resultList[z].ToString() == "1")
                    {
                        resultList[z] = "Win";
                    }
                    else if (resultList[z].ToString() == "-1")
                    {
                        resultList[z] = "Lose";
                    }
                    //Console.Write(resultList[z]);
                    z++;
                }
                //Console.WriteLine(" ");
            }

            string[] resultArray = resultList.ToArray();
            foreach (string text in resultArray)
            {
                //Console.WriteLine(text);
            }
            if(args.Length == 3)
            {
                for (int i = 0, j = 0; j < a.Length; i++, j++)
                {
                    a[j] = resultArray[i];
                }
                for (int i = args.Length, j = 0; j < b.Length; i++, j++)
                {
                    b[j] = resultArray[i];
                }
                for (int i = args.Length * 2, j = 0; j < c.Length; i++, j++)
                {
                    c[j] = resultArray[i];
                }
            }
            if (args.Length == 5)
            {
                for (int i = 0, j = 0; j < a.Length; i++, j++)
                {
                    a[j] = resultArray[i];
                }
                for (int i = args.Length, j = 0; j < b.Length; i++, j++)
                {
                    b[j] = resultArray[i];
                }
                for (int i = args.Length * 2, j = 0; j < c.Length; i++, j++)
                {
                    c[j] = resultArray[i];
                }
                for (int i = args.Length * 3, j = 0; j < d.Length; i++, j++)
                {
                    d[j] = resultArray[i];
                }
                for (int i = args.Length * 4, j = 0; j < e.Length; i++, j++)
                {
                    e[j] = resultArray[i];
                }
            }
            if (args.Length == 7)
            {
                for (int i = 0, j = 0; j < a.Length; i++, j++)
                {
                    a[j] = resultArray[i];
                }
                for (int i = args.Length, j = 0; j < b.Length; i++, j++)
                {
                    b[j] = resultArray[i];
                }
                for (int i = args.Length * 2, j = 0; j < c.Length; i++, j++)
                {
                    c[j] = resultArray[i];
                }
                for (int i = args.Length * 3, j = 0; j < d.Length; i++, j++)
                {
                    d[j] = resultArray[i];
                }
                for (int i = args.Length * 4, j = 0; j < e.Length; i++, j++)
                {
                    e[j] = resultArray[i];
                }
                for (int i = args.Length * 5, j = 0; j < f.Length; i++, j++)
                {
                    f[j] = resultArray[i];
                }
                for (int i = args.Length * 6, j = 0; j < g.Length; i++, j++)
                {
                    g[j] = resultArray[i];
                }
            }
            if (args.Length == 9)
            {
                for (int i = 0, j = 0; j < a.Length; i++, j++)
                {
                    a[j] = resultArray[i];
                }
                for (int i = args.Length, j = 0; j < b.Length; i++, j++)
                {
                    b[j] = resultArray[i];
                }
                for (int i = args.Length * 2, j = 0; j < c.Length; i++, j++)
                {
                    c[j] = resultArray[i];
                }
                for (int i = args.Length * 3, j = 0; j < d.Length; i++, j++)
                {
                    d[j] = resultArray[i];
                }
                for (int i = args.Length * 4, j = 0; j < e.Length; i++, j++)
                {
                    e[j] = resultArray[i];
                }
                for (int i = args.Length * 5, j = 0; j < f.Length; i++, j++)
                {
                    f[j] = resultArray[i];
                }
                for (int i = args.Length * 6, j = 0; j < g.Length; i++, j++)
                {
                    g[j] = resultArray[i];
                }
                for (int i = args.Length * 7, j = 0; j < h.Length; i++, j++)
                {
                    h[j] = resultArray[i];
                }
                for (int i = args.Length * 8, j = 0; j < k.Length; i++, j++)
                {
                    k[j] = resultArray[i];
                }
            }
            Console.WriteLine("\n");
            for (int i = 0; i < args.Length; i++)
            {
                if (args.Length == 3)
                {
                    table.AddRow(list[i], a[i], b[i], c[i]);
                }
                else if (args.Length == 5)
                {
                    table.AddRow(list[i], a[i], b[i], c[i], d[i], e[i]);
                }
                else if (args.Length == 7)
                {
                    table.AddRow(list[i], a[i], b[i], c[i], d[i], e[i], f[i], g[i]);
                }
                else if (args.Length == 9)
                {
                    table.AddRow(list[i], a[i], b[i], c[i], d[i], e[i], f[i], g[i], h[i], k[i]);
                }
            }
            table.Write();
        }
    }
}