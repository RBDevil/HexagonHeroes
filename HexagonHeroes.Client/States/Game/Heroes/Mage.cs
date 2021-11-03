using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Client.States.Game.Heroes
{
    class Mage : Entity
    {
        public Mage(string ID, Point position, Texture2D texture, string factionID) 
            : base(ID, position, texture, factionID)
        {
        }
    }
}
