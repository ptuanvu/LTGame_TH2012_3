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

namespace GoingBeyond
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Camera/View information
        Vector3 cameraPosition = new Vector3(0.0f, 0.0f, GameConstants.CameraHeight);
        float aspectRatio;
        Matrix projectionMatrix;
        Matrix viewMatrix;

        //Set the sound effects to use
        SoundEffect soundEngine;
        SoundEffectInstance soundEngineInstance;
        SoundEffect soundHyperspaceActivation;
        // Cue so we can hang on to the sound of the engine.
        //Cue engineSound = null;

        //Visual components
        Ship ship = new Ship();
        Model asteroidModel;
        Matrix[] asteroidTransforms;
        Asteroid[] asteroidList = new Asteroid[GameConstants.NumAsteroids];
        Random random = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            aspectRatio = (float)GraphicsDeviceManager.DefaultBackBufferWidth / GraphicsDeviceManager.DefaultBackBufferHeight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //audioEngine = new AudioEngine("Content\\Audio\\MyGameAudio.xgs");
            //waveBank = new WaveBank(audioEngine, "Content\\Audio\\Wave Bank.xwb");
            //soundBank = new SoundBank(audioEngine, "Content\\Audio\\Sound Bank.xsb");

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
              MathHelper.ToRadians(45.0f), aspectRatio,
              GameConstants.CameraHeight - 1000.0f,
              GameConstants.CameraHeight + 1000.0f);

            viewMatrix = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            ResetAsteroids();

            IsMouseVisible = true;
            base.Initialize();
        }


        private Matrix[] SetupEffectDefaults(Model myModel)
        {
            Matrix[] absoluteTransforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(absoluteTransforms);

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Projection = projectionMatrix;
                    effect.View = viewMatrix;
                }
            }
            return absoluteTransforms;
        }

        private void ResetAsteroids()
        {
            float xStart;
            float yStart;
            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                if (random.Next(2) == 0)
                {
                    xStart = (float)-GameConstants.PlayfieldSizeX;
                }
                else
                {
                    xStart = (float)GameConstants.PlayfieldSizeX;
                }
                yStart = (float)random.NextDouble() * GameConstants.PlayfieldSizeY;
                asteroidList[i].position = new Vector3(xStart, yStart, 0.0f);
                double angle = random.NextDouble() * 2 * Math.PI;
                asteroidList[i].direction.X = -(float)Math.Sin(angle);
                asteroidList[i].direction.Y = (float)Math.Cos(angle);
                asteroidList[i].speed = GameConstants.AsteroidMinSpeed +
                   (float)random.NextDouble() * GameConstants.AsteroidMaxSpeed;
            }
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load the ship
            ship.Model = Content.Load<Model>("Models/p1_wedge");
            ship.Transforms = SetupEffectDefaults(ship.Model);

            //Load asteroid
            asteroidModel = Content.Load<Model>("Models\\asteroid1");
            asteroidTransforms = SetupEffectDefaults(asteroidModel);

            soundEngine = Content.Load<SoundEffect>(@"Audio\\Waves\\engine_2");
            soundEngineInstance = soundEngine.CreateInstance();

            soundHyperspaceActivation = Content.Load<SoundEffect>(@"Audio\\Waves\\hyperspace_activate");

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Get some input.
            UpdateInput();

            // Update audioEngine.
            //audioEngine.Update();

            // Add velocity to the current position.
            ship.Position += ship.Velocity;

            // Bleed off velocity over time.
            ship.Velocity *= 0.95f;

            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                asteroidList[i].Update(timeDelta);
            }

            base.Update(gameTime);
        }

        protected void UpdateInput()
        {
            // Get the game pad state.
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboard = Keyboard.GetState();

            if (currentState.IsConnected)
            {
                ship.Update(currentState);

                //Play engine sound only when the engine is on.
                if (currentState.Triggers.Right > 0)
                {

                    if (soundEngineInstance.State == SoundState.Stopped)
                    {
                        soundEngineInstance.Volume = 0.75f;
                        soundEngineInstance.IsLooped = true;
                        soundEngineInstance.Play();
                    }
                    else
                        soundEngineInstance.Resume();
                }
                else if (currentState.Triggers.Right == 0)
                {
                    if (soundEngineInstance.State == SoundState.Playing)
                        soundEngineInstance.Pause();
                }


                // In case you get lost, press A to warp back to the center.
                if (currentState.Buttons.A == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Enter))
                {
                    ship.Position = Vector3.Zero;
                    ship.Velocity = Vector3.Zero;
                    ship.Rotation = 0.0f;
                    soundHyperspaceActivation.Play();
                }
            } else
            {
                ship.Update(keyboard);
                if (keyboard.IsKeyDown(Keys.W))
                {
                    if (soundEngineInstance.State == SoundState.Stopped)
                    {
                        soundEngineInstance.Volume = 0.75f;
                        soundEngineInstance.IsLooped = true;
                        soundEngineInstance.Play();
                    }
                    else
                        soundEngineInstance.Resume();
                } else
                {
                    if (soundEngineInstance.State == SoundState.Playing)
                        soundEngineInstance.Pause();
                }

                // In case you get lost, press A to warp back to the center.
                if (currentState.Buttons.A == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Enter))
                {
                    ship.Position = Vector3.Zero;
                    ship.Velocity = Vector3.Zero;
                    ship.Rotation = 0.0f;
                    soundHyperspaceActivation.Play();
                }
            }
        }

        private void DrawModel(Model model, Matrix modelTransform,
            Matrix[] absoluteBoneTransform)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = absoluteBoneTransform[mesh.ParentBone.Index] *
                        modelTransform;
                }
                mesh.Draw();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Matrix shipTransformMatrix = ship.RotationMatrix
                    * Matrix.CreateTranslation(ship.Position);
            DrawModel(ship.Model, shipTransformMatrix, ship.Transforms);
            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                Matrix asteroidTransform =
                    Matrix.CreateTranslation(asteroidList[i].position);
                DrawModel(asteroidModel, asteroidTransform, asteroidTransforms);
            }

            base.Draw(gameTime);
        }
    }
}
