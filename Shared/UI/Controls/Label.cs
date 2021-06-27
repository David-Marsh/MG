using MG.Shared.Global;
using MG.Shared.Global.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MG.Shared.UI.Controls
{
  public class Label : Rectangle
  {
    private SpriteFont font;
    private string text;
    private Vector2 textPosition;
    private Vector2 textZoom;

    public Label(int x, int y, int width, int height, string text, Color? back = null, Color? fore = null) : base(x, y, width, height, back ?? Color.Transparent)
    {
      Colors = new(fore ?? Color.White, back ?? Color.Transparent);
      this.text = text;
    }

    public bool Menu { get; set; } = false;
    public string Text
    {
      get => text; set
      {
        text = value;
        PlaceText();
      }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (!Visable) return;
      spriteBatch.Draw(texture, destinationRectangle, Colors.Back);
      spriteBatch.DrawString(font, text, textPosition, colors.Fore, 0, Vector2.Zero, textZoom, SpriteEffects.None, 0);
    }

    public override void SizeTo(BasicEntityParent parent)
    {
      base.SizeTo(parent);
      PlaceText();
    }
    private void PlaceText()
    {
      Vector2 textSize;
      font = Menu ? Sprite.Menu : Sprite.Font;
      textSize = font.MeasureString(text);
      textZoom.X = textZoom.Y = MathHelper.Min(destinationRectangle.Width / textSize.X, destinationRectangle.Height / textSize.Y) * 0.95f;
      textPosition = destinationRectangle.Location.ToVector2();
      textPosition.X += 0.5f * (destinationRectangle.Width - (textSize.X * textZoom.X));
      textPosition.Y += 0.5f * (destinationRectangle.Height - (textSize.Y * textZoom.Y));
    }
  }
}
