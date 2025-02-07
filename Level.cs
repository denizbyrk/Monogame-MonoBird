using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBird {
    public class Level {

        //UI sprites
        private Sprite Title;
        private Sprite Click;
        private Sprite GameOver;
        private Texture2D miscTexture;

        //white flash that appears on death
        private Rectangle whiteFlash;

        //black flash that appears on reset
        private Rectangle blackFlash;

        //create game objects
        private Background Background;
        private Pipe Pipe;
        private Bird Bird;

        //font for score 
        private SpriteFont SpriteFont;

        //score
        private int score = 0;

        //timers for animations
        private int timer;
        private int animTimer;

        //current frame for animation
        private int currentFrame;

        //check if a sound has been played
        private bool hitPlayed, diePlayed;

        //flash effects 
        private float whiteFlashAlpha = 1;
        private float blackFlashAlpha = 0.01f;

        //check if the flash effects are on max Alpha
        public static bool maxAlpha = false;

        //game over animation check
        private bool playGameOverAnim = false;

        //speed of the text that appears on death
        private float textSpeed;

        //check playAnim
        public static bool playAnim = false;

        public Level() {

            this.Background = new Background();
            this.Pipe = new Pipe();
            this.Bird = new Bird();

            this.timer = 0;
            this.animTimer = 0;
            this.currentFrame = 0;
            this.hitPlayed = false;
            this.diePlayed = false;
            this.textSpeed = 20f;
        }

        //load content
        public void LoadContent() {

            this.miscTexture = Main.contentManager.Load<Texture2D>("Assets\\Misc");

            this.Title = new Sprite(this.miscTexture, new Vector2(0, 0));
            this.Title.Rectangle = new Rectangle(0, 25, 79, 20);
            this.Title.Scale = Main.spriteScale + 2;
            this.Title.Position = new Vector2((Main.screenWidth / 2) - (this.Title.Rectangle.Width / 2 * (Main.spriteScale + 2)), 48);

            this.Click = new Sprite(this.miscTexture, new Vector2(0, 0));
            this.Click.Rectangle = new Rectangle(0, 66, 64, 31);
            this.Click.Scale = Main.spriteScale + 2;
            this.Click.Position = new Vector2((Main.screenWidth / 2) - (this.Click.Rectangle.Width / 2 * (Main.spriteScale + 2)), 216);

            this.SpriteFont = Main.contentManager.Load<SpriteFont>("Assets\\Font");

            this.Background.LoadContent();

            this.Pipe.LoadContent();

            this.Bird.LoadContent(this.Background.getGroundHeight());

            this.whiteFlash = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
            this.blackFlash = this.whiteFlash;

            this.GameOver = new Sprite(this.miscTexture, new Vector2(0, 0));
            this.GameOver.Rectangle = new Rectangle(0, 45, 96, 21);
            this.GameOver.Scale = Main.spriteScale + 2;
            this.GameOver.Position = new Vector2((Main.screenWidth / 2) - (this.GameOver.Rectangle.Width / 2 * (Main.spriteScale + 2)), Main.screenHeight);
        }

        //animate
        private void Animate(GameTime dt) {

            this.animTimer += (int)dt.ElapsedGameTime.TotalMilliseconds;

            if (this.animTimer > 900) {

                this.currentFrame++;
                this.animTimer = 0;

                if (this.currentFrame > 1) {

                    this.currentFrame = 0;
                }
            }

            //animate the "Click"
            this.Click.Rectangle = new Rectangle(this.currentFrame * 64, 66, 64, 31);
        }

        //update
        public void Update(GameTime dt) {

            //reset game properties
            if (Level.maxAlpha == true) {

                this.GameOver.Position = new Vector2((Main.screenWidth / 2) - (this.GameOver.Rectangle.Width / 2 * (Main.spriteScale + 2)), Main.screenHeight);
                this.timer = 0;
                this.animTimer = 0;
                this.currentFrame = 0;
                this.textSpeed = 20f;
                this.score = 0;
                this.whiteFlashAlpha = 1;
                this.blackFlashAlpha = 0.01f;
                this.playGameOverAnim = false;
                Level.playAnim = false;
                Level.maxAlpha = false;
                this.hitPlayed = false;
                this.diePlayed = false;
            }

            if (this.blackFlashAlpha < 1) Main.gameReset = false;

            if (this.whiteFlashAlpha <= 0) this.playGameOverAnim = true;

            //animate black flash
            if (Level.playAnim == true) {

                if (Level.maxAlpha == false) this.blackFlashAlpha += 0.05f;
                if (Level.maxAlpha == true) this.blackFlashAlpha -= 0.05f;

                if (this.blackFlashAlpha >= 1f && Level.maxAlpha == false) {

                    Level.maxAlpha = true;

                } else if (this.blackFlashAlpha <= 0) {

                    Level.playAnim = false;
                }
            }

            if (this.blackFlashAlpha <= 0) Level.maxAlpha = false;

            //play swoosh sound
            if (this.textSpeed == 19.5f) Sounds.SoundEffectInstances["swoosh"].Play();

            //move game over text
            if (this.playGameOverAnim == true) {

                this.GameOver.Position.Y -= this.textSpeed;
                this.textSpeed -= 0.5f;
            }

            this.textSpeed = MathHelper.Clamp(this.textSpeed, 0, 30);
            this.GameOver.Position.Y = MathHelper.Clamp(this.GameOver.Position.Y, Main.screenHeight / 2 - this.GameOver.Rectangle.Height * (Main.spriteScale + 2) / 2, Main.screenHeight);

            if (Level.maxAlpha == true) {

                Main.gameReset = true;
            } 

            //decrease white flash alpha
            if (Main.gameOver == true) {

                this.whiteFlashAlpha -= 0.03f;
            }

            this.whiteFlashAlpha = MathHelper.Clamp(this.whiteFlashAlpha, 0, 1);

            //animate the "Click" if game is not started
            if (Main.gameStarted == false) {

                this.Animate(dt);
            }

            //update background
            this.Background.Update(dt);

            //update pipe
            this.Pipe.Update(dt);

            //update bird
            this.Bird.Update(dt);

            //play die sound
            if (this.hitPlayed == true) {

                this.timer += (int)dt.ElapsedGameTime.TotalMilliseconds;

                if (this.timer > 240 && this.diePlayed == false) {

                    Sounds.SoundEffectInstances["die"].Play();

                    this.diePlayed = true;

                    this.timer = 0;
                }
            }

            this.timer = MathHelper.Clamp(this.timer, 0, 245);

            //check pipe collision
            if ((this.Bird.getBirdRectangle().Intersects(this.Pipe.getTopPipeHitbox()) || this.Bird.getBirdRectangle().Intersects(this.Pipe.getBottomPipeHitbox())) && Main.gameStarted) {

                Main.gameOver = true;

                if (this.hitPlayed == false) {

                    Sounds.SoundEffectInstances["hit"].Play();

                    this.hitPlayed = true;
                }
            }

            //check ground and ceiling collision
            if (this.Bird.getBirdPosition().Y < 2 || this.Bird.getBirdPosition().Y > (Main.screenHeight - (this.Background.getGroundHeight() * Main.spriteScale) - this.Bird.getBirdTextureHeight() * Main.spriteScale / 2) - 2) {

                Main.gameOver = true;

                if (this.hitPlayed == false) {

                    Sounds.SoundEffectInstances["hit"].Play();

                    this.hitPlayed = true;
                }
            }

            //increase score and play point sound
            if (this.Bird.getBirdPosition().X > this.Pipe.getPipePointPosition().X && Pipe.pipePassed == false) {

                this.score++;
                Pipe.pipePassed = true;

                Sounds.SoundEffectInstances["point"].Stop();
                Sounds.SoundEffectInstances["point"].Play();
            }
        }

        //draw
        public void Draw(SpriteBatch b) {

            //draw title and click me
            if (Main.gameStarted == false) {

                b.Draw(this.Title.Texture, this.Title.Position, this.Title.Rectangle, this.Title.Hue, this.Title.Rotation,
                                this.Title.Origin, this.Title.Scale, this.Title.Effect, 0.55f);

                b.Draw(this.Click.Texture, this.Click.Position, this.Click.Rectangle, this.Click.Hue, this.Click.Rotation,
                                this.Click.Origin, this.Click.Scale, this.Click.Effect, 0.55f);
            }

            //draw background
            this.Background.Draw(b);

            //draw pipes
            this.Pipe.Draw(b);

            //draw bird
            this.Bird.Draw(b);

            //draw score
            if (Main.gameStarted == true) {

                b.DrawString(this.SpriteFont, "" + this.score, new Vector2(Main.screenWidth / 2 - this.SpriteFont.MeasureString("" + this.score).Length() / 2, 50), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            }

            //draw game over text
            if (Main.gameOver == true) {

                b.Draw(this.GameOver.Texture, this.GameOver.Position, this.GameOver.Rectangle, this.GameOver.Hue,
                            this.GameOver.Rotation, this.GameOver.Origin, this.GameOver.Scale, this.GameOver.Effect, 0.59f);
            }

            //draw white flash
            if (Main.gameOver == true && this.whiteFlashAlpha > 0) {

                b.Draw(Main.pixel, new Vector2(0, 0), this.whiteFlash,
                            Color.White * this.whiteFlashAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.6f);
            }

            //draw black flash
            if (Level.playAnim == true) {

                b.Draw(Main.pixel, new Vector2(0, 0), this.blackFlash,
                            Color.Black * this.blackFlashAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.61f);
            }
        }
    }
}