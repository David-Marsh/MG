using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MG.VoidControl.Ship
{
    public class Sensor : ShipSystem
    {
        public Sensor(ContentManager content, float scale) : this(content, scale, 0) { }
        public Sensor(ContentManager content, float scale, float quality) : base(scale, quality)
        {
            Image = content.Load<Texture2D>("Art/VoidShip/Sensor");
        }
        private readonly int mask = (512 | 256);
        public float Range;
        public float RangeSquared;
        private float RangeMax;
        public override float Quality
        {
            get => base.Quality; set
            {
                base.Quality = value;
                RangeMax = 30000f;
                Range = (base.Quality * 0.9f + 0.1f) * RangeMax;
                RangeSquared = Range * Range;
            }
        }
        public override void Update(GameTime gameTime)
        {
            Color = (gameTime.TotalGameTime.Milliseconds & mask) != mask ? Color.Transparent : Color.LimeGreen;
        }
    }
}
