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

        //Going Beyond members
        #region members
        //3D Member to draw
        Model myModel;

        // The aspect ratio determine how to scale 3d to 2d projection
        float aspectRatio;

        // Position of model
        Vector3 modelPosition = Vector3.Zero;
        float modelRotation = 0.0f;

        // Set the velocity of the model, applied each frame to the model's position.
        Vector3 modelVelocity = Vector3.Zero;

        //Set the sound effects to use
        SoundEffect soundEngine;
        SoundEffectInstance soundEngineInstance;
        SoundEffect soundHyperspaceActivation;

        // Set position of the camera in world space, for our view matrix
        Vector3 cameraPosition = new Vector3(0.0f, 50.0f, 5000.0f);
        
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
            IsMouseVisible = true;
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

            //Load the ship
            myModel = Content.Load<Model>(@"Models\\p1_wedge");

            soundEngine = Content.Load<SoundEffect>(@"Audio\\Waves\\engine_2");
            soundEngineInstance = soundEngine.CreateInstance();

            soundHyperspaceActivation = Content.Load<SoundEffect>(@"Audio\\Waves\\hyperspace_activate");

            //Load Aspect Ratio
            aspectRatio = GraphicsDevice.Viewport.AspectRatio;
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

            //modelRotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.1f);

            // Get some input
            UpdateInput();

            // Add velocity to the current position
            modelPosition += modelVelocity;

            // Bleed off velocity over time
            modelVelocity *= 0.95f;
            base.Update(gameTime);
        }

        protected void UpdateInput()
        {
            // Get the game pad state.
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            KeyboardState currentKeyState = Keyboard.GetState();

            if (true)
            {
                

                // Rotate the model using the left thumbstick, and scale it down.
                modelRotation -= currentState.ThumbSticks.Left.X * 0.10f;

                // Create some velocity if the right trigger is down.
                Vector3 modelVelocityAdd = Vector3.Zero;

                if (currentKeyState.IsKeyDown(Keys.A))
                    modelRotation += 0.10f;
                else if (currentKeyState.IsKeyDown(Keys.D))
                    modelRotation -= 0.10f;
                
                

                // Find out what direction we should be thrusting, using rotation.
                modelVelocityAdd.X = -(float)Math.Sin(modelRotation);
                modelVelocityAdd.Z = -(float)Math.Cos(modelRotation);


                if (currentKeyState.IsKeyDown(Keys.W))
                {
                    
                    modelVelocityAdd *= 2;
                } 
                    

                // Now scale our direction by how hard the trigger is down.
                // modelVelocityAdd *= currentState.Triggers.Right;

                // Finally, add this vector to our velocity.
                modelVelocity += modelVelocityAdd;
                Window.Title = modelVelocity.ToString();

                GamePad.SetVibration(PlayerIndex.One, currentState.Triggers.Right,
                    currentState.Triggers.Right);

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
                if (currentState.Buttons.A == ButtonState.Pressed || currentKeyState.IsKeyDown(Keys.Enter))
                {
                    modelPosition = Vector3.Zero;
                    modelVelocity = Vector3.Zero;
                    modelRotation = 0.0f;
                    soundHyperspaceActivation.Play();
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Matrix[] transform = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(transform);

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transform[mesh.ParentBone.Index] *
                        Matrix.CreateRotationY(modelRotation) *
                        Matrix.CreateTranslation(modelPosition);

                    effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio
                        ,1.0f, 10000.0f);
                }
                mesh.Draw();
            }


            base.Draw(gameTime);
        }
    }
}
