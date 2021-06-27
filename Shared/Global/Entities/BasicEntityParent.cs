using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MG.Shared.Global.Entities
{
  public class BasicEntityParent : BasicEntityChild
  {
    public Point Grid;
    public List<BasicEntityChild> Children { get; private set; }
    public BasicEntityParent()
    {
      Children = new List<BasicEntityChild>();
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      if (!visable) return;
      base.Draw(spriteBatch);
      foreach (var child in Children) child.Draw(spriteBatch);
    }
    public override void SizeTo(BasicEntityParent parent)
    {
      base.SizeTo(parent);
      foreach (var child in Children) child.SizeTo(this);
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      foreach (var child in Children) child.Update(gameTime);
    }
  }
}
