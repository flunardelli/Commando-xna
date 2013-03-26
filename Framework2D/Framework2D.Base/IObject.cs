using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Commando.GameScreens
{
    public interface IObject
    {
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
    }
}
