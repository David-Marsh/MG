using MG.Shared.Global.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace MG.Shared.UI.Controls
{
  public class Panel : BasicEntityParent
  {
    public Point Margin;
    private ColorScheme colors;
    public Microsoft.Xna.Framework.Rectangle DestinationRectangle { get => destinationRectangle; set => destinationRectangle = value; }
    public ColorScheme Colors
    {
      get => colors; set
      {
        colors = value;
        color = colors.Back;
      }
    }
    public Panel(int x, int y, int width, int height) : this(x, y, width, height, new ColorScheme(Color.Gray, Color.Transparent)) { }
    public Panel(int x, int y, int width, int height, ColorScheme colors)
    {
      texture = Pixel;
      placement = new Microsoft.Xna.Framework.Rectangle(x, y, width, height);
      Colors = colors;
      Margin = new(4, 4);
      Grid = new(width, height);
    }
    public virtual IMouseControl FindControlAt(Point position)
    {
      var child = Children.LastOrDefault(c => c.Contains(position));
      return child is Panel panel ? panel.FindControlAt(position) : child is IMouseControl mouseControl ? mouseControl : null;
    }
    public virtual void SizeTo(int x, int y, GraphicsDeviceManager graphicsDevice)
    {
      SizeTo(new(x, y), new(graphicsDevice.PreferredBackBufferWidth, graphicsDevice.PreferredBackBufferHeight));
    }
    public virtual void SizeTo(int x, int y, GraphicsDevice graphicsDevice)
    {
      SizeTo(new(x, y), new(graphicsDevice.Adapter.CurrentDisplayMode.Width, graphicsDevice.Adapter.CurrentDisplayMode.Height));
    }
    public virtual void SizeTo(Point grid, Point size)
    {
      destinationRectangle.Location = placement.Location * size / grid;
      destinationRectangle.Size = placement.Size * size / grid;
      destinationRectangle.Inflate(-Margin.X, -Margin.Y);
      foreach (var child in Children) child.SizeTo(this);
    }
  }
}
