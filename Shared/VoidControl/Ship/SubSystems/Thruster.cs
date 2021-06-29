using MG.Shared.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MG.Shared.VoidControl.Ship.SubSystems
{
  public class Thruster : SubSystem
  {
    private const int flickermask = 48;                             // 31.25 Hz 1/4 wave = 25% duty cycle
    private Color levelColorA, levelColorB;                         // Colors to alternate between for thrust
    public float Cost, CostFactor;
    public Vector2 Thrust, ThrustDesired;                           // Change in speed in units of pixels per second 
    public float MaxThrust, MaxThrustSquared;                       // Max change in speed in units of pixels per second 
    public float MaxThrustPerScan, MaxThrustPerScanSquared;         // Max change in speed in units of pixels per second 
    public float MaxVelocity, MaxVelocitySquared;                   // Max speed in units of pixels per second 
    public override float Quality
    {
      get => base.Quality; set
      {
        base.Quality = value;
        levelColorA = Sprite.Spectrum((base.Quality * 0.9f) + 0.0f);
        levelColorB = Sprite.Spectrum((base.Quality * 0.9f) + 0.1f);
        CostFactor = 1 / 3000f;
        MaxVelocity = ((base.Quality * 0.5f) + 0.5f) * 3000f;
        MaxVelocitySquared = MaxVelocity * MaxVelocity;
        MaxThrust = ((base.Quality * 0.9f) + 0.1f) * 3000f;
        MaxThrustPerScan = MaxThrust * GameHelper.ScanTime;
        MaxThrustSquared = MaxThrust * MaxThrust;
        MaxThrustPerScanSquared = MaxThrustSquared * GameHelper.ScanTime;
      }
    }
    public Thruster(float scale, float quality) : base(scale, quality)
    {
      Texture = Content.Load<Texture2D>("Art/VoidShip/Thruster");
    }
    public void Update(GameTime gameTime, VoidShip voidShip)
    {
      float cost = ThrustDesired.Length() * CostFactor;
      if (cost > voidShip.Shield.Power)
      {
        Thrust = Vector2.Zero;
      }
      else
      {
        voidShip.Shield.Power -= cost;
        Thrust = ThrustDesired;
      }
      Color = Thrust == Vector2.Zero ? Color.Transparent : (gameTime.TotalGameTime.Milliseconds & flickermask) != flickermask ? levelColorA : levelColorB;
      voidShip.Velocity += Thrust;
      voidShip.Position += voidShip.Velocity * GameHelper.ScanTime;
    }
    public void ControlPC(Vector2 velocity)
    {
      Vector2 VelocityRequest = Input.Stick() * MaxVelocity;
      ThrustDesired = VelocityRequest - velocity;
      if (ThrustDesired != Vector2.Zero)
      {
        if (ThrustDesired.LengthSquared() > MaxThrustPerScanSquared)
        {
          ThrustDesired = Vector2.Normalize(ThrustDesired) * MaxThrustPerScan;
        }
      }
    }
    public void ControlNPC(Vector2 velocity, Vector2 stick)
    {
      ThrustDesired = stick * MaxThrustPerScan;
      if (ThrustDesired != Vector2.Zero)
      {
        Vector2 velocivyDesired = velocity + ThrustDesired;
        if (velocivyDesired.LengthSquared() > MaxVelocitySquared)
        {
          ThrustDesired = Vector2.Zero;
        }
      }
    }
  }
}
