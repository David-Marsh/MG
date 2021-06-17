using MG.Shared.Global.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.Shared.Backgrounds
{
    public class Graph : DrawableGameComponent
    {

        public class Gridline : BasicEntity
        {
            public bool Major;
            public Gridline(Color color)
            {
                this.color = color;
                this.texture = Pixel;
            }
            public byte Alpha { get => color.A; set => color.A = value; }
            public Color Color { get => color; set => color = value; }
        }

        private int cellMask;
        private int linesMajorMask;
        private int cellSizeLog2;
        private Point ViewportSize;                         // Viewport origin pluse cell size
        private Rectangle DrawBounds;                       // Area drawn by this class
        private Rectangle CenterCell;                       // While the screen origin is in this cell no movement calculations required
        private float Zoom,fade;
        private Gridline[] gridlines;

        public int CellSizeLog2
        {
            get => cellSizeLog2; set
            {
                if (cellSizeLog2 == value) return;
                cellSizeLog2 = value;
                CenterCell.Width = CenterCell.Height = 1 << cellSizeLog2;
                cellMask = ~(CenterCell.Width - 1);
                linesMajorMask = ((CenterCell.Width * 4) - 1);
                ReSize(null, null);
            }
        }
        public Graph(Game game) : base(game)
        {
        }
        public virtual void Draw(SpriteBatch spriteBatch) { for (int x = gridlines.Length - 1; x >= 0; x--) { gridlines[x].Draw(spriteBatch); } }
        protected virtual void DrawBoundsChanged()
        {
            DrawBounds.Size = ViewportSize + ViewportSize + CenterCell.Size;
            DrawBounds.Location = CenterCell.Location - ViewportSize;
            gridlines = new Gridline[(DrawBounds.Width / CenterCell.Width) + (DrawBounds.Height / CenterCell.Height)];
            int lineWidth = 5 << (int)(Zoom * 1);
            int i = 0;
            for (int x = DrawBounds.X; x < DrawBounds.Right; x += CenterCell.Width)     // Vertical gridlines
            {
                gridlines[i] = new(Color.Green);
                gridlines[i].Major = 0 == (linesMajorMask & x);
                gridlines[i].X = x;
                gridlines[i].Y = DrawBounds.Y;
                gridlines[i].Height = DrawBounds.Height;
                gridlines[i].Width = lineWidth;
                if(!gridlines[i].Major) gridlines[i].Alpha = (byte)(255 * 0.5f);
                i++;
            }
            for (int y = DrawBounds.Y; y < DrawBounds.Bottom; y += CenterCell.Height)     // Horizontal gridlines
            {
                gridlines[i] = new(Color.Green);
                gridlines[i].Major = 0 == (linesMajorMask & y);
                gridlines[i].X = DrawBounds.X;
                gridlines[i].Y = y;
                gridlines[i].Height = gridlines[i].Major ? lineWidth : lineWidth >> 1;
                gridlines[i].Width = DrawBounds.Width;
                if (!gridlines[i].Major) gridlines[i].Alpha = (byte)(255 * 0.5f);
                i++;
            }
        }
        protected override void LoadContent()
        {
            BasicEntity.CreateThePixel(GraphicsDevice);
            CellSizeLog2 = 6;
            GraphicsDevice.ResourceCreated += ReSize;
            GraphicsDevice.DeviceReset += ReSize;
            base.LoadContent();
        }
        private void ReSize(object sender, EventArgs e)
        {
            ViewportSize.X = GraphicsDevice.Viewport.Width << (int)(Zoom * 2);
            ViewportSize.Y = GraphicsDevice.Viewport.Height << (int)(Zoom * 2);
            ViewportSize += CenterCell.Size;
            ViewportSize.X &= cellMask;
            ViewportSize.Y &= cellMask;
            DrawBoundsChanged();
        }
        public virtual void Update(Point screencenter, float zoom)
        {
            fade = (float)Math.Log2(zoom);
            Zoom = -(float)Math.Floor(fade);
            fade += Zoom;
            fade *= fade;
            CellSizeLog2 = 5 + (int)Zoom * 2;
            for (int x = gridlines.Length - 1; x >= 0; x--)
            {
                if(!gridlines[x].Major) gridlines[x].Color = Color.Lerp(Color.Transparent ,Color.Green, fade);
            }
            Update(screencenter);
        }
        public virtual void Update(Point screencenter)
        {
            if (CenterCell.Contains(screencenter)) return;
            CenterCell.Location = screencenter;
            CenterCell.X &= cellMask;
            CenterCell.Y &= cellMask;
            DrawBoundsChanged();
        }
    }
}
