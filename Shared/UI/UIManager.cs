using MG.Shared.Global;
using MG.Shared.Global.Entities;
using MG.Shared.UI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MG.Shared.UI
{
    public class UIManager : DrawableGameComponent
    {
        protected SpriteBatch spriteBatch;
        private IMouseControl hoverControl;
        private IMouseControl HoverControl
        {
            get => hoverControl; set
            {
                if (hoverControl != value)
                {
                    hoverControl?.OnMouseLeave();
                    hoverControl = value;
                    hoverControl?.OnMouseEnter();
                    Input.MouseOnUI = HoverControl != null;
                }
            }
        }
        public List<BasicEntityChild> Children { get; private set; }
        public UIManager(Game game) : base(game)
        {
            Children = new();
        }
        public override void Update(GameTime gameTime)
        {
            if (!Enabled) return;
            base.Update(gameTime);
            HoverControl = FindControlAt(Input.Position);
            if (Input.MouseOnUI)
            {
                if (Input.OneShotMouseLeft(ButtonState.Pressed)) HoverControl?.OnMouseDown();
                if (Input.OneShotMouseLeft(ButtonState.Released)) HoverControl?.OnMouseUp();
            }
            foreach (var control in Children) control.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            if (!Visible) return;
            base.Draw(gameTime);
            spriteBatch.Begin();
            foreach (var control in Children) control.Draw(spriteBatch);
            spriteBatch.End();
        }
        private IMouseControl FindControlAt(Point position)
        {
            if(!Enabled) return null;
            var child = Children.LastOrDefault(c => c.Contains(position));
            if (child is UIPanel panel)
                return panel.FindControlAt(position);
            else if (child is IMouseControl mouseControl)
                return mouseControl;
            else
                return null;
        }
        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            BasicEntityChild.CreateThePixel(GraphicsDevice);
            Sprite.CreateThePixel(GraphicsDevice);
            Sprite.CreateTheFonts(Game);
            base.Initialize();
        }
        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            HoverControl = null;
            base.OnEnabledChanged(sender, args);
        }
    }
}
