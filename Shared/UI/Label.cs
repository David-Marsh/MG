using MG.Shared.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MG.Shared.UI
{
    public class Label : Control
    {
        public Message Msg;
        public int Margin;

        public Label(Color fore, string text, int col, int row, int colspan, int rowspan) : base(Color.Transparent, fore, col, row, colspan, rowspan)
        {
            Msg.Text = text;
        }
        public Label(Color back, Color fore, string text, int col, int row, int colspan, int rowspan) : base(back, fore, col, row, colspan, rowspan)
        {
            Msg.Text = text;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible) return;
            base.Draw(spriteBatch);
            spriteBatch.DrawString(Msg.Font, Msg.Text, Msg.Position, Colors.Fore, 0, Vector2.Zero, Msg.Zoom, SpriteEffects.None, 0);
        }
        public override void Setup(Panel panel)
        {
            Margin = panel.Padding;
            base.Setup(panel);
        }
        public override void Setup()
        {
            base.Setup();
            ApplyMargin(Margin);
            Msg = new Message(Msg.Text, HitBox);
        }
    }
}
