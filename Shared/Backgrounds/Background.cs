using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.Shared.Backgrounds
{
  public abstract class Background : DrawableGameComponent
  {
    protected Rectangle DrawBounds;
    protected Rectangle CenterCell;
    private Point DrawBoundsOffset;
    private int cellSizeLog2, cellMask;
    public int CellSizeLog2
    {
      get => cellSizeLog2; set
      {
        if (cellSizeLog2 == value) return;
        cellSizeLog2 = value;
        CenterCell.Width = CenterCell.Height = 1 << cellSizeLog2;
        cellMask = ~(CenterCell.Width - 1);
        OnResize(GraphicsDevice, null);
      }
    }
    public Background(Game game) : base(game)
    {
    }
    public abstract void Draw(SpriteBatch spriteBatch);
    protected virtual void DrawBoundsChanged()
    {
      DrawBounds.Location = CenterCell.Location - DrawBoundsOffset;
      DrawBounds.Size = DrawBoundsOffset + DrawBoundsOffset + CenterCell.Size;
    }
    public virtual void OnResize(object sender, EventArgs e)
    {
      DrawBoundsOffset.X = ((GraphicsDevice)sender).Viewport.Width / 2;
      DrawBoundsOffset.Y = ((GraphicsDevice)sender).Viewport.Height / 2;
      DrawBoundsOffset.X = 3840 / 2;
      DrawBoundsOffset.Y = 2160 / 2;
      DrawBoundsOffset += CenterCell.Size;
      DrawBoundsOffset.X &= cellMask;
      DrawBoundsOffset.Y &= cellMask;
      DrawBounds.Size = DrawBoundsOffset + DrawBoundsOffset + CenterCell.Size;
      DrawBoundsChanged();
    }
    public virtual void Update(Point screencenter)
    {
      if (CenterCell.Contains(screencenter)) return;
      CenterCell.Location = screencenter;
      CenterCell.X &= cellMask;
      CenterCell.Y &= cellMask;
      DrawBoundsChanged();
    }
  }
}
