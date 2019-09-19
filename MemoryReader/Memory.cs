using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MemoryReader
{
    public class Memory
    {
        public int ProcessID { get; set; }
        public long MemoryAdress { get; set; }

        public Memory(int processID, long memoryAdress)
        {
            this.ProcessID = processID;
            this.MemoryAdress = memoryAdress;
        }

        public byte[] ReadBytes<T>()
        {
            int bufferSize;
            if (typeof(T) == typeof(int))
                bufferSize = sizeof(int);
            else if (typeof(T) == typeof(string))
                bufferSize = 100;
            else
                throw new NotImplementedException("This type is not implemented.");

            IntPtr processHandle = OpenProcess(PROCESS_VM_OPERATION | PROCESS_WM_READ, false, ProcessID);
            int bytesRead = 0;
            byte[] buffer = new byte[bufferSize];

            bool success = ReadProcessMemory((int)processHandle, MemoryAdress, buffer, bufferSize, ref bytesRead);
            if (!success)
            {
                uint a = GetLastError();
                Console.WriteLine($"Erreur : {a}");
            }
            if (typeof(T) == typeof(string))
                return ResizeStringValueMemory(buffer);
            else
                return buffer;
        }

        public byte[] ResizeStringValueMemory(byte[] buffer)
        {
            byte[] res;
            bool isEnd = false;
            int i;
            for (i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] == 0)
                {
                    if (!isEnd)
                        isEnd = true;
                    else
                        break;
                } else
                    isEnd = false;
            }
            res = buffer.Take(i).ToArray();

            return res;
        }

        #region import

        const int INVALID_HANDLE_VALUE = -1;
        const int PROCESS_WM_READ = 0x0010;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_OPERATION = 0x0008;
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int PROCESS_ALL_ACCESS = 0x1F0FFF;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, long lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        internal static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, IntPtr nSize, ref UInt32 lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        static extern uint GetLastError();

        #endregion
    }
}
