﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckGame
{
    


    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Player player;
        public Timer timer;

        public KeyboardState currentKeyboardState;
        public KeyboardState previousKeyboardState;

        public List<GameObject> objectsInScene;

        Texture2D background;

        public GamePadState currentGamePadState;
        public GamePadState previousGamePadState;

        MouseState currentMouseState;
        MouseState previousMouseState;

        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            objectsInScene = new List<GameObject>();
            player = new Player();
            timer = new Timer(this);
            
            objectsInScene.Add(player);
            objectsInScene.Add(timer);

            TouchPanel.EnabledGestures = GestureType.FreeDrag;


            // I think this always goes last?
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

            Animation playerAnimation = new Animation();
            Texture2D playerTexture = Content.Load<Texture2D>("player");
            playerAnimation.Initialize(playerTexture, Vector2.Zero, 32, 32, 1, 1000, Color.White, 1f, true );

            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Start(this, playerAnimation, playerPosition);
            
            background = Content.Load<Texture2D>("bg_arena");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;

            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            foreach (GameObject go in objectsInScene)
            {
                go.Update(gameTime);
            }

            CheckCollisions();

            //UpdatePlayer(gameTime);


            base.Update(gameTime);
        }

        private void CheckCollisions()
        {
            // Detect collisions
            for (int i = 0; i < objectsInScene.Count; i++)
            {
                ICollideable possibleCollideable = objectsInScene[i] as ICollideable;

                if (possibleCollideable == null) continue;

                for (int j = i + 1; i < objectsInScene.Count; i++)
                {

                    ICollideable secondCollideable = objectsInScene[j] as ICollideable;
                    if (secondCollideable == null) continue;
                    else
                    {
                        if (possibleCollideable.BoundingBox.Intersects(secondCollideable.BoundingBox))
                        {
                            possibleCollideable.Collided(objectsInScene[j]);
                        }
                    }

                }
            }
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            player.Update(gameTime);

            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.White);
            // player.Draw(spriteBatch);

            foreach (GameObject go in objectsInScene)
            {
                go.Draw(spriteBatch);
            }

            spriteBatch.End();
            

            base.Draw(gameTime);
        }

        public GameObject FindGameObjectByTag(string tag)
        {
            foreach (GameObject go in objectsInScene)
            {
                if (go.tag == tag)
                {
                    return go;
                }
            }

            return null;
        }

        public List<GameObject> FindGameObjectsByTag(string tag)
        {
            List<GameObject> objects = new List<GameObject>();
            foreach (GameObject go in objectsInScene)
            {
                if (go.tag == tag)
                {
                    objects.Add(go);
                }
            }

            return objects;
        }

        public void Reset()
        {
            timer.playerTime = 0f;
            player.Position = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            // Figure out how to reset game here
        }
    }
}
