using MG.Shared.Global;
using MG.Shared.VoidControl;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.VoidControl.Ship
{
    public class VoidShip : Entity
    {
        public Cloak Cloak;
        public Reactor Reactor;
        public Sensor Sensor;
        public Shield Shield;
        public Thruster Thruster;
        public Weapons Weapons;
        public Point UID = Point.Zero;

        private Vector2 approch;
        private int ShotDelay;
        private float TargetDistanceSquared;
        public static float MaxShipDistanceSquared;
        protected bool IsPlayer;
        public VoidShip() : this( 0.5f) { }
        public VoidShip(float scale)
        {
            Scale = scale;
            Image = Content.Load<Texture2D>("Art/VoidShip/VoidShip");
            Cloak = new Cloak(Content, scale, 0.0f);
            Reactor = new Reactor(Content, scale, 0.1f);
            Sensor = new Sensor(Content, scale, 0.0f);
            Shield = new Shield(Content, scale, 0.1f);
            Thruster = new Thruster(Content, scale, 0.0f);
            Weapons = new Weapons(Content, scale, 0.2f);
        }
        public override void Update(GameTime gameTime)
        {
            TargetDistanceSquared = IsPlayer ? 0 : (Target - Position).LengthSquared();
            IsExpired |= OutOfRange;
            Shield.Power += Reactor.Output;
            if (ShotDelay > 0) ShotDelay -= gameTime.ElapsedGameTime.Milliseconds;
            Reactor.Update(gameTime);
            Shield.Update(gameTime);
            Weapons.Update(gameTime);
            Thruster.Update(gameTime);
            Cloak.Update(gameTime);
            Sensor.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Cloak.Draw(spriteBatch, Position, Rotation);
            Reactor.Draw(spriteBatch, Position, Rotation);
            Sensor.Draw(spriteBatch, Position, Rotation);
            Shield.Draw(spriteBatch, Position, Rotation);
            Thruster.Draw(spriteBatch, Position, Rotation);
            Weapons.Draw(spriteBatch, Position, Rotation);
            base.Draw(spriteBatch);
        }
        public virtual void SetRotation() => SetRotation(Velocity);
        public virtual void SetRotation(Vector2 direction) => Rotation = (direction == Vector2.Zero) ? Rotation : (float)Math.Atan2(direction.Y, direction.X);
        public override void HandleCollision(Entity other)
        {
            Vector2 collisionNormal = Vector2.Normalize(this.Position - other.Position);
            if (Vector2.Dot(collisionNormal, this.Velocity) > 0 || Vector2.Dot(collisionNormal, other.Velocity) > 0) return;
            Vector2 ThisInline = collisionNormal * Vector2.Dot(collisionNormal, this.Velocity);           // Velocity inline with point of contact
            Vector2 OtherInline = collisionNormal * Vector2.Dot(collisionNormal, other.Velocity);         // Velocity inline with point of contact
            this.Velocity += OtherInline - ThisInline;
            other.Velocity += ThisInline - OtherInline;
        }
        public void HandleCollision(Bullet bullet)
        {
            bullet.IsExpired = true;                                    // Bullet always expires on collision
            if (IsPlayer) Sound.Fuzz((Shield.Power * 2) - 1f);
            if (Shield.Power > bullet.Power)                            // What has more power
                Shield.Power -= bullet.Power;                           // Decrease sheild power base on remaining bullet power
            else
                IsExpired = true;                                       // Destroy ship
        }
        public bool TargetDetected => Sensor.RangeSquared > TargetDistanceSquared;
        public bool TargetInRange => Weapons.RangeSquared > TargetDistanceSquared;
        public bool OutOfRange => TargetDistanceSquared > MaxShipDistanceSquared;
        public virtual Vector2 Target => EntityManager.Player.Position;
        public Vector2 Approch
        {
            get
            {
                approch = Target - position;
                approch -= Vector2.Normalize(approch) * Weapons.Range * 0.9f;
                return approch;
            }
        }
        public void VelocityControl(Vector2 stick, GameTime gameTime)
        {
            Vector2 VelocityRequest = stick * Thruster.MaxVelocity;
            Thruster.Thrust = VelocityRequest - Velocity;
            if (Thruster.Thrust != Vector2.Zero)
            {
                float maxDV = Thruster.MaxThrust * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Thruster.Thrust.LengthSquared() > maxDV * maxDV)                                    // Limit thrust to max thrust
                {
                    Thruster.Thrust = Vector2.Normalize(Thruster.Thrust) * maxDV;
                }
                ApplyThrust(Thruster.Thrust);
            }
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public void AccelerationControl(Vector2 stick, GameTime gameTime)
        {
            Thruster.Thrust = stick * Thruster.MaxThrust * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Thruster.Thrust != Vector2.Zero)
            {
                if (Velocity.LengthSquared() > Thruster.MaxVelocity * Thruster.MaxVelocity)
                {
                    Thruster.Thrust = Vector2.Zero;
                    Velocity = Vector2.Normalize(Velocity) * Thruster.MaxVelocity;
                }
                ApplyThrust(Thruster.Thrust);
            }
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public void ApplyThrust(Vector2 thrust)
        {
            Thruster.Cost = Thruster.Thrust.Length() * Thruster.CostFactor;
            if (Thruster.Cost > Shield.Power)
            {
                Thruster.Thrust = Vector2.Zero;
                Thruster.Cost = 0;
            }
            else
            {
                Shield.Power -= Thruster.Cost;
                Velocity += Thruster.Thrust;
                SetRotation(Thruster.Thrust);
            }
        }
        public void Shoot(GameTime gameTime)
        {
            if (ShotDelay > 0) return;
            ShotDelay += 125;
            if (Shield.Power < 0.3f) return;
            Shield.Power -= 0.25f;
            Bullet bullet = new(this, Target);
            EntityManager.Add(bullet);
        }
    }
}