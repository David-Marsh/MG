using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.Shared.Global
{
    public abstract class Entity
    {
        #region Used in draw function
        private Texture2D image;
        protected Vector2 position;
        private Rectangle sourceRectangle;
        private Color color = Color.White;
        private float rotation = 0;
        private Vector2 origin;
        private Vector2 scale = Vector2.One;
        #endregion



        public float Radius;                        // used for circular collision detection
        public bool IsExpired = false;              // true if the entity was destroyed and should be deleted.
        public Vector2 Velocity;
        public static void Initialize(ContentManager contentManager)
        {
            Content = contentManager;
        }
        protected Texture2D Image
        {
            get => image; set
            {
                image = value;
                sourceRectangle = image.Bounds;
                origin = image == null ? Vector2.Zero : new Vector2(image.Width, image.Height) * 0.5f;
                Radius = Math.Max(origin.X, origin.Y) * Scale;
            }
        }
        public virtual Vector2 Position { get => position; set => position = value; }
        public float Rotation { get => rotation; set => rotation = value; }
        public virtual float Scale { get => scale.X; set => scale = Vector2.One * value; }
        public virtual Color Color { get => color; set => color = value; }
        protected static ContentManager Content { get; set; }

        public virtual bool IsColliding(Entity that) => !that.IsExpired & EntityDistanceSquared(that) <= Math.Pow(this.Radius + that.Radius, 2);
        public float EntityDistanceSquared(Entity that) => Vector2.DistanceSquared(this.Position, that.Position);
        public abstract void HandleCollision(Entity other);
        public abstract void Update(GameTime gameTime);
        public virtual void Draw(SpriteBatch spriteBatch) => spriteBatch.Draw(image, position, sourceRectangle, color, rotation, origin, scale, SpriteEffects.None, 1f);
        public virtual void SetRotation(Vector2 direction) => rotation = (direction == Vector2.Zero) ? rotation : (float)Math.Atan2(direction.Y, direction.X);
        public virtual void SetRotation() => SetRotation(Velocity);
    }
}
