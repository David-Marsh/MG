using MG.Shared.Global;
using MG.Shared.UI.PrefabPanels;
using MG.VoidControl;
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
            Components.Add(new EntityManager(this, graphics));
            Components.Add(new UIManager(this, graphics));
            base.Initialize();
        }
        protected override void Update(GameTime gameTime)
        {
            DiagnosticsPanel.StartTime();
            Input.Update();                                                 // Enable oneshot keys
            base.Update(gameTime);
            DiagnosticsPanel.StopUpdateTime();
        }
        protected override void Draw(GameTime gameTime)
        {
            DiagnosticsPanel.StartTime();
            base.Draw(gameTime);
            DiagnosticsPanel.StopDrawTime();
        }
    }
}
