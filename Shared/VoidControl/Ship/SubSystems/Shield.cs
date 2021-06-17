using MG.Shared.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.Shared.VoidControl.Ship.SubSystems
{
    public class Shield : SubSystem
    {
        private float maxPower;
        private float power;
        public float Power { get => power; set => power = Math.Clamp(value, 0, maxPower); }
        public override float Quality
        {
            get => base.Quality; set
            {
                base.Quality = value;
                maxPower = (base.Quality * 0.6f + 0.4f);
                Power = maxPower;
            }
        }
        public Shield(float scale, float quality) : base(scale, quality)
        {
            Texture = Content.Load<Texture2D>("Art/Circle");
        }
        public void Update(GameTime gameTime)
        {
            Color = Sprite.Spectrum(Power * 1 / 3f);
        }
        public void Update(GameTime gameTime, Reactor reactor)
        {
            Power += reactor.Output;
            Color = Sprite.Spectrum(Power * 1 / 3f);
        }
    }
}
