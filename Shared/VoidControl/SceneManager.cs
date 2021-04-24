using MG.Shared.Global;
using MG.Shared.VoidControl.UI;
using MG.VoidControl;
using Microsoft.Xna.Framework;

namespace MG.Shared.VoidControl
{
    public static class SceneManager
    {
        private static EntityManager entityManager;
        private static TitleUI titleUI;
        private static PlayUI playUI;
        public static void Initialize(Game game, GraphicsDeviceManager graphics)
        {
            entityManager = new(game, graphics);
            titleUI = new(game, graphics);
            playUI = new(game, graphics);
            game.Components.Add(entityManager);
            game.Components.Add(titleUI);
            game.Components.Add(playUI);
            game.IsMouseVisible = true;
            game.FullScreen(graphics);          // Set application to fullscreen
            playUI.Update(null);                // First update laggs ???
            Title();
        }
        public static void Title()
        {
            Input.MouseOnUI = false;
            titleUI.Visible = true;
            playUI.Visible = false;
        }
        public static void Play()
        {
            Input.MouseOnUI = false;
            titleUI.Visible = false;
            playUI.Visible = true;
        }
    }
}
