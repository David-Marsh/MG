using MG.Shared.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static MG.Shared.Global.Sprite;

namespace MG.Shared.VoidControl.UI.Panels
{
    public class MiniMap : Panel
    {
        private Viewport mini, full;
        private Point HalfSize;
        private readonly Button btnZoomIn;
        private readonly Button btnZoomOut;
        private readonly Label labelZoom;
        private readonly Panel mapArea;
        private int zoom;
        private Matrix MapMatrix, ZoomOriginMatrix;
        private Rectangle background, destrec, screenBounds;
        public MiniMap(Color back, Color fore, int col, int row, int colspan, int rowspan) : base(back, fore, col, row, colspan, rowspan)
        {
            Rows = 9;
            Collums = 8;
            mapArea = new(Color.Lime, fore, 0, 0, 8, 8) { CanHover = false };
            labelZoom = new(Color.Gray, "Zoom:50", 2, 8, 4, 1) { CanHover = false };
            btnZoomOut = new(back, fore, ((char)0xF8AB).ToString(), 6, 8, 1, 1) { MenuButton = true, Delay = 150 };
            btnZoomIn = new(back, fore, ((char)0xF8AA).ToString(), 7, 8, 1, 1) { MenuButton = true, Delay = 150 };
            Controls.Add(mapArea);
            Controls.Add(labelZoom);
            Controls.Add(btnZoomIn);
            Controls.Add(btnZoomOut);
            btnZoomIn.Clicked += new EventHandler(delegate (object o, EventArgs a) { Zoom++; });
            btnZoomOut.Clicked += new EventHandler(delegate (object o, EventArgs a) { Zoom--; });
            Zoom = 50;
            Colors.Fore = Color.Lerp(Color.Black, Color.White, 0.08f);
        }
        public void Setup(Panel panel, GraphicsDevice graphicsDevice)
        {
            base.Setup(panel);
            Padding = (int)Cellsize.X / 8;
            mapArea.Setup(this);
            labelZoom.Setup(this);
            btnZoomIn.Setup(this);
            btnZoomOut.Setup(this);

            background = mapArea.HitBox;
            background.Inflate(-2, -1);
            full = graphicsDevice.Viewport;
            mini = new Viewport(background);
            Zoom = zoom;
            screenBounds.Size = new(graphicsDevice.Adapter.CurrentDisplayMode.Width, graphicsDevice.Adapter.CurrentDisplayMode.Height);
            HalfSize = new(screenBounds.Size.X / 2, screenBounds.Size.Y / 2);
        }
        public int Zoom
        {
            get => zoom; set
            {
                zoom = value;
                ZoomOriginMatrix = Matrix.CreateScale(1 / (float)Zoom) * Matrix.CreateTranslation(new Vector3(mapArea.HitBox.Width / 2, mapArea.HitBox.Height / 2, 0));
                destrec.Width = destrec.Height = Zoom * 4;
                labelZoom.Msg.Text = Zoom.ToString("Zoom:0");
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            destrec.Location = EntityManager.Player.Position.ToPoint();                 // Locate player dot
            screenBounds.Location = destrec.Location - HalfSize;                        // move screen bounds with plaver
            MapMatrix = EntityManager.Player.PositionMatrix * ZoomOriginMatrix;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(Pixel, background, Color.Black);                           // Map background black
        }
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            graphicsDevice.Viewport = mini;                                             // Place viewport over minimap
            spriteBatch.Begin(transformMatrix: MapMatrix);                              // Begin drawing world space centered on the player position in minimap
            spriteBatch.Draw(Pixel, screenBounds, Colors.Fore);                         // Draw the screen boumds on minimap
            EntityManager.DrawMap(spriteBatch, destrec);                                // Draw dots on map
            spriteBatch.End();                                                          // End drawing minimap
            graphicsDevice.Viewport = full;                                             // Restore viewport to the full screen
        }
    }
}
