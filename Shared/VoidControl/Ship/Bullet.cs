using MG.Shared.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.VoidControl.Ship
{
    public class Bullet : Entity
    {
        public float Power;
        public float Decay;
        public Bullet(VoidShip voidShip, Vector2 vector)
        {
            Scale = voidShip.Scale * 2;
            Image = Content.Load<Texture2D>("Art/VoidShip/Star");
            Color = Color.White;
            Power = (voidShip.Weapons.Quality * 0.7f + 0.3f);
            Position = voidShip.Position;
            Position += vector * (voidShip.Radius + Radius);
            Velocity = voidShip.Velocity;
            Velocity += vector * voidShip.Weapons.ShotSpeed;
            Decay = 0.5f * Power * voidShip.Weapons.ShotSpeed / voidShip.Weapons.Range;  // half life at range
        }
        public override void HandleCollision(Entity other) => throw new NotImplementedException();
        public override void Update(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Power -= Decay * (float)gameTime.ElapsedGameTime.TotalSeconds;                                            
            Rotation -= 0.1f;
            if(Rotation < 0)
                Rotation += MathHelper.TwoPi;
            IsExpired |= Power <= 0;
            Color = Power < 0.2f ? Color.Lerp(Color.Transparent, Color.Yellow, Power * 5) : Color.Lerp(Color.Yellow, Color.Pink, (Power - 0.2f) * 1.25f);
        }
    }
}
