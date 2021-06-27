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
    public static Color Spectrum(float hue)
    {
      hue *= 6;
      return (int)hue switch
      {
        0 => Color.Lerp(Color.Red, Color.Yellow, hue % 1),
        1 => Color.Lerp(Color.Yellow, Color.Green, hue % 1),
        2 => Color.Lerp(Color.Green, Color.Cyan, hue % 1),
        3 => Color.Lerp(Color.Cyan, Color.Blue, hue % 1),
        4 => Color.Lerp(Color.Blue, Color.Magenta, hue % 1),
        5 => Color.Lerp(Color.Magenta, Color.Red, hue % 1),
        _ => Color.Lerp(Color.Red, Color.Yellow, hue % 1),
      };
    }
  }
}
