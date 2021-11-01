using HexagonHeroes.Client.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Client.States.Game
{
    class Map
    {
        Point size;
        Tile[,] map;

        public Map(Point size)
        {
            this.size = size;
            map = new Tile[size.X, size.Y];
            // generating map
            for (int i = 0; i < size.X; i++)
            {
                for (int j = 0; j < size.Y; j++)
                {
                    map[i, j] = new Tile(new Point(i, j));
                }
            }
        }

        public void Draw(SpriteBatch sb, Point moveIndicatorPosition)
        {
            foreach (Tile tile in map)
            {
                tile.Draw(sb);
            }

            HighlightTile(sb, moveIndicatorPosition);

        }

        void HighlightTile(SpriteBatch sb, Point positionIndex)
        {
            if (positionIndex.X % 2 == 0)
            {
                sb.Draw(Textures.Container["tile_frame"],
                        new Vector2(positionIndex.X * Textures.TileSize * 3 / 4, positionIndex.Y * Textures.TileSize),
                        Color.White);
            }
            else
            {
                sb.Draw(Textures.Container["tile_frame"],
                        new Vector2(positionIndex.X * Textures.TileSize * 3 / 4, (positionIndex.Y * Textures.TileSize) - Textures.TileSize / 2),
                        Color.White);
            }
        }
        /// <summary>
        /// returns -1, -1 if no tile is hovered
        /// </summary>
        /// <returns></returns>
        public Point GetHoveredTile()
        {
            // get which tile is closest
            Point closestTile = new Point(
                MouseManager.Position.X / (Textures.TileSize * 3 / 4),
                MouseManager.Position.Y / Textures.TileSize);
            // this narrows the possible tiles to 6
            // run through these and check if cursor is inside
            Point nextToCheck = closestTile;

            if (closestTile.X < size.X && closestTile.X >= 0 &&
                closestTile.Y < size.Y && closestTile.Y >= 0)
            {
                if (CheckIfHovered(closestTile))
                {
                    return closestTile;
                }
            }

            if (closestTile.X - 1 < size.X && closestTile.X - 1 >= 0 &&
                closestTile.Y - 1 < size.Y && closestTile.Y - 1 >= 0)
            {
                nextToCheck = new Point(closestTile.X - 1, closestTile.Y - 1);
                if (CheckIfHovered(nextToCheck))
                {
                    return nextToCheck;
                }
            }

            if (closestTile.X - 1 < size.X && closestTile.X - 1 >= 0 &&
                closestTile.Y < size.Y && closestTile.Y >= 0)
            {
                nextToCheck = new Point(closestTile.X - 1, closestTile.Y);
                if (CheckIfHovered(nextToCheck))
                {
                    return nextToCheck;
                }
            }

            if (closestTile.X < size.X && closestTile.X >= 0 &&
                closestTile.Y + 1 < size.Y && closestTile.Y + 1 >= 0)
            {
                nextToCheck = new Point(closestTile.X, closestTile.Y + 1);
                if (CheckIfHovered(nextToCheck))
                {
                    return nextToCheck;
                }
            }

            if (closestTile.X + 1 < size.X && closestTile.X + 1 >= 0 &&
                closestTile.Y < size.Y && closestTile.Y >= 0)
            {
                nextToCheck = new Point(closestTile.X + 1, closestTile.Y);
                if (CheckIfHovered(nextToCheck))
                {
                    return nextToCheck;
                }
            }

            if (closestTile.X + 1 < size.X && closestTile.X + 1 >= 0 &&
                closestTile.Y - 1 < size.Y && closestTile.Y - 1 >= 0)
            {
                nextToCheck = new Point(closestTile.X + 1, closestTile.Y - 1);
                if (CheckIfHovered(nextToCheck))
                {
                    return nextToCheck;
                }
            }

            if (closestTile.X < size.X && closestTile.X >= 0 &&
                closestTile.Y - 1 < size.Y && closestTile.Y - 1 >= 0)
            {
                nextToCheck = new Point(closestTile.X, closestTile.Y - 1);
                if (CheckIfHovered(nextToCheck))
                {
                    return nextToCheck;
                }
            }

            return new Point(-1, -1);
        }

        bool CheckIfHovered(Point tileIndex)
        {
            bool collision = false;

            int px = MouseManager.Position.X;
            int py = MouseManager.Position.Y;

            // go through each of the vertices, plus
            // the next vertex in the list
            int next = 0;
            for (int current = 0; current < 6; current++)
            {
                // get next vertex in list
                // if we've hit the end, wrap around to 0
                next = current + 1;
                if (next == 6) next = 0;

                // get the PVectors at our current position
                // this makes our if statement a little cleaner
                Point vc = map[tileIndex.X, tileIndex.Y].vertices[current];    // c for "current"
                Point vn = map[tileIndex.X, tileIndex.Y].vertices[next];       // n for "next"

                // compare position, flip 'collision' variable
                // back and forth
                if (((vc.Y >= py && vn.Y < py) || (vc.Y < py && vn.Y >= py)) &&
                     (px < (vn.X - vc.X) * (py - vc.Y) / (vn.Y - vc.Y) + vc.X))
                {
                    collision = !collision;
                }
            }

            return collision;
        }
    }
}
