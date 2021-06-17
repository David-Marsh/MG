using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MG.Shared.VoidControl.Ship.SubSystems
{
    public class Cloak : SubSystem
    {
        public Cloak(float scale, float quality) : base(scale, quality)
        {
            Texture = Content.Load<Texture2D>("Art/VoidShip/Cloak");
        }
        public void Update(GameTime gameTime)
        {
            Color = Color.LimeGreen;
        }
    }
}
