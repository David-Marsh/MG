using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.Shared.Global.Entities
{
    public class FullEntity
    {
        protected static ContentManager Content { get; set; }
        protected static Texture2D Pixel { get; set; }
        #region Used in draw function
        protected Texture2D texture;
        protected Vector2 position;
        protected Rectangle sourceRectangle;
        protected Color color = Color.White;
        protected float rotation = 0;
        protected Vector2 origin;
        protected Vector2 scale = Vector2.One;
        protected bool enabled = true;
        protected bool visible = true;
        #endregion
        protected Texture2D Texture
        {
            get => texture; set
            {
                texture = value;
                sourceRectangle = texture.Bounds;
                origin = texture == null ? Vector2.Zero : new Vector2(texture.Width, texture.Height) * 0.5f;
            }
        }
        public virtual Vector2 Position { get => position; set => position = value; }
        public float Rotation { get => rotation; set => rotation = value; }
        public virtual Color Color { get => color; set => color = value; }
        public bool Enabled { get => enabled; set => enabled = value; }
        public bool Visible { get => visible; set => visible = value; }
        public static void CreateThePixel(GraphicsDevice graphicsDevice)
        {
            Pixel = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Pixel.SetData(new[] { Color.White });
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
                spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, SpriteEffects.None, 1f);
        }
        public static void Initialize(ContentManager contentManager) => Content = contentManager;
        public virtual void SetRotation(Vector2 direction) => rotation = (direction == Vector2.Zero) ? rotation : (float)Math.Atan2(direction.Y, direction.X);
    }
}
