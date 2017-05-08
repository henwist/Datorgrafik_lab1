﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Components
{
    public class CameraComponent : Component
    {
        private static Vector3 perspectiveOffset = new Vector3(0, 200, -200);

        public ulong cameraEntity { get; set; }

        public Matrix viewMatrix { get; set; }
        public Matrix projectionMatrix { get; set; }

        public Vector3 cameraPosition { get; set; }
        public Vector3 cameraDirection { get; set; }
        public Vector3 cameraUp { get; set; }


        float sinvalue = 0.1f;

        public CameraComponent(GraphicsDevice gd, Vector3 position, Vector3 target, Vector3 up, ulong id)
        {
            cameraEntity = id;
            cameraPosition = position;
            cameraDirection = target - position;
            cameraDirection.Normalize();
            cameraUp = up;
            CreateLookAt();
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, gd.Viewport.AspectRatio, 1.0f, 50000.0f);
        }

        //public override void Initialize()
        //{
        //    base.Initialize();
        //}

        public void CreateLookAt()
        {
            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraPosition + cameraDirection, cameraUp);
        }
    }
}
