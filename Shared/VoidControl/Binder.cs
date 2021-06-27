using MG.Shared.Global;
using MG.Shared.VoidControl.Scenes;
using MG.Shared.VoidControl.UI;
using Microsoft.Xna.Framework;
using System;

namespace MG.Shared.VoidControl
{
  public class Binder : GameComponent
  {
    public enum Scene { Title = 0, Menu = 1, Cutscene01 = 2, Level01 = 3 }
    private readonly TitleScene titleScene;
    private readonly TitleUI titleUI;
    private readonly Level01Scene level01Scene;
    private readonly Level01UI level01UI;
    public Binder(Game game) : base(game)
    {
      titleScene = new(game);
      titleUI = new(game) { PauseGame = false };
      level01UI = new(game);
      level01Scene = new(game);
      Game.Components.Add(this);
      Game.Components.Add(titleScene);
      Game.Components.Add(titleUI);
      Game.Components.Add(level01Scene);
      Game.Components.Add(level01UI);
    }
    public void SwitchTo(Scene scene)
    {
      titleUI.Visible = titleUI.Enabled = scene == Scene.Title;
      titleScene.Enabled = titleScene.Visible = scene == Scene.Title;
      level01UI.Visible = level01UI.Enabled = (scene != Scene.Title) & (scene != Scene.Menu);
      level01Scene.Visible = level01Scene.Enabled = scene == Scene.Level01;
      if (level01Scene.Ships != null)
        level01UI.Ships = level01Scene.Ships;
    }
    public override void Initialize()
    {
      base.Initialize();
      Sound.LoadContent(Game.Content);
      titleUI.Pause += new EventHandler(delegate (object o, EventArgs a) { SwitchTo(Scene.Level01); });
      level01UI.Pause += new EventHandler(delegate (object o, EventArgs a) { SwitchTo(Scene.Title); });
      SwitchTo(Scene.Title);
    }
  }
}
