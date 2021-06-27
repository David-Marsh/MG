using MG.Shared.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.CompilerServices;

namespace MG.Shared.VoidControl.Ship
{
  public class Ships
  {
    private int total;
    private readonly VoidShip[] array;
    private VoidShip swap;
    private Point cellUID;
    private int cellBits, cellMask, cellSize, maxDistance;
    public float maxDistanceSquared;
    public int CellBits
    {
      get => cellBits; set
      {
        cellBits = value;
        cellSize = 1 << cellBits;
        cellMask = cellSize - 1;
        maxDistance = cellSize * cellBits;
        maxDistanceSquared = (float)Math.Pow(maxDistance, 2);
      }
    }
    public Point CellUID
    {
      get => cellUID; set
      {
        value.X &= ~cellMask;
        value.Y &= ~cellMask;
        if (cellUID == value) return;
        cellUID = value;
        Spawn();
      }
    }
    public VoidShipPC Player { get => (VoidShipPC)array[0]; set => array[0] = value; }
    public int Visable { get; set; }
    public int Enabled { get; set; }
    public Ships()
    {
      array = new VoidShip[0x400];
      array[0] = new VoidShipPC();
      Enabled = Visable = total = 1;
      for (int i = 1; i < array.Length; i++)
      {
        array[i] = new VoidShipNPC();
      }
      CellBits = 11;
      cellUID = new Point(1, 1);
    }
    public Vector2 Average()
    {
      if (Visable <= 1)
        return Vector2.Zero;
      Vector2 average = Vector2.Zero;
      for (int i = 1; i < Visable; i++)
      {
        average += array[i].Position;
      }
      average.X /= Visable;
      average.Y /= Visable;
      return average;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      for (int i = 0; i < Visable; i++)
      {
        if (!array[i].Visible) continue;
        array[i].Draw(spriteBatch);
      }
    }
    internal void DrawMap(SpriteBatch spriteBatch, Rectangle shipDot)
    {
      spriteBatch.Draw(Sprite.Pixel, shipDot, Color.Lime);
      for (int i = 1; i < total; i++)
      {
        if (array[i].Dead) continue;
        shipDot.Location = array[i].Position.ToPoint();
        spriteBatch.Draw(Sprite.Pixel, shipDot, Color.Red);
      }
    }
    #region Add Remove Spawn
    private void Spawn()
    {
      Point point, dist;
      for (point.X = -maxDistance; point.X <= maxDistance; point.X += cellSize)
      {
        for (point.Y = -maxDistance; point.Y <= maxDistance; point.Y += cellSize)
        {
          dist = cellUID - point;
          float distSquared = (dist.X * dist.X) + (dist.Y * dist.Y);
          if (distSquared <= maxDistanceSquared)
          {
            if (!Array.Exists(array, ship => ship.UID == point))
            {
              uint hash = SpawnPointHash(point);
              if ((hash & 0x00100100) == 0x00100100)
              {
                Vector2 position = point.ToVector2();
                position.X += hash & cellMask;
                position.Y += (hash >> cellBits) & cellMask;
                ((VoidShipNPC)array[total++]).Init(point, position, hash, 0);
              }
            }
          }
        }
      }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static uint SpawnPointHash(Point point)
    {
      uint hash = 1337;
      hash ^= 1619 * BitConverter.ToUInt32(BitConverter.GetBytes(point.Y), 0);
      hash ^= System.Numerics.BitOperations.RotateRight(hash, 13);
      hash ^= 31337 * BitConverter.ToUInt32(BitConverter.GetBytes(point.X), 0);
      hash ^= System.Numerics.BitOperations.RotateLeft(hash, 23);
      hash = hash * hash * hash * 60493;
      hash = System.Numerics.BitOperations.RotateRight(hash, 13) ^ hash;
      return hash;
    }
    #endregion
    public VoidShip Nearest()
    {
      VoidShip nearest = null;
      float distSquared, mindistSquared = array[0].Weapons.RangeSquared;
      for (int i = 1; i < Visable; i++)
      {
        distSquared = (array[i].Position - array[0].Position).LengthSquared();
        if (mindistSquared < distSquared) continue;
        nearest = array[i];
        mindistSquared = distSquared;
      }
      return nearest;
    }
    public void Sort()
    {
      for (int i = 1; i < total; i++)
      {
        if (array[i].Visible)
        {
          if (i >= Visable)
          {
            if (i > Visable)
            {
              swap = array[i];
              array[i] = array[Visable];
              array[Visable] = swap;
            }
            Visable++;
            if (i >= Enabled)
              Enabled++;
          }
        }
        else if (array[i].Enabled)
        {
          if (i < Visable)
          {
            Visable--;
            if (i < Visable)
            {
              swap = array[i];
              array[i] = array[Visable];
              array[Visable] = swap;
            }
            i--;
          }
          else if (i >= Enabled)
          {
            if (i > Enabled)
            {
              swap = array[i];
              array[i] = array[Enabled];
              array[Enabled] = swap;
            }
            Enabled++;
          }
        }
        else if (array[i].OutOfZone)
        {
          if (i < total)
          {
            total--;
            if (i < total)
            {
              swap = array[i];
              array[i] = array[total];
              array[total] = swap;
            }
            i--;
          }
        }
        else
        {
          if (i < Visable)
          {
            Visable--;
            if (i < Visable)
            {
              swap = array[i];
              array[i] = array[Visable];
              array[Visable] = swap;
            }
            i--;
          }
          else if (i < Enabled)
          {
            Enabled--;
            if (i < Enabled)
            {
              swap = array[i];
              array[i] = array[Enabled];
              array[Enabled] = swap;
            }
            i--;
          }

        }
      }
    }
    public void Update(GameTime gameTime, Bullets bullets, Particles particles)
    {
      Vector2 average = Average();
      array[0].Update(gameTime, bullets, Nearest(), average, maxDistanceSquared);
      for (int i = 1; i < total; i++) array[i].Update(gameTime, bullets, array[0], average, maxDistanceSquared);
      Sort();
      for (int i = 0; i < Visable; i++) bullets.HandleCollision(array[i], particles);
      CellUID = array[0].Position.ToPoint();
    }
  }
}
