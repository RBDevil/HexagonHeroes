using HexagonHeroes.Logic.Heroes;
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
            AddEnemy("enemy1", 8, 3);
        }
        public void DeletePlayer(string ID)
        {
            entities.Remove(entities.Find(e => e.ID == ID));
        }
        public void AddEnemy(string ID, int positionX, int positionY)
        {
            entities.Add(new Entity(ID, positionX, positionY, "enemies"));
        }
        public void AddEntity(string ID, int positionX, int positionY, string factionID, string heroType = "Not a hero")
        {
            switch (heroType)
            {
                case "tank":
                    entities.Add(new Tank(ID, positionX, positionY, factionID));
                    break;
                case "mage":
                    entities.Add(new Tank(ID, positionX, positionY, factionID));
                    break;
                case "support":
                    entities.Add(new Tank(ID, positionX, positionY, factionID));
                    break;
                case "fighter":
                    entities.Add(new Tank(ID, positionX, positionY, factionID));
                    break;
                case "Not a hero":
                    entities.Add(new Entity(ID, positionX, positionY, factionID));
                    break;
            }            
        }
        void MovementInput(string playerID, int toX, int toY)
        {
            Entity player = entities.Find(e => e.ID == playerID);
            player.PositionX = toX;
            player.PositionY = toY;
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
        public List<string> GetAllEntityIDs()
        {
            List<string> IDs = new List<string>();
            foreach (var item in entities)
            {
                IDs.Add(item.ID);
            }

            return IDs;
        }
        public void UpdateTurn()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].MoveIndicatorX != -1)
                {
                    bool isEmpty = true;
                    // check if to field is empty
                    foreach (Entity entity in entities)
                    {
                        if (entity.PositionX == entities[i].MoveIndicatorX && entity.PositionY == entities[i].MoveIndicatorY)
                        {
                            if (entity.FactionID != entities[i].FactionID)
                            {
                                DamageInput(entities[i], entity);
                            }

                            isEmpty = false;
                            break;
                        }
                    }
                    if (isEmpty)
                    {
                        MovementInput(entities[i].ID, entities[i].MoveIndicatorX, entities[i].MoveIndicatorY);
                    }
                }
            }
        }
        public int GetEntitiyHealth(string ID)
        {
            Entity entity = entities.Find(e => e.ID == ID);
            if (entity != null)
            {
                return entity.Health;
            }
            else
            {
                return -1;
            }
        }
        void DamageInput(Entity attacker, Entity reciever)
        {
            reciever.Health -= attacker.Damage;
        }
        public string GetHeroType(string ID)
        {
            Entity entity = entities.Find(e => e.ID == ID);
            if (entity is Tank)
            {
                return "tank";
            }

            return "Not a hero";
        }
        public string GetEntityFaction(string ID)
        {
            return entities.Find(e => e.ID == ID).FactionID;
        }
    }
}
