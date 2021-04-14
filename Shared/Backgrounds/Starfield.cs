using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.CompilerServices;
using static MG.Shared.Global.Sprite;

namespace MG.Shared.Background
{
    public class Starfield : Background
    {
        private Star[] stars;
        public Starfield(GraphicsDeviceManager graphics) : base(graphics)
        {
            MaskBits = 7;                                   // Cell size bits
            Star.Maskbits = MaskBits;
            Star.Mask = ~mask;
            OnResize(graphics, null);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < stars.Length; x++)
            {
                spriteBatch.Draw(Pixel, stars[x].destinationRectangle, stars[x].Color);
            }
        }
        public override void OnResize(object sender, EventArgs e)
        {
            base.OnResize(sender, e);
            stars = new Star[(DrawBounds.Height / CenterCell.Height) * (DrawBounds.Width / CenterCell.Width)];
            DrawBoundsChanged();
        }
        public override void Update(Vector2 screencenter)
        {
            base.Update(screencenter);
            for (int x = 0; x < stars.Length; x++)
            {
                stars[x].Flash();
            }
        }
        protected override void DrawBoundsChanged()
        {
            base.DrawBoundsChanged();
            int i = 0;
            for (int x = DrawBounds.X; x < DrawBounds.Right; x+=CenterCell.Width)
            {
                for (int y = DrawBounds.Y; y < DrawBounds.Bottom; y += CenterCell.Height)
                {
                    stars[i].Init(x, y);
                    i++;
                }
            }
        }
        public struct Star
        {
            private static int mask, maskbits;
            public Color Color;
            public Rectangle destinationRectangle;
            private uint hash, flash;
            public static int Mask { get => mask; set => mask = value; }
            public static int Maskbits { get => maskbits; set => maskbits = value; }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public void Init(int x, int y)
            {
                hash = 2498622551 * (uint)y;
                hash ^= RotateRight(hash, 21);
                hash += 3338943067 * (uint)x;
                hash ^= RotateRight(hash, 17);
                flash = hash * 3338943067;
                destinationRectangle.X = x + ((int)hash & mask); 
                destinationRectangle.Y = y + ((int)(hash >> maskbits) & mask);
                destinationRectangle.Width = destinationRectangle.Height = (int)((hash & 3) ) + 1;
                hash |= 0xFF3F3F3F;
            }
            public void Flash()
            {
                flash = RotateRight(flash, 3) + 7;
                if ((flash & 4) == 0)
                    Color.PackedValue = hash & 0xFF3F3F3F;
                else if ((flash & 2) == 0)
                    Color.PackedValue = hash & 0xFF7F7F7F;
                else if ((flash & 1) == 0)
                    Color.PackedValue = hash;
                else
                    Color.PackedValue = hash & 0xFFBFBFBF;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public static uint RotateRight(uint value, int count)
            {
                return (value >> count) | (value << (32 - count));
            }
        }
    }
}
