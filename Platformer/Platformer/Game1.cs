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
        private SpriteBatch _batch;
        private KeyboardState _oldKeyState;
        private GamePadState _oldPadState;

        private Texture2D blockSprite;

        private List<Block> blocks;

        private Player player;

        private World _world;

        // Simple camera controls
        private Matrix _view;
        private Vector2 _cameraPosition;
        private Vector2 _screenCenter;

        #endregion

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 480;

            Content.RootDirectory = "Content";

            //Create a world with gravity.
            _world = new World(new Vector2(0, 9.82f));
            player = null;

            blocks = new List<Block>();
        }


        protected override void LoadContent()
        {
            // Initialize camera controls
            _view = Matrix.Identity;
            _cameraPosition = Vector2.Zero;
            _screenCenter = new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2f, _graphics.GraphicsDevice.Viewport.Height / 2f);
            _batch = new SpriteBatch(_graphics.GraphicsDevice);

            blockSprite = Content.Load<Texture2D>(@"Images\block");

            //LevelReader finds all files in Content/Levels and parses each file into a jagged char array,
            //then creates a dictionary pairing each jagged array with an int key
            LevelReader.LoadLevelContent<String>(Content, "Levels");

            

            // Farseer expects objects to be scaled to MKS (meters, kilos, seconds)
            // 1 meters equals 64 pixels here
            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);
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
            if(player != null)
            {
                player.getPlayerMovement();
            }
            //We update the world
            _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);


            base.Update(gameTime);
        }

        private void HandleGamePad()
        {
            GamePadState padState = GamePad.GetState(0);

            if (padState.IsConnected)
            {
                if (padState.Buttons.Back == ButtonState.Pressed)
                    Exit();

                _oldPadState = padState;
            }
        }
        private void HandleKeyboard()
        {
            KeyboardState state = Keyboard.GetState();

            //test code for loading a map
            if (state.IsKeyDown(Keys.L) && !(_oldKeyState.IsKeyDown(Keys.L)))
            {
                blocks.Clear();
                
                int insertKeyHere = 3;
                for (int i = 0; i < LevelReader.levelContent[insertKeyHere].Length; i++) //lines
                {
                    for (int j = 0; j < LevelReader.levelContent[insertKeyHere][i].Length; j++) //characters
                    {

                        if (LevelReader.levelContent[insertKeyHere][i][j] == 'P')
                        {
                            player = new Player(_world, Content, new Vector2(j, i));
                        }

                        if (LevelReader.levelContent[insertKeyHere][i][j] == '#')
                        {
                            Block block = new Block(_world, blockSprite, new Vector2(j,i));
                            blocks.Add(block);
                        }
                    }
                }
               
            }

            // Move camera

            if (state.IsKeyDown(Keys.A))
                _cameraPosition.X += 4f;

            if (state.IsKeyDown(Keys.D))
                _cameraPosition.X -= 4f;

            if (state.IsKeyDown(Keys.W))
                _cameraPosition.Y += 4f;

            if (state.IsKeyDown(Keys.S))
                _cameraPosition.Y -= 4f;

            _view = Matrix.CreateTranslation(new Vector3(_cameraPosition - _screenCenter, 0f)) * Matrix.CreateTranslation(new Vector3(_screenCenter, 0f));

            if (state.IsKeyDown(Keys.Escape))
                Exit();

            _oldKeyState = state;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Draw circle and ground
            _batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _view);

            foreach (Block block in blocks)
            {
                _batch.Draw(block.Sprite, ConvertUnits.ToDisplayUnits(block.Origin), null, Color.White, block.Rotation, block.Origin, 1f, SpriteEffects.None, 0f);
            }
            if (player != null)
            {
                player.Draw(_batch);
            }
            
            _batch.End();

            base.Draw(gameTime);
        }
    }
}