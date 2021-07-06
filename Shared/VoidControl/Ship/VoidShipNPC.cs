using MG.Shared.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MG.Shared.VoidControl.Ship
{
  public class VoidShipNPC : VoidShip
  {
    private Vector2 approch, avoid;
    public PID PID = new(0.02f, 0, 3f);
    public void Init(Point uid, Vector2 position, uint hash, float quality)
    {
      Dead = false;
      UID = uid;
      this.position = position;
      Reactor.Quality = quality * HashToPercent(hash, 1);
      Thruster.Quality = quality * HashToPercent(hash, 3);
      Shield.Quality = quality * HashToPercent(hash, 5);
      Weapons.Quality = quality * HashToPercent(hash, 7);
      Sensor.Quality = quality * HashToPercent(hash, 9);
      ECM.Quality = quality * HashToPercent(hash, 11);
    }
    private static float HashToPercent(uint hash, int shift) => ((hash >> shift) & 0xFF) / (float)0xFF;
    public override void Update(GameTime gameTime, Bullets bullets, VoidShip target, Vector2 average, float maxDistanceSquared)
    {
      targetRelativePosition = target.Position - Position;
      targetRelativePositionSquared = targetRelativePosition.LengthSquared();
      OutOfZone = targetRelativePositionSquared > maxDistanceSquared;
      if (OutOfZone | Dead)
      {
        enabled = visible = false;
        return;
      }
      enabled = Sensor.RangeSquared > targetRelativePositionSquared;
      visible = target.Sensor.RangeSquared > targetRelativePositionSquared;
      if (!enabled) return;
      targetRelativeVelocity = target.Velocity - velocity;
      float inaccuracy = MathHelper.Clamp(target.ECM.Quality - Sensor.Quality, 0, 1f);
      targetRelativeVelocity.X += inaccuracy * ((gameTime.TotalGameTime.Ticks & 1023) - 512);
      targetRelativeVelocity.Y += inaccuracy * (((gameTime.TotalGameTime.Ticks >> 10) & 1023) - 512);
      approch = targetRelativePosition;
      approch -= Vector2.Normalize(approch) * Weapons.Range * 0.9f;
      avoid = position - average;
      avoid = 10000 * Vector2.One / avoid;
      Thruster.ControlNPC(velocity, PID.Target(approch + avoid));
      Weapons.ControlNPC(targetRelativePosition, targetRelativeVelocity);
      base.Update(gameTime, bullets);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);
      if (!PID.Visable) return;
      Rectangle destinationRectangle = new(0, 0, 8, 8);
      destinationRectangle.Location = (Position + avoid).ToPoint();
      spriteBatch.Draw(Pixel, destinationRectangle, Color.Red);
      destinationRectangle.Location = (Position + approch).ToPoint();
      spriteBatch.Draw(Pixel, destinationRectangle, Color.Green);
    }
  }
}
