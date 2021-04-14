using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.VoidControl.Ship
{
    public class Shield : ShipSystem
    {
        private float MaxPower;
        private float power;
        public Shield(ContentManager content, float scale) : this(content, scale, 0) { }
        public Shield(ContentManager content, float scale, float quality) : base(scale, quality)
        {
            Image = content.Load<Texture2D>("Art/Circle");
            MaxPower = (base.Quality * 0.6f + 0.4f);
            Power = MaxPower;
        }
        public override float Quality
        {
            get => base.Quality; set
            {
                base.Quality = value;
                MaxPower = (base.Quality * 0.6f + 0.4f);
                Power = MaxPower;
            }
        }
        public float Power { get => power; set => power = Math.Clamp(value, 0, MaxPower); }
        public override void Update(GameTime gameTime)
        {
            Color = Color.Lerp(Color.Red, Color.Green, Power % 1);
            Color = Spectrum(Power * 1 / 3f);
        }
    }
}
