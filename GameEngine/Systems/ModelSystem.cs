using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Components;
using GameEngine.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Systems
{
    public class ModelSystem : ISysDrawable
    {
        private static ModelSystem instance;

        
        //public CameraComponent camera;
        

        public static ModelSystem Instance
        {
            get
            {
                if (instance == null)
                    instance = new ModelSystem();
                return instance;
            }
        }

        public void LoadContent()
        {
            
        }

        //int index = 3;
        public void Draw(BasicEffect effect, GameTime gametime)
        {
            List<ulong> models = ComponentManager.GetAllEntitiesWithComp<ModelComponent>();

            foreach (ulong m in models)
            {
                ModelComponent model = ComponentManager.GetComponent<ModelComponent>(m);
                CameraComponent camera = ComponentManager.GetComponent<CameraComponent>(m);
                TransformComponent transform = ComponentManager.GetComponent<TransformComponent>(m);
                Matrix[] transforms = new Matrix[model.model.Bones.Count];

                Matrix worldMatrix = Matrix.CreateScale(0.05f, 0.05f, 0.05f) *
                    Matrix.CreateFromQuaternion(transform.qRot) *
                    Matrix.CreateTranslation(transform.position);


                model.model.CopyAbsoluteBoneTransformsTo(transforms);

                foreach (ModelMesh mesh in model.model.Meshes)
                {
                    foreach (BasicEffect be in mesh.Effects)
                    {
                        be.EnableDefaultLighting();
                        be.LightingEnabled = true;

                        be.Projection = camera.projectionMatrix;
                        System.Diagnostics.Debug.WriteLine(camera.viewMatrix.Translation);
                        be.View = camera.viewMatrix;
                        be.World = transforms[mesh.ParentBone.Index] * worldMatrix;

                        //be.World  = model.world * mesh.ParentBone.Transform * model.translation * model.scale * transform.World;
                        //index++;
                    }
                    mesh.Draw();
                }
                //index = 3;
            }
        }



    }
}
