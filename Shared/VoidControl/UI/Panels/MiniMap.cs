using MG.Shared.UI;
using MG.Shared.UI.Controls;
using MG.Shared.VoidControl.Ship;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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
    private Microsoft.Xna.Framework.Rectangle background, shipDot, screenBounds;
    private Matrix MapMatrix, ZoomOriginMatrix;
    public VoidShipPC Player;
    public Ships Ships;
    public int Zoom
    {
      get => zoom; set
      {
        zoom = value;
        ZoomOriginMatrix = Matrix.CreateScale(1 / (float)Zoom) * Matrix.CreateTranslation(new Vector3(mapArea.DestinationRectangle.Width / 2, mapArea.DestinationRectangle.Height / 2, 0));
        shipDot.Width = shipDot.Height = Zoom * 4;
        labelZoom.Text = Zoom.ToString("Zoom:0");
      }
    }
    public MiniMap(int x, int y, int width, int height) : base(x, y, width, height)
    {
      mapArea = new(0, 0, 8, 8, new ColorScheme(Color.Gray, Color.Lime));
      labelZoom = new(2, 8, 4, 1, "Zoom:50", Color.Gray);
      btnZoomOut = new(6, 8, 1, 1, ((char)0xF8AB).ToString());
      btnZoomIn = new(7, 8, 1, 1, ((char)0xF8AA).ToString());

      Children.Add(mapArea);
      Children.Add(labelZoom);
      Children.Add(btnZoomIn);
      Children.Add(btnZoomOut);
      btnZoomIn.Clicked += new EventHandler(delegate (object o, EventArgs a) { Zoom++; });
      btnZoomOut.Clicked += new EventHandler(delegate (object o, EventArgs a) { Zoom--; });
      Zoom = 50;
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
      Ships.DrawMap(spriteBatch, shipDot);                                        // Draw dots on map
      spriteBatch.End();                                                          // End drawing minimap
      graphicsDevice.Viewport = full;                                             // Restore viewport to the full screen
    }
    public override void SizeTo(Point grid, Point size)
    {
      base.SizeTo(grid, size);
      Zoom = zoom;
      background = mapArea.DestinationRectangle;
      background.Inflate(-2, -1);
      mini = new Viewport(background);
      screenBounds.Size = size;
      screenBounds.Location = Point.Zero;
      full = new Viewport(screenBounds);
      HalfSize = new(size.X / 2, size.Y / 2);
    }
    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      shipDot.Location = Player.Position.ToPoint();                 // Locate player dot
      screenBounds.Location = shipDot.Location - HalfSize;                        // move screen bounds with player
      MapMatrix = Player.PositionMatrix * ZoomOriginMatrix;
    }
  }
}
