using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MG.VoidControl.Ship
{
    public class Weapons : ShipSystem
    {
        public Weapons(ContentManager content, float scale) : this(content, scale, 0) { }
        public Weapons(ContentManager content, float scale, float quality) : base(scale, quality)
        {
            Image = content.Load<Texture2D>("Art/VoidShip/Weapons");

        }
        public override void Update(GameTime gameTime)
        {
            Color =  (gameTime.TotalGameTime.Seconds & 1) != 1 ? Color.Red : Color.Lime;
        }
        public int Range;
        public float RangeSquared;
        private float RangeMax;
        public override float Quality
        {
            get => base.Quality; set
            {
                base.Quality = value;
                RangeMax = 2000f;
                Range = (int)((base.Quality * 0.6f + 0.4f) * RangeMax);
                RangeSquared = Range * Range;
            }
        }
    }
}
