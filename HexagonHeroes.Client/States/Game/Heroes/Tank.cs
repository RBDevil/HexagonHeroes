using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Client.States.Game.Heores
{
    public enum HeroTypes
    {
        NotHero,
        Tank,
        Mage,
        Support,
        Fighter,
    }
    class Tank : Entity
    {
        public Tank(string ID, Point position, Texture2D texture, string factionID) 
            : base(ID, position, texture, factionID)
        {
        }
    }
}
