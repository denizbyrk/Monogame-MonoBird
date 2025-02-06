using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace MonoBird {
    public class Pipe {

        private Texture2D pipeTexture;
        private string pipeTexturePath;
        private List<Sprite[]> pipeArray;

        private int pipeFrequency;
        private int pipeSpacing;
        private int minPipeHeight;
        private int maxPipeHeight;
        private int timer;
        public static bool pipePassed;
        public static bool firstPipeCreated;
 
        public Pipe() {

            this.pipeArray = new List<Sprite[]>();

            this.pipeFrequency = 900;
            this.pipeSpacing = 216;
            this.minPipeHeight = 500;
            this.maxPipeHeight = 200;

            Pipe.pipePassed = false;
        }

        public void LoadContent() {

            this.pipeTexturePath = "Assets\\Pipe";
            this.pipeTexture = Main.contentManager.Load<Texture2D>(this.pipeTexturePath);

            this.SpawnPipe();
        }

        private void SpawnPipe() {

            Sprite[] pipes_ = new Sprite[2];

            int pipePosition = Main.random.Next(this.maxPipeHeight, this.minPipeHeight);

            var bottomPipePosition = new Vector2(Main.screenWidth, pipePosition);
            var topPipePosition = new Vector2(Main.screenWidth, pipePosition - this.pipeSpacing * Main.spriteScale);

            pipes_[0] = new Sprite(this.pipeTexture, bottomPipePosition);
            pipes_[1] = new Sprite(this.pipeTexture, topPipePosition);

            pipes_[1].Effect = SpriteEffects.FlipVertically;

            for (int i = 0; i < 2; i++) {

                pipes_[i].Scale = Main.spriteScale;
            }

            this.pipeArray.Add(pipes_);
        }

        public void Update(GameTime dt) {

            if (Main.gameReset == true) Pipe.firstPipeCreated = false;

            if (Main.gameStarted == false) this.pipeArray.Clear();

            if (Main.gameStarted == true) {

                this.timer += (int)dt.ElapsedGameTime.TotalMilliseconds;

                if (this.timer > this.pipeFrequency) {

                    this.SpawnPipe();
                    Pipe.firstPipeCreated = true;
                    this.timer = 0;
                }
            }

            foreach (Sprite[] sprite in this.pipeArray) {

                foreach (Sprite sprite1 in sprite) {

                    sprite1.Position.X -= Main.gameSpeed;
                }
            }

            if (this.pipeArray.Count > 0) {

                if (this.pipeArray.First()[0].Position.X < 0 - this.pipeArray.First()[0].Texture.Width * Main.spriteScale) {

                    this.pipeArray.Remove(this.pipeArray.First());
                    Pipe.pipePassed = false;
                }
            }

            if (this.pipeArray.Count > 0 && Main.gameStarted == true) {

                this.pipeArray.First()[0].Rectangle = new Rectangle(0, 0, this.pipeTexture.Width * Main.spriteScale, this.pipeTexture.Height * Main.spriteScale);
                this.pipeArray.First()[1].Rectangle = new Rectangle(0, 0, this.pipeTexture.Width * Main.spriteScale, this.pipeTexture.Height * Main.spriteScale);
            } 
        }

        public void Draw(SpriteBatch b) {

            foreach (Sprite[] sprite in this.pipeArray) {

                foreach (Sprite sprite1 in sprite) {
                    
                    b.Draw(sprite1.Texture, sprite1.Position, null, sprite1.Hue, sprite1.Rotation,
                                sprite1.Origin, sprite1.Scale, sprite1.Effect, 0.2f);
                }
            }

            if (Main.debug == true) {

                b.Draw(Main.pixel, new Vector2(this.pipeArray.First()[0].Position.X, this.pipeArray.First()[0].Position.Y), this.pipeArray.First()[0].Rectangle, Color.Green, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.31f);
                b.Draw(Main.pixel, new Vector2(this.pipeArray.First()[1].Position.X, this.pipeArray.First()[1].Position.Y), this.pipeArray.First()[1].Rectangle, Color.Green, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.31f);
            }
        }

        public Rectangle getTopPipeHitbox() {

            if (this.pipeArray.Count == 0) return new Rectangle(Main.screenWidth, 0, 0, 0);

            return new Rectangle((int)(this.pipeArray.First()[0].Position.X), (int)(this.pipeArray.First()[0].Position.Y), this.pipeArray.First()[0].Rectangle.Width, this.pipeArray.First()[0].Rectangle.Height);
        }

        public Rectangle getBottomPipeHitbox() {

            if (this.pipeArray.Count == 0) return new Rectangle(Main.screenWidth, 0, 0, 0);

            return new Rectangle((int)(this.pipeArray.First()[1].Position.X), (int)(this.pipeArray.First()[1].Position.Y), this.pipeArray.First()[1].Rectangle.Width, this.pipeArray.First()[1].Rectangle.Height);
        }

        public Vector2 getPipePointPosition() {

            if (this.pipeArray.Count == 0) return new Vector2(Main.screenWidth, 0);

            return new Vector2((this.pipeArray.First()[0].Position.X + this.pipeTexture.Width * Main.spriteScale / 2) - 16, 0);
        }
    }
}