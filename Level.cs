using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBird {
    public class Level {

        private Sprite Title;
        private Sprite Click;
        private Sprite GameOver;
        private Texture2D miscTexture;
        private Rectangle whiteFlash;
        private Rectangle blackFlash;

        private Background Background;
        private Pipe Pipe;
        private Bird bird;
        private SpriteFont SpriteFont;

        private int score = 0;
        private int timer;
        private int animTimer;
        private int currentFrame;
        private bool hitPlayed, diePlayed;
        private float whiteFlashAlpha = 1;
        private float blackFlashAlpha = 0.01f;
        private bool playGameOverAnim = false;
        private float textSpeed;
        public static bool playAnim = false;
        public static bool maxAlpha = false;

        public Level() {

            this.Background = new Background();
            this.Pipe = new Pipe();
            this.bird = new Bird();

            this.timer = 0;
            this.animTimer = 0;
            this.currentFrame = 0;
            this.hitPlayed = false;
            this.diePlayed = false;
            this.textSpeed = 20f;
        }

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

            this.bird.LoadContent(this.Background.getGroundHeight());

            this.whiteFlash = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
            this.blackFlash = this.whiteFlash;

            this.GameOver = new Sprite(this.miscTexture, new Vector2(0, 0));
            this.GameOver.Rectangle = new Rectangle(0, 45, 96, 21);
            this.GameOver.Scale = Main.spriteScale + 2;
            this.GameOver.Position = new Vector2((Main.screenWidth / 2) - (this.GameOver.Rectangle.Width / 2 * (Main.spriteScale + 2)), Main.screenHeight);
        }

        private void Animate(GameTime dt) {

            this.animTimer += (int)dt.ElapsedGameTime.TotalMilliseconds;

            if (this.animTimer > 900) {

                this.currentFrame++;
                this.animTimer = 0;

                if (this.currentFrame > 1) {

                    this.currentFrame = 0;
                }
            }

            this.Click.Rectangle = new Rectangle(this.currentFrame * 64, 66, 64, 31);
        }

        public void Update(GameTime dt) {

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

            if (this.textSpeed == 19.5f) Sounds.SoundEffectInstances["swoosh"].Play();

            if (this.playGameOverAnim == true) {

                this.GameOver.Position.Y -= this.textSpeed;
                this.textSpeed -= 0.5f;
            }

            this.textSpeed = MathHelper.Clamp(this.textSpeed, 0, 30);
            this.GameOver.Position.Y = MathHelper.Clamp(this.GameOver.Position.Y, Main.screenHeight / 2 - this.GameOver.Rectangle.Height * (Main.spriteScale + 2) / 2, Main.screenHeight);

            if (Level.maxAlpha == true) {

                Main.debug = false;
                Main.gameReset = true;
            } 

            if (Main.gameOver == true) {

                this.whiteFlashAlpha -= 0.03f;
            }

            this.whiteFlashAlpha = MathHelper.Clamp(this.whiteFlashAlpha, 0, 1);

            if (Main.gameStarted == false) {

                this.Animate(dt);
            }

            this.Background.Update(dt);

            this.Pipe.Update(dt);

            this.bird.Update(dt);

            if (this.hitPlayed == true) {

                this.timer += (int)dt.ElapsedGameTime.TotalMilliseconds;

                if (this.timer > 240 && this.diePlayed == false) {

                    Sounds.SoundEffectInstances["die"].Play();

                    this.diePlayed = true;

                    this.timer = 0;
                }
            }

            this.timer = MathHelper.Clamp(this.timer, 0, 245);

            if ((this.bird.getBirdRectangle().Intersects(this.Pipe.getTopPipeHitbox()) || this.bird.getBirdRectangle().Intersects(this.Pipe.getBottomPipeHitbox())) && Main.gameStarted) {

                Main.gameOver = true;

                if (this.hitPlayed == false) {

                    Sounds.SoundEffectInstances["hit"].Play();

                    this.hitPlayed = true;
                }
            }

            if (this.bird.getBirdPosition().Y < 2 || this.bird.getBirdPosition().Y > (Main.screenHeight - (this.Background.getGroundHeight() * Main.spriteScale) - this.bird.getBirdTextureHeight() * Main.spriteScale / 2) - 2) {

                Main.gameOver = true;

                if (this.hitPlayed == false) {

                    Sounds.SoundEffectInstances["hit"].Play();

                    this.hitPlayed = true;
                }
            }

            if (this.bird.getBirdPosition().X > this.Pipe.getPipePointPosition().X && Pipe.pipePassed == false) {

                this.score++;
                Pipe.pipePassed = true;

                Sounds.SoundEffectInstances["point"].Stop();
                Sounds.SoundEffectInstances["point"].Play();
            }
        }

        public void Draw(SpriteBatch b) {

            if (Main.gameStarted == false) {

                b.Draw(this.Title.Texture, this.Title.Position, this.Title.Rectangle, this.Title.Hue, this.Title.Rotation,
                                this.Title.Origin, this.Title.Scale, this.Title.Effect, 0.55f);

                b.Draw(this.Click.Texture, this.Click.Position, this.Click.Rectangle, this.Click.Hue, this.Click.Rotation,
                                this.Click.Origin, this.Click.Scale, this.Click.Effect, 0.55f);
            }

            this.Background.Draw(b);

            this.Pipe.Draw(b);

            this.bird.Draw(b);

            if (Main.gameStarted == true) {

                b.DrawString(this.SpriteFont, "" + this.score, new Vector2(Main.screenWidth / 2 - this.SpriteFont.MeasureString("" + this.score).Length() / 2, 50), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            }

            if (Main.gameOver == true) {

                b.Draw(this.GameOver.Texture, this.GameOver.Position, this.GameOver.Rectangle, this.GameOver.Hue,
                            this.GameOver.Rotation, this.GameOver.Origin, this.GameOver.Scale, this.GameOver.Effect, 0.59f);
            }

            if (Main.gameOver == true && this.whiteFlashAlpha > 0) {

                b.Draw(Main.pixel, new Vector2(0, 0), this.whiteFlash,
                            Color.White * this.whiteFlashAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.6f);
            }

            if (Level.playAnim == true) {

                b.Draw(Main.pixel, new Vector2(0, 0), this.blackFlash,
                            Color.Black * this.blackFlashAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.61f);
            }
        }
    }
}