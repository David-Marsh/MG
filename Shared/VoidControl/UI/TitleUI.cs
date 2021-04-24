using MG.Shared.UI;
using MG.Shared.UI.Panels;
using Microsoft.Xna.Framework;
using System;

namespace MG.Shared.VoidControl.UI
{
    public class TitleUI : ControlManager
    {
        public Panel titlePanel;
        private readonly AppControl applicationPanel;
        private readonly Button btnStart;
        public TitleUI(Game game, GraphicsDeviceManager graphics) : base(game)
        {
            base.LoadContent();
            titlePanel = new(Color.Transparent, Color.White, 0, 0, 64, 36, 64, 36);
            btnStart = new(Color.Transparent, Color.Lerp(Color.Black, Color.White, 0.05f), "Void Control", 0, 0, 64, 36) { CanHover = false };
            applicationPanel = new(game, graphics, Color.Transparent, Color.White, 56, 0, 8, 1);
            applicationPanel.btnMenu.Visible = false;
            Controls.Add(titlePanel);
            titlePanel.Controls.Add(btnStart);
            titlePanel.Controls.Add(applicationPanel);
            graphics.DeviceReset += Setup;
            btnStart.MouseUp += new EventHandler(delegate (object o, EventArgs a)
            {
                SceneManager.Play();
            });
        }
        public override void Setup(object sender, EventArgs e)
        {
            titlePanel.Setup(((GraphicsDeviceManager)sender));
            btnStart.Setup(titlePanel);
            applicationPanel.Setup(titlePanel);
        }
    }
}
