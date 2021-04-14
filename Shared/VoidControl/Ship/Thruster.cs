using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MG.VoidControl.Ship
{
    public class Thruster : ShipSystem
    {
        private const int flickermask = 48;                             // 31.25 Hz 1/4 wave = 25% duty cycle
        private Color levelColorA, levelColorB;                         // Colors to alternate between for thrust
        public Vector2 Thrust;                                          // Change in speed in units of pixels per second 
        public float MaxThrust;                                         // Max change in speed in units of pixels per second 
        public float MaxVelocity;                                       // Max speed in units of pixels per second 
        public float Cost;
        public float CostFactor;
        public Thruster(ContentManager content, float scale) : this(content, scale, 0) { }
        public Thruster(ContentManager content, float scale, float quality) : base(scale, quality)
        {
            Image = content.Load<Texture2D>("Art/VoidShip/Thruster");
            Thrust = Vector2.Zero;
        }
        public override float Quality
        {
            get => base.Quality; set
            {
                base.Quality = value;
                levelColorA = Spectrum((base.Quality * 0.9f + 0.0f));
                levelColorB = Spectrum((base.Quality * 0.9f + 0.1f));
                MaxVelocity = 3000f;
                MaxThrust = (base.Quality * 0.9f + 0.1f) * MaxVelocity;
                CostFactor = 1 / MaxVelocity;
            }
        }
        public override void Update(GameTime gameTime)
        {
            Color = Thrust == Vector2.Zero ? Color.Transparent
                : (gameTime.TotalGameTime.Milliseconds & flickermask) != flickermask ? levelColorA : levelColorB;
        }
    }
}
