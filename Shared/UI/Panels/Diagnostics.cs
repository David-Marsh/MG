using MG.Shared.Global;
using MG.Shared.UI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace MG.Shared.UI.Panels
{
  public class Diagnostics : Panel
  {
    private readonly Label lblDraw;
    private readonly Label lblUpdates;
    private readonly Label lblScan;

    private static readonly Stopwatch stopWatch = new();
    private static double tsUpdate;
    private static double tsDraw;

    public Diagnostics(int x, int y, int width, int height, Color back = default) : base(x, y, width, height, back)
    {
      lblDraw = new(0, 0, 4, 1, "Draw 0.00mS", Colors.Back, Colors.Fore);
      lblUpdates = new(0, 1, 4, 1, "Update 0.00mS", Colors.Back, Colors.Fore);
      lblScan = new(0, 2, 4, 1, (GameHelper.ScanTime * 1000).ToString("Scan 0.00mS"), Colors.Back, Colors.Fore);
      Children.Add(lblDraw);
      Children.Add(lblUpdates);
      Children.Add(lblScan);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      if (!Visable) return;
      base.Draw(spriteBatch);
      foreach (var child in Children) child.Draw(spriteBatch);
    }
    public static void Reset()
    {
      tsDraw = double.NaN;
      tsUpdate = double.NaN;
    }
    public static void StartTime() => stopWatch.Restart();
    public static void StopDrawTime()
    {
      if (double.IsNaN(tsDraw)) tsDraw = stopWatch.Elapsed.TotalSeconds * 1000;
      tsDraw = (tsDraw * 0.99f) + (0.01f * stopWatch.Elapsed.TotalSeconds * 1000);
    }
    public static void StopUpdateTime()
    {
      if (double.IsNaN(tsUpdate)) tsUpdate = stopWatch.Elapsed.TotalSeconds * 1000;
      tsUpdate = (tsUpdate * 0.99f) + (0.01f * stopWatch.Elapsed.TotalSeconds * 1000);
    }
    public override void Update(GameTime gameTime)
    {
      if (!Visable) return;
      base.Update(gameTime);
      lblUpdates.Text = tsUpdate.ToString("Update 0.00mS");
      lblDraw.Text = tsDraw.ToString("Draw 0.00mS");
    }
  }
}
