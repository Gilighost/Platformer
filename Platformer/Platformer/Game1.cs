using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics;
using FarseerPhysics.Dynamics;

namespace Platformer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private World world;

        private MouseState lastMouseState;

        private KeyboardState lastKeyBoardState;

        private int levelKey;

        private Texture2D playerTexture;
        private Texture2D blockTexture;
        private Texture2D enemy1Texture;
        private Texture2D enemy2Texture;

        private Texture2D explosionTexture;

        //background
        private Texture2D blueLines;
        private Texture2D purpleLines;
        private Texture2D goldLines;
        private Vector2 bgLayer1 = Vector2.Zero;
        private Vector2 bgLayer2 = Vector2.Zero;
        private Vector2 bgLayer3 = Vector2.Zero;


        private SpriteFont titleFont;
        private SpriteFont startFont;
        private SpriteFont instructionFont;

        private Explosion explosion;

        //managing player
        private const int STARTING_LIVES = 3;
        private int playerLives;

        private bool gameWon;

        //managing time
        GameTimer gameTimer;

        //score
       
        private int totalScore;
        private const int SCORE_MULTIPLIER = 100;

        //sounds
        private Song backgroundMusic;
        private bool songStart = false;
        private SoundEffect playerJump;
        private SoundEffect playerDie;
        private SoundEffect enemyDie;

        public static float HalfScreenWidth { get; private set; }
        public static float HalfScreenHeight { get; private set; }

        public Game1()
        {
            this.IsMouseVisible = true;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            world = new World(new Vector2(0, 9.82f));


        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            Vector2 topLeft = ConvertUnits.ToSimUnits(new Vector2(0, 0));
            Vector2 bottomRight = ConvertUnits.ToSimUnits(
                new Vector2(GraphicsDevice.Viewport.Width,
                    GraphicsDevice.Viewport.Height));

            HalfScreenWidth = GraphicsDevice.Viewport.Width / 2;
            HalfScreenHeight = GraphicsDevice.Viewport.Height / 2;

            levelKey = 1;
            
            playerLives = STARTING_LIVES;
            gameTimer = new GameTimer();
            gameWon = false;
  
            totalScore = 0;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            playerTexture = Content.Load<Texture2D>(@"Images\record_98px");
            blockTexture = Content.Load<Texture2D>(@"Images\block");
            enemy1Texture = Content.Load<Texture2D>(@"Images\ipod");

            //www.wpclipart.com
            enemy2Texture = Content.Load<Texture2D>(@"Images\redSquare");


            explosionTexture = Content.Load<Texture2D>(@"Images\smoke");
            explosion = new Explosion(explosionTexture);

            titleFont = Content.Load<SpriteFont>(@"Fonts\titleFont");
            instructionFont = Content.Load<SpriteFont>(@"Fonts\instructionFont");
            startFont = Content.Load<SpriteFont>(@"Fonts\startFont");

            //backgtound textures from disco-very.net
            blueLines = Content.Load<Texture2D>(@"Images\blueLines");
            purpleLines = Content.Load<Texture2D>(@"Images\purpleLines");
            goldLines = Content.Load<Texture2D>(@"Images\goldLines");

            //sounds
            //https://www.jamendo.com/en/artist/349632/wma
            backgroundMusic = Content.Load<Song>(@"Sounds\disco");
            MediaPlayer.IsRepeating = true;
            playerDie = Content.Load<SoundEffect>(@"Sounds\PlayerKilled");
            playerJump = Content.Load<SoundEffect>(@"Sounds\PlayerJump");
            enemyDie = Content.Load<SoundEffect>(@"Sounds\EnemyDie");

            LevelReader.Levels.LoadContent(Content, "Levels");

            LevelReader.Levels.ReadInLevelComponents(world, levelKey);
            BuildGameComponents();
        }

        private void BuildGameComponents()
        {
            Camera.Current.StopTracking();

            // Clear the world of previous components
            world.Clear();

            foreach (Component component in LevelReader.Levels.Components)
            {
                if (component is Player)
                {
                    component.BuildComponent(world, playerTexture);
                    Camera.Current.StartTracking(component.Body);
                    component.Body.OnCollision += Player_OnCollision;
                }
                if (component is Block)
                {
                    component.BuildComponent(world, blockTexture);
                }
                if (component is Goal)
                {
                    component.BuildComponent(world, playerTexture);//replace playerTexture with goalTexture
                    Camera.Current.CenterPointTarget = ConvertUnits.ToDisplayUnits(component.Body.Position.X);
                }
                if (component is Enemy1)
                {
                    component.BuildComponent(world, enemy1Texture);
                }
                if (component is Enemy2)
                {
                    component.BuildComponent(world, enemy2Texture);
                }
                //todo:  add more stuff
            }
        }

        private bool Player_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if ((string)fixtureB.Body.UserData == "goal")
            {
                nextLevel();
            }

            else if ((string)fixtureB.Body.UserData == "enemy")
            {
                if (fixtureB.Body.Position.Y > fixtureA.Body.Position.Y + 0.2)
                {
                    explosion.Activate(fixtureB.Body);

                    enemyDie.Play();
                    
                    fixtureA.Body.ApplyLinearImpulse(new Vector2(0, -3));
                    foreach (Component component in LevelReader.Levels.Components)
                    {
                        if (fixtureB.Body.BodyId == component.Body.BodyId)
                        {
                           
                            ((Enemy)component).Die();
                        }
                    }
                    fixtureB.Body.Dispose();
                }
                else
                {
                    explosion.Activate(fixtureA.Body);
                    playerDie.Play();
                    if(playerLives > 0)
                    {
                        playerLives--;
                        resetLevel();
                    }
                    else
                    {
                        resetGame();
                    }
                }
            }

            //only one jump
            if (fixtureB.Body.Position.Y > fixtureA.Body.Position.Y + 0.2)
            {
                foreach (Component component in LevelReader.Levels.Components)
                {
                    if (fixtureA.Body.BodyId == component.Body.BodyId)
                    {
                        if (((Player)component).Airborne == true)
                        {
                            playerJump.Play();
                            ((Player)component).Airborne = false;
                        }
                    }
                }
                
            }

            return true;
        }

        private void nextLevel()
        {
            levelKey++;
            LevelReader.Levels.ReadInLevelComponents(world, levelKey);
            BuildGameComponents();

            totalScore += ((int)gameTimer.timeLeft) * (SCORE_MULTIPLIER * (playerLives + 1));

            gameTimer.resetTimer();


            if (levelKey > 4)
            {
                gameWon = true;
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void HandleKeyPadInput()
        {
            KeyboardState keyBoardState = Keyboard.GetState();

            if (gameWon)
            {
                if (keyBoardState.IsKeyDown(Keys.Enter))
                {
                    resetGame();
                }
                if (keyBoardState.IsKeyDown(Keys.Escape))
                {
                    Exit();
                }
            }
            else
            {

                if ((keyBoardState.IsKeyDown(Keys.LeftShift) || keyBoardState.IsKeyDown(Keys.RightShift)) && keyBoardState.IsKeyDown(Keys.R))
                {
                    resetGame();
                }
                if (keyBoardState.IsKeyDown(Keys.R))
                {
                    resetLevel();
                }
                if (keyBoardState.IsKeyDown(Keys.OemQuestion))
                {
                    foreach (Component component in LevelReader.Levels.Components)
                    {
                        if(component is Enemy)
                        {
                            ((Enemy)component).Die();
                            component.Body.Dispose();
                        }
                    }
                }
            }

            lastKeyBoardState = keyBoardState;
        }

        private void HandleMouseInput()
        {
            MouseState mouseState = Mouse.GetState();

            lastMouseState = mouseState;
        }

        private void resetLevel()
        {
            gameTimer.resetTimer();
            BuildGameComponents();

            foreach (Component component in LevelReader.Levels.Components)
            {
                if (component is Enemy)
                {
                    ((Enemy)component).Revive();
                }
            }
        }
        private void resetGame()
        {
            gameWon = false;
            levelKey = 1;
            playerLives = STARTING_LIVES;
            LevelReader.Levels.ReadInLevelComponents(world, levelKey);
            BuildGameComponents();
          
            totalScore = 0;
           
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            HandleMouseInput();
            HandleKeyPadInput();

            gameTimer.Update(gameTime);

            if (!songStart)
            {
                MediaPlayer.Play(backgroundMusic);
                songStart = true;
            }

            if (gameTimer.timeLeft < 0)
            {
                playerLives--;
                if (playerLives >= 0)
                {
                    resetLevel();
                }
                else
                {
                    resetGame();
                }
            }

            if (LevelReader.Levels.Components != null)
            {
                foreach (Component component in LevelReader.Levels.Components)
                {
                    component.Update();

                    //for parallax
                    if (component is Player)
                    {
                        bgLayer1.X = component.Body.Position.X * -10;
                        bgLayer2.X = component.Body.Position.X * -5;
                        bgLayer3.X = component.Body.Position.Y * 4;

                        bgLayer1.Y = component.Body.Position.Y * 9;
                        bgLayer2.Y = component.Body.Position.X * 5;
                        bgLayer3.Y = component.Body.Position.X * -7;
                    }
                }
            }
            // Update Farseer world
            world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

            // Allow camera to update
            Camera.Current.Update();
            
            
            base.Update(gameTime);
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Indigo);

            //background
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
            if (gameWon)
            {
                spriteBatch.DrawString(titleFont, "You got fine moves baby!", new Vector2(18, 50), Color.Yellow);
                spriteBatch.DrawString(startFont, "Score: " + totalScore.ToString(), new Vector2(200, 150), Color.DeepPink);
                spriteBatch.DrawString(instructionFont, "Press Enter to play again.", new Vector2(200, 210), Color.White);
                spriteBatch.DrawString(instructionFont, "Press Escape to exit.", new Vector2(200, 240), Color.White);
            }
            else
            {
                spriteBatch.Draw(blueLines, new Rectangle((int)bgLayer1.X, (int)bgLayer1.Y, (int)blueLines.Width, (int)blueLines.Width), Color.White);
                spriteBatch.Draw(purpleLines, new Rectangle((int)bgLayer2.X, (int)bgLayer2.Y, (int)purpleLines.Width, (int)purpleLines.Width), Color.White);
                spriteBatch.Draw(goldLines, new Rectangle((int)bgLayer3.X, (int)bgLayer3.Y, (int)goldLines.Width, (int)goldLines.Width), Color.White);
                spriteBatch.End();

                //everything else
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null,
                    null, Camera.Current.TransformationMatrix);

                if (levelKey == 1)
                {
                    spriteBatch.DrawString(titleFont, "DISCO-GO-GO!", new Vector2(460, 150), Color.DeepPink);
                    spriteBatch.DrawString(instructionFont, "By Nathan and Cameron", new Vector2(480, 210), Color.White);
                    spriteBatch.DrawString(startFont, "START ->", new Vector2(800, 265), Color.Yellow);
                }

                if (LevelReader.Levels.Components != null)
                {
                    foreach (Component component in LevelReader.Levels.Components)
                    {
                        component.Draw(spriteBatch);
                    }
                }

                if (levelKey == 1)
                {
                    spriteBatch.DrawString(instructionFont, "How to play:", new Vector2(360, 350), Color.Black);
                    spriteBatch.DrawString(instructionFont, "Use the Left and Right arrow keys to move.", new Vector2(360, 380), Color.Black);
                    spriteBatch.DrawString(instructionFont, "Use Up arrow key to jump.", new Vector2(360, 410), Color.Black);
                    spriteBatch.DrawString(instructionFont, "Land on top of enemies to kill them.", new Vector2(360, 440), Color.Black);
                    spriteBatch.DrawString(instructionFont, "Boogie to the end before times up!", new Vector2(360, 470), Color.Black);
                    spriteBatch.DrawString(instructionFont, "R=Restart level, Shift+R=Restart game.", new Vector2(360, 500), Color.Black);

                }
                else
                {
                    int level = levelKey - 1;
                    spriteBatch.DrawString(instructionFont, "Time: " + gameTimer.timeLeft.ToString(), new Vector2(Camera.Current.offset.X + 10, Camera.Current.offset.Y + 3), Color.Black);
                    spriteBatch.DrawString(instructionFont, "Lives: " + playerLives, new Vector2(Camera.Current.offset.X + 10, Camera.Current.offset.Y + 25), Color.Black);
                    spriteBatch.DrawString(instructionFont, "Level: " + level.ToString(), new Vector2(Camera.Current.offset.X + 10, Camera.Current.offset.Y + 47), Color.Black);
                }

                explosion.Draw(spriteBatch, gameTime);
            }

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}