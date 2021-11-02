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
        public Point MoveIndicator { get; set; }
        public Texture2D Texture { get; }
        public Entity(string ID, Point position, Texture2D texture)
        {
            this.ID = ID;
            PositionIndex = position;
            Texture = texture;
            MoveIndicator = new Point(-1, -1);
        }

        public void Move(Point position)
        {
            PositionIndex = position;
        }
        public void Draw(SpriteBatch sb)
        {
            DrawMoveIndicator(sb);

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
        void DrawMoveIndicator(SpriteBatch sb)
        {    
            if (PositionIndex.X % 2 == 0)
            {
                Vector2 tileMiddlePosition = new Vector2
                            (PositionIndex.X * Textures.TileSize * 3 / 4 + Textures.TileSize / 2,
                            (PositionIndex.Y * Textures.TileSize) + Textures.TileSize / 2);
                
                if (PositionIndex.X - 1 == MoveIndicator.X && PositionIndex.Y == MoveIndicator.Y)
                {
                    sb.Draw(Textures.Container["move_indicator"], tileMiddlePosition, null, 
                        Color.White, (float)(Math.PI + Math.PI / 6), Textures.moveIndicatorOrigin, 1f, SpriteEffects.None, 1);
                }
                else if (PositionIndex.X - 1 == MoveIndicator.X && PositionIndex.Y + 1 == MoveIndicator.Y)
                {
                    sb.Draw(Textures.Container["move_indicator"], tileMiddlePosition, null,
                        Color.White, (float)(Math.PI - Math.PI / 6), Textures.moveIndicatorOrigin, 1f, SpriteEffects.None, 1);
                }
                else if (PositionIndex.X == MoveIndicator.X && PositionIndex.Y + 1 == MoveIndicator.Y)
                {
                    sb.Draw(Textures.Container["move_indicator"], tileMiddlePosition, null,
                        Color.White, (float)(Math.PI / 2), Textures.moveIndicatorOrigin, 1f, SpriteEffects.None, 1);
                }
                else if (PositionIndex.X + 1 == MoveIndicator.X && PositionIndex.Y + 1 == MoveIndicator.Y)
                {
                    sb.Draw(Textures.Container["move_indicator"], tileMiddlePosition, null,
                        Color.White, (float)(Math.PI / 6), Textures.moveIndicatorOrigin, 1f, SpriteEffects.None, 1);
                }
                else if (PositionIndex.X + 1 == MoveIndicator.X && PositionIndex.Y == MoveIndicator.Y)
                {
                    sb.Draw(Textures.Container["move_indicator"], tileMiddlePosition, null,
                        Color.White, (float)(-Math.PI / 6), Textures.moveIndicatorOrigin, 1f, SpriteEffects.None, 1);
                }
                else if (PositionIndex.X == MoveIndicator.X && PositionIndex.Y - 1 == MoveIndicator.Y)
                {
                    sb.Draw(Textures.Container["move_indicator"], tileMiddlePosition, null,
                        Color.White, (float)(-Math.PI / 2), Textures.moveIndicatorOrigin, 1f, SpriteEffects.None, 1);
                }
            }
            else
            {
                Vector2 tileMiddlePosition = new Vector2
                        (PositionIndex.X * Textures.TileSize * 3 / 4 + Textures.TileSize / 2,
                        (PositionIndex.Y * Textures.TileSize));

                if (PositionIndex.X - 1 == MoveIndicator.X && PositionIndex.Y == MoveIndicator.Y)
                {
                    sb.Draw(Textures.Container["move_indicator"], tileMiddlePosition, null,
                        Color.White, (float)(Math.PI + -Math.PI / 6), Textures.moveIndicatorOrigin, 1f, SpriteEffects.None, 1);
                }
                else if (PositionIndex.X - 1 == MoveIndicator.X && PositionIndex.Y - 1 == MoveIndicator.Y)
                {
                    sb.Draw(Textures.Container["move_indicator"], tileMiddlePosition, null,
                        Color.White, (float)(Math.PI + Math.PI / 6), Textures.moveIndicatorOrigin, 1f, SpriteEffects.None, 1);
                }
                else if (PositionIndex.X == MoveIndicator.X && PositionIndex.Y + 1 == MoveIndicator.Y)
                {
                    sb.Draw(Textures.Container["move_indicator"], tileMiddlePosition, null,
                        Color.White, (float)(Math.PI / 2), Textures.moveIndicatorOrigin, 1f, SpriteEffects.None, 1);
                }
                else if (PositionIndex.X + 1 == MoveIndicator.X && PositionIndex.Y - 1 == MoveIndicator.Y)
                {
                    sb.Draw(Textures.Container["move_indicator"], tileMiddlePosition, null,
                        Color.White, (float)(-Math.PI / 6), Textures.moveIndicatorOrigin, 1f, SpriteEffects.None, 1);
                }
                else if (PositionIndex.X + 1 == MoveIndicator.X && PositionIndex.Y == MoveIndicator.Y)
                {
                    sb.Draw(Textures.Container["move_indicator"], tileMiddlePosition, null,
                        Color.White, (float)(Math.PI / 6), Textures.moveIndicatorOrigin, 1f, SpriteEffects.None, 1);
                }
                else if (PositionIndex.X == MoveIndicator.X && PositionIndex.Y - 1 == MoveIndicator.Y)
                {
                    sb.Draw(Textures.Container["move_indicator"], tileMiddlePosition, null,
                        Color.White, (float)(-Math.PI / 2), Textures.moveIndicatorOrigin, 1f, SpriteEffects.None, 1);
                }
            }
        }
    }
}
