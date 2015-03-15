using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



namespace Platformer
{
    public class Game1 : Game
    {
        #region Private Members
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        private KeyboardState oldKeyState;
        private GamePadState oldPadState;

        private List<Component> components;

        private World world;

        public static Texture2D blockTexture;
        public static Texture2D playerTexture;

        public static float HalfScreenWidth { get; private set; }

        private int levelKey;

        #endregion

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 480;

            Content.RootDirectory = "Content";

            //Create a world with gravity.
            world = new World(new Vector2(0, 9.82f));
        }

        protected override void Initialize()
        {
            levelKey = 1;

            HalfScreenWidth = _graphics.GraphicsDevice.Viewport.Width / 2;
            components = new List<Component>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //LevelReader finds all files in Content/Levels and parses each file into a jagged char array,
            //then creates a dictionary pairing each jagged array with an int key
            LevelReader.LoadLevelContent<String>(Content, "Levels");

            // Images

            blockTexture = Content.Load<Texture2D>(@"Images\block");
            playerTexture = Content.Load<Texture2D>(@"Images\record_98px");

            // Farseer expects objects to be scaled to MKS (meters, kilos, seconds)
            // 1 meters equals 64 pixels here
            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);
        }

        private void CreateGameComponents(int levelKey)
        {
            Camera.Current.StopTracking();
            components.Clear();
            components = LevelReader.ReadInLevelComponents(world, levelKey);
            foreach (Component component in components)
            {
                if (component is Player)
                {
                    Camera.Current.StartTracking(component.Body);
                }
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            HandleGamePad();
            HandleKeyboard();

            if (components != null)
            {
                foreach(Component component in components)
                {
                    component.Update(gameTime);
                }
            }

            //We update the world
            world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);


            base.Update(gameTime);
        }

        private void HandleGamePad()
        {
            GamePadState padState = GamePad.GetState(0);

            if (padState.IsConnected)
            {
                if (padState.Buttons.Back == ButtonState.Pressed)
                    Exit();

                oldPadState = padState;
            }
        }
        private void HandleKeyboard()
        {
            KeyboardState state = Keyboard.GetState();

            // Rudimentary level select

            if (state.IsKeyDown(Keys.D1))
                levelKey = 1;

            if (state.IsKeyDown(Keys.D1))
                levelKey = 2;

            if (state.IsKeyDown(Keys.D1))
                levelKey = 3;

            // Load level onto screen

            if (state.IsKeyDown(Keys.L) && !oldKeyState.IsKeyDown(Keys.L))
            {
                CreateGameComponents(levelKey);
            }

            // Exit

            if (state.IsKeyDown(Keys.Escape))
                Exit();

            oldKeyState = state;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Draw circle and ground
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.Current.TransformationMatrix);

            if (components != null)
            {
                foreach (Component component in components)
                {
                    component.Draw(spriteBatch);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}