using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Logic
{
    class Entity
    {
        public string ID { get; }
        public int PositionX {get; set; }
        public int PositionY { get; set; }
        public int MoveIndicatorX { get; set; }
        public int MoveIndicatorY { get; set; }
        public int Health { get; set; }
        public Entity(string ID, int positionX, int positionY)
        {
            this.ID = ID;
            PositionX = positionX;
            PositionY = positionY;
            MoveIndicatorX = -1;
            MoveIndicatorY = -1;
        }
    }
}
