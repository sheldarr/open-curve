using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenCurve
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new OpenCurveGame())
            {
                game.Run();
            }
        }
    }
#endif
}