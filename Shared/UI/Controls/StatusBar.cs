using MG.Shared.Global;
using MG.Shared.Global.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.Shared.UI.Controls
{
  public class StatusBar : Rectangle, IMouseControl
  {
    private bool hovering;
    private SpriteFont font;
    private string text;
    private Vector2 textPosition, textZoom;
    private Microsoft.Xna.Framework.Rectangle Bar, BarBox;
    private float value;
    public bool Pressed;
    public bool CanHover;
    public float Value
    {
      get => value; set
      {
        if (this.value == value) return;
        this.value = Math.Clamp(value, 0, 1);
        Bar.Width = (int)(BarBox.Width * Value);
      }
    }
    public string Text
    {
      get => text; set
      {
        text = value;
        PlaceText();
      }
    }
    protected bool Hovering
    {
      get => hovering; set
      {
        hovering = value;
        colors.Set(Enabled, hovering);
      }
    }
    public event EventHandler Clicked;
    public event EventHandler MouseDown;
    public event EventHandler MouseUp;
    public event EventHandler MouseLeave;
    public event EventHandler MouseEnter;
    public event EventHandler<PushStatusEventArgs> PushStatus;
    public StatusBar(int x, int y, int width, int height, string text, ColorScheme colors) : base(x, y, width, height, colors)
    {
      CanHover = true;
      this.text = text;
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      if (!Visable) return;
      spriteBatch.Draw(texture, destinationRectangle, Colors.Back);
      spriteBatch.DrawString(font, text, textPosition, colors.Fore, 0, Vector2.Zero, textZoom, SpriteEffects.None, 0);
      spriteBatch.Draw(Pixel, BarBox, Color.Red);
      spriteBatch.Draw(Pixel, Bar, Color.Lime);
    }
    public virtual void OnClicked() => Clicked?.Invoke(this, new EventArgs());
    public virtual void OnMouseDown()
    {
      if (!Pressed) Clicked?.Invoke(this, new EventArgs());
      Pressed = true;
      MouseDown?.Invoke(this, new EventArgs());
    }
    public virtual void OnMouseEnter()
    {
      Hovering = CanHover;
      MouseEnter?.Invoke(this, new EventArgs());
    }
    public virtual void OnMouseLeave()
    {
      Hovering = Pressed = false;
      MouseLeave?.Invoke(this, new EventArgs());
    }
    public virtual void OnMouseUp()
    {
      Pressed = false;
      MouseUp?.Invoke(this, new EventArgs());
    }
    private void PlaceText()
    {
      Vector2 textSize;
      font = Sprite.Font;
      textSize = font.MeasureString(text);
      textZoom.X = textZoom.Y = MathHelper.Min(destinationRectangle.Width / textSize.X, 0.6f * destinationRectangle.Height / textSize.Y) * 0.95f;
      textPosition = destinationRectangle.Location.ToVector2();
      textPosition.X += 0.5f * (destinationRectangle.Width - (textSize.X * textZoom.X));
      textPosition.Y += 0.5f * ((0.6f * destinationRectangle.Height) - (textSize.Y * textZoom.Y));
    }
    public override void SizeTo(BasicEntityParent parent)
    {
      base.SizeTo(parent);
      PlaceText();
      BarBox = destinationRectangle;
      BarBox.Y += (int)(destinationRectangle.Height * 0.6f);
      BarBox.Height -= (int)(destinationRectangle.Height * 0.7f);
      Bar = BarBox;
      Bar.Width = (int)(BarBox.Width * Value);
    }
    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (Pressed)
        if (BarBox.Contains(Input.Position)) PushStatus?.Invoke(this, new PushStatusEventArgs() { Value = (Input.Position.X - BarBox.X) / (float)BarBox.Width });
    }
  }
  public class PushStatusEventArgs : EventArgs
  {
    public float Value { get; set; }
  }
}
