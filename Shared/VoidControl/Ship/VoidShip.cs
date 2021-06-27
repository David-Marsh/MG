using MG.Shared.Global.Entities;
using MG.Shared.VoidControl.Ship.SubSystems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.Shared.VoidControl.Ship
{
  public class VoidShip : FullEntityChild, IComparable<VoidShip>
  {
    public enum State { Visable = 0, Enabled = 1, Disabled = 2 }

    protected Vector2 velocity;

    public ECM ECM;
    public Reactor Reactor;
    public Sensor Sensor;
    public Shield Shield;
    public Thruster Thruster;
    public Weapons Weapons;

    #region AI
    protected float targetRelativePositionSquared, targetRelativeVelocitySquared;
    protected Vector2 targetRelativePosition, targetRelativeVelocity;
    #endregion

    public Vector2 Velocity { get => velocity; set => velocity = value; }
    public float Scale { get => scale.X; }
    public float Radius;
    public Point UID = Point.Zero;
    public bool OutOfZone, Dead;
    public VoidShip() : this(0.5f) { }
    public VoidShip(float scale)
    {
      this.scale = Vector2.One * scale;
      Texture = Content.Load<Texture2D>("Art/VoidShip/VoidShip");
      Radius = Math.Max(origin.X, origin.Y) * scale;
      ECM = new(scale, 0.0f);
      Reactor = new(scale, 1f);
      Sensor = new(scale, 0.0f);
      Shield = new(scale, 1f);
      Thruster = new(scale, 1f);
      Weapons = new(scale, 1f);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      ECM.Draw(spriteBatch, position, rotation);
      Reactor.Draw(spriteBatch, position, rotation);
      Sensor.Draw(spriteBatch, position, rotation);
      Shield.Draw(spriteBatch, position, rotation);
      Thruster.Draw(spriteBatch, position, rotation);
      Weapons.Draw(spriteBatch, position, rotation);
      base.Draw(spriteBatch);
    }
    public virtual void Update(GameTime gameTime, Bullets bullets, VoidShip voidShip, Vector2 average, float maxDistanceSquared) => throw new NotImplementedException();
    public virtual void Update(GameTime gameTime, Bullets bullets)
    {
      Reactor.Update(gameTime);
      Shield.Update(Reactor);
      Thruster.Update(gameTime, this);
      Weapons.Update(gameTime, this, bullets);
      ECM.Update();
      Sensor.Update(gameTime);
      SetRotation(Thruster.Thrust);
    }

    #region IComparable
    public int CompareTo(VoidShip other) => throw new NotImplementedException();
    public static bool operator <(VoidShip left, VoidShip right) => left.CompareTo(right) < 0;
    public static bool operator <=(VoidShip left, VoidShip right) => left.CompareTo(right) <= 0;
    public static bool operator >(VoidShip left, VoidShip right) => left.CompareTo(right) > 0;
    public static bool operator >=(VoidShip left, VoidShip right) => left.CompareTo(right) >= 0;

    #endregion
  }
}
