using MG.Shared.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MG.Shared.Test.UI
{
  public class TestUI : Game
  {
    public TestUI()
    {
      _ = new GraphicsDeviceManager(this)
      {
        IsFullScreen = true,
        HardwareModeSwitch = false,
        GraphicsProfile = GraphicsProfile.HiDef
      };
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
      _ = new Scenes(this);
    }
    protected override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();
      Input.Update();
      base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.Black);
      base.Draw(gameTime);
    }
  }
}
