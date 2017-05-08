using GameEngine.Systems;
using GameEngine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using GameEngine.Objects;
using Microsoft.Xna.Framework.Content;

namespace GameEngine.Managers
{
    public class SceneManager
    {

        private GraphicsDevice gd;

        private List<HeightmapObject> heightmapObjects;

        private HeightmapSystem heightmapSystem;

        private Matrix world;

        public SceneManager(GraphicsDevice gd, Matrix world)
        {
            this.gd = gd;
            this.world = world;

            heightmapObjects = new List<HeightmapObject>();
            createHeightmapObjects();

            heightmapSystem = new HeightmapSystem(gd, heightmapObjects);

        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(BasicEffect effect, GameTime gameTime)
        {
            heightmapSystem.Draw(effect);
        }

        private void LoadComponents()
        {

        }

        private void createHeightmapObjects()
        {
            HeightmapObject hmobj = new HeightmapObject();
            hmobj.scaleFactor = 0.5f*Vector3.One;
            hmobj.position = Vector3.Zero;
            hmobj.terrainMapName = "..\\..\\..\\..\\Content\\Textures\\US_Canyon.png";
            hmobj.textureName = "..\\..\\..\\..\\Content\\Textures\\grass.png";
            hmobj.objectWorld = Matrix.Identity;
            hmobj.world = Matrix.Identity;
            heightmapObjects.Add(hmobj);

        }

    }
}