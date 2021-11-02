using HexagonHeroes.Client.Resources;
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
        public static void Exit()
        {
            active = false;
        }
        public static void Activate()
        {
            entities = new List<Entity>();

            active = true;
            client = new Networking.Client(14242, "127.0.0.1", "game");
            client._MessageRecived += ReceiveMessage;
            map = new Map(new Point(30, 10));
            countdown = 10;
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
                    SpawnPlayer((SpawnPacket)packet);
                    break;
                case (int)PacketTypes.CounterPacket:
                    packet = new TimerPacket();
                    packet.NetIncomingMessageToPacket(message);
                    SetTimer((TimerPacket)packet);
                    break;
            }
        }
        private static void SetTimer(TimerPacket packet)
        {
            countdown = (int)packet.Counter;
            if (countdown == 0)
            {
                // send move input to sever
                if (localPlayer.MoveIndicator.X != -1 && localPlayer.MoveIndicator.Y != -1)
                {
                    NetOutgoingMessage message = client.client.CreateMessage();
                    new PlayerInputPacket() { playerID = localPlayerID,
                        X = localPlayer.MoveIndicator.X,
                        Y = localPlayer.MoveIndicator.Y }
                    .PacketToNetOutGoingMessage(message);
                    client.client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
                    client.client.FlushSendQueue();
                }
            }
        }
        static void SpawnPlayer(SpawnPacket packet)
        {
            Entity entity = new Entity(packet.playerID, new Point((int)packet.X, (int)packet.Y), Textures.Container["player"]);
            entities.Add(entity);
            if (localPlayer == null && packet.playerID == localPlayerID)
            {
                localPlayer = entity;
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
                    // check if the same position
                    if (hoveredTile == localPlayer.PositionIndex)
                    {
                        localPlayer.MoveIndicator = hoveredTile;
                    }
                    // check if tile is adjacent to player position
                    {
                        if (CheckIfAdjacent(localPlayer.PositionIndex, hoveredTile))
                        {
                            localPlayer.MoveIndicator = hoveredTile;
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
