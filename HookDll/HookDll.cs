using System;
using System.Runtime.InteropServices;
using EasyHook;

namespace Dll_hook
{
    public class Hook : IEntryPoint
    {
        private static ServerInterface _server;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int MessageBoxDelegate(IntPtr hWnd, string lpText, string lpCaption, uint uType);
        private static MessageBoxDelegate _originalMessageBoxW;

        public static int HookedMessageBox(IntPtr hWnd, string lpText, string lpCaption, uint uType)
        {
            Console.WriteLine($"MessageBoxW intercepted! Message: {lpText}, Caption: {lpCaption}");

            _server.ReportMessage(lpText);

            return _originalMessageBoxW(hWnd, lpText, lpCaption, uType);
        }

        public Hook(RemoteHooking.IContext context, string channelName)
        {
            _server = RemoteHooking.IpcConnectClient<ServerInterface>(channelName);
            _server.ReportMessage("Hook initialized.");
        }

        public void Run(RemoteHooking.IContext context, string channelName)
        {
            try
            {
                var hook = LocalHook.Create(
                    LocalHook.GetProcAddress("user32.dll", "MessageBoxW"),
                    new MessageBoxDelegate(HookedMessageBox),
                    this
                );

                _originalMessageBoxW = Marshal.GetDelegateForFunctionPointer<MessageBoxDelegate>(
                    LocalHook.GetProcAddress("user32.dll", "MessageBoxW"));

                hook.ThreadACL.SetExclusiveACL(new[] { 0 });

                _server.ReportMessage("Hook installed.");

                while (true)
                {
                    Console.WriteLine("This program was hooked");
                    System.Threading.Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                _server.ReportMessage($"Error: {ex.Message}");
            }
        }
    }

    public class ServerInterface : MarshalByRefObject
    {
        public void ReportMessage(string message)
        {
            Console.WriteLine($"Captured Message: {message}");
        }
    }
}
