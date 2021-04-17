using MG.Shared.Global;
using MG.VoidControl.Ship;
using Microsoft.Xna.Framework;

namespace MG.Shared.VoidControl
{
    public class NonPlayer : VoidShip
    {
        private Vector2 approch;
        public PID PID;
        public NonPlayer() : this(Vector2.Zero) { }
        public NonPlayer(Vector2 position) : base()
        {
            Position = position;

            Reactor.Quality = 0.1f;
            Thruster.Quality = 0.0f;
            Shield.Quality = 0.1f;
            Weapons.Quality = 0.2f;
            Sensor.Quality = 0.0f;
            Cloak.Quality = 0.0f;
            PID = new(0.02f, 0, 3f);
        }
        public NonPlayer(Vector2 position, uint hash, int maskSize, int mask, Point uid) : base()
        {
            Position = position;
            Position += new Vector2(hash & mask, (hash >> maskSize) & mask);
            UID = uid;

            Reactor.Quality = 0.1f * HashToPercent(hash, 1);
            Thruster.Quality = 0.0f * HashToPercent(hash, 3);
            Shield.Quality = 0.1f * HashToPercent(hash, 5);
            Weapons.Quality = 0.2f * HashToPercent(hash, 7);
            Sensor.Quality = 0.0f * HashToPercent(hash, 9);
            Cloak.Quality = 0.0f * HashToPercent(hash, 11);
            PID = new(0.02f, 0, 3f);
        }
        private static float HashToPercent(uint hash, int shift)
        {
            return ((hash >> shift) & 0xFF) / (float)0xFF;
        }
        public Vector2 Approch
        {
            get
            {
                approch = TargetRelativePosition;
                approch -= Vector2.Normalize(approch) * Weapons.Range * 0.9f;
                return approch;
            }
        }
        private Vector2 Avoid => EntityManager.AvoidShips(Position);
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsExpired) return;
            if (!TargetDetected)
            {
                VelocityControl(Vector2.Zero, gameTime);
                return;
            }
            AccelerationControl(Stick(), gameTime);
            if (TargetInRange)
                AutoShoot();
        }
        private Vector2 Stick() => PID.Target(Approch, Avoid);

    }
}
