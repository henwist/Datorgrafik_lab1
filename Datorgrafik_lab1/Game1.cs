using GameEngine.Components;
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

        private Matrix _view, _projection;

        private Vector3 cameraPosition = new Vector3(200.0f, 200.0f, 100.0f);

        Texture2D grass;

        private SceneManager sceneManager;
        private ModelSystem modelSystem;
        private TransformSystem transformSystem;

        private ulong CHOPPERID = 1;
        private float CHOPPER_SCALE = 1f;
        private Vector3 CHOPPER_TRANSLATION = new Vector3(150, 150, 100);

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

            modelSystem = ModelSystem.Instance;
            transformSystem = TransformSystem.Instance;

            createGameEntities();

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


        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            foreach (Keys k in Keyboard.GetState().GetPressedKeys())
                if (k.Equals(Keys.Escape))
                    this.Exit();

            transformSystem.Update(gameTime);
            ModelSystem.Instance.Update();
            CameraSystem.Instance.Update(gameTime);

           base.Update(gameTime);

        }


        protected override void Draw(GameTime gameTime)
        {
            device.Clear(Color.DarkSlateBlue);

            Window.Title = "Controller keys: a,d,s,w, LShift, Space, Arrows, Numpad1, Numpad0. Av: Rasmus Lundquist (S142465) och Henrik Wistbacka(S142066)";

            modelSystem.Draw(effect, gameTime);
            sceneManager.Draw(effect, gameTime);

            CameraComponent camera = ComponentManager.GetComponent<CameraComponent>(CHOPPERID);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                effect.View = camera.viewMatrix;
            }

            base.Draw(gameTime);
        }


        public void createGameEntities()
        {
            ulong id = ComponentManager.GetNewId();
            ChopperComponent chopper = new ChopperComponent();
            TransformComponent transform = new TransformComponent(new Vector3(200.0f, 300.0f, 100.0f), 0f, 10f);

            Model model = Content.Load<Model>(@"Models/Chopper");
            CameraComponent camera = new CameraComponent(graphics.GraphicsDevice, cameraPosition, Vector3.Zero, Vector3.Up, id);

            ComponentManager.StoreComponent(id, camera);
            ComponentManager.StoreComponent(id, new ModelComponent(model, CHOPPER_SCALE, CHOPPER_TRANSLATION,
                                                                   0f, 0f, MathHelper.PiOver2, true) { world = Matrix.Identity });
            ComponentManager.StoreComponent(id, transform);
            ComponentManager.StoreComponent(id, chopper);
        }
    }
}
