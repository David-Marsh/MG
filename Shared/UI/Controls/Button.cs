using Microsoft.Xna.Framework;
using System;

namespace MG.Shared.UI.Controls
{
  public class Button : Label, IMouseControl
  {
    private bool hovering;
    private int Time;           // Remaining time until repeating
    public int Delay;           // Repeat while held down every x mS unless x = 0
    public bool Pressed;
    public bool CanHover;

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
    public Button(int x, int y, int width, int height, string text, Color? back = null, Color? fore = null, bool menu = false) : base(x, y, width, height, text, back, fore)
    {
      CanHover = true;
      Menu = menu;
      Delay = 0;
    }

    public void Fore(Color color) => colors.Init(color, colors.BackNormal, enabled, hovering);
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
      Time = 0;
    }
    public override void Update(GameTime gameTime)
    {
      if (!Pressed) return;
      if (Delay == 0) return;
      Time += gameTime.ElapsedGameTime.Milliseconds;
      while (Time > Delay)
      {
        OnClicked();
        Time -= Delay;
      }
    }
  }
}
