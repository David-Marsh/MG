using MG.Shared.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MG.Shared.Background
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
        private static Background background;
        private static Setting setting;
        public static Setting Setting
        {
            get => setting;
            set
            {
                if (setting == value) return;
                setting = value;
                background = value switch
                {
                    Setting.Starfield => new Starfield(graphics),
                    Setting.Grid => new Grid(graphics),
                    Setting.Default => new Grid(graphics),
                    Setting.Other => new Grid(graphics),
                    _ => new Grid(graphics),
                };
            }
        }
        public static void Initialize(GraphicsDeviceManager graphicsDeviceManager)
        {
            graphics = graphicsDeviceManager;
            graphics.DeviceReset += OnResize;
            Setting = Setting.Starfield;
        }
        public static void OnResize(Object sender, EventArgs e) => background.OnResize(sender, e);
        public static void Update(Vector2 screencenter)
        {
            if (Input.OneShotKey(Keys.B))
            {
                if (setting == Setting.Starfield) Setting = Setting.Grid;
                else Setting = Setting.Starfield;
            }
            background.Update(screencenter);
        }
        public static void Draw(SpriteBatch spriteBatch) => background.Draw(spriteBatch);
    }
}
