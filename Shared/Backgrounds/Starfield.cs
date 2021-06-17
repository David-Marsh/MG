using MG.Shared.Global.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.CompilerServices;

namespace MG.Shared.Backgrounds
{
    public class Starfield : Background
    {
        public class Star : BasicEntity
        {
            private uint hash, flash;
            private static int cellMask, cellSizeLog2;
            public Star()
            {
                texture = Pixel;
            }
            public static int CellSizeLog2
            {
                get => cellSizeLog2; set
                {
                    if (cellSizeLog2 == value) return;
                    cellSizeLog2 = value;
                    cellMask = ((1 << cellSizeLog2) - 1);
                }
            }
            public void Init(int x, int y)
            {
                hash = 2498622551 * (uint)y;
                hash ^= RotateRight(hash, 21);
                hash += 3338943067 * (uint)x;
                hash ^= RotateRight(hash, 17);
                flash = hash * 3338943067;
                destinationRectangle.X = x + (((int)hash & cellMask) << 2);
                destinationRectangle.Y = y + (((int)(hash >> cellSizeLog2) & cellMask) << 2);
                destinationRectangle.Width = destinationRectangle.Height = (int)((hash & 3)) + 1;
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
                color.PackedValue = (flash & 7) switch
                {
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
            public static uint RotateRight(uint value, int count) => (value >> count) | (value << (32 - count));
        }
        private int pass;
        private Star[] stars;
        public Starfield(Game game) : base(game) { }
        public override void Draw(SpriteBatch spriteBatch) { for (int x = stars.Length - 1; x >= 0; x--) stars[x].Draw(spriteBatch); }
        public int Count { get => (DrawBounds.Height / CenterCell.Height) * (DrawBounds.Width / CenterCell.Width); }
        protected override void DrawBoundsChanged()
        {
            base.DrawBoundsChanged();
            stars ??= Array.Empty<Star>();
            if (stars.Length != Count)
            {
                stars = new Star[(DrawBounds.Height / CenterCell.Height) * (DrawBounds.Width / CenterCell.Width)];
                for (int x = 0; x < stars.Length; x++)
                {
                    stars[x] = new();
                }
            }
            int i = 0;
            for (int x = DrawBounds.X; x < DrawBounds.Right; x += CenterCell.Width)
            {
                for (int y = DrawBounds.Y; y < DrawBounds.Bottom; y += CenterCell.Height)
                {
                    stars[i].Init(x, y);
                    i++;
                }
            }
        }
        protected override void LoadContent()
        {
            BasicEntity.CreateThePixel(GraphicsDevice);
            stars = Array.Empty<Star>();
            pass = 0;
            CellSizeLog2 = Star.CellSizeLog2 = 6;                                   // Cell size bits
            GraphicsDevice.DeviceReset += OnResize;
            base.LoadContent();
        }
        public override void Update(Point screencenter)
        {
            base.Update(screencenter);
            for (int x = pass; x < stars.Length; x += 4)
            {
                stars[x].Flash();
            }
            pass++;
            pass &= 3;
        }
    }
}
