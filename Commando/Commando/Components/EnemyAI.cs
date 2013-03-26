using Commando.GameScreens;
using Commando.Singleton;
using Microsoft.Xna.Framework;

namespace Commando.Components
{
    public class EnemyAI
    {
        public static Player PlayerClosest(Enemy enemy, float visionRadius)
        {
            Player playerDetected = null;
            float playerDist = float.MaxValue;

            foreach (IObject item in SceneItems.Instance)
            {
                Player player = item as Player;

                if (player != null)
                {
                    float dist = Vector2.Distance(enemy.Sprite.Position, player.Sprite.Position);

                    if (dist < visionRadius)
                    {
                        if (playerDetected == null || dist < playerDist)
                        {
                            playerDetected = player;
                            playerDist = dist;
                        }
                    }
                }
            }

            return playerDetected;
        }
    }
}
