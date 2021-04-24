using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.VoidControl.Ship
{
    public abstract class ShipSystem
    {
        private Texture2D image;
        private Rectangle sourceRectangle;
        private Color color = Color.White;
        private Vector2 Origin;
        private float scale;
        private float quality;
        protected ShipSystem(float scale, float quality)
        {
            Scale = scale;
            Quality = quality;
        }
        protected Texture2D Image
        {
            get => image; set
            {
                image = value;
                sourceRectangle = image.Bounds;
                Origin = image == null ? Vector2.Zero : new Vector2(image.Width, image.Height) * 0.5f;
            }
        }
        public virtual float Scale { get => scale; set => scale = value; }
        public virtual Color Color { get => color; set => color = value; }
        public virtual float Quality { get => quality; set => quality = Math.Clamp(value, 0, 1); }
        public abstract void Update(GameTime gameTime);
        public static Color Spectrum(float hue)
        {
            hue *= 6;
            return (int)hue switch
            {
                0 => Color.Lerp(Color.Red, Color.Yellow, hue % 1),
                1 => Color.Lerp(Color.Yellow, Color.Green, hue % 1),
                2 => Color.Lerp(Color.Green, Color.Cyan, hue % 1),
                3 => Color.Lerp(Color.Cyan, Color.Blue, hue % 1),
                4 => Color.Lerp(Color.Blue, Color.Magenta, hue % 1),
                5 => Color.Lerp(Color.Magenta, Color.Red, hue % 1),
                _ => Color.Lerp(Color.Red, Color.Yellow, hue % 1),
            };
        }
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation)
        {
            spriteBatch.Draw(Image, position, sourceRectangle, Color, rotation, Origin, Scale, SpriteEffects.None, 1f);
        }
    }
}
