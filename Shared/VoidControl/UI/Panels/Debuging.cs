using MG.Shared.UI.Controls;
using MG.Shared.UI.Panels;
using Microsoft.Xna.Framework;

namespace MG.Shared.VoidControl.UI.Panels
{
  public class Debuging : Panel
  {
    private readonly Button btnSpawn;
    private readonly Button btnClear;
    private readonly Label lblShipCount;
    readonly TuningPID PIDPanel;
    public Debuging(int x, int y, int width, int height, Color back = default) : base(x, y, width, height, back)
    {
      btnSpawn = new(0, 0, 3, 1, "Spawn", Colors.Back, Colors.Fore);
      btnClear = new(3, 0, 3, 1, "Clear", Colors.Back, Colors.Fore);
      lblShipCount = new(6, 0, 2, 1, "0", Colors.Back, Colors.Fore);
      PIDPanel = new(0, 1, 8, 3, Colors.Back);
      Children.Add(btnSpawn);
      Children.Add(btnClear);
      Children.Add(lblShipCount);
      Children.Add(PIDPanel);
      //btnSpawn.Clicked += new EventHandler(delegate (object o, EventArgs a) { EntityManager.Spawn(); PID.Visable = PIDPanel.Visible = true; });
      //btnClear.Clicked += new EventHandler(delegate (object o, EventArgs a) { EntityManager.Clear(); });
    }
    public override void Update(GameTime gameTime)
    {
      //lblShipCount.Msg.Text = EntityManager.ShipCount().ToString("Ships:000");
      //PIDPanel.Update(gameTime);
      base.Update(gameTime);
    }
  }
}
