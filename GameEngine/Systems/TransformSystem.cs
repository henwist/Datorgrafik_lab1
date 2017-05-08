using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Managers;
using GameEngine.Components;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Systems
{
    public class TransformSystem : IUdatable
    {
        private static TransformSystem instance;

        private MouseState prevMouseState;

        private float yaw = 0, pitch = 0, roll = 0f;

        public static TransformSystem Instance
        {
            get
            {
                if (instance == null)
                    instance = new TransformSystem();
                return instance;
            }
        }

        private void Move(ref Vector3 position, Quaternion qRot, float speed)
        {
            position += Vector3.Transform(new Vector3(0, 0, -1), qRot) * speed;
        }


        private void ResetAngle(ref float currentAngle)
        {
            if (currentAngle >= MathHelper.TwoPi)
                currentAngle -= MathHelper.TwoPi;

            if (currentAngle < -MathHelper.TwoPi)
                currentAngle += MathHelper.TwoPi;
        }


        public void Update(GameTime gameTime)
        {
            List<ulong> comps = ComponentManager.GetAllEntitiesWithComp<TransformComponent>();

            foreach (ulong c in comps)
            {
                CameraComponent camera = ComponentManager.GetComponent<CameraComponent>(c);
                TransformComponent transform = ComponentManager.GetComponent<TransformComponent>(c);
                ModelComponent model = ComponentManager.GetComponent<ModelComponent>(c);

                //float leftRightRot = 0;
                //float upDownRot = 0;

                //float turningSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f;
                //turningSpeed += .025f;

                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    //transform.position += transform.speed.X * gameTime.ElapsedGameTime.Milliseconds * model.model.Root.Transform.Right;
                    transform.position += transform.speed.X * gameTime.ElapsedGameTime.Milliseconds * transform.right;
                    //transform.position.X += transform.speed.X * gameTime.ElapsedGameTime.Milliseconds;
                    //leftRightRot += turningSpeed;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    //transform.position = transform.speed.X * gameTime.ElapsedGameTime.Milliseconds * model.model.Root.Transform.Right;
                    transform.position += transform.speed.X * gameTime.ElapsedGameTime.Milliseconds * -1 * transform.right;
                    //transform.position.X -= transform.speed.X * gameTime.ElapsedGameTime.Milliseconds;
                    //leftRightRot -= turningSpeed;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    //transform.position += transform.speed.Z * gameTime.ElapsedGameTime.Milliseconds * model.model.Root.Transform.Forward;
                    transform.position += transform.speed.Z * gameTime.ElapsedGameTime.Milliseconds * transform.forward;
                    //transform.position.Z -= transform.speed.Z * gameTime.ElapsedGameTime.Milliseconds;
                    //upDownRot += turningSpeed;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    //transform.position -= transform.speed.Z * gameTime.ElapsedGameTime.Milliseconds * model.model.Root.Transform.Forward;
                    transform.position += transform.speed.Z * gameTime.ElapsedGameTime.Milliseconds * -1*transform.forward;
                    //transform.position.Z += transform.speed.Z * gameTime.ElapsedGameTime.Milliseconds;
                    //upDownRot -= turningSpeed;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    //transform.position += transform.speed.Y * gameTime.ElapsedGameTime.Milliseconds * model.model.Root.Transform.Down;
                    transform.position += transform.speed.Y * gameTime.ElapsedGameTime.Milliseconds * -1 * transform.up;
                    //transform.position.Y += transform.speed.Y * gameTime.ElapsedGameTime.Milliseconds;
                    //upDownRot += turningSpeed;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                {

                    //transform.position += transform.speed.Y * gameTime.ElapsedGameTime.Milliseconds * model.model.Root.Transform.Down;
                    transform.position += transform.speed.Y * gameTime.ElapsedGameTime.Milliseconds * transform.up;
                    //upDownRot -= turningSpeed;
                }

                Vector3 axis = Vector3.Zero;
                float angle = (float)-gameTime.ElapsedGameTime.TotalMilliseconds * .01f;
                

                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    pitch -= .03f;
                    //axis = new Vector3(1f, 0, 0);
                    //upDownRot += turningSpeed;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    pitch += .03f;
                    //axis = new Vector3(-1f, 0, 0);
                    //upDownRot += turningSpeed;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    roll += .03f;
                    //axis = new Vector3(0, -1f, 0);
                    //upDownRot += turningSpeed;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    roll -= .03f;
                    //axis = new Vector3(0, 1f, 0);
                    //upDownRot += turningSpeed;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad0))
                {
                    yaw += .03f;
                    //axis = new Vector3(0, 1f, 0);
                    //upDownRot += turningSpeed;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad1))
                {
                    yaw -= .03f;
                    //axis = new Vector3(0, 1f, 0);
                    //upDownRot += turningSpeed;
                }

                ResetAngle(ref yaw);
                ResetAngle(ref pitch);
                ResetAngle(ref roll);

                Quaternion extraRot = Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);
                extraRot.Normalize();
                
                transform.qRot = extraRot;
                transform.forward = Vector3.Transform(Vector3.Forward, extraRot);
                transform.up = Vector3.Transform(Vector3.Up, extraRot);
                transform.right = Vector3.Transform(Vector3.Right, extraRot);

                //Quaternion extraRot = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, -1), leftRightRot)
                //                        * Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), upDownRot);
                //Quaternion extraRot = Quaternion.CreateFromAxisAngle(axis, angle);

                //extraRot.Normalize();
                //transform.qRot *= extraRot;

                //float moveSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 500f;
                //Move(ref transform.position, transform.qRot, moveSpeed);

                //if (Keyboard.GetState().IsKeyDown(Keys.W))
                //{
                //    Matrix movement = Matrix.CreateRotationY(transform.rotation);
                //    Vector3 v = new Vector3(0, 0, transform.speed);
                //    v = Vector3.Transform(v, movement);

                //    transform.position.Z += v.Z;
                //    transform.position.Z += v.X;
                //    //camera.cameraPosition += camera.cameraDirection * transform.speed;
                //}
                //if (Keyboard.GetState().IsKeyDown(Keys.S))
                //{

                //    Matrix movement = Matrix.CreateRotationY(transform.rotation);
                //    Vector3 v = new Vector3(0, 0, -transform.speed);
                //    v = Vector3.Transform(v, movement);

                //    transform.position.Z += v.Z;
                //    transform.position.Z += v.X;
                //    //camera.cameraPosition -= camera.cameraDirection * transform.speed;

                //}

                //if (Keyboard.GetState().IsKeyDown(Keys.D))
                //    transform.rotation += transform.speed;
                //    //camera.cameraPosition += Vector3.Cross(camera.cameraUp, camera.cameraDirection) * transform.speed;
                //if (Keyboard.GetState().IsKeyDown(Keys.A))
                //    transform.rotation += transform.speed;
                //    //camera.cameraPosition -= Vector3.Cross(camera.cameraUp, camera.cameraDirection) * transform.speed;

                //camera.CreateLookAt();
            }
        }
    }
}
