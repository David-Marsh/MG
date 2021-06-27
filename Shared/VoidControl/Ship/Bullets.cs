using MG.Shared.Global;
using MG.Shared.Global.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.Shared.VoidControl.Ship
{
  public class Bullets
  {
    class Bullet : FullEntity
    {
      private Vector2 velocity;
      private float decay;

      public float Power { get; set; }
      public float Radius { get; private set; }
      public Bullet() : this(1f) { }
      public Bullet(float scale)
      {
        this.scale = Vector2.One * scale;
        Texture = Content.Load<Texture2D>("Art/VoidShip/Star");
        Radius = Math.Max(origin.X, origin.Y);
        Color = Color.White;
        Power = 0;
        visible = enabled = false;
      }
      public void Init(VoidShip voidShip)
      {
        Power = (voidShip.Weapons.Quality * 0.7f) + 0.3f;
        position = voidShip.Position;
        position += voidShip.Weapons.ShotVector * (voidShip.Radius + Radius + 3);
        velocity = voidShip.Velocity;
        velocity += voidShip.Weapons.ShotVector * voidShip.Weapons.ShotSpeed;
        decay = 0.5f * Power * voidShip.Weapons.ShotSpeed / voidShip.Weapons.Range;  // half life at range
        visible = enabled = true;
      }
      public void HandleCollision(FullEntity other) => throw new NotImplementedException();
      public void Update()
      {
        if (!enabled) return;
        position += velocity * GameHelper.ScanTime;
        Power -= decay * GameHelper.ScanTime;
        visible = enabled = Power > 0;
        rotation -= 0.1f;
        if (rotation < 0) rotation += MathHelper.TwoPi;
        color = Power < 0.2f ? Color.Lerp(Color.Transparent, Color.Yellow, Power * 5) : Color.Lerp(Color.Yellow, Color.Pink, (Power - 0.2f) * 1.25f);
      }
    }

    private readonly float radius;
    private int start;
    private readonly Bullet[] array;
    public int Count { get; set; }
    public int Start
    {
      get { return start; }
      set { start = value & 0x3FF; }
    }
    public Bullets()
    {
      array = new Bullet[0x400];
      for (int i = 0; i < array.Length; i++)
      {
        array[i] = new();
      }
      radius = array[0].Radius;
    }
    public void Add(VoidShip voidShip)
    {
      array[(start + Count) & 0x3FF].Init(voidShip);
      if (voidShip is VoidShipPC) Sound.Impulse44((voidShip.Shield.Power * 2) - 1f);
      Count++;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      for (int i = 0; i < Count; i++)
      {
        array[(start + i) & 0x3FF].Draw(spriteBatch);
      }
    }
    public void Update()
    {
      while (!array[start].Enabled & Count > 0)
      {
        Count--;
        Start++;
      }
      for (int i = 0; i < Count; i++)
      {
        array[(start + i) & 0x3FF].Update();
      }
    }
    public void HandleCollision(VoidShip voidShip, Particles particles)
    {
      Vector2 dist;
      Bullet bullet;
      float distSquared, hitDistSquared = (float)Math.Pow(voidShip.Radius + radius, 2);
      for (int i = 0; i < Count; i++)
      {
        bullet = array[(start + i) & 0x3FF];
        if (!bullet.Enabled) continue;
        dist = voidShip.Position - bullet.Position;
        distSquared = (dist.X * dist.X) + (dist.Y * dist.Y);
        if (distSquared < hitDistSquared)
        {
          bullet.Visible = bullet.Enabled = false;
          if (voidShip is VoidShipPC) Sound.Fuzz((voidShip.Shield.Power * 2) - 1f);
          if (voidShip.Shield.Power <= bullet.Power)
          {
            voidShip.Visible = voidShip.Enabled = false;
            voidShip.Dead = true;
            particles.Add(voidShip.Position);
          }
          else
            voidShip.Shield.Power -= bullet.Power;
        }
      }
    }
  }
}
