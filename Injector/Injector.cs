using System;
using EasyHook;

namespace Injector
{
    class Program
    {
        static void Main(string[] args)
        {
            int targetPID = 0;

            Console.Write("Enter target process ID: ");
            targetPID = Int32.Parse(Console.ReadLine());

            string channelName = null;

            RemoteHooking.IpcCreateServer<Dll_hook.ServerInterface>(
                ref channelName,
                System.Runtime.Remoting.WellKnownObjectMode.Singleton);

            string injectionLibrary = (@"D:\System Programing\hw_hooks2\HookDll\bin\Debug\HookDll.dll");

            try
            {
                RemoteHooking.Inject(
                    targetPID,
                    injectionLibrary,
                    injectionLibrary,
                    channelName);

                Console.WriteLine("Injection successful.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Injection failed: {ex.Message}");
            }
            Console.ReadLine();
        }
    }
}
