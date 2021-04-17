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
        public float TargetRelativePositionSquared, TargetRelativeVelocitySquared;
        public Vector2 TargetRelativePosition, TargetRelativeVelocity;
        protected bool IsPlayer;
        private float a, b, c, determinant, t1, t2;
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
            TargetRelativePosition = IsPlayer ? Vector2.Zero : Target.Position - Position;
            TargetRelativePositionSquared = TargetRelativePosition.LengthSquared();
            IsExpired |= OutOfRange;
            Shield.Power += Reactor.Output;
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
        public bool TargetDetected => Sensor.RangeSquared > TargetRelativePositionSquared;
        public bool TargetInRange => Weapons.RangeSquared > TargetRelativePositionSquared;
        public bool OutOfRange => TargetRelativePositionSquared > MaxShipDistanceSquared;
        public virtual Entity Target
        {
            set { }
            get
            {
                return EntityManager.Player;
            }
        }
        public static float MaxShipDistanceSquared { get; set; }
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
                ApplyThrust();
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
                ApplyThrust();
            }
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public void ApplyThrust()
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
        public void Shoot(Vector2 vector)
        {
            if (Weapons.Delay > 0) return;
            if (Shield.Power < 0.3f) return;
            Bullet bullet = new(this, vector);
            EntityManager.Add(bullet);
            Weapons.Delay += 125;
            Shield.Power -= 0.25f;
        }
        public virtual void AutoShoot()
        {
            if (Weapons.Delay > 0) return;
            if (Shield.Power < 0.3f) return;
            float t = ShotTime();
            if (t < 0.3f) return;
            Bullet bullet = new(this, Vector2.Normalize(TargetRelativePosition + t * TargetRelativeVelocity));
            EntityManager.Add(bullet);
            Weapons.Delay += 125;
            Shield.Power -= 0.25f;
        }
        public float ShotTime()
        {
            TargetRelativeVelocity = Target.Velocity - Velocity;
            TargetRelativeVelocitySquared = TargetRelativeVelocity.LengthSquared();
            if (TargetRelativeVelocitySquared < 0.001f)
                return 0f;
            a = TargetRelativeVelocitySquared - Weapons.ShotSpeed * Weapons.ShotSpeed;
            if (Math.Abs(a) < 0.001f)
            {
                return Math.Max(-TargetRelativePositionSquared / (2f * Vector2.Dot(TargetRelativeVelocity, TargetRelativePosition)), 0f);
            }
            b = 2f * Vector2.Dot(TargetRelativeVelocity, TargetRelativePosition);
            c = TargetRelativePositionSquared;
            determinant = b * b - 4f * a * c;
            if (determinant > 0f)                       //determinant > 0; two intercept paths (most common)
            {
                t1 = (float)((-b + Math.Sqrt(determinant)) / (2f * a));
                t2 = (float)((-b - Math.Sqrt(determinant)) / (2f * a));
                return t2 > t1 && t1 > 0.001f ? t1 : t2 > 0.001f ? t2 : 0;
            }
            else if (determinant < 0f) return 0f;       //determinant < 0; no intercept path
            else return Math.Max(-b / (2f * a), 0f);    //determinant = 0; one intercept path, pretty much never happens
        }
    }
}