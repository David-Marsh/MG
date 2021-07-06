using MG.Shared.Global.Entities;
using Microsoft.Xna.Framework;

namespace MG.Shared.UI.Controls
{
  public class Rectangle : BasicEntityChild
  {
    public Point Margin;
    public int MarginRatio;
    protected ColorScheme colors;
    public Rectangle(int x, int y, int width, int height, Color back) : this(x, y, width, height, new ColorScheme(back)) { }
    public Rectangle(int x, int y, int width, int height, ColorScheme colors)
    {
      Colors = colors;
      texture = Pixel;
      placement = new Microsoft.Xna.Framework.Rectangle(x, y, width, height);
      Margin = Point.Zero;
      MarginRatio = 8;
    }
    public ColorScheme Colors
    {
      get => colors; set
      {
        colors = value;
        color = colors.Back;
      }
    }

    public override void SizeTo(BasicEntityParent parent)
    {
      base.SizeTo(parent);
      Margin.X = Margin.Y = MathHelper.Min(destinationRectangle.Width, destinationRectangle.Height) / MarginRatio;
      destinationRectangle.Inflate(-Margin.X, -Margin.Y);
    }
  }
}
