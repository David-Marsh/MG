using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static MG.Shared.Global.Sprite;

namespace MG.Shared.Global
{
  public class Particles
  {
    public class Particle
    {
      #region Used in draw function
      private Texture2D texture;
      private Vector2 position;
      private Rectangle sourceRectangle;
      private Color color;
      private float rotation;
      private Vector2 origin;
      private Vector2 scale = Vector2.One;
      #endregion

      private Color startingcolor;
      private Vector2 Velocity;
      private float Decay;                            // percent Life lost per update
      private float Life;                             // Remaining Life until IsExpired
      public bool IsExpired;                          // true if it should be deleted.

      public void Init(Vector2 position, Color color, Vector2 scale, Vector2 velocity, float decay)
      {
        texture = Pixel;
        this.position = position;
        sourceRectangle = Pixel.Bounds;
        startingcolor = this.color = color;
        rotation = (velocity == Vector2.Zero) ? 0 : (float)Math.Atan2(velocity.Y, velocity.X);
        origin = Pixel == null ? Vector2.Zero : new Vector2(Pixel.Width, Pixel.Height) * 0.5f;
        this.scale = scale;
        Velocity = velocity;
        Decay = decay;
        Life = 1f;
        IsExpired = false;
      }
      public void Update()
      {
        if (IsExpired) return;
        Life -= Decay;
        IsExpired = Life <= 0;
        position += Velocity;
        color = Color.Lerp(Color.Transparent, startingcolor, Life * Life);
      }
      public void Draw(SpriteBatch spriteBatch)
      {
        if (IsExpired) return;
        spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, SpriteEffects.None, 1f);
      }
    }
    private int start;
    private readonly Particle[] array;
    private static readonly Random rand = new();
    public Particles()
    {
      array = new Particle[0x400];
      for (int i = 0; i < array.Length; i++)
      {
        array[i] = new();
      }
    }

    public int Start
    {
      get { return start; }
      set { start = value & 0x3FF; }
    }
    public int Count { get; set; }

    public static float NextFloat(float minValue, float maxValue)
    {
      return ((float)rand.NextDouble() * (maxValue - minValue)) + minValue;
    }
    public static Vector2 NextVector2(float minLength, float maxLength)
    {
      double theta = rand.NextDouble() * 2 * Math.PI;
      float length = NextFloat(minLength, maxLength);
      return new Vector2(length * (float)Math.Cos(theta), length * (float)Math.Sin(theta));
    }
    public void Add(Vector2 position)
    {
      Vector2 velocity;
      Vector2 scale = Vector2.One * 6;
      for (int i = 0; i < 128; i++)
      {
        velocity = NextVector2(0.1f, 0.5f);
        Add(position, Color.White, scale, velocity, 0.003f);
      }
    }
    public void Add(Vector2 position, Color color, Vector2 scale, Vector2 velocity, float decay)
    {
      array[(start + Count) & 0x3FF].Init(position, color, scale, velocity, decay);
      Count++;
    }
    public void Update()
    {
      while (array[start].IsExpired & Count > 0)
      {
        Count--;
        Start++;
      }
      for (int i = 0; i < Count; i++)
      {
        array[(start + i) & 0x3FF].Update();
      }
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      for (int i = 0; i < Count; i++)
      {
        array[(start + i) & 0x3FF].Draw(spriteBatch);
      }
    }
  }
}
