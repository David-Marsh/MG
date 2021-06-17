using Microsoft.Xna.Framework;
using System.Windows.Forms;

namespace MG.Shared.Global
{
    public static class GameHelper
    {
        public const float ScanTime = 1/60f;
        public static void FullScreen(this Game game, GraphicsDeviceManager graphics) => game.SetFormState(graphics, FormWindowState.Normal, true);
        public static void Maximize(this Game game, GraphicsDeviceManager graphics) => game.SetFormState(graphics, FormWindowState.Maximized);
        public static void Normalize(this Game game, GraphicsDeviceManager graphics) => game.SetFormState(graphics, FormWindowState.Normal);
        public static void Minimize(this Game game, GraphicsDeviceManager graphics) => game.SetFormState(graphics, FormWindowState.Minimized, graphics.IsFullScreen);
        private static void SetFormState(this Game game, GraphicsDeviceManager graphics, FormWindowState formWindowState, bool fullscreen = false)
        {
            graphics.HardwareModeSwitch = false;
            if (fullscreen != graphics.IsFullScreen) graphics.ToggleFullScreen();
            game.Window.AllowUserResizing = (formWindowState != FormWindowState.Normal);
            game.Window.IsBorderless = graphics.IsFullScreen;
            Form Form = (Form)Control.FromHandle(game.Window.Handle);
            Form.WindowState = formWindowState;
            Form.MaximizeBox = false;
            Form.MinimizeBox = false;
            Form.ControlBox = false;
            if (graphics.IsFullScreen)          // This is fullscreen, position the window
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
