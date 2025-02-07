using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBird {
    public class Bird {

        //bird sprite
        private Sprite bird;
        private Texture2D birdTexture;
        private string birdTexturePath;

        //bird hitbox
        private Rectangle birdHitbox;

        //velocity
        private Vector2 velocity;

        //max velocity ()
        private float maxVelocity;

        //rotation speed
        private float rotationSpeed;

        //gravity level
        private float gravity;

        //jump strength
        private float jumpStrength;

        //ground height (for colliding with ground)
        private int groundHeight;

        //animation properties
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

        //load content
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

        //animate the bird
        private void AnimateBird(GameTime dt) {

            this.timer += (int)dt.ElapsedGameTime.TotalMilliseconds;

            //reverse the animation if it is completed
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

            //set current sprite
            this.bird.Rectangle = new Rectangle(this.currentFrame * 17, 0, 17, 12);
        }

        //update
        public void Update(GameTime dt) {

            //reset bird properties
            if (Main.gameReset == true) {

                this.velocity.Y = 0;
                this.bird.Position = new Vector2(Main.screenWidth / 10, ((Main.screenHeight - (groundHeight * Main.spriteScale)) / 2) - (birdTexture.Height));
                this.bird.Rotation = 0f;

                Main.gameOver = false;
                Main.gameStarted = false;
                Main.gameReset = false;
            }

            float deltaTime = (float)dt.ElapsedGameTime.TotalSeconds;

            //stop animating on death
            if (Main.gameOver == false) this.AnimateBird(dt);

            //press left click to jump
            if (Input.IsLeftClickDown() && Main.gameOver == false && Input.checkMouseCoordinates()) {

                this.velocity.Y = 0;
                this.velocity.Y -= this.jumpStrength * deltaTime;

                Sounds.SoundEffectInstances["wing"].Stop();
                Sounds.SoundEffectInstances["wing"].Play();

                Main.gameStarted = true;
            }

            //apply jump physics
            if (Main.gameStarted == true) this.velocity.Y += this.gravity * deltaTime;
            this.velocity.Y = MathHelper.Clamp(this.velocity.Y, 0 - this.maxVelocity, this.maxVelocity);

            //rotation logic for bird, according to Y velocity
            if (this.velocity.Y > 0) {

                this.bird.Rotation += this.rotationSpeed * deltaTime;

            } else if (this.velocity.Y < 0) {
                
                if (this.bird.Rotation > MathHelper.TwoPi / 16) {

                    this.bird.Rotation -= this.rotationSpeed * 10 * deltaTime;
                } else {

                    this.bird.Rotation -= this.rotationSpeed * deltaTime;
                }
            }

            //rotate bird
            this.bird.Rotation = MathHelper.Clamp(this.bird.Rotation, -MathHelper.TwoPi / 12, MathHelper.TwoPi / 4);

            //move bird
            this.bird.Position.Y += this.velocity.Y * deltaTime;
            this.bird.Position.Y = MathHelper.Clamp(this.bird.Position.Y, 0, Main.screenHeight - (this.groundHeight * Main.spriteScale) - this.bird.Texture.Height * Main.spriteScale / 2);

            //set hitbox for bird
            this.birdHitbox = new Rectangle((int)this.bird.Position.X - 8 * Main.spriteScale, (int)this.bird.Position.Y - 6 * Main.spriteScale,
                                                    this.bird.Rectangle.Width * Main.spriteScale, this.bird.Rectangle.Height * Main.spriteScale);
        }

        //draw
        public void Draw(SpriteBatch b) {

            //draw bird
            b.Draw(this.bird.Texture, this.bird.Position, this.bird.Rectangle, this.bird.Hue, this.bird.Rotation,
                        this.bird.Origin, this.bird.Scale, this.bird.Effect, 0.4f);
        }

        public Rectangle getBirdRectangle() => new Rectangle((int)(this.bird.Position.X - 24), (int)(this.bird.Position.Y - 18), this.birdHitbox.Width, this.birdHitbox.Height);

        public Vector2 getBirdPosition() => new Vector2(this.bird.Position.X, this.bird.Position.Y);

        public int getBirdTextureHeight() => this.birdTexture.Height;
    }
}   