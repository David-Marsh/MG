using MG.Shared.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.Shared.VoidControl.Ship
{
  public class VoidShipPC : VoidShip
  {
    private Matrix originMatrix;
    private Matrix ScaleOriginMatrix;
    public Matrix PositionMatrix;
    public Matrix ViewMatrix;
    public void OnResize(object sender, EventArgs e = null)
    {
      Input.WindowOrigin = new(((GraphicsDevice)sender).Viewport.Width / 2, ((GraphicsDevice)sender).Viewport.Height / 2);
      originMatrix = Matrix.CreateTranslation(new Vector3(Input.WindowOrigin.ToVector2(), 0.0f));
      ScaleOriginMatrix = Matrix.CreateScale(((GraphicsDevice)sender).Viewport.Height / 2160f) * originMatrix;
    }
    public override void Update(GameTime gameTime, Bullets bullets, VoidShip target, Vector2 average, float maxDistanceSquared)
    {
      if (target != null)
      {
        Weapons.targetRelativePosition = target.Position - position;
        Weapons.targetRelativePositionSquared = Weapons.targetRelativePosition.LengthSquared();
        Weapons.targetRelativeVelocity = target.Velocity - velocity;
      }
      else
      {
        Weapons.targetRelativePosition = Vector2.Zero;
        Weapons.targetRelativePositionSquared = 0;
        Weapons.targetRelativeVelocity = Vector2.Zero;
      }
      Thruster.ControlPC(velocity);
      base.Update(gameTime, bullets);
      Sound.Thrust = Thruster.Thrust;
      PositionMatrix = Matrix.CreateTranslation(new Vector3(-Position, 0.0f));
      ViewMatrix = PositionMatrix * ScaleOriginMatrix;
    }
  }
}
