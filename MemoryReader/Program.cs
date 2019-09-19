using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MemoryReader
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("PID: ");
            int pID = int.Parse(Console.ReadLine());
            // Integer
            Console.Write("Memory address of the integer to read(in hexadecimal): ");
            string adressHexaInt = Console.ReadLine();
            long adressInt = Convert.ToInt64(adressHexaInt, 16);
            Memory m = new Memory(pID, adressInt);
            var resInt = m.ReadBytes<int>();
            // String
            Console.Write("Memory address of the string to read(in hexadecimal): ");
            string adressHexaString = Console.ReadLine();
            long adressString = Convert.ToInt64(adressHexaString, 16);
            Memory mString = new Memory(pID, adressString);
            var resString = mString.ReadBytes<string>();
            
            Console.WriteLine("Value read (Integer) : " + BitConverter.ToInt32(resInt, 0));
            Console.WriteLine("Value read (String) : " + Encoding.Unicode.GetString(resString));

            Console.ReadKey();
        }
    }
}
