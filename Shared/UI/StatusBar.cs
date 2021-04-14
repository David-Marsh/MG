using MG.Shared.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static MG.Shared.Global.Sprite;

namespace MG.Shared.UI
{
    public class StatusBar : Control
    {
        private float value;
        public int Margin;
        private Rectangle Bar;
        private Rectangle BarBox;
        public Message Msg;
        public float Value
        {
            get => value; set
            {
                if (this.value == value) return;
                this.value = Math.Clamp(value, 0, 1);
                Bar.Width = (int)(BarBox.Width * Value);
            }
        }
        public StatusBar(Color back, Color fore, string text, int col, int row, int colspan, int rowspan) : base(back, fore, col, row, colspan, rowspan)
        {
            Msg.Text = text;
            MouseEnter += new EventHandler(delegate (object o, EventArgs a) { Sound.Drip(); });
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible) return;
            base.Draw(spriteBatch);
            spriteBatch.DrawString(Msg.Font, Msg.Text, Msg.Position, Colors.Fore, 0, Vector2.Zero, Msg.Zoom, SpriteEffects.None, 0);
            if (Enabled)
            {
                spriteBatch.Draw(Pixel, BarBox, Color.Red);
                spriteBatch.Draw(Pixel, Bar, Color.Lime);
            }
        }
        public override void Setup()
        {
            base.Setup();
            ApplyMargin(Margin);
            Msg = new Message(Msg.Text, new Rectangle(HitBox.Location, new Point(HitBox.Width, (int)(HitBox.Height * 0.6f))));
            BarBox = new Rectangle(HitBox.Location + new Point(0, (int)(HitBox.Height * 0.6f)), HitBox.Size - new Point(0, (int)(HitBox.Height * 0.7f)));
            Bar = new Rectangle(BarBox.Location, BarBox.Size) { Width = (int)(BarBox.Width * Value) };
        }
        public override void Setup(Panel panel)
        {
            base.Setup(panel);
            Margin = panel.Padding;
            Setup();
        }
        public event EventHandler<PushStatusEventArgs> PushStatus;
        //internal override void OnMouseDown()
        //{
        //    base.OnMouseDown();
        //    if (BarBox.Contains(Input.Position)) PushStatus?.Invoke(this, new PushStatusEventArgs() { Value = (Input.Position.X - BarBox.X) / (float)BarBox.Width });
        //}
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsPressed)
                if (BarBox.Contains(Input.Position)) PushStatus?.Invoke(this, new PushStatusEventArgs() { Value = (Input.Position.X - BarBox.X) / (float)BarBox.Width });
        }
    }
    public class PushStatusEventArgs : EventArgs
    {
        public float Value { get; set; }
    }
}
