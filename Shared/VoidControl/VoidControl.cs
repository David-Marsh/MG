using MG.Shared.Global;
using MG.Shared.UI.Panels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MG.Shared.VoidControl
{
    public class VoidControl : Game
    {
        public GraphicsDeviceManager graphics;
        public VoidControl()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = true,
                HardwareModeSwitch = false,
                GraphicsProfile = GraphicsProfile.HiDef
            };
        }
        protected override void Initialize()
        {
            Content.RootDirectory = "Content";
            SceneManager.Initialize(this, graphics);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            base.LoadContent();
        }
        protected override void Update(GameTime gameTime)
        {
            Diagnostics.StartTime();
            Input.Update();                                                 // Enable oneshot keys
            base.Update(gameTime);
            Diagnostics.StopUpdateTime();
        }
        protected override void Draw(GameTime gameTime)
        {
            Diagnostics.StartTime();
            base.Draw(gameTime);
            Diagnostics.StopDrawTime();
        }

    }
}
