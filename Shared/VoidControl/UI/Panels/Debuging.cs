using MG.Shared.Global;
using MG.Shared.UI;
using MG.Shared.UI.Controls;
using MG.Shared.UI.Panels;
using MG.Shared.VoidControl.Ship;
using Microsoft.Xna.Framework;
using System;

namespace MG.Shared.VoidControl.UI.Panels
{
  public class Debuging : Panel
  {
    private readonly Button btnSpawn;
    private readonly Button btnClear;
    private readonly Label lblVisable;
    private readonly Label lblEnabled;
    private readonly Label lblTotal;
    readonly TuningPID PIDPanel;
    public Ships Ships;
    public Debuging(int x, int y, int width, int height) : base(x, y, width, height)
    {
      btnSpawn = new(0, 0, 3, 1, "Spawn", Colors);
      btnClear = new(3, 0, 3, 1, "Clear", Colors);
      lblVisable = new(0, 1, 2, 1, "Visable:000", Colors);
      lblEnabled = new(3, 1, 2, 1, "Enabled:000", Colors);
      lblTotal = new(6, 1, 2, 1, "Total:000", Colors);
      PIDPanel = new(0, 2, 8, 3);
      Children.Add(btnSpawn);
      Children.Add(btnClear);
      Children.Add(lblVisable);
      Children.Add(lblEnabled);
      Children.Add(lblTotal);
      Children.Add(PIDPanel);
      btnSpawn.Clicked += new EventHandler(delegate (object o, EventArgs a) { Ships.SpawnClick(); PID.Visable = PIDPanel.Visable = true; });
      btnClear.Clicked += new EventHandler(delegate (object o, EventArgs a) { Ships.ClearClick(); PID.Visable = PIDPanel.Visable = false; });
    }
    public override void Update(GameTime gameTime)
    {
      lblVisable.Text = Ships.Visable.ToString("Visable:000");
      lblEnabled.Text = Ships.Enabled.ToString("Enabled:000");
      lblTotal.Text = Ships.Total.ToString("Total:000");
      //PIDPanel.Update(gameTime);
      base.Update(gameTime);
    }
  }
}
