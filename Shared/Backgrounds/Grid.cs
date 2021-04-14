using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static MG.Shared.Global.Sprite;

namespace MG.Shared.Background
{
    public class Grid : Background
    {
        private Vector2 ScaleHorizontal;
        private Vector2 ScaleVertical;
        private Vector2[] LinesHorizontal;
        private Vector2[] LinesVertical;
        public Grid(GraphicsDeviceManager graphics) : base(graphics)
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < LinesHorizontal.Length; x++)
            {
                spriteBatch.Draw(Pixel, LinesHorizontal[x], null, Color.Lime, 0, Vector2.Zero, ScaleHorizontal, SpriteEffects.None, 0);
            }
            for (int x = 0; x < LinesVertical.Length; x++)
            {
                spriteBatch.Draw(Pixel, LinesVertical[x], null, Color.Lime, 0, Vector2.Zero, ScaleVertical, SpriteEffects.None, 0);
            }
        }
        protected override void DrawBoundsChanged()
        {
            base.DrawBoundsChanged();
            for (int x = 0; x < LinesHorizontal.Length; x++)
            {
                LinesHorizontal[x] = DrawBounds.Location.ToVector2();
                LinesHorizontal[x].Y += CenterCell.Width * x;
            }
            for (int x = 0; x < LinesVertical.Length; x++)
            {
                LinesVertical[x] = DrawBounds.Location.ToVector2();
                LinesVertical[x].X += CenterCell.Height * x;
            }
        }
        public override void OnResize(object sender, EventArgs e)
        {
            base.OnResize(sender, e);
            ScaleVertical = new Vector2(2, DrawBounds.Height);
            ScaleHorizontal = new Vector2(DrawBounds.Width, 1);
            LinesHorizontal = new Vector2[DrawBounds.Height / CenterCell.Height];
            LinesVertical = new Vector2[DrawBounds.Width / CenterCell.Width];
            DrawBoundsChanged();
        }
    }
}
