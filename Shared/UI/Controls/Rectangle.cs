using MG.Shared.Global.Entities;
using Microsoft.Xna.Framework;

namespace MG.Shared.UI.Controls
{
  public class Rectangle : BasicEntityChild
  {
    public Point Margin;
    protected ColorScheme colors;
    public Rectangle(int x, int y, int width, int height, Color back = new Color())
    {
      texture = Pixel;
      placement = new Microsoft.Xna.Framework.Rectangle(x, y, width, height);
      Colors = new(back);
      Margin = new(4, 4);
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
      destinationRectangle.Inflate(-Margin.X, -Margin.Y);
    }
  }
}
