using System;

namespace OpenCurve
{
    using Microsoft.Xna.Framework;
    using SimpleInjector;

    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = new Container();

            container.RegisterSingle<OpenCurveGame>();

            using (var openCurveGame = container.GetInstance<OpenCurveGame>())
            {
                openCurveGame.Run();
            }
        }
    }
}
