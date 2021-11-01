using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Logic
{     
    class Map
    {
        public Entity[,] Array { get; set; }
        public Map(int sizeX, int sizeY)
        {
            Array = new Entity[sizeX, sizeY];
        }
    }
}
