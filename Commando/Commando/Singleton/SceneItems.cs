using System.Collections.Generic;
using Commando.GameScreens;

namespace Commando.Singleton
{
    public sealed class SceneItems : List<IObject>
    {
        private static readonly SceneItems instance = new SceneItems();

        private SceneItems() { }

        public static SceneItems Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
