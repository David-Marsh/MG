using MG.Shared.UI.Panels;
using Microsoft.Xna.Framework;
using System;

namespace MG.Shared.Test.UI
{
  public class Scenes : GameComponent
  {
    public enum Scene { Title = 0, Menu = 1, Cutscene = 2, Level01 = 3 }
    public TitleUI titleUI;
    public AppControl appUI;
    public Scenes(Game game) : base(game)
    {
      titleUI = new(game) { PauseGame = false };
      appUI = new(game);
      game.Components.Add(titleUI);
      game.Components.Add(appUI);
      game.Components.Add(this);
      SwitchTo(Scene.Title);
    }
    public void SwitchTo(Scene scene)
    {
      titleUI.Visible = titleUI.Enabled = scene == Scene.Title;
      appUI.Visible = appUI.Enabled = (scene != Scene.Title) & (scene != Scene.Menu);
    }
    public override void Initialize()
    {
      base.Initialize();
      titleUI.Pause += new EventHandler(delegate (object o, EventArgs a) { SwitchTo(Scene.Level01); });
      appUI.Pause += new EventHandler(delegate (object o, EventArgs a) { SwitchTo(Scene.Title); });
    }
  }
}
