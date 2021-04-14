using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.Shared.Global
{
    public abstract class Entity
    {
        protected static ContentManager Content;

        private Texture2D image;
        protected Rectangle sourceRectangle;
        protected Color color = Color.White;
        public Vector2 Size, Origin;
        public float Radius;                        // used for circular collision detection
        public float Rotation = 0;
        public bool IsExpired = false;              // true if the entity was destroyed and should be deleted.
        private float scale;
        protected Vector2 position;
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
                Size = image == null ? Vector2.Zero : new Vector2(image.Width, image.Height);
                Origin = Size * 0.5f;
                Radius = Math.Max(Origin.X, Origin.Y) * Scale;
            }
        }

        public virtual Vector2 Position { get => position; set => position = value; }
        public virtual float Scale { get => scale; set => scale = value; }
        public virtual Color Color { get => color; set => color = value; }
        public virtual bool IsColliding(Entity that) => !that.IsExpired & EntityDistanceSquared(that) <= Math.Pow(this.Radius + that.Radius, 2);
        public float EntityDistanceSquared(Entity that) => Vector2.DistanceSquared(this.Position, that.Position);
        public abstract void HandleCollision(Entity other);
        public abstract void Update(GameTime gameTime);
        public virtual void Draw(SpriteBatch spriteBatch) => spriteBatch.Draw(image, Position, sourceRectangle, Color, Rotation, Origin, Scale, SpriteEffects.None, 1f);
    }
}
