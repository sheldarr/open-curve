namespace OpenCurve.Engine
{
    using Microsoft.Xna.Framework;

    public static class GameServices
    {
        private static GameServiceContainer _gameServiceContainer;

        public static GameServiceContainer GameServiceContainer
        {
            get { return _gameServiceContainer ?? (_gameServiceContainer = new GameServiceContainer()); }
        }

        public static T GetService<T>()
        {
            return (T)GameServiceContainer.GetService(typeof(T));
        }

        public static void AddService<T>(T service)
        {
            GameServiceContainer.AddService(typeof(T), service);
        }

        public static void RemoveService<T>()
        {
            GameServiceContainer.RemoveService(typeof(T));
        }
    }
}
