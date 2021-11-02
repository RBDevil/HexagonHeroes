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
        public int Health { get; set; }
        public int MaxHealth { get; private set; }
        public string FactionID { get; }
        public int Damage { get; set; }
        public Entity(string ID, Point position, Texture2D texture, string factionID)
        {
            this.ID = ID;
            PositionIndex = position;
            Texture = texture;
            MoveIndicator = new Point(-1, -1);
            FactionID = factionID;
            MaxHealth = 100;
            Damage = 15;
        }

        public void RegisterDamage(int damage)
        {
            Health -= damage;
        }
        public void Move(Point position)
        {
            PositionIndex = position;
        }
        public void Draw(SpriteBatch sb)
        {
            Vector2 tileMiddlePosition;
            if (PositionIndex.X % 2 == 0)
            {
                tileMiddlePosition = new Vector2
                (PositionIndex.X * Textures.TileSize * 3 / 4 + Textures.TileSize / 2,
                (PositionIndex.Y * Textures.TileSize) + Textures.TileSize / 2);
            }
            else
            {
                tileMiddlePosition = new Vector2
                        (PositionIndex.X * Textures.TileSize * 3 / 4 + Textures.TileSize / 2,
                        (PositionIndex.Y * Textures.TileSize));
            }

            DrawMoveIndicator(sb, tileMiddlePosition);

            Vector2 offset = new Vector2(10, 0);
            if (PositionIndex.X % 2 == 0)
            {
                sb.Draw(Texture,
                        new Vector2(PositionIndex.X * Textures.TileSize * 3 / 4, PositionIndex.Y * Textures.TileSize) + offset,
                        Color.White);
            }
            else
            {
                sb.Draw(Texture,
                        new Vector2(PositionIndex.X * Textures.TileSize * 3 / 4, (PositionIndex.Y * Textures.TileSize) - Textures.TileSize / 2)
                        + offset,
                        Color.White);
            }

            DrawHealthBar(sb, tileMiddlePosition);
        }

        void DrawHealthBar(SpriteBatch sb, Vector2 tileMiddlePosition)
        {
            Vector2 offset = new Vector2(-25, -15);
            sb.Draw(Textures.Container["healthbar_frame"], tileMiddlePosition + offset, Color.White);

            int textureHeight = Textures.Container["healthbar_frame"].Height;
            // caluclate health percent
            double percentHealth = ((double)Health / (double)MaxHealth) * 100;
            // calculate how many pixels needed
            int pixelPercentValue = MaxHealth / (textureHeight - 2);
            double pixelCount = percentHealth / pixelPercentValue;
            
            offset.Y += Textures.Container["healthbar_frame"].Height;
            offset.X += 1;
            for (int i = 0; i < pixelCount; i++)
            {
                offset.Y -= 1;
                sb.Draw(Textures.Container["percent_health"], tileMiddlePosition + offset, Color.White);
            }
        }

        void DrawMoveIndicator(SpriteBatch sb, Vector2 tileMiddlePosition)
        {    
            if (PositionIndex.X % 2 == 0)
            {             
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
