using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Client.States.Game
{
    public enum HeroTypes
    {
        NotHero,
        Tank,
        Mage,
        Support,
        Fighter,
    }
    class Hero : Entity
    {
        public HeroTypes HeroType;
        public Hero(string ID, Point position, Texture2D texture, string factionID, HeroTypes heroType) : base(ID, position, texture, factionID)
        {
            HeroType = heroType;
        }
    }
}
