using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MG.Shared.VoidControl.Ship.SubSystems
{
  public class ECM : SubSystem
  {
    public ECM(float scale, float quality) : base(scale, quality)
    {
      Texture = Content.Load<Texture2D>("Art/VoidShip/Cloak");
    }
    public void Update()
    {
      Color = Color.LimeGreen;
    }
  }
}
