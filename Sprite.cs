using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBird {
    public class Sprite {

        public Texture2D Texture; //texture of the sprite
        public Rectangle Rectangle; //rectangle (drawn area) of the sprite
        public Vector2 Position; //position of the sprite
        public Vector2 Origin; //origin of the sprite
        public Color Hue; //hue of the sprite
        public float Rotation; //rotation level of the sprite
        public float Scale; //scale of the sprite
        public float Depth; //depth of the sprite
        public SpriteEffects Effect; //effect of the sprite

        //instantiate a sprite with texture and position
        public Sprite(Texture2D t, Vector2 p) {

            if (t != null) this.Texture = t;

            this.Position = p;

            //set other values to default
            this.Default();
        }

        //default values
        private void Default() {

            this.Origin = Vector2.Zero;
            this.Hue = Color.White;
            this.Rotation = 0f;
            this.Scale = 1f;
            this.Depth = 0f;
            this.Effect = SpriteEffects.None;
        }
    }
}