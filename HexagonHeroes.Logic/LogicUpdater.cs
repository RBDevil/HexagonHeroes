using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Logic
{
    public class LogicUpdater
    {
        Map map;
        public LogicUpdater()
        {
            map = new Map(30, 10);
        }
        public void AddPlayer(string ID, int positionX, int positionY)
        {
            map.Array[positionX, positionY] = new Entity(ID);
        }
        public void MovementInput(string playerID, int toX, int toY)
        {
            int[] entityPosition = GetEntityPosition(playerID);
            // check if to field is empty
            if (map.Array[toX, toY] == null)
            {
                map.Array[toX, toY] = map.Array[entityPosition[0], entityPosition[1]];
                map.Array[entityPosition[0], entityPosition[1]] = null;
            }
        }

        int[] GetEntityPosition(string ID)
        {
            for (int i = 0; i < map.Array.GetLength(0); i++)
            {
                for (int j = 0; j < map.Array.GetLength(1); j++)
                {
                    if (map.Array[i,j].ID == ID)
                    {
                        return new int[] { i, j };
                    }
                }
            }

            return new int[] { -1, -1 };
        }
    }
}
