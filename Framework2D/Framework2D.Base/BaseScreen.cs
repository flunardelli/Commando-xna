using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Framework2D.Base
{
    public abstract class BaseScreen : DrawableGameComponent
    {
        public List<GameComponent> Components { get; private set; }

        protected ScreenManager ScreenManager;

        public BaseScreen Tag { get; private set; }

        public BaseScreen(Game game, ScreenManager manager)
            : base(game)
        {
            this.Components = new List<GameComponent>();
            this.ScreenManager = manager;
            this.Tag = this;
        }

        internal protected virtual void ScreenChange(object sender, EventArgs e)
        {
            if (ScreenManager.CurrentScreen == Tag)
                Show();
            else
                Hide();
        }

        protected virtual void Show()
        {
            this.Visible = true;
            this.Enabled = true;

            foreach (GameComponent component in Components)
            {
                component.Enabled = true;

                DrawableGameComponent drawableGameComponent = component as DrawableGameComponent;
                if (drawableGameComponent != null)
                    drawableGameComponent.Visible = true;
            }
        }

        protected virtual void Hide()
        {
            this.Visible = false;
            this.Enabled = false;

            foreach (GameComponent component in Components)
            {
                component.Enabled = false;

                DrawableGameComponent drawableGameComponent = component as DrawableGameComponent;
                if (drawableGameComponent != null)
                    drawableGameComponent.Visible = false;
            }
        }
    }
}
