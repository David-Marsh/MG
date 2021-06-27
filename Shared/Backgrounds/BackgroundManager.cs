using MG.Shared.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MG.Shared.Backgrounds
{
  public enum Setting
  {
    Default = 0,
    Starfield = 1,
    Grid = 2,
    Other = 200
  }
  public static class BackgroundManager
  {
    private static GraphicsDeviceManager graphics;
    private static Background_Old background;
    private static Setting setting;
    private static readonly int maskSize = 7;
    public static Setting Setting
    {
      get => setting;
      set
      {
        if (setting == value) return;
        setting = value;
        background = value switch
        {
          Setting.Starfield => new Starfield_Old(graphics, maskSize),
          Setting.Grid => new Grid(graphics, maskSize),
          Setting.Default => new Grid(graphics, maskSize),
          Setting.Other => new Grid(graphics, maskSize),
          _ => new Grid(graphics, maskSize),
        };
      }
    }
    public static void Initialize(GraphicsDeviceManager graphicsDeviceManager)
    {
      graphics = graphicsDeviceManager;
      graphics.DeviceReset += OnResize;
      Setting = Setting.Starfield;
    }
    public static void OnResize(object sender, EventArgs e) => background.OnResize(sender, e);
    public static void OnResize(GraphicsDevice graphicsDevice) => background.OnResize(graphicsDevice);
    public static void Update(Vector2 screencenter)
    {
      if (Input.OneShotKey(Keys.B))
      {
        Setting = setting == Setting.Starfield ? Setting.Grid : Setting.Starfield;
      }
      background.Update(screencenter);
    }
    public static void Draw(SpriteBatch spriteBatch) => background.Draw(spriteBatch);
  }
}
