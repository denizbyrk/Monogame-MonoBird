using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBird {
    public class Bird {

        private Sprite bird;
        private Texture2D birdTexture;
        private string birdTexturePath;
        private Rectangle birdHitbox;

        private Vector2 velocity;
        private float maxVelocity;
        private float rotationSpeed;
        private float gravity;
        private float jumpStrength;
        private int groundHeight;
        private int animationSpeed;
        private int currentFrame;
        private int frames;
        private int timer;
        private bool reverseAnim;

        public Bird () {

            this.birdTexturePath = "Assets\\Bird";

            this.velocity = new Vector2(0, 0);
            this.maxVelocity = 750;
            this.rotationSpeed = 4f;
            this.gravity = 1500f;
            this.jumpStrength = 32000f;
            this.animationSpeed = 100;
            this.currentFrame = 0;
            this.frames = 2;
            this.timer = 0;
            this.reverseAnim = true;
        }

        public void LoadContent(int groundHeight) {

            this.birdTexture = Main.contentManager.Load<Texture2D>(this.birdTexturePath);
            var birdPosition = new Vector2(Main.screenWidth / 10, ((Main.screenHeight - (groundHeight * Main.spriteScale)) / 2) - (birdTexture.Height));
            
            this.bird = new Sprite(birdTexture, birdPosition);

            this.bird.Rotation = 0;
            this.bird.Scale = Main.spriteScale;
            this.bird.Rectangle = new Rectangle(0, 0, 17, 12);
            this.bird.Origin = new Vector2((float)this.bird.Rectangle.Width / 2, (float)this.bird.Rectangle.Height / 2);

            this.birdHitbox = new Rectangle((int)this.bird.Position.X - 8 * Main.spriteScale, (int)this.bird.Position.Y - 6 * Main.spriteScale,
                                                    this.bird.Rectangle.Width * Main.spriteScale, this.bird.Rectangle.Height * Main.spriteScale);

            this.groundHeight = groundHeight;
        }

        private void AnimateBird(GameTime dt) {

            this.timer += (int)dt.ElapsedGameTime.TotalMilliseconds;

            if (this.timer > this.animationSpeed) {

                if (this.reverseAnim == true) {

                    this.currentFrame--;

                } else if (this.reverseAnim == false) {

                    this.currentFrame++;
                }
                
                this.timer = 0;

                if (this.currentFrame < 0) {

                    this.currentFrame = 1;
                    this.reverseAnim = false;
                }

                if (this.currentFrame > this.frames) {

                    this.currentFrame = 1;
                    this.reverseAnim = true;
                }
            }

            this.bird.Rectangle = new Rectangle(this.currentFrame * 17, 0, 17, 12);
        }

        public void Update(GameTime dt) {

            if (Main.gameReset == true) {

                this.velocity.Y = 0;
                this.bird.Position = new Vector2(Main.screenWidth / 10, ((Main.screenHeight - (groundHeight * Main.spriteScale)) / 2) - (birdTexture.Height));
                this.bird.Rotation = 0f;

                Main.gameOver = false;
                Main.gameStarted = false;
                Main.gameReset = false;
            }

            float deltaTime = (float)dt.ElapsedGameTime.TotalSeconds;

            if (Main.gameOver == false) this.AnimateBird(dt);

            if (Input.IsLeftClickDown() && Main.gameOver == false && Input.checkMouseCoordinates()) {

                this.velocity.Y = 0;
                this.velocity.Y -= this.jumpStrength * deltaTime;

                Sounds.SoundEffectInstances["wing"].Stop();
                Sounds.SoundEffectInstances["wing"].Play();

                Main.gameStarted = true;
            }

            if (Main.gameStarted == true) this.velocity.Y += this.gravity * deltaTime;
            this.velocity.Y = MathHelper.Clamp(this.velocity.Y, 0 - this.maxVelocity, this.maxVelocity);

            if (this.velocity.Y > 0) {

                this.bird.Rotation += this.rotationSpeed * deltaTime;

            } else if (this.velocity.Y < 0) {
                
                if (this.bird.Rotation > MathHelper.TwoPi / 16) {

                    this.bird.Rotation -= this.rotationSpeed * 10 * deltaTime;
                } else {

                    this.bird.Rotation -= this.rotationSpeed * deltaTime;
                }
            }

            this.bird.Rotation = MathHelper.Clamp(this.bird.Rotation, -MathHelper.TwoPi / 12, MathHelper.TwoPi / 4);

            this.bird.Position.Y += this.velocity.Y * deltaTime;
            this.bird.Position.Y = MathHelper.Clamp(this.bird.Position.Y, 0, Main.screenHeight - (this.groundHeight * Main.spriteScale) - this.bird.Texture.Height * Main.spriteScale / 2);

            this.birdHitbox = new Rectangle((int)this.bird.Position.X - 8 * Main.spriteScale, (int)this.bird.Position.Y - 6 * Main.spriteScale,
                                                    this.bird.Rectangle.Width * Main.spriteScale, this.bird.Rectangle.Height * Main.spriteScale);
        }

        public void Draw(SpriteBatch b) {

            b.Draw(this.bird.Texture, this.bird.Position, this.bird.Rectangle, this.bird.Hue, this.bird.Rotation,
                        this.bird.Origin, this.bird.Scale, this.bird.Effect, 0.4f);

            if (Main.debug == true) {

                b.Draw(Main.pixel, new Vector2(this.bird.Position.X - (float)4.25, this.bird.Position.Y - (float)2.75), this.birdHitbox, Color.Yellow, this.bird.Rotation, new Vector2(this.birdHitbox.Width / 2, this.birdHitbox.Height / 2), 1f, SpriteEffects.None, 0.41f);
            }
        }

        public Rectangle getBirdRectangle() => new Rectangle((int)(this.bird.Position.X - 24), (int)(this.bird.Position.Y - 18), this.birdHitbox.Width, this.birdHitbox.Height);

        public Vector2 getBirdPosition() => new Vector2(this.bird.Position.X, this.bird.Position.Y);

        public int getBirdTextureHeight() => this.birdTexture.Height;
    }
}