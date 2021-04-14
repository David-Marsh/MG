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
            switch ((int)hue)
            {
                case 0:
                    return Color.Lerp(Color.Red, Color.Yellow, hue % 1);
                case 1:
                    return Color.Lerp(Color.Yellow, Color.Green, hue % 1);
                case 2:
                    return Color.Lerp(Color.Green, Color.Cyan, hue % 1);
                case 3:
                    return Color.Lerp(Color.Cyan, Color.Blue, hue % 1);
                case 4:
                    return Color.Lerp(Color.Blue, Color.Magenta, hue % 1);
                case 5:
                    return Color.Lerp(Color.Magenta, Color.Red, hue % 1);
                default:
                    return Color.Lerp(Color.Red, Color.Yellow, hue % 1);
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation)
        {
            spriteBatch.Draw(Image, position, sourceRectangle, Color, rotation, Origin, Scale, SpriteEffects.None, 1f);
        }
    }
}
