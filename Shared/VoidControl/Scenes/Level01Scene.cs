using MG.Shared.Backgrounds;
using MG.Shared.Global;
using MG.Shared.Global.Entities;
using MG.Shared.VoidControl.Ship;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MG.Shared.VoidControl.Scenes
{
  public class Level01Scene : Starfield
  {
    #region Entitys
    public VoidShipPC Player;
    public Ships Ships { get; set; }
    public Bullets Bullets { get; set; }
    #endregion

    private SpriteBatch spriteBatch;
    private Color color;
    private readonly Particles particles;
    public Level01Scene(Game game) : base(game)
    {
      particles = new();
      color = Color.Lerp(Color.Black, Color.White, 0.05f);
    }
    public override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(color);
      spriteBatch.Begin(transformMatrix: Player.ViewMatrix);              // Map the world space around the position to the full screen
      base.Draw(spriteBatch);                                             // Draw background below entitys first
      Ships.Draw(spriteBatch);
      Bullets.Draw(spriteBatch);
      particles.Draw(spriteBatch);
      spriteBatch.End();
      base.Draw(gameTime);
    }
    protected override void LoadContent()
    {
      base.LoadContent();
      spriteBatch = new SpriteBatch(GraphicsDevice);
      FullEntity.Initialize(Game.Content);
      FullEntity.CreateThePixel(GraphicsDevice);
      Ships = new();
      Player = Ships.Player;
      Player.OnResize(GraphicsDevice);
      GraphicsDevice.DeviceReset += Player.OnResize;
      Bullets = new();
    }
    public override void Update(GameTime gameTime)
    {
      particles.Update();
      Bullets.Update();
      Ships.Update(gameTime, Bullets, particles);
      Update(Player.Position.ToPoint());
      base.Update(gameTime);
    }
  }
}
