using MG.Shared.Global.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MG.Shared.Cards
{
  public enum Suit { Clubs, Diamonds, Hearts, Spades, None }
  public interface ISCard
  {
    public Suit Suit { get; }
    int Rank { get; }
  }
  public class Standard : FullEntity , ISCard
  {
    public Suit Suit { get; private set; }
    public int Rank { get; private set; }
    public Standard(int ID) : this((Suit)(ID / 13), ID % 13) { }
    public Standard(Suit suit, int rank)
    {
      Suit = (Suit)MathHelper.Clamp((int)suit,0,4);
      Rank = Suit == Suit.None ? MathHelper.Clamp(rank, 1, 2) : MathHelper.Clamp(rank,1,13);
      texture = Content.Load<Texture2D>("Art/Cards");
      sourceRectangle.Width = texture.Width / 13;
      sourceRectangle.Height = texture.Height / 5;
      sourceRectangle.X = sourceRectangle.Width * (Rank - 1);
      sourceRectangle.Y = sourceRectangle.Height * (int)Suit;
      origin = sourceRectangle.Size.ToVector2() * 0.5f;
      position = Vector2.One;
    }
  }
}
