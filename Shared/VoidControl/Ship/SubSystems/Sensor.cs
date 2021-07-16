using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MG.Shared.VoidControl.Ship.SubSystems
{
  public class Sensor : SubSystem
  {
    private readonly int mask = 512 | 256;
    private float rangeMax;
    private int milliseconds;
    public float Range;
    public float RangeSquared;
    public override float Quality
    {
      get => base.Quality; set
      {
        base.Quality = value;
        rangeMax = 100000f;
        Range = ((base.Quality * 0.9f) + 0.1f) * rangeMax;
        RangeSquared = Range * Range;
      }
    }
    public Sensor(float scale, float quality) : base(scale, quality)
    {
      Texture = Content.Load<Texture2D>("Art/VoidShip/Sensor");
    }
    public void Update()
    {
      milliseconds += 16;
      milliseconds &= 1023;
      Color = (milliseconds & mask) != mask ? Color.Transparent : Color.LimeGreen;
    }
  }
}
