namespace OpenCurve.Engine
{
    using Microsoft.Xna.Framework;

    public interface IOpenCurveComponent
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
