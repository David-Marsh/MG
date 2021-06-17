using MG.Shared.Global.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace MG.Shared.UI.Controls
{
    public class UIPanel : BasicEntityParent
    {
        public Point Margin;
        private ColorScheme colors;
        public Rectangle DestinationRectangle { get => destinationRectangle; set => destinationRectangle = value; }
        public ColorScheme Colors
        {
            get => colors; set
            {
                colors = value;
                color = colors.Back;
            }
        }
        public UIPanel(int x, int y, int width, int height, Color back = new Color())
        {
            texture = Pixel;
            placement = new Rectangle(x, y, width, height);
            Colors = new(back);
            Margin = new(4, 4);
            Grid = new(width, height);
        }
        public virtual IMouseControl FindControlAt(Point position)
        {
            var child = Children.LastOrDefault(c => c.Contains(position));
            if (child is UIPanel panel)
                return panel.FindControlAt(position);
            else if (child is IMouseControl mouseControl)
                return mouseControl;
            else
                return null;
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
            destinationRectangle.Location = (placement.Location * size) / grid;
            destinationRectangle.Size = (placement.Size * size) / grid;
            destinationRectangle.Inflate(-Margin.X, -Margin.Y);
            foreach (var child in Children) child.SizeTo(this);
        }
    }
}
