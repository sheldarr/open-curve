namespace OpenCurve.Engine.Bonuses
{
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class BonusTextureProvider
    {
        private readonly Texture2D _speedTexture;
        private readonly Texture2D _slowTexture;

        public BonusTextureProvider(ContentManager content)
        {
            _speedTexture = content.Load<Texture2D>("speed");
            _slowTexture = content.Load<Texture2D>("speed");
        }

        public Texture2D GetTextureForBonus(BonusType bonusType)
        {
            switch (bonusType)
            {
                case(BonusType.SpeedGreen):
                case(BonusType.SpeedRed):
                    return _speedTexture;
                case (BonusType.SlowGreen):
                case (BonusType.SlowRed):
                    return _slowTexture;
                default:
                    return _speedTexture;
            }
        }
    }
}
