using System;
using System.Runtime.InteropServices;

namespace Dummy
{
    class Program
    {
        static void Main()
        {
            int testInt = 123456;
            string testString = "Default String";
            unsafe
            {
                // Assign the address of number to a pointer: 
                int* intPointer = &testInt;
                char* stringPointer;
                fixed (char* p = testString)
                {
                    stringPointer = p;
                }

                while (true)
                {
                    Console.WriteLine($"Process ID: {GetCurrentProcessId()}");

                    IntPtr aIntPointer = (IntPtr)intPointer;
                    string adressIntPointer = "0x" + aIntPointer.ToString("x").ToUpper();
                    // Integer
                    Console.WriteLine("Value at the location pointed to by intPointer: {0}", *intPointer);
                    Console.WriteLine("The address stored in intPointer: {0}", adressIntPointer);
                    // String
                    IntPtr aStringPointer = (IntPtr)stringPointer;
                    string adressStringPointer = "0x" + aStringPointer.ToString("x").ToUpper();
                    Console.WriteLine("Value at the location pointed to by stringPointer: {0}", *stringPointer);
                    Console.WriteLine("The address stored in stringPointer[0]: {0}", adressStringPointer);

                    Console.ReadKey(); 
                }
            }
        }


        #region import

        [DllImport("kernel32.dll")]
        static extern uint GetCurrentProcessId();

        #endregion
    }
}