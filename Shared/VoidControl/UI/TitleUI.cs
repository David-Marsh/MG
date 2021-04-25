using MG.Shared.UI;
using MG.Shared.UI.Panels;
using Microsoft.Xna.Framework;
using System;
using System.Runtime.CompilerServices;

namespace MG.Shared.VoidControl.UI
{
    public class TitleUI : ControlManager
    {
        public Panel titlePanel;
        private readonly AppControl appControl;
        private readonly Button btnStartTop;
        private readonly Button btnStartBot;
        public TitleUI(Game game, GraphicsDeviceManager graphics) : base(game)
        {
            base.LoadContent();
            titlePanel = new(Color.Transparent, Color.White, 0, 0, 64, 36, 64, 36);
            btnStartTop = new(Color.Transparent, Color.Black, "Void", 0, 0, 64, 18, false);
            btnStartBot = new(Color.Transparent, Color.Black, "Control", 0, 18, 64, 18, false);
            appControl = new(game, graphics, Color.Transparent, Color.White, 56, 0, 8, 1);
            appControl.btnMenu.Visible = false;
            appControl.PauseGame = false;
            Controls.Add(titlePanel);
            titlePanel.Controls.Add(btnStartTop);
            titlePanel.Controls.Add(btnStartBot);
            titlePanel.Controls.Add(appControl);
            graphics.DeviceReset += Setup;
            btnStartTop.MouseUp += new EventHandler(delegate (object o, EventArgs a)
            {
                SceneManager.Play();
            });
            btnStartBot.MouseUp += new EventHandler(delegate (object o, EventArgs a)
            {
                SceneManager.Play();
            });
            appControl.btnPause.Clicked += new EventHandler(delegate (object o, EventArgs a)
            {
                SceneManager.Play();
                appControl.PauseGame = false;
            });
        }
        public override void Setup(object sender, EventArgs e)
        {
            titlePanel.Setup(((GraphicsDeviceManager)sender));
            btnStartTop.Setup(titlePanel);
            btnStartBot.Setup(titlePanel);
            appControl.Setup(titlePanel);
        }
        public override void Update(GameTime gameTime)
        {
            btnStartTop.Fore(Color.Lerp(Color.Transparent, Color.Black, TwoSecondTriangleWave(gameTime.TotalGameTime)));
            btnStartBot.Fore(Color.Lerp(Color.Transparent, Color.Black, TwoSecondTriangleWave(gameTime.TotalGameTime)));
            base.Update(gameTime);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float TwoSecondTriangleWave(TimeSpan timeSpan) => Math.Abs(-(timeSpan.Seconds & 1) + timeSpan.Milliseconds * 0.001f);
    }
}
