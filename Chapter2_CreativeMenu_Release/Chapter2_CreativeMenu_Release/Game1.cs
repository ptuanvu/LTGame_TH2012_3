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

namespace Chapter2_CreativeMenu_Release
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private static int GAME_MENU = 0;
        private static int GAME_PAUSE = 1;
        private static int GAME_START = 2;
        private int gameState = GAME_MENU;
        SpriteFont font;

        List<GameObject> gameObjects;

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
            gameObjects = new List<GameObject>();
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 700;
            graphics.ApplyChanges();
            Window.Title = "1212521-PhanTuanVu-SimpleMenu";
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

            Menu menu = (Menu)CreateObject("Menu", null);
            MenuItem startGame = (MenuItem)CreateObject("MenuItem", "Start");
            startGame.SetPosition(new Vector2(400, 200));
            startGame.Click += new MenuItem.ClickHandler(StartGame_Click);

            MenuItem pauseGame = (MenuItem)CreateObject("MenuItem", "Pause");
            pauseGame.SetPosition(new Vector2(400, 300));
            pauseGame.Click += new MenuItem.ClickHandler(PauseGame_Click);

            MenuItem exitGame = (MenuItem)CreateObject("MenuItem", "Exit");
            exitGame.SetPosition(new Vector2(400, 400));
            exitGame.Click += new MenuItem.ClickHandler(ExitGame_Click);

            menu.MenuItems.Add(startGame);
            menu.MenuItems.Add(pauseGame);
            menu.MenuItems.Add(exitGame);

            gameObjects.Add(menu);

            font = Content.Load<SpriteFont>("font");

            DisplayMessage messageGameStart = new DisplayMessage("GAMMING...PRESS ESC TO SHOW MENU...", TimeSpan.FromSeconds(1.0), new Vector2(300, 300), Color.White, font, "Gamming");
            gameObjects.Add(messageGameStart);

            DisplayMessage messageGameStart2 = new DisplayMessage("PAUSING...PRESS ESC TO SHOW MENU...", TimeSpan.FromSeconds(1.0), new Vector2(300, 300), Color.White, font, "Pausing");
            gameObjects.Add(messageGameStart2);
        }

        private void ExitGame_Click(object sender, MyMenuItemEventArgs e)
        {
            this.Exit();
        }

        private void PauseGame_Click(object sender, MyMenuItemEventArgs e)
        {
            Window.Title = "The Game Is Paused";
            gameState = GAME_PAUSE;
        }

        private void StartGame_Click(object sender, MyMenuItemEventArgs e)
        {
            Window.Title = "StartGame Is Pressed";
            gameState = GAME_START;
        }

        public object CreateObject(string modelName, object param)
        {
            switch (modelName)
            {
                case "Menu":
                    return CreateMenuObject(param);
                    break;
                case "MenuItem":
                    return CreateMenuItemObject(param);
                    break;
                default:
                    break;
            }

            return null;
        }

        private MenuItem CreateMenuItemObject(object param)
        {
            string p = (string)param;
            Texture2D buttonNormal;
            Texture2D buttonClicked;

            switch (p)
            {
                case "Start":
                    buttonNormal = Content.Load<Texture2D>(@"Models\\Menus\\StartButton");
                    buttonClicked = Content.Load<Texture2D>(@"Models\\Menus\\StartButtonClicked");
                    break;
                case "Pause":
                    buttonNormal = Content.Load<Texture2D>(@"Models\\Menus\\PauseButton");
                    buttonClicked = Content.Load<Texture2D>(@"Models\\Menus\\PauseButtonClicked");
                    break;
                case "Exit":
                    buttonNormal = Content.Load<Texture2D>(@"Models\\Menus\\ExitButton");
                    buttonClicked = Content.Load<Texture2D>(@"Models\\Menus\\ExitButtonClicked");
                    break;
                default:
                    buttonNormal = null;
                    buttonClicked = null;
                    break;
            }


            if (buttonNormal == null || buttonClicked == null)
            {
                return null;
            }

            MenuItem menuItem = new MenuItem(buttonNormal, buttonClicked);
            return menuItem;
        }

        private Menu CreateMenuObject(object param)
        {
            Menu menu = new Menu();
            Vector2 size = new Vector2();
            size.X = graphics.GraphicsDevice.DisplayMode.Width;
            size.Y = graphics.GraphicsDevice.DisplayMode.Height;
            menu.Size = size;

            return menu;
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

            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) gameState = GAME_MENU;

            foreach (GameObject gameObject in gameObjects)
            {
                if (gameState == GAME_MENU && gameObject.ObjectName() == "Menu")
                {
                    Window.Title = "1212521-PhanTuanVu-SimpleMenu";
                    ((Menu)gameObject).Update(gameTime, Mouse.GetState(), Keyboard.GetState());
                }
                if (gameState == GAME_START && gameObject.ObjectName() == "Gamming")
                {
                    gameObject.Update(gameTime, Mouse.GetState(), Keyboard.GetState());
                }

                if (gameState == GAME_PAUSE && gameObject.ObjectName() == "Pausing")
                {
                    gameObject.Update(gameTime, Mouse.GetState(), Keyboard.GetState());
                }

            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            foreach (GameObject gameObject in gameObjects)
            {
                if (gameState == GAME_MENU && gameObject.ObjectName() == "Menu")
                    gameObject.Draw(gameTime, spriteBatch);
                else if (gameState == GAME_START)
                {
                    Window.Title = "Game Starting";
                    if (gameObject.ObjectName() == "Gamming")
                    {
                        gameObject.Draw(gameTime, spriteBatch);
                    }
                }
                   
                else if (gameState == GAME_PAUSE)
                {
                    Window.Title = "Game Pausing";
                    if (gameObject.ObjectName() == "Pausing")
                    {
                        gameObject.Draw(gameTime, spriteBatch);
                    }
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
