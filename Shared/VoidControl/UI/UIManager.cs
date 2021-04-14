using MG.Shared.Global;
using MG.Shared.UI;
using MG.VoidControl.UI;
using Microsoft.Xna.Framework;
using System;

namespace MG.VoidControl
{
    public class UIManager : ControlManager
    {
        private readonly TrayRight trayRight;
        public UIManager(Game game, GraphicsDeviceManager graphics) : base(game)
        {
            base.LoadContent();
            Game.IsMouseVisible = true;
            Game.FullScreen(graphics);                                                // Set application to fullscreen
            trayRight = new(Game, graphics, Color.Transparent, Color.Gray, 56, 0, 8, 36);
            Controls.Add(trayRight);
            graphics.DeviceReset += Setup;
        }
            
        public override void Setup(object sender, EventArgs e) => trayRight.Setup(((GraphicsDeviceManager)sender));
    }
}
