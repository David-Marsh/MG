using MG.Shared.UI;
using MG.Shared.UI.Controls;
using Microsoft.Xna.Framework;
using System;

namespace MG.Shared.Test.UI
{
    public class TitleUI : AppUI
    {
        private UIPanel titlePanel;
        private UIButton btnStartTop;
        private UIButton btnStartBot;
        private UILabel lblAppButtons;
        private UILabel lblShowWhen;
        public TitleUI(Game game) : base(game)
        {
        }
        protected override void LoadContent()
        {
            titlePanel = new(0, 0, 64, 36, Color.Transparent);
            lblAppButtons = new(0, 1, 64, 1, "Play icon to launch/resume game. Volume icon to mute/unmute sound. Hambuger to access menu.", null, Color.Lime);
            lblShowWhen = new(0, 35, 64, 1, "This scene is shown when the game is launched. Automatically changes to cutscene after X seconds. Click background to start game.", null, Color.Lime);
            btnStartTop = new(0, 0, 64, 18, "UI Test") { CanHover = false, Margin = Point.Zero };
            btnStartBot = new(0, 18, 64, 18, "Title Scene") { CanHover = false, Margin = Point.Zero };
            titlePanel.Children.Add(lblAppButtons);
            titlePanel.Children.Add(lblShowWhen);
            titlePanel.Children.Add(btnStartTop);
            titlePanel.Children.Add(btnStartBot);
            Children.Add(titlePanel);
            btnStartTop.Clicked += new EventHandler(delegate (object o, EventArgs a)
            {
                PauseGame = false;
            });
            btnStartBot.Clicked += new EventHandler(delegate (object o, EventArgs a)
            {
                PauseGame = false;
            });
            titlePanel.SizeTo(64, 36, GraphicsDevice);
            base.LoadContent();
        }
        public override void ReSize(object sender, EventArgs e)
        {
            base.ReSize(sender, e);
            titlePanel.SizeTo(64, 36, (GraphicsDeviceManager)sender);
        }
    }
}
