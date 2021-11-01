using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Client.States.Game
{
    class Entity
    {
        public Point Position { get; private set; }
        public Texture2D texture { get; }
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        public Entity(Point position, Texture2D texture, int maxHealth)
        {
            Position = position;
            this.texture = texture;
            MaxHealth = maxHealth;
            Health = MaxHealth;
        }

        public void Draw(SpriteBatch sb)
        {

        }
    }
}
