using MG.Shared.Global.Entities;
using Microsoft.Xna.Framework;
using System;

namespace MG.Shared.VoidControl.Ship.SubSystems
{
    public class SubSystem : FullEntityChild
    {

        private float quality;

        public SubSystem(float scale, float quality)
        {
            Scale = scale;
            Quality = quality;
        }

        public virtual float Scale { get => scale.X; set => scale = Vector2.One * value; }
        public virtual float Quality { get => quality; set => quality = Math.Clamp(value, 0, 1); }
    }
}
