using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.Shared.Background
{
    public abstract class Background
    {
        protected Rectangle DrawBounds;
        protected Rectangle CenterCell;
        private Point DrawBoundsOffset;

        private int maskSize;
        protected int mask;
        public int MaskBits
        {
            get => maskSize; set
            {
                maskSize = value;
                CenterCell.Width = CenterCell.Height = 1 << maskSize;
                mask = ~(CenterCell.Width - 1);
            }
        }
        public Background(GraphicsDeviceManager graphics, int maskSize)
        {
            MaskBits = maskSize;                                   
            OnResize(graphics, null);
        }
        public virtual void OnResize(object sender, EventArgs e)
        {
            DrawBoundsOffset = new(((GraphicsDeviceManager)sender).PreferredBackBufferWidth, ((GraphicsDeviceManager)sender).PreferredBackBufferHeight);
            DrawBoundsOffset.X /= 2;
            DrawBoundsOffset.Y /= 2;
            DrawBoundsOffset += CenterCell.Size;
            DrawBoundsOffset.X &= mask;
            DrawBoundsOffset.Y &= mask;
            DrawBounds.Size = DrawBoundsOffset + DrawBoundsOffset + CenterCell.Size;
        }
        public virtual void Update(Vector2 screencenter)
        {
            if (CenterCell.Contains(screencenter)) return;
            CenterCell.Location = screencenter.ToPoint();
            CenterCell.X &= mask;
            CenterCell.Y &= mask;
            DrawBoundsChanged();
        }
        protected virtual void DrawBoundsChanged() => DrawBounds.Location = CenterCell.Location - DrawBoundsOffset;
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
