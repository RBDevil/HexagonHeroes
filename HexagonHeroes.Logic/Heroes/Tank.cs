using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Logic.Heroes
{
    public enum HeroTypes
    {
        Tank,
        Mage,
        Support,
        Fighter,
    }
    class Tank : Entity
    {
        public Tank(string ID, int positionX, int positionY, string factionID) 
            : base(ID, positionX, positionY, factionID)
        {           
        }
    }
}
