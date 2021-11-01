using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Client.States.Game
{
    static class GameState
    {
        static bool active;
        static Networking.Client client;
        static Map map;
        static List<Entity> entities;
        public static void Exit()
        {
            active = false;
        }
        public static void Activate()
        {
            active = true;
            client = new Networking.Client(14242, "127.0.0.1", "game");
            map = new Map(new Point(30, 10));
        }
        public static void Update()
        {
            if (active)
            {

            }
        }
        public static void Draw(SpriteBatch sb)
        {
            if (active)
            {
                map.Draw(sb);
            }
        }
    }
}
