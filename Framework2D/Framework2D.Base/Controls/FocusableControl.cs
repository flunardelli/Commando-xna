using Microsoft.Xna.Framework;
using System;
namespace Framework2D.Base.Controls
{
    public abstract class FocusableControl : Control
    {
        public Color SelectedColor { get; set; }
        public bool HasFocus { get; set; }

        public event EventHandler Selected;

        public abstract bool CanFocus();
        public abstract void HandleInput();

        public FocusableControl()
        {
            this.SelectedColor = Color.Red;
        }

        protected virtual void OnSelected(EventArgs e)
        {
            if (Selected != null)
            {
                Selected(this, e);
            }
        }
    }
}
