using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoBird {
    public class Main : Game {

        //game speed
        public static int gameSpeed = 5;

        //create graphics manager, content manager, sprite batch for drawing, and and pixel for shape drawing
        private GraphicsDeviceManager graphics;
        public static ContentManager contentManager;
        private SpriteBatch b;
        public static Texture2D pixel;

        //define window width and height
        public const int screenWidth = 1280;
        public const int screenHeight = 720;

        //sprite scale
        public static int spriteScale = 3;

        //check game events
        public static bool gameStarted = false;
        public static bool gameOver = false;
        public static bool gameReset = false;

        //create object for generating random values
        public static Random random = new Random();

        //create Level object
        private Level Level;

        public Main() {

            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.SynchronizeWithVerticalRetrace = false;

            this.Window.AllowAltF4 = true;
            this.Window.AllowUserResizing = false;

            this.Content.RootDirectory = "Content";
            this.IsFixedTimeStep = true;
            this.IsMouseVisible = true;
        }

        protected override void Initialize() {

            //set window width and height
            this.graphics.PreferredBackBufferWidth = Main.screenWidth;
            this.graphics.PreferredBackBufferHeight = Main.screenHeight;
            this.graphics.ApplyChanges();

            this.Level = new Level();

            //set master volume
            SoundEffect.MasterVolume = 1f;

            base.Initialize();
        }

        //load content
        protected override void LoadContent() {

            this.b = new SpriteBatch(this.GraphicsDevice);

            //create pixel
            Main.pixel = new Texture2D(this.GraphicsDevice, 1, 1);
            Main.pixel.SetData(new Color[] { Color.White });

            Main.contentManager = new ContentManager(this.Content.ServiceProvider, "Content");

            //load game statte and sounds
            this.Level.LoadContent();
            Sounds.Load();
        }

        //update
        protected override void Update(GameTime dt) {

            if (this.IsActive == true) {
                
                //update inputs
                Input.Update();

                //update level and sounds
                this.Level.Update(dt);
                Sounds.Update();

                if (Main.gameOver == true) Main.gameSpeed = 0;

                //play swoosh sounds
                if (Input.IsLeftClickDown() && Main.gameOver == true && Input.checkMouseCoordinates()) {

                    Level.playAnim = true;
                    Sounds.SoundEffectInstances["swoosh"].Stop();
                    Sounds.SoundEffectInstances["swoosh"].Play();
                }

                if (Main.gameOver == false && Main.gameStarted == false) {

                    Main.gameReset = false;
                    Main.gameSpeed = 5;
                }

                base.Update(dt);
            }
        }

        //draw
        protected override void Draw(GameTime dt) {

            //set background color to black (not visible)
            this.GraphicsDevice.Clear(Color.Black);

            this.b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);

            //draw level
            this.Level.Draw(this.b);

            this.b.End();

            base.Draw(dt);
        }
    }
}