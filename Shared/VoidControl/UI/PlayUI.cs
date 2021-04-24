using MG.Shared.UI;
using MG.Shared.UI.Panels;
using MG.Shared.VoidControl;
using MG.Shared.VoidControl.UI.Panels;
using Microsoft.Xna.Framework;
using System;

namespace MG.VoidControl
{
    public class PlayUI : ControlManager
    {
        private readonly Panel rightHUD;
        private readonly AppControl applicationPanel;
        private readonly ShipStatus shipStatus;
        private readonly Diagnostics diagnostics;
        private readonly MiniMap miniMap;
        private readonly Debuging debuging;
        public PlayUI(Game game, GraphicsDeviceManager graphics) : base(game)
        {
            base.LoadContent();
            rightHUD = new(Color.Transparent, Color.Gray, 56, 0, 8, 36, 8, 36);
            Controls.Add(rightHUD);
            applicationPanel = new(game, graphics, Color.Transparent, Color.Gray, 0, 0, 8, 1);
            applicationPanel.btnMenu.Clicked += new EventHandler(delegate (object o, EventArgs a)
            {
                miniMap.Visible = applicationPanel.MenuVisable;
                shipStatus.Visible = applicationPanel.MenuVisable;
                debuging.Visible = applicationPanel.MenuVisable;
                diagnostics.Visible = applicationPanel.MenuVisable;
            });
            applicationPanel.btnPause.Clicked += new EventHandler(delegate (object o, EventArgs a)
            {
                SceneManager.Title();
            });
            miniMap = new(Color.Transparent, Color.White, 0, 1, 8, 9);
            shipStatus = new(Color.Transparent, Color.Gray, 0, 10, 8, 5);
            debuging = new(Color.Transparent, Color.Gray, 0, 27, 8, 7);
            diagnostics = new(Color.Transparent, Color.Lime, 4, 34, 4, 2);
            rightHUD.Controls.Add(applicationPanel);
            rightHUD.Controls.Add(miniMap);
            rightHUD.Controls.Add(shipStatus);
            rightHUD.Controls.Add(debuging);
            rightHUD.Controls.Add(diagnostics);
            graphics.DeviceReset += Setup;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            miniMap.Draw(spriteBatch, GraphicsDevice);
        }

        public override void Setup(object sender, EventArgs e)
        {
            rightHUD.Setup(((GraphicsDeviceManager)sender), 64, 36);
            applicationPanel.Setup(rightHUD);
            miniMap.Setup(rightHUD, GraphicsDevice);
            shipStatus.Setup(rightHUD);
            debuging.Setup(rightHUD);
            diagnostics.Setup(rightHUD);
        }
    }
}
