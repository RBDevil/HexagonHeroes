using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Logic
{
    class Entity
    {
        public string ID { get; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int MoveIndicatorX { get; set; }
        public int MoveIndicatorY { get; set; }
        public int SpellIndicatorX { get; set; }
        public int SpellIndicatorY { get; set; }
        public int SpellID { get; set; }
        public int Health { get; set; }
        public string FactionID { get; }
        public int Damage { get; set; }
        public Entity(string ID, int positionX, int positionY, string factionID)
        {
            this.ID = ID;
            PositionX = positionX;
            PositionY = positionY;
            MoveIndicatorX = -1;
            MoveIndicatorY = -1;
            FactionID = factionID;
            Health = 100;
            Damage = 10;
        }
    }
}
