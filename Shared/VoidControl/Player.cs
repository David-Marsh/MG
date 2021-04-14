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
        public void OnResize(object sender, EventArgs e)
        {
            WindowOrigin = new(((GraphicsDeviceManager)sender).PreferredBackBufferWidth / 2, ((GraphicsDeviceManager)sender).PreferredBackBufferHeight / 2);
            OriginMatrix = Matrix.CreateTranslation(new Vector3(WindowOrigin.ToVector2(), 0.0f));
        }
        public Matrix ViewMatrix() => PositionMatrix * OriginMatrix;
        public override Vector2 Target => Position + (Input.Position - WindowOrigin).ToVector2();   // Where the mouse is pointed but in world space
        private static bool Trigger => !Input.MouseOnUI & Input.MouseLeft(ButtonState.Pressed);
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            VelocityControl(Input.Stick(), gameTime);
            Sound.Thrust = Thruster.Thrust;
            if (Trigger)
                Shoot(gameTime);
            PositionMatrix = Matrix.CreateTranslation(new Vector3(-Position, 0.0f));
        }
    }
}
