using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static MG.Shared.Global.Sprite;

namespace MG.Shared.UI
{
    public abstract class Control
    {
        protected bool IsPressed;
        private bool isHovering;
        private bool enabled;
        protected Rectangle PanelPlacement;
        public bool CanHover;
        public bool Visible;
        public Vector2 Location = Vector2.Zero;
        public Vector2 Size = Vector2.Zero;
        public Vector2 ParentLocation = Vector2.Zero;
        protected ColorScheme Colors;
        public Rectangle HitBox { get; protected set; }
        public bool Enabled
        {
            get => enabled; set
            {
                enabled = value;
                Colors.Set(enabled, isHovering);
            }
        }
        protected bool IsHovering
        {
            get => isHovering; set
            {
                isHovering = value;
                Colors.Set(enabled, isHovering);
            }
        }

        public event EventHandler Clicked;
        public event EventHandler MouseDown;
        public event EventHandler MouseUp;
        public event EventHandler MouseLeave;
        public event EventHandler MouseEnter;
        protected Control(Color back) : this(back, Color.Gray) { }
        protected Control(Color back, Color fore)
        {
            Enabled = true;
            Visible = true;
            CanHover = true;
            Colors = new ColorScheme(fore, back);
        }
        protected Control(Color back, Color fore, int col, int row, int colspan, int rowspan) : this(back, fore)
        {
            PanelPlacement = new(col, row, colspan, rowspan);
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
        public virtual void Setup(Panel panel)
        {
            Vector2 cellsize = panel.Cellsize;
            ParentLocation = panel.Location + panel.ParentLocation;
            Location = new Vector2(cellsize.X * PanelPlacement.X, cellsize.Y * PanelPlacement.Y);
            Size = new Vector2(cellsize.X * PanelPlacement.Width, cellsize.Y * PanelPlacement.Height);
            Setup();
        }
        public virtual void Setup() => HitBox = new Rectangle((Location + ParentLocation).ToPoint(), Size.ToPoint());
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Visible) spriteBatch.Draw(Pixel, HitBox, Colors.Back);
        }
        public virtual void Update(GameTime gameTime) { }
        public virtual bool Contains(Point point) => HitBox.Contains(point);
        internal virtual void OnMouseDown()
        {
            if (!IsPressed) Clicked?.Invoke(this, new EventArgs());
            IsPressed = true;
            MouseDown?.Invoke(this, new EventArgs());
        }
        internal virtual void OnMouseUp()
        {
            IsPressed = false;
            MouseUp?.Invoke(this, new EventArgs());
        }
        internal virtual void OnMouseEnter()
        {
            IsHovering = CanHover;
            MouseEnter?.Invoke(this, new EventArgs());
        }
        internal virtual void OnMouseLeave()
        {
            IsHovering = IsPressed = false;
            MouseLeave?.Invoke(this, new EventArgs());
        }
        internal virtual void OnClicked() => Clicked?.Invoke(this, new EventArgs());
        public virtual void ApplyMargin(int margin) { HitBox = new Rectangle(HitBox.Location + new Point(margin, margin), HitBox.Size - new Point(margin * 2, margin * 2)); }
    }
}
