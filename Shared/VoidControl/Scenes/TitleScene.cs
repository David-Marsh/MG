using MG.Shared.Backgrounds;
using MG.Shared.Global;
using MG.Shared.Global.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.CompilerServices;

namespace MG.Shared.VoidControl.Scenes
{
  public class TitleScene : Starfield
  {
    private SpriteBatch spriteBatch;
    private Color Color;
    private Matrix OriginMatrix, ViewMatrix;
    private Vector2 Position;
    private readonly Particles particleManager;
    private Vector2 start, end;
    public TitleScene(Game game) : base(game)
    {
      particleManager = new();
      Color = Color.Lerp(Color.Black, Color.White, 0.05f);
    }
    public override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color);
      spriteBatch.Begin(transformMatrix: ViewMatrix);                     // Map the world space around the position to the full screen
      Draw(spriteBatch);                                                  // Draw background below entitys first
      particleManager.Draw(spriteBatch);
      spriteBatch.End();
      base.Draw(gameTime);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float IntToFloatHash(int x)
    {
      int n = 1337;
      n ^= 1619 * x;
      return n * n * n * 60493 / (float)2147483648.0;
    }
    protected override void LoadContent()
    {
      base.LoadContent();
      spriteBatch = new SpriteBatch(GraphicsDevice);
      FullEntity.Initialize(Game.Content);
      FullEntity.CreateThePixel(GraphicsDevice);
    }
    public override void OnResize(object sender, EventArgs e)
    {
      base.OnResize(sender, e);
      OriginMatrix = Matrix.CreateTranslation(new Vector3(new Point(((GraphicsDevice)sender).Viewport.Width / 2, ((GraphicsDevice)sender).Viewport.Height / 2).ToVector2(), 0.0f));
    }
    public override void Update(GameTime gameTime)
    {
      int speed = 2;
      start.X = IntToFloatHash(gameTime.TotalGameTime.Seconds) * speed;
      start.Y = IntToFloatHash(gameTime.TotalGameTime.Seconds + 10) * speed;
      end.X = IntToFloatHash(gameTime.TotalGameTime.Seconds + 1) * speed;
      end.Y = IntToFloatHash(gameTime.TotalGameTime.Seconds + 11) * speed;
      Position += Vector2.Lerp(start, end, gameTime.TotalGameTime.Milliseconds * 0.001f);
      ViewMatrix = Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) * OriginMatrix;
      particleManager.Update();
      Update(Position.ToPoint());
      base.Update(gameTime);
    }
  }
}
