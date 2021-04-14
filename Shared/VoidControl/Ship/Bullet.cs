using MG.Shared.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.VoidControl.Ship
{
    public class Bullet : Entity
    {
        public float Power;
        public Bullet(VoidShip voidShip, Vector2 target) : this(voidShip,target,1) { }
        public Bullet(VoidShip voidShip, Vector2 target, float scale)
        {
            Scale = scale;
            Image = Content.Load<Texture2D>("Art/VoidShip/Star");
            Power = (voidShip.Weapons.Quality * 0.7f + 0.3f);
            Position = voidShip.Position;
            Velocity = Vector2.Normalize(target - voidShip.Position);
            Position += Velocity * (voidShip.Radius + Radius);
            Velocity *= 10;
            Color = Color.White;
        }
        public override void HandleCollision(Entity other) => throw new NotImplementedException();
        public override void Update(GameTime gameTime)
        {
            Position += Velocity;
            Power -= 0.00390625f;                                            // 4.2 seconds at 60 updates per second
            Rotation -= 0.1f;
            if(Rotation < 0)
                Rotation += MathHelper.TwoPi;
            IsExpired |= Power <= 0;
            color.PackedValue -= (uint)0x00010101;
        }
    }
}
