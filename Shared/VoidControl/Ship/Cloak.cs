using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MG.VoidControl.Ship
{
    public class Cloak : ShipSystem
    {
        public Cloak(ContentManager content, float scale) : this(content, scale, 0) { }
        public Cloak(ContentManager content, float scale, float quality) : base(scale, quality)
        {
            Image = content.Load<Texture2D>("Art/VoidShip/Cloak");
        }
        public override void Update(GameTime gameTime)
        {
            Color = Color.LimeGreen;
        }
    }
}
