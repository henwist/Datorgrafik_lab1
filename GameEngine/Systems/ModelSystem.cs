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

        //public void Update()
        //{
        //    List<ulong> models = ComponentManager.GetAllEntitiesWithComp<ModelComponent>();

        //    foreach(ulong mC in models)
        //    {
        //        if (!ComponentManager.HasComponent<ChopperComponent>(mC))
        //            continue;
        //        ModelComponent m = ComponentManager.GetComponent<ModelComponent>(mC);
        //        ChopperComponent chopper = ComponentManager.GetComponent<ChopperComponent>(mC);

        //        Vector3 translation = m.chopperMeshWorldMatrices[0].Translation;
        //        Matrix transform = m.model.Meshes[0].ParentBone.Transform;

        //        m.model.Meshes[0].ParentBone.Transform *= -Matrix.CreateTranslation(translation);
        //        //var x = Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(chopper.rotorAngle)) * m.model.Meshes[0].ParentBone.Transform.Rotation;
        //        Quaternion x = m.model.Meshes[0].ParentBone.Transform.Rotation * Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(chopper.rotorAngle));

        //        x.Normalize();
        //        Matrix x2 = Matrix.CreateFromQuaternion(x);
        //        m.chopperMeshWorldMatrices[0] = x2;


        //        var translation2 = m.chopperMeshWorldMatrices[2].Translation;
        //        m.model.Meshes[2].ParentBone.Transform *= -Matrix.CreateTranslation(translation2);

        //        //var y = Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationX(chopper.rotorAngle)) * m.model.Meshes[2].ParentBone.Transform.Rotation;

        //        Quaternion y = m.model.Meshes[2].ParentBone.Transform.Rotation * Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(chopper.rotorAngle));
        //        y.Normalize();

        //        Matrix y2 = Matrix.CreateFromQuaternion(y);
        //        m.chopperMeshWorldMatrices[2] = y2;

        //        chopper.rotorAngle += .03f;
        //    }
        //}

        //public void Update()
        //{
        //}



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

        //int index = 3;
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

                        //be.World = mesh.ParentBone.Transform * Matrix.CreateFromQuaternion(m.chopperMeshWorldMatrices[index].Rotation) * worldMatrix;
                        be.World = mesh.ParentBone.Transform * m.chopperMeshWorldMatrices[index] * worldMatrix;
                        be.View = camera.viewMatrix;
                        be.Projection = camera.projectionMatrix;
                    }
                    mesh.Draw();
                }
            }



            //foreach (ulong m in models)
            //{
            //    if (ComponentManager.HasComponent<ChopperComponent>(m))
            //        DrawChopper();
            //    else
            //    {

            //        ModelComponent model = ComponentManager.GetComponent<ModelComponent>(m);
            //        CameraComponent camera = ComponentManager.GetComponent<CameraComponent>(m);
            //        TransformComponent transform = ComponentManager.GetComponent<TransformComponent>(m);
            //        Matrix[] transforms = new Matrix[model.model.Bones.Count];

            //        Matrix worldMatrix = Matrix.CreateScale(0.05f, 0.05f, 0.05f) *
            //            Matrix.CreateFromQuaternion(transform.qRot) *
            //            Matrix.CreateTranslation(transform.position);


            //        model.model.CopyAbsoluteBoneTransformsTo(transforms);

            //        foreach (ModelMesh mesh in model.model.Meshes)
            //        {
            //            foreach (BasicEffect be in mesh.Effects)
            //            {
            //                be.EnableDefaultLighting();
            //                be.LightingEnabled = true;

            //                be.Projection = camera.projectionMatrix;
            //                System.Diagnostics.Debug.WriteLine(camera.viewMatrix.Translation);
            //                be.View = camera.viewMatrix;
            //                be.World = transforms[mesh.ParentBone.Index] * worldMatrix;

            //                //be.World  = model.world * mesh.ParentBone.Transform * model.translation * model.scale * transform.World;
            //                //index++;
            //            }
            //            mesh.Draw();
            //        }
            //    }
            //    //index = 3;
            //}
        }

        private void DrawChopper()
        {
            List<ulong> choppers = ComponentManager.GetAllEntitiesWithComp<ChopperComponent>();

            foreach (ulong c in choppers)
            {
                ModelComponent m = ComponentManager.GetComponent<ModelComponent>(c);
                CameraComponent cam = ComponentManager.GetComponent<CameraComponent>(c);
                TransformComponent transform = ComponentManager.GetComponent<TransformComponent>(c);
                ChopperComponent chopper = ComponentManager.GetComponent<ChopperComponent>(c);




                Matrix worldMatrix = Matrix.CreateScale(0.05f, 0.05f, 0.05f) *
                    Matrix.CreateFromQuaternion(transform.qRot) *
                    Matrix.CreateTranslation(transform.position);


                for (int index = 0; index < m.model.Meshes.Count; index++)
                {
                    ModelMesh mesh = m.model.Meshes[index];
                    foreach (BasicEffect be in mesh.Effects)
                    {
                        be.EnableDefaultLighting();
                        be.PreferPerPixelLighting = true;

                        be.World = mesh.ParentBone.Transform * m.chopperMeshWorldMatrices[index] * worldMatrix;
                        be.View = cam.viewMatrix;
                        be.Projection = cam.projectionMatrix;
                    }
                    mesh.Draw();
                }
            }
        }

    }
}
