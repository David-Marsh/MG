using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.CompilerServices;
using static MG.Shared.Global.Sprite;

namespace MG.Shared.Background
{
    public class Starfield : Background
    {
        private int pass;
        private Star[] stars;
        public Starfield(GraphicsDeviceManager graphics, int maskSize) : base(graphics, maskSize)
        {
            MaskBits = maskSize;                                   // Cell size bits
            pass = 0;
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
            for (int x = pass; x < stars.Length; x+=4)
            {
                stars[x].Flash();
            }
            pass++;
            pass &= 3;
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
                destinationRectangle.X = x + (((int)hash & mask) << 2); 
                destinationRectangle.Y = y + (((int)(hash >> maskbits) & mask) << 2);
                destinationRectangle.Width = destinationRectangle.Height = (int)((hash & 3) ) + 1;
                hash = (hash & 3) switch
                {
                    0 => 0xFFFFFFFF,
                    1 => hash | 0xFF7F7F7F,
                    2 => hash | 0xFF3F3F3F,
                    _ => hash | 0xFF1F1F1F,
                };
            }
            public void Flash()
            {
                flash = RotateRight(flash, 3) + 7;
                Color.PackedValue = (flash & 7) switch
                {
                    //0 => 0xFF1F1F1F,
                    //1 => 0xFF2F2F2F,
                    //2 => 0xFF3F3F3F,
                    //3 => 0xFF4F4F4F,
                    //4 => 0xFF5F5F5F,
                    //5 => 0xFF6F6F6F,
                    //6 => 0xFF7F7F7F,
                    //_ => 0xFF8F8F8F,
                    0 => hash & 0xFF1F1F1F,
                    1 => hash & 0xFF2F2F2F,
                    2 => hash & 0xFF3F3F3F,
                    3 => hash & 0xFF4F4F4F,
                    4 => hash & 0xFF5F5F5F,
                    5 => hash & 0xFF6F6F6F,
                    6 => hash & 0xFF7F7F7F,
                    _ => hash,
                };
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public static uint RotateRight(uint value, int count)
            {
                return (value >> count) | (value << (32 - count));
            }
        }
    }
}
