﻿using GameEngine.Components;
using GameEngine.Managers;
using GameEngine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Datorgrafik_lab1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;

        BasicEffect effect;

        private float angle = 0f;

        private Matrix _view, _projection;

        private Vector3 cameraPosition = new Vector3(200.0f, 200.0f, 100.0f);

        float radx = 0f;
        float scale = 1f;
        float rotation = 0.1f;

        Texture2D grass;

        private SceneManager sceneManager;
        private ModelSystem modelSystem;
        private CameraSystem cameraSystem;
        private TransformSystem transformSystem;

        private Controller controller;
        private float scaleMove = 1.0f; //used to scale movements if move is to fast or slow.

        private ulong CHOPPERID = 1;

        public CameraComponent camera { get; protected set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            cameraSystem = CameraSystem.Instance;
            modelSystem = ModelSystem.Instance;
            transformSystem = TransformSystem.Instance;

            cameraSystem.setUpCamera(this, cameraPosition, Vector3.Zero, Vector3.Up);
            modelSystem.camera = cameraSystem.camera;

            createGameEntity();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            device = graphics.GraphicsDevice;

            grass = Content.Load<Texture2D>("Textures/grass");

            _view = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            _projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 50000f);

            setEffectOptions();

            sceneManager = new SceneManager(graphics.GraphicsDevice, effect.World);

            controller = new Controller(scaleMove);

        }


        private void setEffectOptions()
        {
            effect = new BasicEffect(graphics.GraphicsDevice);

            effect.World = Matrix.Identity;
            effect.View = _view;
            effect.Projection = _projection;
            effect.PreferPerPixelLighting = true;

            effect.EnableDefaultLighting();
            effect.LightingEnabled = true;
            effect.TextureEnabled = true;
            effect.Texture = grass;

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            rs.FillMode = FillMode.Solid;
            device.RasterizerState = rs;
        }


        protected override void UnloadContent()
        {
        }



        private void rotateRotors()
        {
            Quaternion q;

            Quaternion quaternion_main = Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(rotation));
            quaternion_main.Normalize();

            Quaternion quaternion_tail = Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(rotation));
            quaternion_tail.Normalize();

            rotation += 0.00001f;

            ModelComponent chopper = ComponentManager.GetComponent<ModelComponent>(CHOPPERID);
            CameraComponent camera = ComponentManager.GetComponent<CameraComponent>(CHOPPERID);
            TransformComponent transform = ComponentManager.GetComponent<TransformComponent>(CHOPPERID);

            foreach (ModelMesh mesh in chopper.model.Meshes)
            {

                if (mesh.ParentBone.Index == (int)meshindex.main_rotor)
                {
                    q = mesh.ParentBone.Transform.Rotation * quaternion_main;
                    q.Normalize();
                    mesh.ParentBone.Transform = Matrix.CreateTranslation(mesh.ParentBone.Transform.Translation)
                                               * Matrix.CreateFromQuaternion(q);
                }

                if (mesh.ParentBone.Index == (int)meshindex.tail_rotor)
                {
                    q = mesh.ParentBone.Transform.Rotation * quaternion_tail;
                    q.Normalize();
                    mesh.ParentBone.Transform =
                                               Matrix.CreateFromQuaternion(q)
                                               * Matrix.CreateTranslation(mesh.ParentBone.Transform.Translation);
                }

            }

        }


        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            angle += 0.005f;


            transformSystem.Update(gameTime);
            modelSystem.camera = cameraSystem.camera;
            cameraSystem.Update(gameTime);

            rotateRotors();

                base.Update(gameTime);


            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                cameraPosition.X += 1.0f;

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                cameraPosition.X -= 1.0f;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                cameraPosition.Y += 1.0f;

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                cameraPosition.Y -= 1.0f;

            if (Keyboard.GetState().IsKeyDown(Keys.A))
                radx += 0.1f;

            //if (Keyboard.GetState().IsKeyDown(Keys.S))
            //    rady += 0.1f;

            if (Keyboard.GetState().IsKeyDown(Keys.D))
                radx += 0.1f;

            //if (Keyboard.GetState().IsKeyDown(Keys.W))
            //    scale += 0.01f;


            if (Keyboard.GetState().IsKeyDown(Keys.E))
                scale -= 0.01f;
        }


        protected override void Draw(GameTime gameTime)
        {
            device.Clear(Color.DarkSlateBlue);


            modelSystem.Draw(effect, gameTime);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up) * Matrix.CreateTranslation(-radx, 0, 0);

                sceneManager.Draw(effect, gameTime);
            }

            base.Draw(gameTime);
        }

        public ulong createGameEntity()
        {
            ulong id = ComponentManager.GetNewId();
            TransformComponent transform = new TransformComponent(new Vector3(200.0f, 300.0f, 100.0f), 0f, 10f);

            Model model = Content.Load<Model>(@"Models/Chopper");
            
            ComponentManager.StoreComponent(id, CameraSystem.Instance.camera);
            ComponentManager.StoreComponent(id, new ModelComponent(GraphicsDevice, model) {world = Matrix.Identity });
            ComponentManager.StoreComponent(id, transform);
            //ComponentManager.StoreComponent(id, Controller);

            return id;
        }



        private enum meshindex : int
        {
           main_rotor = 1, 
           tail_rotor = 3
        }
    }
}
