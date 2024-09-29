using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using SHA3.Net;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Utilities;
using System.Security.Policy;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var path = @"D:\BSTU\Courses\Task 2\Files";
            var sha3_256 = Sha3.Sha3256();
            int i = 1, j = 1;
            var dir = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            var arr = new List<string> { };
            Console.WriteLine("File hashes: \n");
            foreach (var file in dir)
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                var hash = sha3_256.ComputeHash(fs);
                var str = BitConverter.ToString(hash).ToLower().Replace("-", "").ToString();
                Console.WriteLine(i + ". " + Convert.ToString(str));
                i++;
                arr.Add(str);
            }
            Console.WriteLine("\nFile hashes by ascending:\n");
            arr.Sort();
            foreach (var end in arr)
            {
                Console.WriteLine(j + ". " + Convert.ToString(end));
                j++;
            }
            string txt = String.Concat(arr);
            string email = "xxxxxxxxx@gmail.com";
            string final = txt + email;
            Console.WriteLine("\nAll hashes together plus email:\n");
            Console.WriteLine(final);
            var finalTranslate = sha3_256.ComputeHash(Encoding.UTF8.GetBytes(final));
            var finalHash = BitConverter.ToString(finalTranslate).ToLower().Replace("-", "").ToString();
            Console.WriteLine("\nScore:\n");
            Console.WriteLine(Convert.ToString(finalHash));
            Console.ReadLine();
        }
    }
}
