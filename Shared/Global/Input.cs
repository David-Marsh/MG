using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MG.Shared.Global
{
    public static class Input
    {
        private static KeyboardState keyboardState, lastKeyboardState;
        private static MouseState mouseState, lastMouseState;
        private static GamePadState gamepadState, lastGamepadState;
        private static bool mouseOnUI;
        public static Vector2 MousePosition { get { return new Vector2(mouseState.X, mouseState.Y); } }
        public static Point Position { get => mouseState.Position;}
        public static bool MouseOnUI { get => mouseOnUI; set => mouseOnUI = value; }
        public static void Update()
        {
            lastKeyboardState = keyboardState;
            lastMouseState = mouseState;
            lastGamepadState = gamepadState;

            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            gamepadState = GamePad.GetState(PlayerIndex.One);
        }
        public static bool OneShotKey(Keys key) => keyboardState.IsKeyDown(key) & lastKeyboardState.IsKeyUp(key);
        public static bool Pressed(Keys key) => keyboardState.IsKeyDown(key);
        public static bool OneShotMouseLeft(ButtonState buttonState) => mouseState.LeftButton == buttonState & lastMouseState.LeftButton != buttonState;
        public static bool OneShotMouseRight(ButtonState buttonState) => mouseState.RightButton == buttonState & lastMouseState.RightButton != buttonState;
        public static bool MouseLeft(ButtonState buttonState) => mouseState.LeftButton == buttonState;
        public static bool MouseRight(ButtonState buttonState) => mouseState.RightButton == buttonState;
        public static bool OneShotGamePad(Buttons buttons) => gamepadState.IsButtonDown(buttons) & lastGamepadState.IsButtonUp(buttons);
        public static Vector2 Stick()
        {
            Vector2 direction = gamepadState.ThumbSticks.Left;
            direction.Y *= -1;  // invert the y-axis
            if (keyboardState.IsKeyDown(Keys.A))
                direction.X -= 1;
            if (keyboardState.IsKeyDown(Keys.D))
                direction.X += 1;
            if (keyboardState.IsKeyDown(Keys.W))
                direction.Y -= 1;
            if (keyboardState.IsKeyDown(Keys.S))
                direction.Y += 1;
            // Clamp the length of the vector to a maximum of 1.
            if (direction.LengthSquared() > 1)
                direction.Normalize();
            return direction;
        }
    }
}
