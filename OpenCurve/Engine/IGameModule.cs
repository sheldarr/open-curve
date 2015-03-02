namespace OpenCurve.Engine
{
    using Microsoft.Xna.Framework;

    public interface IGameModule
    {
        void Initialize();
        void LoadContent();
        void UnloadContent();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
