using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MG.Shared.Global
{
    public static class Sprite
    {
        public static Texture2D Pixel { get; set; }
        public static SpriteFont Font { get; set; }
        public static SpriteFont Menu { get; set; }
        public static void CreateThePixel(GraphicsDevice graphicsDevice)
        {
            Pixel = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Pixel.SetData(new[] { Color.White });
        }
        public static void CreateTheFonts(Game game)
        {
            Font = game.Content.Load<SpriteFont>("Font/Font");
            Menu = game.Content.Load<SpriteFont>("Font/Menu");
        }
    }
}
