using MG.Shared.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.Shared.VoidControl.Ship.SubSystems
{
  public class Reactor : SubSystem
  {
    private Color qualityColor;
    private float seconds;
    public float Output;
    public Reactor(float scale, float quality) : base(scale, quality)
    {
      Texture = Content.Load<Texture2D>("Art/VoidShip/Reactor");
    }
    public override float Quality
    {
      get => base.Quality; set
      {
        base.Quality = value;
        qualityColor = Sprite.Spectrum(base.Quality * 0.1f);
      }
    }
    public void Update()
    {
      seconds += GameHelper.ScanTime;
      if (seconds > 2) seconds -= 2;
      Color = Color.Lerp(qualityColor, Color.Gray, Math.Abs(seconds - 1f));
      Output = GameHelper.ScanTime * ((Quality * 0.9f) + 0.1f); ;
    }
  }
}
