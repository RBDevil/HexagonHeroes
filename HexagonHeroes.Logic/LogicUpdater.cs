using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Logic
{
    public class LogicUpdater
    {
        List<Entity> entities;
        public LogicUpdater()
        {
            entities = new List<Entity>();
        }
        public void DeletePlayer(string ID)
        {
            entities.Remove(entities.Find(e => e.ID == ID));
        }
        public void AddPlayer(string ID, int positionX, int positionY)
        {
            entities.Add(new Entity(ID, positionX, positionY));
        }
        void MovementInput(string playerID, int toX, int toY)
        {
            bool isEmpty = true;
            // check if to field is empty
            foreach (Entity entity in entities)
            {
                if (entity.PositionX == toX && entity.PositionY == toY)
                {
                    isEmpty = false;
                    break;
                }
            }
            if (isEmpty)
            {
                Entity player = entities.Find(e => e.ID == playerID);
                player.PositionX = toX;
                player.PositionY = toY;
            }
        }
        public bool UpdateMoveIndicator(string ID, int toX, int toY)
        {
            // check if no other indicator points here
            bool isEmpty = true;
            foreach (var item in entities)
            {
                if(item.MoveIndicatorX == toX && item.MoveIndicatorY == toY)
                {
                    isEmpty = false;
                    break;
                }
            }
            if (isEmpty)
            {
                Entity player = entities.Find(e => e.ID == ID);
                player.MoveIndicatorX = toX;
                player.MoveIndicatorY = toY;
            }

            return isEmpty;
        }
        public int[] GetEntityPosition(string ID)
        {
            Entity entity = entities.Find(e => e.ID == ID);
            if (entity != null)
            {
                return new int[] { entity.PositionX, entity.PositionY };
            }
            else
            {
                return new int[] { -1, -1 };
            }
        }
        public void UpdateTurn()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].MoveIndicatorX != -1)
                {
                    MovementInput(entities[i].ID, entities[i].MoveIndicatorX, entities[i].MoveIndicatorY);
                }
            }
        }
    }
}
