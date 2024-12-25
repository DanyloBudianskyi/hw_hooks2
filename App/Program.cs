using System;
using System.Runtime.InteropServices;

namespace App
{
    public class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int MessageBoxW(IntPtr hWnd, string lpText, string lpCaption, uint uType);

        static void Main(string[] args)
        {
            MessageBoxW(IntPtr.Zero, "Hello world", "Target Application", 0);

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
