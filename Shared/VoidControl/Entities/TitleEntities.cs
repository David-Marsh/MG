using MG.Shared.Background;
using MG.Shared.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MG.Shared.VoidControl.Entities
{
    public class TitleEntities : DrawableGameComponent
    {
        private Color Color;
        private Matrix OriginMatrix, ViewMatrix;
        private Vector2 Position;
        private readonly SpriteBatch spriteBatch;
        private static ParticleManager particleManager;
        private Vector2 start, end;
        public TitleEntities(Game game, GraphicsDeviceManager graphics) : base(game)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            particleManager = new();
            graphics.DeviceReset += OnResize;
            OnResize(graphics, null);
            Color = Color.Lerp(Color.Transparent, Color.White, 0.02f);
        }
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color);
            spriteBatch.Begin(transformMatrix: ViewMatrix);                     // Map the world space around the position to the full screen
            BackgroundManager.Draw(spriteBatch);                                // Draw background below entitys first
            particleManager.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        public void OnResize(object sender, EventArgs e) => OriginMatrix = Matrix.CreateTranslation(new Vector3(new Point(((GraphicsDeviceManager)sender).PreferredBackBufferWidth / 2, ((GraphicsDeviceManager)sender).PreferredBackBufferHeight / 2).ToVector2(), 0.0f));
        public override void Update(GameTime gameTime)
        {
            start.X = IntToFloatHash(gameTime.TotalGameTime.Seconds) * 10;
            start.Y = IntToFloatHash(gameTime.TotalGameTime.Seconds + 10) * 10;
            end.X = IntToFloatHash(gameTime.TotalGameTime.Seconds + 1) * 10;
            end.Y = IntToFloatHash(gameTime.TotalGameTime.Seconds + 11) * 10;
            Position += Vector2.Lerp(start, end, gameTime.TotalGameTime.Milliseconds * 0.001f);
            ViewMatrix = Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) * OriginMatrix;
            BackgroundManager.Setting = (gameTime.TotalGameTime.Seconds & 8) == 0 ? Setting.Starfield : Setting.Grid;
            BackgroundManager.Update(Position);
            particleManager.Update();
            base.Update(gameTime);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float IntToFloatHash(int x)
        {
            int n = 1337;
            n ^= 1619 * x;
            return (n * n * n * 60493) / (float)2147483648.0;
        }
    }
}
