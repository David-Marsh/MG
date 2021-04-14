using MG.Shared.UI;
using MG.Shared.VoidControl;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static MG.Shared.Global.Sprite;

namespace MG.VoidControl
{
    public class MiniMap : Panel
    {
        Viewport mapView;
        Viewport origView;
        private readonly GraphicsDeviceManager graphicsref;
        Rectangle background;
        public int zoom;
        private Rectangle destrec;
        private Rectangle screenBounds;
        private Point HalfSize;
        Matrix ZoomOriginMatrix;

        public MiniMap(GraphicsDeviceManager graphics, Color back, Color fore, int col, int row, int colspan, int rowspan) : base(back, fore, col, row, colspan, rowspan)
        {
            graphicsref = graphics;
            zoom = 50;
        }
        public void SetZoomOrigin() => ZoomOriginMatrix = Matrix.CreateScale(1 / (float)zoom) * Matrix.CreateTranslation(new Vector3(HitBox.Width / 2, HitBox.Height / 2, 0));
        public Matrix MapMatrix => EntityManager.Player.PositionMatrix * ZoomOriginMatrix;
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);                                                     // Draw border in background color
            destrec.Location = EntityManager.Player.Position.ToPoint();                 // Locate player dot
            screenBounds.Location = destrec.Location - HalfSize;                        // move screen bounds with plaver
            spriteBatch.Draw(Pixel, background, Color.Black);                           // Map background black
            spriteBatch.End();                                                          // End drawing controls
            graphicsref.GraphicsDevice.Viewport = mapView;                              // Place viewport over minimap
            spriteBatch.Begin( transformMatrix: MapMatrix);                             // Begin drawing world space centered on the player position in minimap
            spriteBatch.Draw(Pixel, screenBounds, Colors.Fore);                         // Draw the screen boumds on minimap
            EntityManager.DrawMap(spriteBatch, destrec);                                // Draw dots on map
            spriteBatch.End();                                                          // End drawing minimap
            graphicsref.GraphicsDevice.Viewport = origView;                             // Restore viewport to the full screen
            spriteBatch.Begin();                                                        // Resume drawing controls in window space
        }
        public override void Setup(Panel panel)
        {
            base.Setup(panel);
            origView = graphicsref.GraphicsDevice.Viewport;
            mapView = new Viewport(HitBox);
            background = HitBox;
            background.Inflate(-2, -1);
            SetZoomOrigin();
            SetDotSize();
            screenBounds.Size = new(graphicsref.GraphicsDevice.Adapter.CurrentDisplayMode.Width, graphicsref.GraphicsDevice.Adapter.CurrentDisplayMode.Height);
            HalfSize = new(screenBounds.Size.X / 2, screenBounds.Size.Y / 2);
        }
        public void SetDotSize() => destrec.Width = destrec.Height = zoom * 4;
        public string ZoomIn()
        {
            zoom++;
            SetZoomOrigin();
            SetDotSize();
            return zoom.ToString("Zoom:0");
        }
        public string ZoomOut()
        {
            zoom--;
            SetZoomOrigin();
            SetDotSize();
            return zoom.ToString("Zoom:0");
        }
    }
}
