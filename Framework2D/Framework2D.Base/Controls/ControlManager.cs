using System.Collections.Generic;
using System.Linq;
using Framework2D.Base.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Framework2D.Base.Controls
{
    public class ControlManager : List<Control>
    {
        public static SpriteFont SpriteFont { get; private set; }

        private int selectedIndex = 0;

        public ControlManager(SpriteFont spriteFont)
            : base()
        {
            ControlManager.SpriteFont = spriteFont;
        }

        public void Update(GameTime gameTime)
        {
            if (Count == 0)
                return;

            foreach (Control c in this)
            {
                if (c.Enabled)
                    c.Update(gameTime);
            }

            foreach (Control c in this)
            {
                FocusableControl focusableControl = c as FocusableControl;
                if (focusableControl != null && focusableControl.HasFocus)
                {
                    focusableControl.HandleInput();
                    break;
                }
            }

            if (InputHandler.KeyPressed(Keys.Down))
                NextControl();

            if (InputHandler.KeyPressed(Keys.Up))
                PreviousControl();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Control c in this)
            {
                if (c.Visible)
                    c.Draw(gameTime, spriteBatch);
            }
        }

        public void NextControl()
        {
            if (this.OfType<FocusableControl>().ToList().Count == 0)
                return;

            FocusableControl control;

            control = this[selectedIndex] as FocusableControl;
            if (control != null)
                control.HasFocus = false;

            int currentIndex = selectedIndex;
            do
            {
                selectedIndex++;

                if (selectedIndex == this.Count)
                    selectedIndex = 0;

                control = this[selectedIndex] as FocusableControl;
                if (control != null && control.CanFocus())
                {
                    break;
                }

            } while (currentIndex != selectedIndex);

            control.HasFocus = true;
        }

        public void PreviousControl()
        {
            if (this.OfType<FocusableControl>().ToList().Count == 0)
                return;

            FocusableControl control;

            control = this[selectedIndex] as FocusableControl;
            if (control != null)
                control.HasFocus = false;

            int currentIndex = selectedIndex;

            do
            {
                selectedIndex--;

                if (selectedIndex < 0)
                    selectedIndex = this.Count - 1;

                control = this[selectedIndex] as FocusableControl;
                if (control != null && control.CanFocus())
                {
                    break;
                }

            } while (currentIndex != selectedIndex);

            control.HasFocus = true;
        }
    }
}
