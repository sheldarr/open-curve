namespace OpenCurve.Engine.Bonuses
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class BonusGenerator
    {
        private readonly ContentManager _content;
        private readonly SpriteBatch _spriteBatch;
        private readonly BoardSize _boardSize;
        private readonly Random _random;

        private const double BonusRespawnDelay = 5.0d;
        private TimeSpan BonusTimer { get; set; }

        private ICollection<IMapBonus> MapBonuses { get; set; }

        public BonusGenerator(ContentManager content, SpriteBatch spriteBatch, BoardSize boardSize)
        {
            _content = content;
            _spriteBatch = spriteBatch;
            _boardSize = boardSize;
            _random = new Random();
            BonusTimer = TimeSpan.FromSeconds(BonusRespawnDelay);
        }

        private void Update(GameTime gameTime)
        {
            BonusTimer -= gameTime.ElapsedGameTime;
            if (BonusTimer < TimeSpan.FromSeconds(0))
            {
                GenerateNewBonus();
                BonusTimer = TimeSpan.FromSeconds(BonusRespawnDelay);
            }
        }

        private void GenerateNewBonus()
        {
        }

        public BonusType RandomBonusType()
        {
            var bonuses = Enum.GetValues(typeof(BonusType));
            var randomBonus = (BonusType)bonuses.GetValue(_random.Next(bonuses.Length));

            return randomBonus;
        }
    }
}
