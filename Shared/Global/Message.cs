using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MG.Shared.Global
{
  public struct Message
  {
    public Message(string characters, Vector2 position, float zoom)
    {
      Text = characters;
      Position = position;
      Zoom = zoom;
      Font = Sprite.Font;
    }
    public Message(string characters, Rectangle textBox, bool menu = false)
    {
      Text = characters;
      Vector2 textSize;
      if (menu)
      {
        Font = Sprite.Menu;
        textSize = Font.MeasureString(((char)0xE700).ToString());
        Zoom = textBox.Height / textSize.Y;
      }
      else
      {
        Font = Sprite.Font;
        textSize = Font.MeasureString(Text);
        Zoom = MathHelper.Min(textBox.Width / textSize.X, textBox.Height / textSize.Y) * 0.95f;
      }
      Position = textBox.Location.ToVector2();
      Position.X += 0.5f * (textBox.Width - (textSize.X * Zoom));
      Position.Y += 0.5f * (textBox.Height - (textSize.Y * Zoom));
    }
    public SpriteFont Font;
    public string Text;
    public Vector2 Position;
    public float Zoom;
  }
}
