using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MG.Shared.Global.Entities
{
    public class BasicEntity
    {
        protected static ContentManager Content { get; set; }
        protected static Texture2D Pixel { get; set; }
        #region Used in draw function
        protected Texture2D texture;
        protected Rectangle destinationRectangle;
        protected Color color;
        #endregion
        public int X { get => destinationRectangle.X; set => destinationRectangle.X = value; }
        public int Y { get => destinationRectangle.Y; set => destinationRectangle.Y = value; }
        public int Width { get => destinationRectangle.Width; set => destinationRectangle.Width = value; }
        public int Height { get => destinationRectangle.Height; set => destinationRectangle.Height = value; }
        public Point Location { get => destinationRectangle.Location; set => destinationRectangle.Location = value; }
        public Point Size { get => destinationRectangle.Size; set => destinationRectangle.Size = value; }
        public static void CreateThePixel(GraphicsDevice graphicsDevice)
        {
            Pixel = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Pixel.SetData(new[] { Color.White });
        }
        public virtual void Draw(SpriteBatch spriteBatch) => spriteBatch.Draw(texture, destinationRectangle, color);
        public static void Initialize(ContentManager contentManager) => Content = contentManager;
    }
}