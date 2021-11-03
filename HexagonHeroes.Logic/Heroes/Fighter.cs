using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Logic.Heroes
{
    class Fighter : Entity
    {
        public Fighter(string ID, int positionX, int positionY, string factionID) 
            : base(ID, positionX, positionY, factionID)
        {
        }
    }
}
