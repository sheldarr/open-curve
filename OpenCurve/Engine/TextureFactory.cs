namespace OpenCurve.Engine
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public static class TextureFactory
    {
        public static Texture2D CreateCircleTexture(GraphicsDevice graphicsDevice, int radius)
        {
            var texture = new Texture2D(graphicsDevice, radius, radius);
            var colorData = new Color[radius * radius];

            var diam = radius / 2f;
            var diamsq = diam * diam;

            for (var x = 0; x < radius; x++)
            {
                for (var y = 0; y < radius; y++)
                {
                    var index = x * radius + y;
                    var pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            
            return texture;
        }
    }
}
