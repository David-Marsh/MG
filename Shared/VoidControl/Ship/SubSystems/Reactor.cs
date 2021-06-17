using MG.Shared.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.Shared.VoidControl.Ship.SubSystems
{
    public class Reactor : SubSystem
    {
        private Color qualityColor;
        public float Output;
        public Reactor(float scale, float quality) : base(scale, quality)
        {
            Texture = Content.Load<Texture2D>("Art/VoidShip/Reactor");
        }
        public override float Quality
        {
            get => base.Quality; set
            {
                base.Quality = value;
                qualityColor = Sprite.Spectrum(base.Quality * 0.1f);
            }
        }
        public void Update(GameTime gameTime)
        {
            Color = Color.Lerp(qualityColor, Color.Gray, Math.Abs(-(gameTime.TotalGameTime.Seconds & 1) + gameTime.TotalGameTime.Milliseconds * 0.001f));
            Output = (float)gameTime.ElapsedGameTime.TotalSeconds * (Quality * 0.9f + 0.1f); ;
        }
    }
}
