using MG.Shared.Global;
using Microsoft.Xna.Framework;
using System;

namespace MG.Shared.UI.PrefabPanels
{
    public class PIDPanel : Panel
    {
        private readonly Button btnKpInc;
        private readonly Button btnKpDec;
        private readonly Label lblKp;
        private readonly Label lblP;
        private readonly Button btnKiInc;
        private readonly Button btnKiDec;
        private readonly Label lblKi;
        private readonly Label lblI;
        private readonly Button btnKdInc;
        private readonly Button btnKdDec;
        private readonly Label lblKd;
        private readonly Label lblD;

        public PIDPanel(Color back, Color fore, int col, int row, int colspan, int rowspan) : base(back, fore, col, row, colspan, rowspan)
        {
            Rows = 3;
            Collums = 10;

            lblKp = new(Colors.Back, Colors.Fore, "Kp:0.0000000", 0, 0, 5, 1) { CanHover = false };
            btnKpDec = new(Colors.Back, Color.White, ((char)0xF8AB).ToString(), 5, 0, 1, 1) { MenuButton = true, Delay = 150 };
            btnKpInc = new(Colors.Back, Color.White, ((char)0xF8AA).ToString(), 6, 0, 1, 1) { MenuButton = true, Delay = 150 };
            lblP = new(Colors.Back, Colors.Fore, "P:0.0000", 7, 0, 3, 1) { CanHover = false };
            Controls.Add(lblKp);
            Controls.Add(lblP);
            Controls.Add(btnKpInc);
            Controls.Add(btnKpDec);
            lblKi = new(Colors.Back, Colors.Fore, "Ki:0.0000000", 0, 1, 5, 1) { CanHover = false };
            btnKiDec = new(Colors.Back, Color.White, ((char)0xF8AB).ToString(), 5, 1, 1, 1) { MenuButton = true, Delay = 150 };
            btnKiInc = new(Colors.Back, Color.White, ((char)0xF8AA).ToString(), 6, 1, 1, 1) { MenuButton = true, Delay = 150 };
            lblI = new(Colors.Back, Colors.Fore, "I:0.0000", 7, 1, 3, 1) { CanHover = false };
            Controls.Add(lblKi);
            Controls.Add(lblI);
            Controls.Add(btnKiInc);
            Controls.Add(btnKiDec);
            lblKd = new(Colors.Back, Colors.Fore, "Kd:0.0000000", 0, 2, 5, 1) { CanHover = false };
            btnKdDec = new(Colors.Back, Color.White, ((char)0xF8AB).ToString(), 5, 2, 1, 1) { MenuButton = true, Delay = 150 };
            btnKdInc = new(Colors.Back, Color.White, ((char)0xF8AA).ToString(), 6, 2, 1, 1) { MenuButton = true, Delay = 150 };
            lblD = new(Colors.Back, Colors.Fore, "D:0.0000", 7, 2, 3, 1) { CanHover = false };
            Controls.Add(lblKd);
            Controls.Add(lblD);
            Controls.Add(btnKdInc);
            Controls.Add(btnKdDec);

            btnKpInc.Clicked += new EventHandler(delegate (object o, EventArgs a)
            {
                PID.Tuning.Kp += 0.0001f;
            });
            btnKpDec.Clicked += new EventHandler(delegate (object o, EventArgs a)
            {
                PID.Tuning.Kp -= 0.0001f;
            });
            btnKiInc.Clicked += new EventHandler(delegate (object o, EventArgs a)
            {
                PID.Tuning.Ki += 0.0000001f;
            });
            btnKiDec.Clicked += new EventHandler(delegate (object o, EventArgs a)
            {
                PID.Tuning.Ki -= 0.0000001f;
            });
            btnKdInc.Clicked += new EventHandler(delegate (object o, EventArgs a)
            {
                PID.Tuning.Kd += 0.01f;
            });
            btnKdDec.Clicked += new EventHandler(delegate (object o, EventArgs a)
            {
                PID.Tuning.Kd -= 0.01f;
            });
        }

        public override void Setup()
        {
            base.Setup();
            Padding = (int)Cellsize.X / 10;

            lblKp.Setup(this);
            btnKpDec.Setup(this);
            btnKpInc.Setup(this);
            lblP.Setup(this);

            lblKi.Setup(this);
            btnKiDec.Setup(this);
            btnKiInc.Setup(this);
            lblI.Setup(this);

            lblKd.Setup(this);
            btnKdDec.Setup(this);
            btnKdInc.Setup(this);
            lblD.Setup(this);
        }
        public override void Update(GameTime gameTime)
        {
            lblKp.Msg.Text = PID.Tuning.Kp.ToString("Kp:0.0000000");
            lblKi.Msg.Text = PID.Tuning.Ki.ToString("Ki:0.0000000");
            lblKd.Msg.Text = PID.Tuning.Kd.ToString("Kd:0.0000000");
            lblP.Msg.Text = PID.Tuning.P.Length().ToString("P:0.00");
            lblI.Msg.Text = PID.Tuning.I.Length().ToString("I:0.00");
            lblD.Msg.Text = PID.Tuning.D.Length().ToString("D:0.00");
            base.Update(gameTime);
        }
    }
}
