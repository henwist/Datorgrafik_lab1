using Microsoft.Xna.Framework;
using GameEngine.Components;
using GameEngine.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Systems
{
    public class DrawSystem : IUdatable
    {

        private SpriteBatch spriteBatch;


        public DrawSystem(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}