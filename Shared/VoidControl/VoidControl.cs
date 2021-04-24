using MG.Shared.Global;
using MG.Shared.UI.Panels;
using Microsoft.Xna.Framework;

namespace MG.Shared.VoidControl
{
    public class VoidControl : Game
    {
        public GraphicsDeviceManager graphics;
        public VoidControl()
        {
            graphics = new GraphicsDeviceManager(this);
        }
        protected override void Initialize()
        {
            Content.RootDirectory = "Content";
            SceneManager.Initialize(this, graphics);
            base.Initialize();
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
