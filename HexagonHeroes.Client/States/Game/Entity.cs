using HexagonHeroes.Client.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Client.States.Game
{
    class Entity
    {
        public string ID { get; }
        public Point PositionIndex { get; private set; }
        public Texture2D Texture { get; }
        public Entity(string ID, Point position, Texture2D texture)
        {
            this.ID = ID;
            PositionIndex = position;
            Texture = texture;
        }

        public void Move(Point position)
        {
            PositionIndex = position;
        }
        public void Draw(SpriteBatch sb)
        {
            if (PositionIndex.X % 2 == 0)
            {
                sb.Draw(Texture,
                        new Vector2(PositionIndex.X * Textures.TileSize * 3 / 4, PositionIndex.Y * Textures.TileSize),
                        Color.White);
            }
            else
            {
                sb.Draw(Texture,
                        new Vector2(PositionIndex.X * Textures.TileSize * 3 / 4, (PositionIndex.Y * Textures.TileSize) - Textures.TileSize / 2),
                        Color.White);
            }
        }
    }
}
