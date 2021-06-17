using MG.Shared.Global;
using MG.Shared.UI;
using MG.Shared.UI.Panels;
using Microsoft.Xna.Framework;
using System;

namespace MG.Shared.VoidControl.UI.Panels
{
    public class Debuging : Panel
    {
        private readonly Button btnSpawn;
        private readonly Button btnClear;
        private readonly Label lblShipCount;
        readonly TuningPID PIDPanel;

        public Debuging(Color back, Color fore, int col, int row, int colspan, int rowspan) : base(back, fore, col, row, colspan, rowspan)
        {
            Rows = 7;
            Collums = 10;
            lblShipCount = new(Colors.Back, Colors.Fore, "Ships:050", 6, 0, 4, 1);
            btnSpawn = new(Colors.Back, Color.White, "Spawn", 0, 0, 3, 1);
            btnClear = new(Colors.Back, Color.White, "Clear", 3, 0, 3, 1);
            Controls.Add(lblShipCount);
            Controls.Add(btnSpawn);
            Controls.Add(btnClear);
            //btnSpawn.Clicked += new EventHandler(delegate (object o, EventArgs a) { EntityManager.Spawn(); PID.Visable = PIDPanel.Visible = true; });
            //btnClear.Clicked += new EventHandler(delegate (object o, EventArgs a) { EntityManager.Clear(); });
            PIDPanel = new(Colors.Back, Colors.Fore, 0, 1, 10, 2) { Visible = false };
            Controls.Add(PIDPanel);
        }

        public override void Setup()
        {
            base.Setup();
            Padding = (int)Cellsize.X / 4;
            btnSpawn.Setup(this);
            btnClear.Setup(this);
            lblShipCount.Setup(this);
            PIDPanel.Setup(this);
        }
        public override void Update(GameTime gameTime)
        {
            //lblShipCount.Msg.Text = EntityManager.ShipCount().ToString("Ships:000");
            PIDPanel.Update(gameTime);
            base.Update(gameTime);
        }
    }
}
