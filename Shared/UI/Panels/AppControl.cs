using MG.Shared.Global;
using MG.Shared.UI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.Shared.UI.Panels
{
  public class AppControl : UIManager
  {
    private Panel appControl;
    private Button btnPause;
    private Button btnMute;
    private Button btnMenu;
    private Button btnMinimize;
    private Button btnNormalize;
    private Button btnMaximize;
    private Button btnFullScreen;
    private Button btnExit;
    private bool pauseGame = true;
    public bool PauseGame
    {
      get => pauseGame; set
      {
        pauseGame = value;
        if (!(btnPause is null)) btnPause.Text = pauseGame ? ((char)0xEDB4).ToString() : ((char)0xEDB5).ToString();
        Pause?.Invoke(this, new EventArgs());
      }
    }
    public bool MenuVisable { get; set; } = true;
    public event EventHandler Menu;
    public event EventHandler Pause;
    public AppControl(Game game) : base(game)
    {
    }
    public virtual void ReSize(object sender, EventArgs e)
    {
      appControl.SizeTo(64, 36, (GraphicsDeviceManager)sender);
    }
    protected override void LoadContent()
    {
      appControl = new Panel(56, 0, 8, 1, Color.Transparent);
      btnPause = new(0, 0, 1, 1, ((char)0xEDB4).ToString(), Color.Transparent, Color.Blue, true);
      PauseGame = pauseGame;
      btnMute = new(1, 0, 1, 1, ((char)0xE995).ToString(), Color.Transparent, Color.Cyan, true);
      btnMenu = new(2, 0, 1, 1, ((char)0xE700).ToString(), Color.Transparent, Color.Lime, true);
      btnMinimize = new(3, 0, 1, 1, ((char)0xE921).ToString(), Color.Transparent, Color.Yellow, true);
      btnNormalize = new(4, 0, 1, 1, ((char)0xE923).ToString(), Color.Transparent, Color.Yellow, true);
      btnMaximize = new(5, 0, 1, 1, ((char)0xE922).ToString(), Color.Transparent, Color.Yellow, true);
      btnFullScreen = new(6, 0, 1, 1, ((char)0xE92D).ToString(), Color.Transparent, Color.Yellow, true);
      btnExit = new(7, 0, 1, 1, ((char)0xE8BB).ToString(), Color.Transparent, Color.Red, true);
      appControl.Children.Add(btnPause);
      appControl.Children.Add(btnMute);
      appControl.Children.Add(btnMenu);
      appControl.Children.Add(btnMinimize);
      appControl.Children.Add(btnNormalize);
      appControl.Children.Add(btnMaximize);
      appControl.Children.Add(btnFullScreen);
      appControl.Children.Add(btnExit);
      Children.Add(appControl);
      ((GraphicsDeviceManager)Game.Services.GetService<IGraphicsDeviceService>()).DeviceReset += ReSize;
      btnPause.Clicked += new EventHandler(delegate (object o, EventArgs a)
      {
        PauseGame = pauseGame;
      });
      btnMute.Clicked += new EventHandler(delegate (object o, EventArgs a)
      {
        Sound.Mute = !Sound.Mute;
        btnMute.Text = Sound.Mute ? ((char)0xE198).ToString() : ((char)0xE995).ToString();
      });
      btnMenu.Clicked += new EventHandler(delegate (object o, EventArgs a)
      {
        MenuVisable = !MenuVisable;
        Menu?.Invoke(this, new EventArgs());
      });
      btnMinimize.Clicked += new EventHandler(delegate (object o, EventArgs a)
      {
        Game.Minimize((GraphicsDeviceManager)Game.Services.GetService<IGraphicsDeviceService>());
      });
      btnNormalize.Clicked += new EventHandler(delegate (object o, EventArgs a)
      {
        Game.Normalize((GraphicsDeviceManager)Game.Services.GetService<IGraphicsDeviceService>());
      });
      btnMaximize.Clicked += new EventHandler(delegate (object o, EventArgs a)
      {
        Game.Maximize((GraphicsDeviceManager)Game.Services.GetService<IGraphicsDeviceService>());
      });
      btnFullScreen.Clicked += new EventHandler(delegate (object o, EventArgs a)
      {
        Game.FullScreen((GraphicsDeviceManager)Game.Services.GetService<IGraphicsDeviceService>());
      });
      btnExit.Clicked += new EventHandler(delegate (object o, EventArgs a)
      {
        Game.Exit();
      });
      appControl.SizeTo(64, 36, GraphicsDevice);
      base.LoadContent();
    }
  }
}
