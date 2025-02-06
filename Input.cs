using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoBird {
    public class Input {

        private static KeyboardState prevKState;
        private static KeyboardState currentKState;

        private static MouseState prevMState;
        private static MouseState currentMState;

        private static Rectangle mouseRectangle;

        public static Rectangle getMouseRectangle() => Input.mouseRectangle;

        public static bool IsKeyDown(Keys k) => Input.prevKState.IsKeyUp(k) && Input.currentKState.IsKeyDown(k);

        public static bool IsKeyHold(Keys k) => Input.currentKState.IsKeyDown(k);

        public static bool IsLeftClickDown() => Input.prevMState.LeftButton == ButtonState.Released && Input.currentMState.LeftButton == ButtonState.Pressed;

        public static bool checkMouseCoordinates() => Input.getMouseRectangle().X < Main.screenWidth && Input.getMouseRectangle().Y < Main.screenHeight && Input.getMouseRectangle().X > 0 && Input.getMouseRectangle().Y > 0;
        

        public static void Update() {

            Input.prevKState = Input.currentKState;
            Input.currentKState = Keyboard.GetState();

            Input.prevMState = Input.currentMState;
            Input.currentMState = Mouse.GetState();

            Input.mouseRectangle = new Rectangle(Input.currentMState.X, Input.currentMState.Y, 1, 1);
        }
    }
}