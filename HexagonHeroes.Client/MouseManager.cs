using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Client
{
    static class MouseManager
    {
        public static bool LeftClick { get; private set; }
        public static Point ClickStartingPosition { get; private set; }
        public static Point Position { get; private set; }

        static bool leftButtonPressed, previousLeftButtonPressed;
        public static void Update()
        {
            LeftClick = false;
            MouseState mstate = Mouse.GetState();
            Position = mstate.Position;
            previousLeftButtonPressed = leftButtonPressed;
            leftButtonPressed = mstate.LeftButton == ButtonState.Pressed;

            if (leftButtonPressed)
            {
                if (!previousLeftButtonPressed)
                {
                    ClickStartingPosition = mstate.Position; // Holding left click starts here
                }
            }
            else if (previousLeftButtonPressed)
            {
                LeftClick = true;
            }
        }
    }
}
