using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Framework2D.Base.Input
{
    public class InputHandler : GameComponent
    {
        static KeyboardState KeyboardState;
        static KeyboardState LastKeyboardState;
        static GamePadState GamePadState;
        static GamePadState LastGamePadState;


        public InputHandler(Game game) 
            : base(game)
        {
            KeyboardState = Keyboard.GetState();
            GamePadState = GamePad.GetState(PlayerIndex.One);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            LastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
            GamePadState = GamePad.GetState(PlayerIndex.One);
        }

        public static bool KeyReleased(Keys key)
        {            
            return KeyboardState.IsKeyUp(key) &&
                LastKeyboardState.IsKeyDown(key);
        }

        public static bool KeyPressed(Keys key)
        {
            return KeyboardState.IsKeyDown(key) &&
                LastKeyboardState.IsKeyUp(key);
        }

        public static bool KeyDown(Keys key)
        {
            return KeyboardState.IsKeyDown(key);
        }

        public static bool KeyReleased(Buttons key)
        {
            if (GamepadConnected())
            {
                return GamePadState.IsButtonUp(key) &&
                    LastGamePadState.IsButtonDown(key);
            }
            else
            {
                return false;
            }
        }

        public static bool KeyPressed(Buttons key)
        {
            if (GamepadConnected())
            {
                return GamePadState.IsButtonDown(key) &&
                    LastGamePadState.IsButtonUp(key);
            }
            else
            {
                return false;
            }
        }

        public static bool KeyDown(Buttons key)
        {
            if (GamepadConnected())
            {
                return GamePadState.IsButtonDown(key);
            }
            else
            {
                return false;
            }
        }

        public static bool GamepadConnected()
        {
            return GamePadState.IsConnected;
        }
    }
}
