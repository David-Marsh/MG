using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using MG.Shared.Global;

namespace MG.Shared.UI
{
    public abstract class ControlManager : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Control hoverControl;
        public List<Control> Controls { get; private set; }
        protected ControlManager(Game game) : base(game)
        {
            Controls = new List<Control>();
        }
        protected override void LoadContent()
        {
            base.LoadContent();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Sprite.CreateThePixel(GraphicsDevice);
            Sprite.CreateTheFonts(Game);
        }
        public abstract void Setup(object sender, EventArgs e);     // Override to setup all controls size and positions
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (hoverControl != FindControlAt(Input.Position))
            {
                hoverControl?.OnMouseLeave();
                hoverControl = FindControlAt(Input.Position); 
                hoverControl?.OnMouseEnter();
                Input.MouseOnUI = hoverControl != null;
            }
            if(Input.MouseOnUI)
            {
                if (Input.OneShotMouseLeft(ButtonState.Pressed)) hoverControl?.OnMouseDown();
                if (Input.OneShotMouseLeft(ButtonState.Released)) hoverControl?.OnMouseUp();
            }
            foreach (var control in Controls) control.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Begin();
            foreach (var control in Controls) control.Draw(spriteBatch);
            spriteBatch.End();
        }
        private Control FindControlAt(Point position)
        {
            var control = Controls.LastOrDefault(c => c.Contains(position));
            if (control is Panel panel)
                return panel.FindControlAt(position);
            else
                return control;
        }
    }
}
