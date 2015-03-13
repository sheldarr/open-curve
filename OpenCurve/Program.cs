namespace OpenCurve
{
    using System;
    using SimpleInjector;

    public static class Program
    {
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
