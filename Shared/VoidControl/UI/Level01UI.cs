using MG.Shared.UI.Panels;
using MG.Shared.VoidControl.Ship;
using MG.Shared.VoidControl.UI.Panels;
using Microsoft.Xna.Framework;
using System;

namespace MG.Shared.VoidControl.UI
{
  public class Level01UI : AppControl
  {
    private ShipStatus shipStatus;
    private MiniMap miniMap;
    private Debuging debuging;
    private Diagnostics uIDiagnostics;

    public Ships Ships
    {
      get => miniMap.Ships; set
      {
        if (!(miniMap is null))
        {
          miniMap.Ships = value;
          miniMap.Player = value.Player;
        }
        if (!(shipStatus is null))
          shipStatus.Player = value.Player;
      }
    }
    public Level01UI(Game game) : base(game) { }
    public override void Draw(GameTime gameTime)
    {
      base.Draw(gameTime);
      miniMap.Draw(spriteBatch, GraphicsDevice);
    }
    public override void ReSize(object sender, EventArgs e)
    {
      base.ReSize(sender, e);
      miniMap.SizeTo(64, 36, (GraphicsDeviceManager)sender);
      shipStatus.SizeTo(64, 36, (GraphicsDeviceManager)sender);
      debuging.SizeTo(64, 36, (GraphicsDeviceManager)sender);
      uIDiagnostics.SizeTo(64, 54, (GraphicsDeviceManager)sender);
    }
    protected override void LoadContent()
    {
      miniMap = new(56, 1, 8, 9, Color.Transparent);
      shipStatus = new(56, 10, 8, 7, Color.Transparent);
      debuging = new(56, 18, 8, 4, Color.Transparent);
      uIDiagnostics = new(60, 51, 4, 3, Color.Transparent);
      miniMap.SizeTo(64, 36, GraphicsDevice);
      shipStatus.SizeTo(64, 36, GraphicsDevice);
      debuging.SizeTo(64, 36, GraphicsDevice);
      uIDiagnostics.SizeTo(64, 54, GraphicsDevice);
      Children.Add(miniMap);
      Children.Add(shipStatus);
      Children.Add(debuging);
      Children.Add(uIDiagnostics);
      base.LoadContent();
    }
    protected override void OnEnabledChanged(object sender, EventArgs args)
    {
      Diagnostics.Reset();
      base.OnEnabledChanged(sender, args);
    }
  }
}
