using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Framework2D.Base
{
    public class ScreenManager : GameComponent
    {
        public event EventHandler OnScreenChange;

        private const int startDrawOrder = 5000;
        private const int drawOrderInc = 100;

        private int drawOrder;
        private Stack<BaseScreen> screens = new Stack<BaseScreen>();

        public BaseScreen CurrentScreen { get { return screens.Peek(); } }

        public ScreenManager(Game game)
            : base(game)
        {
            drawOrder = startDrawOrder;
        }

        private void RemoveScreen()
        {
            BaseScreen screen = screens.Peek();

            OnScreenChange -= screen.ScreenChange;
            Game.Components.Remove(screen);

            screens.Pop();
        }

        public void PushState(BaseScreen newScreen)
        {
            drawOrder += drawOrderInc;
            newScreen.DrawOrder = drawOrder;

            AddScreen(newScreen);

            if (OnScreenChange != null)
                OnScreenChange(this, null);
        }

        private void AddScreen(BaseScreen newScreen)
        {
            screens.Push(newScreen);

            Game.Components.Add(newScreen);

            OnScreenChange += newScreen.ScreenChange;
        }

        public void ChangeScreen(BaseScreen newScreen)
        {
            while (screens.Count > 0)
                RemoveScreen();

            newScreen.DrawOrder = startDrawOrder;
            drawOrder = startDrawOrder;

            AddScreen(newScreen);

            if (OnScreenChange != null)
                OnScreenChange(this, null);
        }
    }
}
