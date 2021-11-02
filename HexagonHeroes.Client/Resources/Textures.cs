using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Client.Resources
{
    public static class Textures
    {
        public static int TileSize;
        public static Point[] TileVertexOffsets =
        {
            new Point(16, 0),
            new Point(56, 0),
            new Point(72, 35),
            new Point(56, 72),
            new Point(17, 72),
            new Point(0, 35)
        };
        public static Vector2 moveIndicatorOrigin { get; private set; }
        public static Dictionary<string, Texture2D> Container;
        public static void LoadTextures(ContentManager content)
        {
            Container = new Dictionary<string, Texture2D>();

            Container.Add("tile", content.Load<Texture2D>("tile"));
            TileSize = Container["tile"].Width;
            Container.Add("tile_frame", content.Load<Texture2D>("tile_frame"));
            Container.Add("player", content.Load<Texture2D>("player"));
            Container.Add("move_indicator", content.Load<Texture2D>("move_indicator"));
            moveIndicatorOrigin = new Vector2(Container["move_indicator"].Width / 2, Container["move_indicator"].Height / 2);
        }
    }
}

