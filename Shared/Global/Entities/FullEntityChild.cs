using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MG.Shared.Global.Entities
{
  public class FullEntityChild : FullEntity
  {
    public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation)
    {
      if (visible)
        spriteBatch.Draw(texture, position, sourceRectangle, Color, rotation, origin, scale, SpriteEffects.None, 1f);
    }
  }
}
