using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Logic
{
    public enum HeroTypes
    {
        Tank,
        Mage,
        Support,
        Fighter,
    }
    class Hero : Entity
    {
        public HeroTypes HeroType;
        public Hero(string ID, int positionX, int positionY, string factionID, HeroTypes heroType) 
            : base(ID, positionX, positionY, factionID)
        {
            HeroType = heroType;
        }

    }
}
