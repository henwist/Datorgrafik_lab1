﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Components;
using GameEngine.Managers;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Systems
{
    public class CameraSystem : IUdatable
    {
        private static CameraSystem instance;
        public GraphicsDevice device { get; protected set; }
        public CameraComponent camera { get; protected set; }

        public static CameraSystem Instance
        {
            get
            {
                if (instance == null)
                    instance = new CameraSystem();
                return instance;
            }
        }

        private CameraSystem()
        {
            
        }

        public void setUpCamera(Game game, Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUp)
        {
            camera = new CameraComponent(game, cameraPosition, cameraTarget, cameraUp);
        }

        public void Update(GameTime gameTime)
        {
            //List<Component> comps = ComponentManager.GetComponents<CameraComponent>();
            List<ulong> comps = ComponentManager.GetAllEntitiesWithComp<CameraComponent>();

            foreach (ulong c in comps)
            {
                TransformComponent transform = ComponentManager.GetComponent<TransformComponent>(c);
                CameraComponent curCam = ComponentManager.GetComponent<CameraComponent>(c);

                curCam.cameraPosition = transform.position;
                Matrix rotation = Matrix.CreateRotationY(transform.rotation);
                Vector3 transformedRef = Vector3.Transform(curCam.cameraDirection, rotation);
                curCam.viewMatrix = Matrix.CreateLookAt(curCam.cameraPosition, curCam.cameraPosition + transformedRef, Vector3.Up);

                if (Keyboard.GetState().IsKeyDown(Keys.W))
                    curCam.cameraPosition += curCam.cameraDirection * transform.speed;
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                    curCam.cameraPosition -= curCam.cameraDirection * transform.speed;
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    curCam.cameraPosition += Vector3.Cross(curCam.cameraUp, curCam.cameraDirection) * transform.speed;
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    curCam.cameraPosition -= Vector3.Cross(curCam.cameraUp, curCam.cameraDirection) * transform.speed;
                curCam.CreateLookAt();



                //curCam.Update(gameTime);
            }
        }
    }
}
