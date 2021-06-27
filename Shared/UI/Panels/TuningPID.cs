using MG.Shared.Global;
using MG.Shared.UI.Controls;
using Microsoft.Xna.Framework;
using System;

namespace MG.Shared.UI.Panels
{
  public class TuningPID : Panel
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
    public TuningPID(int x, int y, int width, int height, Color back = default) : base(x, y, width, height, back)
    {
      lblKp = new(0, 0, 4, 1, "Kp:0.0000000", Colors.Back, Colors.Fore);
      btnKpDec = new(4, 0, 1, 1, ((char)0xF8AB).ToString(), Colors.Back, Color.White) { Menu = true, Delay = 150 };
      btnKpInc = new(5, 0, 1, 1, ((char)0xF8AA).ToString(), Colors.Back, Color.White) { Menu = true, Delay = 150 };
      lblP = new(6, 0, 2, 1, "P:+0.00", Colors.Back, Colors.Fore);
      Children.Add(lblKp);
      Children.Add(lblP);
      Children.Add(btnKpInc);
      Children.Add(btnKpDec);
      lblKi = new(0, 1, 4, 1, "Ki:0.0000000", Colors.Back, Colors.Fore);
      btnKiDec = new(4, 1, 1, 1, ((char)0xF8AB).ToString(), Colors.Back, Color.White) { Menu = true, Delay = 150 };
      btnKiInc = new(5, 1, 1, 1, ((char)0xF8AA).ToString(), Colors.Back, Color.White) { Menu = true, Delay = 150 };
      lblI = new(6, 1, 2, 1, "I:+0.00", Colors.Back, Colors.Fore);
      Children.Add(lblKi);
      Children.Add(lblI);
      Children.Add(btnKiInc);
      Children.Add(btnKiDec);
      lblKd = new(0, 2, 4, 1, "Kd:0.0000000", Colors.Back, Colors.Fore);
      btnKdDec = new(4, 2, 1, 1, ((char)0xF8AB).ToString(), Colors.Back, Color.White) { Menu = true, Delay = 150 };
      btnKdInc = new(5, 2, 1, 1, ((char)0xF8AA).ToString(), Colors.Back, Color.White) { Menu = true, Delay = 150 };
      lblD = new(6, 2, 2, 1, "D:+0.00", Colors.Back, Colors.Fore);
      Children.Add(lblKd);
      Children.Add(lblD);
      Children.Add(btnKdInc);
      Children.Add(btnKdDec);
      PID.Tuning = new(0, 0, 0);
      PID.Visable = true;
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
    public override void Update(GameTime gameTime)
    {
      Visable = PID.Visable;
      if (!visable) return;
      lblKp.Text = PID.Tuning.Kp.ToString("Kp:0.0000000");
      lblKi.Text = PID.Tuning.Ki.ToString("Ki:0.0000000");
      lblKd.Text = PID.Tuning.Kd.ToString("Kd:0.0000000");
      lblP.Text = PID.Tuning.P.Length().ToString("P:0.00");
      lblI.Text = PID.Tuning.I.Length().ToString("I:0.00");
      lblD.Text = PID.Tuning.D.Length().ToString("D:0.00");
      base.Update(gameTime);
    }
  }
}
