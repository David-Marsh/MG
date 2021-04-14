using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.VoidControl.Ship
{
    public class Reactor : ShipSystem
    {
        public Reactor(ContentManager content, float scale) : this(content, scale, 0) { }
        public Reactor(ContentManager content, float scale, float quality) : base(scale, quality)
        {
            Image = content.Load<Texture2D>("Art/VoidShip/Reactor");
            levelColor = Spectrum(Quality * 0.1f);
        }
        private Color levelColor;
        public float Output;
        public override float Quality
        {
            get => base.Quality; set
            {
                base.Quality = value;
                levelColor = Spectrum(base.Quality * 0.1f); 
            }
        }    
        public override void Update(GameTime gameTime)
        {
            Color = Color.Lerp(levelColor,Color.Gray, Math.Abs(-(gameTime.TotalGameTime.Seconds & 1) + gameTime.TotalGameTime.Milliseconds * 0.001f));
            Output = (float)gameTime.ElapsedGameTime.TotalSeconds * (Quality * 0.9f + 0.1f); ;
        }
    }
}
