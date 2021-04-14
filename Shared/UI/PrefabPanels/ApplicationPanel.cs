using MG.Shared.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace MG.Shared.UI.PrefabPanels
{
    public class ApplicationPanel : Panel
    {
        private readonly Button btnPause;
        private readonly Button btnMute;
        public readonly Button btnMenu;
        private readonly Button btnMinimize;
        private readonly Button btnNormalize;
        private readonly Button btnMaximize;
        private readonly Button btnFullScreen;
        private readonly Button btnExit;

        public bool PauseGame;
        public bool MenuVisable = true;

        public ApplicationPanel(Game game, GraphicsDeviceManager graphics, Color back, Color fore, int col, int row, int colspan, int rowspan) : base(back, fore, col, row, colspan, rowspan)
        {
            Rows = 1;
            Collums = 8;
            btnPause = new(Colors.Back, Color.Blue, ((char)0xEDB4).ToString(), 0, 0, 1, 1) { MenuButton = true };
            btnMute = new(Colors.Back, Color.Cyan, ((char)0xE995).ToString(), 1, 0, 1, 1) { MenuButton = true };
            btnMenu = new(Colors.Back, Color.Lime, ((char)0xE700).ToString(), 2, 0, 1, 1) { MenuButton = true };
            btnMinimize = new(Colors.Back, Color.Yellow, ((char)0xE921).ToString(), 3, 0, 1, 1) { MenuButton = true };
            btnNormalize = new(Colors.Back, Color.Yellow, ((char)0xE923).ToString(), 4, 0, 1, 1) { MenuButton = true };
            btnMaximize = new(Colors.Back, Color.Yellow, ((char)0xE922).ToString(), 5, 0, 1, 1) { MenuButton = true };
            btnFullScreen = new(Colors.Back, Color.Yellow, ((char)0xE92D).ToString(), 6, 0, 1, 1) { MenuButton = true };
            btnExit = new(Colors.Back, Color.Red, ((char)0xE8BB).ToString(), 7, 0, 1, 1) { MenuButton = true };

            Controls.Add(btnPause);
            Controls.Add(btnMute);
            Controls.Add(btnMenu);
            Controls.Add(btnMinimize);
            Controls.Add(btnNormalize);
            Controls.Add(btnMaximize);
            Controls.Add(btnFullScreen);
            Controls.Add(btnExit);

            btnPause.Clicked += new EventHandler(delegate (object o, EventArgs a)
            {
                PauseGame = !PauseGame;
                btnPause.Msg.Text = PauseGame ? ((char)0xEDB4).ToString() : ((char)0xEDB5).ToString();
            });
            btnMute.Clicked += new EventHandler(delegate (object o, EventArgs a)
            {
                Sound.Mute = !Sound.Mute;
                btnMute.Msg.Text = Sound.Mute ? ((char)0xE198).ToString() : ((char)0xE995).ToString();
            });
            btnMenu.Clicked += new EventHandler(delegate (object o, EventArgs a)
            {
                MenuVisable = !MenuVisable;
            });
            btnMinimize.Clicked += new EventHandler(delegate (object o, EventArgs a)
            {
                game.Minimize(graphics);
            });
            btnNormalize.Clicked += new EventHandler(delegate (object o, EventArgs a)
            {
                game.Normalize(graphics);
            });
            btnMaximize.Clicked += new EventHandler(delegate (object o, EventArgs a)
            {
                game.Maximize(graphics);
            });
            btnFullScreen.Clicked += new EventHandler(delegate (object o, EventArgs a)
            {
                game.FullScreen(graphics);
            });
            btnExit.Clicked += new EventHandler(delegate (object o, EventArgs a)
            {
                game.Exit();
            });
        }

        public override void Setup()
        {
            base.Setup();
            Padding = (int)Cellsize.X / 8;
            btnPause.Setup(this);
            btnMute.Setup(this);
            btnMenu.Setup(this);
            btnMinimize.Setup(this);
            btnNormalize.Setup(this);
            btnMaximize.Setup(this);
            btnFullScreen.Setup(this);
            btnExit.Setup(this);
        }
        public override void Update(GameTime gameTime)
        {
            if (Input.OneShotKey(Keys.Tab)) btnMaximize.OnClicked();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) btnExit.OnClicked();
        }
    }
}
