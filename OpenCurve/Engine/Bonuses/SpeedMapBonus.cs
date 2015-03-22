namespace OpenCurve.Engine.Bonuses
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class SpeedMapBonus : IMapBonus
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _bonusTexture;
        private readonly Vector2 _position;

        public SpeedMapBonus(SpriteBatch spriteBatch, Texture2D bonusTexture, Vector2 position)
        {
            _spriteBatch = spriteBatch;
            _bonusTexture = bonusTexture;

            _position = position;
        }

        public void Draw()
        {
            _spriteBatch.Draw(_bonusTexture, _position, Color.Red);
        }
    }
}
