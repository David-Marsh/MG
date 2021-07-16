using MG.Shared.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MG.Shared.VoidControl.Ship.SubSystems
{
  public class Weapons : SubSystem
  {
    #region AI
    public float targetRelativePositionSquared, targetRelativeVelocitySquared;
    public Vector2 targetRelativePosition, targetRelativeVelocity;
    #endregion
    private bool autoAim, autoGun, gunReady;
    private float rangeMax, delay;
    public float Range, RangeSquared, ShotSpeed;
    public Vector2 ShotVector;
    public bool InRange;
    public override float Quality
    {
      get => base.Quality; set
      {
        base.Quality = value;
        rangeMax = 4000f;
        Range = (int)(((base.Quality * 0.6f) + 0.4f) * rangeMax);
        RangeSquared = Range * Range;
        ShotSpeed = Range * 0.3f;
      }
    }
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
    public Weapons(float scale, float quality) : base(scale, quality)
    {
      Texture = Content.Load<Texture2D>("Art/VoidShip/Weapons");
      AutoGun = true;
    }
    internal void Update(VoidShip voidShip, Bullets bullets)
    {
      if(delay > 0)
      {
        delay -= GameHelper.ScanTime;
        delay = MathHelper.Max(delay, 0);
      }
      gunReady = (delay == 0) & (voidShip.Shield.Power > 0.3f);
      InRange = RangeSquared > targetRelativePositionSquared;
      color = gunReady ? Color.Lime : Color.Red;
      if (!gunReady | !InRange) return;
      if ((AutoGun) | (AutoAim & Input.TriggerLeft))
      {
        if (!Shoot()) return;
      }
      else if (Input.TriggerLeft)
        ShotVector = Input.OriginVector;
      else 
        return;
      delay += 0.125f;
      voidShip.Shield.Power -= 0.25f;
      bullets.Add(voidShip);
    }
    public bool Shoot()
    {
      targetRelativeVelocitySquared = targetRelativeVelocity.LengthSquared();
      if (targetRelativeVelocitySquared < 0.001f)
      {
        ShotVector = Vector2.Normalize(targetRelativePosition);
        return true;
      }
      float a = targetRelativeVelocitySquared - (ShotSpeed * ShotSpeed);
      if (Math.Abs(a) < 0.001f)
      {
        float t = Math.Max(-targetRelativePositionSquared / (2f * Vector2.Dot(targetRelativeVelocity, targetRelativePosition)), 0f);
        if (t < 0.3f) return false;
        ShotVector = Vector2.Normalize(targetRelativePosition + (t * targetRelativeVelocity));
        return true;
      }
      float b = 2f * Vector2.Dot(targetRelativeVelocity, targetRelativePosition);
      float c = targetRelativePositionSquared;
      float determinant = (b * b) - (4f * a * c);
      if (determinant > 0f)                       //determinant > 0; two intercept paths (most common)
      {
        float t1 = (float)((-b + Math.Sqrt(determinant)) / (2f * a));
        float t2 = (float)((-b - Math.Sqrt(determinant)) / (2f * a));
        float t = t2 > t1 && t1 > 0.001f ? t1 : t2 > 0.001f ? t2 : 0;
        if (t < 0.3f) return false;
        ShotVector = Vector2.Normalize(targetRelativePosition + (t * targetRelativeVelocity));
        return true;
      }
      else if (determinant < 0f) return false;    //determinant < 0; no intercept path
      else                                        //determinant = 0; one intercept path, pretty much never happens
      {
        float t = Math.Max(-b / (2f * a), 0f);
        if (t < 0.3f) return false;
        ShotVector = Vector2.Normalize(targetRelativePosition + (t * targetRelativeVelocity));
        return true;
      }
    }
    //public void ControlPC(Vector2 targetRelativePosition, Vector2 targetRelativeVelocity)
    //{
    //  if (!gunReady) return;
    //  if ((AutoGun) | (AutoAim & Trigger))
    //  {
    //    if (targetRelativePosition != Vector2.Zero)
    //      ShootAt(targetRelativePosition, targetRelativeVelocity);
    //  }
    //  else if (Trigger)
    //  {
    //    ShotVector = (Input.Position - windowOrigin).ToVector2();   // Where the mouse is pointed but in world space
    //    ShotVector.Normalize();
    //    Fire = true;
    //  }
    //}
    //public void ControlNPC(Vector2 targetRelativePosition, Vector2 targetRelativeVelocity)
    //{
    //  if (gunReady)
    //    ShootAt(targetRelativePosition, targetRelativeVelocity);
    //}
    //public void ShootAt(Vector2 targetRelativePosition, Vector2 targetRelativeVelocity)
    //{
    //  float TargetRelativeVelocitySquared = targetRelativeVelocity.LengthSquared();
    //  float TargetRelativePositionSquared = targetRelativePosition.LengthSquared();
    //  if (RangeSquared < TargetRelativePositionSquared) return;
    //  if (TargetRelativeVelocitySquared < 0.001f)
    //  {
    //    ShotVector = Vector2.Normalize(targetRelativePosition);
    //    Fire = true;
    //    return;
    //  }
    //  float a = TargetRelativeVelocitySquared - (ShotSpeed * ShotSpeed);
    //  if (Math.Abs(a) < 0.001f)
    //  {
    //    float t = Math.Max(-TargetRelativePositionSquared / (2f * Vector2.Dot(targetRelativeVelocity, targetRelativePosition)), 0f);
    //    if (t < 0.3f) return;
    //    ShotVector = Vector2.Normalize(targetRelativePosition + (t * targetRelativeVelocity));
    //    Fire = true;
    //    return;
    //  }
    //  float b = 2f * Vector2.Dot(targetRelativeVelocity, targetRelativePosition);
    //  float c = TargetRelativePositionSquared;
    //  float determinant = (b * b) - (4f * a * c);
    //  if (determinant > 0f)                       //determinant > 0; two intercept paths (most common)
    //  {
    //    float t1 = (float)((-b + Math.Sqrt(determinant)) / (2f * a));
    //    float t2 = (float)((-b - Math.Sqrt(determinant)) / (2f * a));
    //    float t = t2 > t1 && t1 > 0.001f ? t1 : t2 > 0.001f ? t2 : 0;
    //    if (t < 0.3f) return;
    //    ShotVector = Vector2.Normalize(targetRelativePosition + (t * targetRelativeVelocity));
    //    Fire = true;
    //    return;
    //  }
    //  else if (determinant < 0f) return;          //determinant < 0; no intercept path
    //  else                                        //determinant = 0; one intercept path, pretty much never happens
    //  {
    //    float t = Math.Max(-b / (2f * a), 0f);
    //    if (t < 0.3f) return;
    //    ShotVector = Vector2.Normalize(targetRelativePosition + (t * targetRelativeVelocity));
    //    Fire = true;
    //    return;
    //  }
    //}
  }
}
