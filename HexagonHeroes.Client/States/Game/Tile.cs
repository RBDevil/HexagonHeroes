using HexagonHeroes.Client.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Client.States.Game
{
    class Tile
    {
        Point positionIndex;
        public Point[] vertices { get; private set; }
        public Tile(Point positionIndex)
        {
            this.positionIndex = positionIndex;
            // set vertex positions
            vertices = new Point[6];
            if (positionIndex.X % 2 == 0)
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] = Textures.TileVertexOffsets[i] +
                        new Point(positionIndex.X * Textures.TileSize * 3 / 4, positionIndex.Y * Textures.TileSize);
                }
            }
            else
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] = Textures.TileVertexOffsets[i] +
                        new Point(positionIndex.X * Textures.TileSize * 3 / 4, (positionIndex.Y * Textures.TileSize) - Textures.TileSize / 2);
                }
            }
        }
        public void Draw(SpriteBatch sb)
        {
            if (positionIndex.X % 2 == 0)
            {
                sb.Draw(Textures.Container["tile"],
                        new Vector2(positionIndex.X * Textures.TileSize * 3 / 4, positionIndex.Y * Textures.TileSize),
                        Color.White);
            }
            else
            {
                sb.Draw(Textures.Container["tile"],
                        new Vector2(positionIndex.X * Textures.TileSize * 3 / 4, (positionIndex.Y * Textures.TileSize) - Textures.TileSize / 2),
                        Color.White);
            }
        }
    }
}
