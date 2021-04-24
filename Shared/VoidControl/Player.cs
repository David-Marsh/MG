using MG.Shared.Global;
using MG.VoidControl.Ship;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace MG.Shared.VoidControl
{
    public class Player : VoidShip
    {
        public Matrix OriginMatrix;
        public Matrix PositionMatrix;
        private Point WindowOrigin;
        private bool autoAim;
        private bool autoGun;

        public Player() : base() 
        {
            IsPlayer = true;
            Reactor.Quality = 1.0f;
            Thruster.Quality = 0.2f;
            Shield.Quality = 1.0f;
            Weapons.Quality = 0.1f;
            Sensor.Quality = 0.0f;
            Cloak.Quality = 0.0f;
        }
        public void Respawn()
        {
            Position = Vector2.Zero;
            Velocity = Vector2.Zero;
            IsExpired = false;
            Shield.Power = 1;
        }
        public void OnResize(object sender, EventArgs e)
        {
            WindowOrigin = new(((GraphicsDeviceManager)sender).PreferredBackBufferWidth / 2, ((GraphicsDeviceManager)sender).PreferredBackBufferHeight / 2);
            OriginMatrix = Matrix.CreateTranslation(new Vector3(WindowOrigin.ToVector2(), 0.0f));
        }
        public Matrix ViewMatrix() => PositionMatrix * OriginMatrix;
        public Vector2 MouseTarget => Position + (Input.Position - WindowOrigin).ToVector2();   // Where the mouse is pointed but in world space
        private static bool Trigger => !Input.MouseOnUI & Input.MouseLeft(ButtonState.Pressed);
        public override Entity Target { get; set; }
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            VelocityControl(Input.Stick(), gameTime);
            PositionMatrix = Matrix.CreateTranslation(new Vector3(-Position, 0.0f));
            Sound.Thrust = Thruster.Thrust;
            if ((AutoGun) | (AutoAim & Trigger)) AutoShoot();
            else if (Trigger) Shoot(Vector2.Normalize(MouseTarget));
        }

        public override void AutoShoot()
        {
            Target = EntityManager.NearestShip();
            if (Target is null) return;
            TargetRelativePosition = Target.Position - Position;
            TargetRelativePositionSquared = TargetRelativePosition.LengthSquared();
            if (TargetInRange) base.AutoShoot();
        }
    }
}
