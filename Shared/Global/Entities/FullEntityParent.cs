using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MG.Shared.Global.Entities
{
  public class FullEntityParent : FullEntityChild
  {
    public List<FullEntityChild> Children { get; private set; }
    public override void Draw(SpriteBatch spriteBatch)
    {
      if (!visible) return;
      base.Draw(spriteBatch);
      foreach (var child in Children) child.Draw(spriteBatch, position, rotation);
    }
  }
}
