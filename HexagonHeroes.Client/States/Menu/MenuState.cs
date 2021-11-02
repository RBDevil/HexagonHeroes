using HexagonHeroes.Client.Resources;
using HexagonHeroes.Client.States.Game;
using HexagonHeroes.Client.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Client.States.Menu
{
    static class MenuState
    {
        static UI_Updater UI;
        static bool active;
        public static void Update()
        {
            if (active)
            {
                UI.Update();
            }
        }
        public static void Draw(SpriteBatch sb)
        {
            if (active)
            {
                UI.Draw(sb);
            }
        }
        public static void Exit()
        {
            active = false;
        }
        public static void Activate()
        {
            InitUI();
            active = true;
        }
        static void InitUI()
        {
            UI = new UI_Updater();
            UI.AddButton(Textures.Container["button"], new Point(150, 200), PlayButtonAction);
        }
        static void PlayButtonAction()
        {
            Exit();
            GameState.Activate();
        }
    }
}
