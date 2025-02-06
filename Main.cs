using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoBird {
    public class Main : Game {

        public static int gameSpeed = 5;

        private GraphicsDeviceManager graphics;
        private SpriteBatch b;

        public const int screenWidth = 1280;
        public const int screenHeight = 720;
        public static ContentManager contentManager;
        public static Texture2D pixel;
        public static int spriteScale = 3;
        public static bool gameStarted = false;
        public static bool gameOver = false;
        public static bool gameReset = false;
        public static Random random = new Random();

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

            this.graphics.PreferredBackBufferWidth = Main.screenWidth;
            this.graphics.PreferredBackBufferHeight = Main.screenHeight;
            this.graphics.ApplyChanges();

            this.Level = new Level();

            SoundEffect.MasterVolume = 0.3f;

            base.Initialize();
        }

        protected override void LoadContent() {

            this.b = new SpriteBatch(this.GraphicsDevice);

            Main.pixel = new Texture2D(this.GraphicsDevice, 1, 1);
            Main.pixel.SetData(new Color[] { Color.White });

            Main.contentManager = new ContentManager(this.Content.ServiceProvider, "Content");

            this.Level.LoadContent();
            Sounds.Load();
        }

        protected override void Update(GameTime dt) {

            if (this.IsActive == true) {
                
                Input.Update();

                this.Level.Update(dt);
                Sounds.Update();

                if (Main.gameOver == true) Main.gameSpeed = 0;

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

        protected override void Draw(GameTime dt) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this.b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);

            this.Level.Draw(this.b);

            this.b.End();

            base.Draw(dt);
        }
    }
}