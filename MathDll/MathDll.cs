using RGiesecke.DllExport;
using System.Runtime.InteropServices;

namespace MathDll
{
    public class MyMath
    {
        [DllExport("Multiply", CallingConvention = CallingConvention.StdCall)]
        public static int Multiply(int a, int b)
        {
            return a * b;
        }
    }
}
