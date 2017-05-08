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


        public void Update()
        {
            float rotation = 0.1f;

            Quaternion q;

            Quaternion quaternion = Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(rotation));
            quaternion.Normalize();

            ModelComponent chopper = ComponentManager.GetComponent<ModelComponent>(1);

            foreach (ModelMesh mesh in chopper.model.Meshes)
            {

                if (mesh.ParentBone.Index == 3 || mesh.ParentBone.Index == 1)
                {
                    q = mesh.ParentBone.Transform.Rotation * quaternion;
                    q.Normalize();
                    mesh.ParentBone.Transform = Matrix.CreateFromQuaternion(q)
                                                * Matrix.CreateTranslation(mesh.ParentBone.Transform.Translation);
                                               
                }

                if (rotation >= MathHelper.TwoPi)
                    rotation -= MathHelper.TwoPi;

            }
        }


        public void Draw(BasicEffect effect, GameTime gametime)
        {
            List<ulong> models = ComponentManager.GetAllEntitiesWithComp<ModelComponent>();

            foreach(ulong mC in models)
            {
                ModelComponent m = ComponentManager.GetComponent<ModelComponent>(mC);
                CameraComponent camera = ComponentManager.GetComponent<CameraComponent>(mC);
                TransformComponent transform = ComponentManager.GetComponent<TransformComponent>(mC);
                Matrix[] transforms = new Matrix[m.model.Bones.Count];

                Matrix worldMatrix = Matrix.CreateScale(0.05f, 0.05f, 0.05f) *
                    Matrix.CreateFromQuaternion(transform.qRot) *
                    Matrix.CreateTranslation(transform.position);


                m.model.CopyAbsoluteBoneTransformsTo(transforms);

                for (int index = 0; index < m.model.Meshes.Count; index++)
                {
                    ModelMesh mesh = m.model.Meshes[index];
                    foreach (BasicEffect be in mesh.Effects)
                    {
                        be.EnableDefaultLighting();
                        be.PreferPerPixelLighting = true;

                        be.World = mesh.ParentBone.Transform * m.chopperMeshWorldMatrices[index] * worldMatrix;
                        be.View = camera.viewMatrix;
                        be.Projection = camera.projectionMatrix;
                    }
                    mesh.Draw();
                }
            }
        }

    }
}
