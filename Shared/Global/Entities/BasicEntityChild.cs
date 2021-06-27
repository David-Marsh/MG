using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MG.Shared.Global.Entities
{
  public class BasicEntityChild : BasicEntity
  {

    protected Rectangle placement;
    protected bool visable;
    protected bool enabled;

    public bool Enabled { get => enabled; set => enabled = value; }
    public bool Visable { get => visable; set => visable = value; }

    public BasicEntityChild()
    {
      Enabled = true;
      Visable = true;
      texture = Pixel;
      color = Color.White;
    }
    public virtual bool Contains(Point point) => destinationRectangle.Contains(point);

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (visable) spriteBatch.Draw(texture, destinationRectangle, color);
    }
    public virtual void Update(GameTime gameTime) { }

    public virtual void SizeTo(BasicEntityParent parent)
    {
      destinationRectangle.Location = parent.Location + (placement.Location * parent.Size / parent.Grid);
      destinationRectangle.Size = placement.Size * parent.Size / parent.Grid;
    }
  }
}
