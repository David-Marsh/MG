using MG.Shared.Background;
using MG.Shared.Global;
using MG.Shared.VoidControl.Entities;
using MG.Shared.VoidControl.UI;
using MG.VoidControl;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MG.Shared.VoidControl
{
    public static class SceneManager
    {
        private static TitleEntities titleEntities;
        private static TitleUI titleUI;
        private static EntityManager entityManager;
        private static PlayUI playUI;
        public static void Initialize(Game game, GraphicsDeviceManager graphics)
        {
            Entity.Initialize(game.Content);
            Sound.LoadContent(game.Content);
            BackgroundManager.Initialize(graphics);
            titleEntities = new(game, graphics);
            titleUI = new(game, graphics);
            entityManager = new(game, graphics);
            playUI = new(game, graphics);
            game.Components.Add(titleEntities);
            game.Components.Add(entityManager);
            game.Components.Add(titleUI);
            game.Components.Add(playUI);
            game.IsMouseVisible = true;
            game.FullScreen(graphics);          // Set application to fullscreen
            Title();
        }
        public static void Title()
        {
            Input.MouseOnUI = false;
            titleEntities.Enabled = titleUI.Enabled = titleEntities.Visible = titleUI.Visible = true;
            entityManager.Enabled = playUI.Enabled = entityManager.Visible = playUI.Visible = false;
        }
        public static void Play()
        {
            Input.MouseOnUI = false;
            titleEntities.Enabled = titleUI.Enabled = titleEntities.Visible = titleUI.Visible = false;
            entityManager.Enabled = playUI.Enabled = entityManager.Visible = playUI.Visible = true;
            BackgroundManager.Setting = Setting.Starfield;
        }
    }
}
