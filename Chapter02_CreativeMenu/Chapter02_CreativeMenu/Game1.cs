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

namespace Chapter02_CreativeMenu
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private static int GAME_MENU = 0;
        private static int GAME_PAUSE = 1;
        private static int GAME_START = 2;

        Camera camera;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState previousMouseState;
        private int gameState = GAME_MENU;

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
            camera = new Camera(GraphicsDevice.Viewport);

            gameObjects = new List<GameObject>();
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 700;
            graphics.ApplyChanges();
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
            startGame.SetPosition(new Vector2(100, 100));
            startGame.Click += new MenuItem.ClickHandler(StartGame_Click);

            MenuItem pauseGame = (MenuItem)CreateObject("MenuItem", "Pause");
            pauseGame.SetPosition(new Vector2(100, 200));
            pauseGame.Click += new MenuItem.ClickHandler(PauseGame_Click);

            MenuItem exitGame = (MenuItem)CreateObject("MenuItem", "Exit");
            exitGame.SetPosition(new Vector2(100, 300));
            exitGame.Click += new MenuItem.ClickHandler(ExitGame_Click);

            menu.MenuItems.Add(startGame);
            menu.MenuItems.Add(pauseGame);
            menu.MenuItems.Add(exitGame);

            gameObjects.Add(menu);

            Ship ship = CreateShipObject(null);
            ship.SetPosition(new Vector2(200, 200));
            gameObjects.Add(ship);

            List<Texture2D> bullets = new List<Texture2D>();
            Texture2D bullet1 = Content.Load<Texture2D>(@"Models\\Bullets\\Bullet1");
            Texture2D bullet2 = Content.Load<Texture2D>(@"Models\\Bullets\\Bullet2");
            Texture2D bullet3 = Content.Load<Texture2D>(@"Models\\Bullets\\Bullet3");
            Texture2D bullet4 = Content.Load<Texture2D>(@"Models\\Bullets\\Bullet4");
            Texture2D bullet5 = Content.Load<Texture2D>(@"Models\\Bullets\\Bullet5");
            Texture2D bullet6 = Content.Load<Texture2D>(@"Models\\Bullets\\Bullet6");
            Texture2D bullet7 = Content.Load<Texture2D>(@"Models\\Bullets\\Bullet7");

            bullets.Add(bullet1);
            bullets.Add(bullet2);
            bullets.Add(bullet3);
            bullets.Add(bullet4);
            bullets.Add(bullet5);
            bullets.Add(bullet6);
            bullets.Add(bullet7);

            Textures.AddBullet(bullets);
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

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public object CreateObject(string modelName, object param)
        {
            switch(modelName)
            {
                case "Menu":
                    return CreateMenuObject(param);
                    break;
                case "MenuItem":
                    return CreateMenuItemObject(param);
                    break;
                case "Ship":
                    return CreateShipObject(param);
                    break;
                default:
                    break;
            }

            return null;
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

        private Ship CreateShipObject(object param)
        {
            Texture2D ship = Content.Load<Texture2D>(@"Models\\Ship01");
            Ship result = new Ship(ship);
            return result;
        }

        private MenuItem CreateMenuItemObject(object param)
        {
            string p = (string)param;
            Texture2D buttonNormal;
            Texture2D buttonClicked;

            switch (p)
            {
                case "Start":
                    buttonNormal = Content.Load<Texture2D>(@"Models\\Buttons\\StartButton");
                    buttonClicked = Content.Load<Texture2D>(@"Models\\Buttons\\StartButtonClicked");
                    break;
                case "Pause":
                    buttonNormal = Content.Load<Texture2D>(@"Models\\Buttons\\PauseButton");
                    buttonClicked = Content.Load<Texture2D>(@"Models\\Buttons\\PauseButtonClicked");
                    break;
                case "Exit":
                    buttonNormal = Content.Load<Texture2D>(@"Models\\Buttons\\ExitButton");
                    buttonClicked = Content.Load<Texture2D>(@"Models\\Buttons\\ExitButtonClicked");
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

            previousMouseState = Mouse.GetState();

            foreach (GameObject gameObject in gameObjects)
            {
                if (gameState == GAME_MENU && gameObject.ObjectName() == "Menu")
                {
                    ((Menu)gameObject).Update(gameTime, previousMouseState);
                } else if (gameState == GAME_START && gameObject.ObjectName() == "GameObject")
                {
                    ((Ship)gameObject).Update(gameTime, Keyboard.GetState());
                    camera.Update(gameTime, (Ship)gameObject);
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
            if (gameState == GAME_START)
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);
            else spriteBatch.Begin();
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameState == GAME_MENU && gameObject.ObjectName() == "Menu")
                    gameObject.Draw(gameTime, spriteBatch);
                else if (gameState == GAME_START && gameObject.ObjectName() == "GameObject")
                    gameObject.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
