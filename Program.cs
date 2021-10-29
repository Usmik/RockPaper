using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


namespace rps
{
    class Hmac
    {
        public static void PrintHmac(byte[] key, string pcStep)
        {
            byte[] pcStepBytes = Encoding.ASCII.GetBytes(pcStep);
            HMACSHA256 hmac = new HMACSHA256(key);
            var pcStepHash = hmac.ComputeHash(pcStepBytes);
            Console.WriteLine(BitConverter.ToString(pcStepHash));
        }
    }

    class key
    {
        const int KEY_LENGTH = 128;
        public static byte[] keyGenerate()
        {
            byte[] resultKey = new Byte[KEY_LENGTH / 8];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(resultKey);
            return resultKey;
        }
    }

    class Program
    {
        static void menu(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine($"{i + 1} - {args[i]}");
            }
            Console.WriteLine("? - help\n0 - Exit");
        }

        static int computerChoice(int limit)
        {
            return RandomNumberGenerator.GetInt32(limit);
        }

        static void Main(string[] args)
        {

            if (args.Length <= 1)
            {
                Console.WriteLine("Wrong args(enter 3 or more)");
                return;
            }
            else if (args.Length % 2 == 0)
            {
                Console.WriteLine("Wrong args(Number of args must be uneven)");
                return;
            }
            else if (args.Length != args.Select(x => x.ToLower()).Distinct<string>().Count())
            {
                Console.WriteLine("Wrong args(they can't repeat)");
                return;
            }

            byte[] secretKey = key.keyGenerate();
            int pcChoice = computerChoice(args.Length);
            Console.WriteLine("HMAC:");
            Hmac.PrintHmac(secretKey, args[pcChoice]);

            while (true)
            {
                Console.WriteLine("Available moves:");
                menu(args);
                Console.Write("Enter your move:");
                string userChoice = Console.ReadLine();
                if (userChoice == "?")
                {
                    Console.WriteLine("LETS IMAGINE YOU SEE A HELP TABLE");
                    continue;
                }
                else if (userChoice == "0")
                {
                    Console.WriteLine("Ok, goodbye");
                    return;
                }
                else if (!Char.IsSeparator(userChoice[0]) && Char.IsNumber(userChoice, 0))
                {
                    int userChoiceChecked = Convert.ToInt32(userChoice) - 1;
                    if (userChoiceChecked < 0 || userChoiceChecked > args.Length)
                    {
                        Console.WriteLine("Enter a correct number");
                        continue;
                    }
                    Console.WriteLine($"Your move: {args[userChoiceChecked]}");
                    Console.WriteLine($"Computer move: {args[pcChoice]}");
                    if (pcChoice == (userChoiceChecked))
                        Console.WriteLine("Draw");

                    else if (pcChoice > userChoiceChecked && (args.Length / 2 >= (pcChoice - userChoiceChecked)))
                        Console.WriteLine("You lose! (:");

                    else if (pcChoice < userChoiceChecked && ((userChoiceChecked - pcChoice) > (args.Length / 2)))
                        Console.WriteLine("You lose!");

                    else
                        Console.WriteLine("You Win! Gratz!");

                    Console.WriteLine("HMAC Key:");
                    Console.WriteLine(BitConverter.ToString(secretKey));
                }
                else
                    Console.WriteLine("Try again");
            }
        }
    }
}
