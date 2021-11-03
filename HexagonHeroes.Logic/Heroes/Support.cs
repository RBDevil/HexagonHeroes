using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Logic.Heroes
{
    class Support : Entity
    {
        public Support(string ID, int positionX, int positionY, string factionID) 
            : base(ID, positionX, positionY, factionID)
        {
        }
    }
}
