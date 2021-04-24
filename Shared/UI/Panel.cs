using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace MG.Shared.UI
{
    public class Panel : Control
    {
        public List<Control> Controls { get; private set; }
        public int Rows, Collums, Padding;
        public Vector2 Cellsize { get =>  new(Size.X / Collums, Size.Y / Rows); }
        public Panel(Color back, Color fore, int col, int row, int colspan, int rowspan, int collums = 0, int rows = 0) : base(back, fore, col, row, colspan, rowspan)
        {
            Controls = new List<Control>();
            Rows = rows;                        
            Collums = collums;
            Padding = 0;
            ParentLocation = Vector2.Zero;
        }
        public virtual void Setup(GraphicsDeviceManager graphics)         // place at root
        {
            Setup(graphics,Collums,Rows);
        }
        public virtual void Setup(GraphicsDeviceManager graphics, int colums, int rows)         // place at root
        {
            Vector2 parentsize = new(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            Vector2 cellsize = new(parentsize.X / colums, parentsize.Y / rows);
            ParentLocation = Vector2.Zero;
            Location = new Vector2(cellsize.X * PanelPlacement.X, cellsize.Y * PanelPlacement.Y);
            Size = new Vector2(cellsize.X * PanelPlacement.Width, cellsize.Y * PanelPlacement.Height);
            Setup();
        }
        public override void Update(GameTime gameTime)
        {
            if (!Visible) return;
            foreach (var control in Controls) control.Update(gameTime);
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible) return;
            base.Draw(spriteBatch);
            foreach (var control in Controls) control.Draw(spriteBatch);
        }
        public virtual Control FindControlAt(Point position)
        {
            var control = Controls.LastOrDefault(c => c.Contains(position));
            if (control is Panel panel)
                return panel.FindControlAt(position);
            else
                return control;
        }
    }
}
