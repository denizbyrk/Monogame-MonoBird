using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBird {
    internal class Sprite {

        public Texture2D Texture;
        public Rectangle Rectangle;
        public Vector2 Position;
        public Vector2 Origin;
        public Color Hue;
        public float Rotation;
        public float Scale;
        public float Depth;
        public SpriteEffects Effect;

        public Sprite(Texture2D t, Vector2 p) {

            if (t != null) this.Texture = t;

            this.Position = p;

            this.Default();
        }

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