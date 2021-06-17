using MG.Shared.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.Shared.VoidControl.Ship
{
    public class VoidShipPC : VoidShip
    {
        private Matrix originMatrix;
        public Matrix PositionMatrix;
        public Matrix ViewMatrix;
        public void OnResize(object sender, EventArgs e = null)
        {
            Weapons.WindowOrigin = new(((GraphicsDevice)sender).Viewport.Width / 2, ((GraphicsDevice)sender).Viewport.Height / 2);
            originMatrix = Matrix.CreateTranslation(new Vector3(Weapons.WindowOrigin.ToVector2(), 0.0f));
        }
        public override void Update(GameTime gameTime, Bullets bullets, VoidShip target, Vector2 average, float maxDistanceSquared)
        {
            if (target != null)
            {
                targetRelativePosition = target.Position - position;
                targetRelativeVelocity = target.Velocity - velocity;
            }
            else
                targetRelativePosition = Vector2.Zero;
            Thruster.ControlPC(velocity);
            Weapons.ControlPC(targetRelativePosition, targetRelativeVelocity);
            base.Update(gameTime, bullets);
            Sound.Thrust = Thruster.Thrust;
            PositionMatrix = Matrix.CreateTranslation(new Vector3(-Position, 0.0f));
            ViewMatrix = PositionMatrix * originMatrix;
        }
    }
}
