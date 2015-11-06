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

        Matrix projectionMatrix;
        Matrix viewMatrix;

        Ship ship = new Ship();
        //Going Beyond members
        #region members
        //3D Member to draw
        //Model myModel;

        //Stone
        Model asteroidModel;
        Matrix[] asteroidTransforms;
        Asteroid[] asteroidList = new Asteroid[GameConstants.NumAsteroids];
        Random random = new Random();

        //Set the sound effects to use
        SoundEffect soundEngine;
        SoundEffectInstance soundEngineInstance;
        SoundEffect soundHyperspaceActivation;

        // Set position of the camera in world space, for our view matrix
        Vector3 cameraPosition = new Vector3(0.0f, 0.0f, GameConstants.CameraHeight);

        #endregion


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

    //        projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
    //MathHelper.ToRadians(45.0f),
    //GraphicsDevice.DisplayMode.AspectRatio,
    //20000.0f, 30000.0f);

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45.0f),
                GraphicsDevice.DisplayMode.AspectRatio,
                GameConstants.CameraHeight - 1000.0f, GameConstants.CameraHeight + 1000.0f);
            viewMatrix = Matrix.CreateLookAt(cameraPosition,
                Vector3.Zero, Vector3.Up);

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

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load the ship
            ship.Model = Content.Load<Model>("Models/p1_wedge");
            ship.transform = SetupEffectDefaults(ship.Model);

            //Load asteroid
            asteroidModel = Content.Load<Model>("Models\\asteroid1");
            asteroidTransforms = SetupEffectDefaults(asteroidModel);

            soundEngine = Content.Load<SoundEffect>(@"Audio\\Waves\\engine_2");
            soundEngineInstance = soundEngine.CreateInstance();

            soundHyperspaceActivation = Content.Load<SoundEffect>(@"Audio\\Waves\\hyperspace_activate");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                asteroidList[i].Update(timeDelta);
            }
            
            //modelRotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.1f);

            // Get some input
            UpdateInput();


            ship.Position += ship.Velocity;
            ship.Velocity *= 0.95f;

            //// Add velocity to the current position
            //modelPosition += modelVelocity;

            //// Bleed off velocity over time
            //modelVelocity *= 0.95f;
            base.Update(gameTime);
        }

        protected void UpdateInput()
        {
            // Get the game pad state.
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            KeyboardState currentKeyState = Keyboard.GetState();

            ship.Update(currentKeyState);

            // In case you get lost, press A to warp back to the center.
            if (currentState.Buttons.A == ButtonState.Pressed || currentKeyState.IsKeyDown(Keys.Enter))
            {
                ship.Position = Vector3.Zero;
                ship.Velocity = Vector3.Zero;
                ship.Rotation = 0.0f;
                soundHyperspaceActivation.Play();
            }
        //    if (true)
        //    {


            //        // Rotate the model using the left thumbstick, and scale it down.
            //        modelRotation -= currentState.ThumbSticks.Left.X * 0.10f;

            //        // Create some velocity if the right trigger is down.
            //        Vector3 modelVelocityAdd = Vector3.Zero;

            //        if (currentKeyState.IsKeyDown(Keys.A))
            //            modelRotation += 0.10f;
            //        else if (currentKeyState.IsKeyDown(Keys.D))
            //            modelRotation -= 0.10f;



            //        // Find out what direction we should be thrusting, using rotation.
            //        modelVelocityAdd.X = -(float)Math.Sin(modelRotation);
            //        modelVelocityAdd.Z = -(float)Math.Cos(modelRotation);


            //        if (currentKeyState.IsKeyDown(Keys.W))
            //        {

            //            modelVelocityAdd *= 2;
            //        } 


            //        // Now scale our direction by how hard the trigger is down.
            //        // modelVelocityAdd *= currentState.Triggers.Right;

            //        // Finally, add this vector to our velocity.
            //        modelVelocity += modelVelocityAdd;
            //        Window.Title = modelVelocity.ToString();

            //        GamePad.SetVibration(PlayerIndex.One, currentState.Triggers.Right,
            //            currentState.Triggers.Right);

            //        //Play engine sound only when the engine is on.
            //        if (currentState.Triggers.Right > 0)
            //        {

            //            if (soundEngineInstance.State == SoundState.Stopped)
            //            {
            //                soundEngineInstance.Volume = 0.75f;
            //                soundEngineInstance.IsLooped = true;
            //                soundEngineInstance.Play();
            //            }
            //            else
            //                soundEngineInstance.Resume();
            //        }
            //        else if (currentState.Triggers.Right == 0)
            //        {
            //            if (soundEngineInstance.State == SoundState.Playing)
            //                soundEngineInstance.Pause();
            //        }

            //        // In case you get lost, press A to warp back to the center.
            //        if (currentState.Buttons.A == ButtonState.Pressed || currentKeyState.IsKeyDown(Keys.Enter))
            //        {
            //            modelPosition = Vector3.Zero;
            //            modelVelocity = Vector3.Zero;
            //            modelRotation = 0.0f;
            //            soundHyperspaceActivation.Play();
            //        }
            //    }
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

            //Matrix[] transform = new Matrix[myModel.Bones.Count];
            //myModel.CopyAbsoluteBoneTransformsTo(transform);

            //foreach (ModelMesh mesh in myModel.Meshes)
            //{
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.EnableDefaultLighting();
            //        effect.World = transform[mesh.ParentBone.Index] *
            //            Matrix.CreateRotationY(modelRotation) *
            //            Matrix.CreateTranslation(modelPosition);

            //        effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            //        effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio
            //            ,1.0f, 10000.0f);
            //    }
            //    mesh.Draw();
            //}

            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                Matrix asteroidTransform =
                    Matrix.CreateTranslation(asteroidList[i].position);
                DrawModel(asteroidModel, asteroidTransform, asteroidTransforms);
            }

            Matrix shipTransformMatrix = ship.RotationMatrix
                * Matrix.CreateTranslation(ship.Position);

            DrawModel(ship.Model, shipTransformMatrix, ship.transform);

            base.Draw(gameTime);
        }
    }
}
