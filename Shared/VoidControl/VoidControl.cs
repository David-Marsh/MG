using MG.Shared.Global;
using MG.Shared.UI.Panels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MG.Shared.VoidControl
{
  public class VoidControl : Game
  {
    public GraphicsDeviceManager graphics;
    public VoidControl()
    {
      graphics = new GraphicsDeviceManager(this) { IsFullScreen = true, HardwareModeSwitch = false, GraphicsProfile = GraphicsProfile.HiDef };
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
      _ = new Binder(this);
    }
    protected override void Update(GameTime gameTime)
    {
      Diagnostics.StartTime();
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();
      Input.Update();                                                 // Enable oneshot keys
      base.Update(gameTime);
      Diagnostics.StopUpdateTime();
    }
    protected override void Draw(GameTime gameTime)
    {
      Diagnostics.StartTime();
      GraphicsDevice.Clear(Color.Black);
      base.Draw(gameTime);
      Diagnostics.StopDrawTime();
    }
  }
}
