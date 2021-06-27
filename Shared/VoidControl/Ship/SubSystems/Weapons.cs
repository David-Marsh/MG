using MG.Shared.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MG.Shared.VoidControl.Ship.SubSystems
{
  public class Weapons : SubSystem
  {
    private bool autoAim, autoGun, canFire;
    private Point windowOrigin;
    private float rangeMax;
    public float Range;
    public float RangeSquared;
    public float ShotSpeed;
    public Vector2 ShotVector;
    public bool Fire;
    public override float Quality
    {
      get => base.Quality; set
      {
        base.Quality = value;
        rangeMax = 2000f;
        Range = (int)(((base.Quality * 0.6f) + 0.4f) * rangeMax);
        RangeSquared = Range * Range;
        ShotSpeed = Range;
      }
    }
    public Point WindowOrigin { get => windowOrigin; set => windowOrigin = value; }
    public bool AutoGun
    {
      get => autoGun; set
      {
        autoGun = value;
        autoAim &= !autoGun;
      }
    }
    public bool AutoAim
    {
      get => autoAim; set
      {
        autoAim = value;
        autoGun &= !autoAim;
      }
    }
    private static bool Trigger => !Input.MouseOnUI & Input.OneShotMouseLeft(ButtonState.Pressed);
    public float Delay { get; private set; }
    public Weapons(float scale, float quality) : base(scale, quality)
    {
      Texture = Content.Load<Texture2D>("Art/VoidShip/Weapons");
    }
    internal void Update(GameTime gameTime, VoidShip voidShip, Bullets bullets)
    {
      if (Fire)
      {
        Fire = false;
        Delay += 0.125f;
        voidShip.Shield.Power -= 0.25f;
        bullets.Add(voidShip);
      }
      Delay -= GameHelper.ScanTime;
      Delay = MathHelper.Max(Delay, 0);
      color = (gameTime.TotalGameTime.Seconds & 1) != 1 ? Color.Red : Color.Lime;
      canFire = (Delay <= 0) & (voidShip.Shield.Power > 0.3f);
    }
    public void ControlPC(Vector2 targetRelativePosition, Vector2 targetRelativeVelocity)
    {
      if (!canFire) return;
      if ((AutoGun) | (AutoAim & Trigger))
      {
        if (targetRelativePosition != Vector2.Zero)
          ShootAt(targetRelativePosition, targetRelativeVelocity);
      }
      else if (Trigger)
      {
        ShotVector = (Input.Position - windowOrigin).ToVector2();   // Where the mouse is pointed but in world space
        ShotVector.Normalize();
        Fire = true;
      }
    }
    public void ControlNPC(Vector2 targetRelativePosition, Vector2 targetRelativeVelocity)
    {
      if (canFire)
        ShootAt(targetRelativePosition, targetRelativeVelocity);
    }
    public void ShootAt(Vector2 targetRelativePosition, Vector2 targetRelativeVelocity)
    {
      float TargetRelativeVelocitySquared = targetRelativeVelocity.LengthSquared();
      float TargetRelativePositionSquared = targetRelativePosition.LengthSquared();
      if (RangeSquared < TargetRelativePositionSquared) return;
      if (TargetRelativeVelocitySquared < 0.001f)
      {
        ShotVector = Vector2.Normalize(targetRelativePosition);
        Fire = true;
        return;
      }
      float a = TargetRelativeVelocitySquared - (ShotSpeed * ShotSpeed);
      if (Math.Abs(a) < 0.001f)
      {
        float t = Math.Max(-TargetRelativePositionSquared / (2f * Vector2.Dot(targetRelativeVelocity, targetRelativePosition)), 0f);
        if (t < 0.3f) return;
        ShotVector = Vector2.Normalize(targetRelativePosition + (t * targetRelativeVelocity));
        Fire = true;
        return;
      }
      float b = 2f * Vector2.Dot(targetRelativeVelocity, targetRelativePosition);
      float c = TargetRelativePositionSquared;
      float determinant = (b * b) - (4f * a * c);
      if (determinant > 0f)                       //determinant > 0; two intercept paths (most common)
      {
        float t1 = (float)((-b + Math.Sqrt(determinant)) / (2f * a));
        float t2 = (float)((-b - Math.Sqrt(determinant)) / (2f * a));
        float t = t2 > t1 && t1 > 0.001f ? t1 : t2 > 0.001f ? t2 : 0;
        if (t < 0.3f) return;
        ShotVector = Vector2.Normalize(targetRelativePosition + (t * targetRelativeVelocity));
        Fire = true;
        return;
      }
      else if (determinant < 0f) return;          //determinant < 0; no intercept path
      else                                        //determinant = 0; one intercept path, pretty much never happens
      {
        float t = Math.Max(-b / (2f * a), 0f);
        if (t < 0.3f) return;
        ShotVector = Vector2.Normalize(targetRelativePosition + (t * targetRelativeVelocity));
        Fire = true;
        return;
      }
    }
  }
}
