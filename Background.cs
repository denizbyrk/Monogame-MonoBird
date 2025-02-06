using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace MonoBird {
    public class Background {

        private Sprite[] Sky;
        private List<Sprite> groundArray;

        private string skyTexturePath;
        private string groundTexturePath;
 
        public Background () {

            this.skyTexturePath = "Assets\\Background";
            this.groundTexturePath = "Assets\\Ground";

            this.Sky = new Sprite[3];
            this.groundArray = new List<Sprite>();
        }

        public void LoadContent() {

            Sprite s1, s2, s3;

            var skyTexture = Main.contentManager.Load<Texture2D>(this.skyTexturePath);
            var skyPosition = new Vector2(0, 0 - skyTexture.Height / 4);

            s1 = new Sprite(skyTexture, skyPosition);
            s2 = new Sprite(skyTexture, new Vector2(skyPosition.X + skyTexture.Width * Main.spriteScale, s1.Position.Y));
            s3 = new Sprite(skyTexture, new Vector2((skyPosition.X + skyTexture.Width * Main.spriteScale * 2), s1.Position.Y));

            this.Sky[0] = s1;
            this.Sky[1] = s2;
            this.Sky[2] = s3;

            for (int i = 0; i <= 2; i++) {

                this.Sky[i].Scale = Main.spriteScale;
            }

            Sprite g1, g2, g3;

            var groundTexture = Main.contentManager.Load<Texture2D>(this.groundTexturePath);

            g1 = new Sprite(groundTexture, new Vector2(0, Main.screenHeight - groundTexture.Height * Main.spriteScale));
            g2 = new Sprite(groundTexture, new Vector2(g1.Position.X + groundTexture.Width * Main.spriteScale, g1.Position.Y));
            g3 = new Sprite(groundTexture, new Vector2((g1.Position.X + groundTexture.Width * Main.spriteScale) * 2, g1.Position.Y));

            this.groundArray.Add(g1);
            this.groundArray.Add(g2);
            this.groundArray.Add(g3);

            foreach (var g in this.groundArray) {

                g.Scale = Main.spriteScale;
            }
        }

        public void Update(GameTime dt) {

            if (this.groundArray.First().Position.X < -36) {

                foreach (var g in this.groundArray) {

                    g.Position.X += 36;
                }
            }

            foreach (var g in this.groundArray) {

                g.Position.X -= Main.gameSpeed;
            }
        }

        public void Draw(SpriteBatch b) {

            foreach (var s in this.Sky) {

                b.Draw(s.Texture, s.Position, null, s.Hue, s.Rotation,
                            s.Origin, s.Scale, s.Effect, 0.1f);
            }

            foreach (var g in this.groundArray) {

                b.Draw(g.Texture, g.Position, null, g.Hue, g.Rotation,
                            g.Origin, g.Scale, g.Effect, 0.3f);
            }
        }

        public int getGroundHeight() => (int)this.groundArray.First().Texture.Height;
    }
}