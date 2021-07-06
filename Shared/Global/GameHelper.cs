using Microsoft.Xna.Framework;
using System.Windows.Forms;

namespace MG.Shared.Global
{
  public static class GameHelper
  {
    private static System.Drawing.Rectangle normalPosition;
    public const float ScanTime = 1 / 60f;
    public static void FullScreen(this Game game, GraphicsDeviceManager graphics) => game.SetFormState(graphics, FormWindowState.Normal, true);
    public static void Maximize(this Game game, GraphicsDeviceManager graphics) => game.SetFormState(graphics, FormWindowState.Maximized);
    public static void Normalize(this Game game, GraphicsDeviceManager graphics) => game.SetFormState(graphics, FormWindowState.Normal);
    public static void Minimize(this Game game, GraphicsDeviceManager graphics) => game.SetFormState(graphics, FormWindowState.Minimized, graphics.IsFullScreen);
    private static void SetFormState(this Game game, GraphicsDeviceManager graphics, FormWindowState formWindowState, bool fullscreen = false)
    {
      graphics.HardwareModeSwitch = false;
      Form Form = (Form)Control.FromHandle(game.Window.Handle);
      if (Form.WindowState == FormWindowState.Normal & !graphics.IsFullScreen) normalPosition = Form.Bounds;
      if (fullscreen != graphics.IsFullScreen) graphics.ToggleFullScreen();
      game.Window.AllowUserResizing = !fullscreen;
      game.Window.IsBorderless = fullscreen;
      Form.WindowState = formWindowState;
      Form.MaximizeBox = false;
      Form.MinimizeBox = false;
      Form.ControlBox = false;
      if (Form.WindowState == FormWindowState.Normal & !graphics.IsFullScreen)
      {
        if (normalPosition == System.Drawing.Rectangle.Empty) normalPosition = new(graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width / 4,
                                                                                   graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height / 4,
                                                                                   graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width / 2,
                                                                                   graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height / 2);
        Form.Bounds = normalPosition;
      }
      if (graphics.IsFullScreen)
      {
        Form.Location = new System.Drawing.Point(0, 0);
        game.Window.Position = Point.Zero;
      }
      graphics.ApplyChanges();
      game.SetBackBuffer(graphics);
    }
    private static void SetBackBuffer(this Game game, GraphicsDeviceManager graphics)
    {
      if (graphics.IsFullScreen)
      {
        if (graphics.PreferredBackBufferWidth != graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width
            || graphics.PreferredBackBufferHeight != graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height)
        {
          graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
          graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
          graphics.ApplyChanges();
        }
      }
      else
      {
        if (game.Window.ClientBounds.Width > 0
            && game.Window.ClientBounds.Height > 0
            && (graphics.PreferredBackBufferWidth != game.Window.ClientBounds.Width
            || graphics.PreferredBackBufferHeight != game.Window.ClientBounds.Height))
        {
          graphics.PreferredBackBufferWidth = game.Window.ClientBounds.Width;
          graphics.PreferredBackBufferHeight = game.Window.ClientBounds.Height;
          graphics.ApplyChanges();
        }
      }
    }
  }
}
