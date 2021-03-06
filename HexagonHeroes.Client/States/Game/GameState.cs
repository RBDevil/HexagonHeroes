using HexagonHeroes.Client.Resources;
using HexagonHeroes.Client.States.Game.Heores;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonHeroes.Client.States.Game
{
    static class GameState
    {
        static bool active;
        static Networking.Client client;
        static Map map;
        static string localPlayerID;
        static Entity localPlayer;
        static List<Entity> entities;
        static int countdown;
        static HeroTypes chosenHero;
        public static void Exit()
        {
            active = false;
        }
        public static void Activate(HeroTypes chosenHero)
        {
            entities = new List<Entity>();

            active = true;
            client = new Networking.Client(14242, "188.142.219.78", "game");
            client._MessageRecived += ReceiveMessage;
            map = new Map(new Point(30, 10));
            countdown = 10;
            GameState.chosenHero = chosenHero;
        }
        public static void ReceiveMessage(NetIncomingMessage message)
        {
            var packetType = (int)message.ReadByte();

            Packet packet;

            switch (packetType)
            {
                case (int)PacketTypes.LocalPlayerPacket:
                    packet = new LocalPlayerPacket();
                    packet.NetIncomingMessageToPacket(message);
                    ExtractLocalPlayerInformation((LocalPlayerPacket)packet);
                    // send chosen hero type
                    NetOutgoingMessage messageOut = client.client.CreateMessage();
                    new ChosenHeroPacket { entityID = localPlayerID, heroType = chosenHero.ToString().ToLower() }
                    .PacketToNetOutGoingMessage(messageOut);
                    client.client.SendMessage(messageOut, NetDeliveryMethod.ReliableOrdered);
                    client.client.FlushSendQueue();
                    break;
                case (int)PacketTypes.PlayerDisconnectsPacket:
                    packet = new PlayerDisconnectsPacket();
                    packet.NetIncomingMessageToPacket(message);
                    DisconnectPlayer((PlayerDisconnectsPacket)packet);
                    break;
                case (int)PacketTypes.PositionPacket:
                    packet = new PositionPacket();
                    packet.NetIncomingMessageToPacket(message);
                    UpdatePlayerPosition((PositionPacket)packet);
                    break;
                case (int)PacketTypes.SpawnPacket:
                    packet = new SpawnPacket();
                    packet.NetIncomingMessageToPacket(message);
                    SpawnEntity((SpawnPacket)packet);
                    break;
                case (int)PacketTypes.CounterPacket:
                    packet = new TimerPacket();
                    packet.NetIncomingMessageToPacket(message);
                    SetTimer((TimerPacket)packet);
                    break;
                case (int)PacketTypes.MoveIndicatorPacket:
                    packet = new MoveIndicatorPacket();
                    packet.NetIncomingMessageToPacket(message);
                    SetMoveIndicator((MoveIndicatorPacket)packet);
                    break;
                case (int)PacketTypes.HealthPacket:
                    packet = new HealthPacket();
                    packet.NetIncomingMessageToPacket(message);
                    SetEntityHealth((HealthPacket)packet);
                    break;
                case (int)PacketTypes.SpellIndicatorPacket:
                    packet = new SpellIndicatorPacket();
                    packet.NetIncomingMessageToPacket(message);
                    SetSpellIndicator((SpellIndicatorPacket)packet);
                    break;
            }
        }

        static void SetSpellIndicator(SpellIndicatorPacket packet)
        {
            Entity player = entities.Find(e => e.ID == packet.playerID);
            player.SpellIndicator = new Point((int)packet.X, (int)packet.Y);
            player.spellID = (int)packet.spellID;
        }

        static void SetEntityHealth(HealthPacket packet)
        {
            Entity player = entities.Find(e => e.ID == packet.entityID);
            player.Health = (int)packet.Health;
        }
        static void SetMoveIndicator(MoveIndicatorPacket packet)
        {
            Entity player = entities.Find(e => e.ID == packet.playerID);
            player.MoveIndicator = new Point((int)packet.X, (int)packet.Y);
        }
        static void SetTimer(TimerPacket packet)
        {
            countdown = (int)packet.Counter;
        }
        static void SpawnEntity(SpawnPacket packet)
        {
            switch (packet.heroType)
            {
                case "tank":
                    Tank tank = new Tank(
                            packet.playerID,
                            new Point((int)packet.X, (int)packet.Y),
                            Textures.Container["tank"],
                            packet.factionID);
                    
                    entities.Add(tank);

                    if (localPlayer == null && packet.playerID == localPlayerID)
                    {
                        localPlayer = tank;
                    }
                    break;
                case "mage":
                    Tank mage = new Tank(
                        packet.playerID,
                        new Point((int)packet.X, (int)packet.Y),
                        Textures.Container["mage"],
                        packet.factionID);

                    entities.Add(mage);

                    if (localPlayer == null && packet.playerID == localPlayerID)
                    {
                        localPlayer = mage;
                    }
                    break;
                case "support":
                    Tank support = new Tank(
                        packet.playerID,
                        new Point((int)packet.X, (int)packet.Y),
                        Textures.Container["support"],
                        packet.factionID);

                    entities.Add(support);

                    if (localPlayer == null && packet.playerID == localPlayerID)
                    {
                        localPlayer = support;
                    }
                    break;
                case "fighter":
                    Tank fighter = new Tank(
                        packet.playerID,
                        new Point((int)packet.X, (int)packet.Y),
                        Textures.Container["fighter"],
                        packet.factionID);

                    entities.Add(fighter);

                    if (localPlayer == null && packet.playerID == localPlayerID)
                    {
                        localPlayer = fighter;
                    }
                    break;
                default:
                    Entity entity = new Entity(
                            packet.playerID,
                            new Point((int)packet.X, (int)packet.Y),
                            Textures.Container["default_entity"],
                            packet.factionID);

                    entities.Add(entity);

                    if (localPlayer == null && packet.playerID == localPlayerID)
                    {
                        localPlayer = entity;
                    }
                    break;
            }
        }
        static void UpdatePlayerPosition(PositionPacket packet)
        {
            entities.Find(e => e.ID == packet.playerID).Move(new Point((int)packet.X, (int)packet.Y));
        }
        static void DisconnectPlayer(PlayerDisconnectsPacket packet)
        {
            entities.Remove(entities.Find(e => e.ID == packet.playerID));
        }
        static void ExtractLocalPlayerInformation(LocalPlayerPacket packet)
        {
            localPlayerID = packet.ID;
        }
        static void CheckMouseInput()
        {
            if (MouseManager.LeftClick)
            {
                Point hoveredTile = map.GetHoveredTile();
                // nullcheck basically
                if (hoveredTile.X != -1 && hoveredTile.Y != -1 && localPlayer != null)
                {
                    NetOutgoingMessage message = client.client.CreateMessage();
                    // check if the same position
                    if (hoveredTile == localPlayer.PositionIndex)
                    {
                        // send to sever
                        new MoveIndicatorPacket() 
                        { playerID = localPlayerID, 
                            X = hoveredTile.X,
                            Y = hoveredTile.Y }
                        .PacketToNetOutGoingMessage(message);

                        client.client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
                        client.client.FlushSendQueue();
                    }
                    // check if tile is adjacent to player position
                    {
                        if (CheckIfAdjacent(localPlayer.PositionIndex, hoveredTile))
                        {
                            // send to sever
                            new MoveIndicatorPacket()
                            {
                                playerID = localPlayerID,
                                X = hoveredTile.X,
                                Y = hoveredTile.Y
                            }
                            .PacketToNetOutGoingMessage(message);

                            client.client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
                            client.client.FlushSendQueue();
                        }
                    }
                }
            }
        }
        static bool CheckIfAdjacent(Point positionIndex1, Point positionIndex2)
        {
            if (positionIndex1.X % 2 == 0)
            {
                if (positionIndex1.X - 1 == positionIndex2.X && positionIndex1.Y == positionIndex2.Y ||
                    positionIndex1.X - 1 == positionIndex2.X && positionIndex1.Y + 1 == positionIndex2.Y ||
                    positionIndex1.X == positionIndex2.X && positionIndex1.Y + 1 == positionIndex2.Y ||
                    positionIndex1.X + 1 == positionIndex2.X && positionIndex1.Y + 1 == positionIndex2.Y ||
                    positionIndex1.X + 1 == positionIndex2.X && positionIndex1.Y == positionIndex2.Y ||
                    positionIndex1.X == positionIndex2.X && positionIndex1.Y - 1 == positionIndex2.Y)
                {
                    return true;
                }
                else return false;
            }
            else if (positionIndex1.X - 1 == positionIndex2.X && positionIndex1.Y - 1 == positionIndex2.Y ||
                    positionIndex1.X - 1 == positionIndex2.X && positionIndex1.Y == positionIndex2.Y ||
                    positionIndex1.X == positionIndex2.X && positionIndex1.Y + 1 == positionIndex2.Y ||
                    positionIndex1.X + 1 == positionIndex2.X && positionIndex1.Y == positionIndex2.Y ||
                    positionIndex1.X + 1 == positionIndex2.X && positionIndex1.Y - 1 == positionIndex2.Y ||
                    positionIndex1.X == positionIndex2.X && positionIndex1.Y - 1 == positionIndex2.Y)
            {
                return true;
            }
            else return false;

        }
        public static void Update()
        {
            if (active)
            {
                CheckMouseInput();
            }
        }
        public static void Draw(SpriteBatch sb)
        {
            if (active)
            {
                if (localPlayer != null)
                {
                    map.Draw(sb, localPlayer.MoveIndicator);
                }
                foreach (Entity entity in entities)
                {
                    entity.Draw(sb);
                }
                if (countdown == 0)
                {
                    sb.Draw(Textures.Container["tile_frame"], new Vector2(5, 5), Color.White);
                }
            }
        }
    }
}
