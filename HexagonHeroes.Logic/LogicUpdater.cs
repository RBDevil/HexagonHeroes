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
                    entities.Add(new Mage(ID, positionX, positionY, factionID));
                    break;
                case "support":
                    entities.Add(new Support(ID, positionX, positionY, factionID));
                    break;
                case "fighter":
                    entities.Add(new Fighter(ID, positionX, positionY, factionID));
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
                                BasicAttackInput(entities[i], entity);
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
        void BasicAttackInput(Entity attacker, Entity reciever)
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
            else if (entity is Mage)
            {
                return "mage";
            }
            else if (entity is Support)
            {
                return "support";
            }
            else if (entity is Fighter)
            {
                return "fighter";
            }

            return "Not a hero";
        }
        public string GetEntityFaction(string ID)
        {
            return entities.Find(e => e.ID == ID).FactionID;
        }
        void SupportSpell1(Entity caster)
        {
            caster.Health += 15;
            int [] casterPosition = GetEntityPosition(caster.ID);
            List<int[]> adjacentTiles = GetAdjacentTiles(casterPosition[0], casterPosition[1]);
            foreach (var entity in entities)
            {
                int[] entityPosition = GetEntityPosition(entity.ID);
                foreach (var tile in adjacentTiles)
                {
                    if (entityPosition[0] == tile[0] && entityPosition[1] == tile[1] &&
                        caster.FactionID == entity.FactionID)
                    {
                        entity.Health += 15;
                    }
                }
            }
        }
        List<int[]> GetAdjacentTiles(int positionX, int positionY)
        {
            List<int[]> adjacentTiles = new List<int[]>();

            adjacentTiles.Add(new int[] { positionX - 1, positionY });
            adjacentTiles.Add(new int[] { positionX, positionY + 1 });
            adjacentTiles.Add(new int[] { positionX + 1, positionY });
            adjacentTiles.Add(new int[] { positionX, positionY - 1 });

            if (positionX % 2 == 0)
            {
                adjacentTiles.Add(new int[] { positionX + 1, positionY + 1 });
                adjacentTiles.Add(new int[] { positionX - 1, positionY + 1 });
            }
            else
            {
                adjacentTiles.Add(new int[] { positionX + 1, positionY - 1 });
                adjacentTiles.Add(new int[] { positionX - 1, positionY - 1 });
            }

            return adjacentTiles;
        }       
    }
}
