using MG.Shared.UI;
using MG.Shared.UI.Controls;
using MG.Shared.UI.Panels;
using Microsoft.Xna.Framework;
using System;

namespace MG.Shared.VoidControl.UI
{
  public class TitleUI : AppControl
  {
    private Panel titlePanel;
    private Button btnStartTop;
    private Button btnStartBot;
    public TitleUI(Game game) : base(game)
    {
    }
    protected override void LoadContent()
    {
      titlePanel = new(0, 0, 64, 36);
      btnStartTop = new(0, 0, 64, 18, "Void", Color.Black) { CanHover = false, Margin = Point.Zero };
      btnStartBot = new(0, 18, 64, 18, "Control", Color.Black) { CanHover = false, Margin = Point.Zero };
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
