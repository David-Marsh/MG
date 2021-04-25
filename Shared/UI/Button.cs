using MG.Shared.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MG.Shared.UI
{
    public class Button : Control
    {
        public int Margin;
        public Message Msg;
        public int Delay;           // Repeat while held down every x mS unless x = 0
        private int Time;           // Remaining time until repeating
        public bool MenuButton;
        public Button(Color back, Color fore, string text, int col, int row, int colspan, int rowspan, bool canHover = true) : base(back, fore, col, row, colspan, rowspan)
        {
            MenuButton = false;
            Msg.Text = text;
            CanHover = canHover;
            if (canHover)
                MouseEnter += new EventHandler(delegate (object o, EventArgs a) { Sound.Drip(); });
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible) return;
            base.Draw(spriteBatch);
            spriteBatch.DrawString(Msg.Font, Msg.Text, Msg.Position, Colors.Fore, 0, Vector2.Zero, Msg.Zoom, SpriteEffects.None, 0);
        }
        public override void Setup()
        {
            base.Setup();
            ApplyMargin(Margin);
            Msg = new Message(Msg.Text, HitBox, MenuButton);
        }
        public override void Setup(Panel panel)
        {
            Margin = panel.Padding;
            base.Setup(panel);
        }
        public override void Update(GameTime gameTime)
        {
            if (!IsPressed) return;
            if (Delay == 0) return;
            Time += gameTime.ElapsedGameTime.Milliseconds;
            while(Time > Delay)
            {
                OnClicked();
                Time -= Delay;
            }
        }
        internal override void OnMouseUp()
        {
            Time = 0;
            base.OnMouseUp();
        }
    }
}
