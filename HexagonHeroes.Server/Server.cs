using System;
using System.Collections.Generic;
using System.Threading;
using Packets;
using Logging;
using Lidgren.Network;
using HexagonHeroes.Logic;

namespace Networking
{
	public class Server
	{
		private NetServer server;
		private Thread listenerThread, countDownThread;
		private int countDown;
		LogicUpdater logic;

		public Server()
		{
			logic = new LogicUpdater();
			countDown = 10;

			NetPeerConfiguration config = new NetPeerConfiguration("game");
			config.MaximumConnections = 100;
			config.Port = 14242;

			server = new NetServer(config);
			server.Start();

			listenerThread = new Thread(Listen);
			listenerThread.Start();

			countDownThread = new Thread(CountDown);
			countDownThread.Start();
		}
		void CountDown()
        {
			while (0 == 0)
			{
				Thread.Sleep(1000);		
				countDown--;

				List<NetConnection> all = server.Connections;
				if (all.Count > 0)
				{
					SendTimerToAll(all, countDown);
				}

				if (countDown == 0)
				{
					// apply turn cahnges
					UpdateTurn();
					countDown = 10;
				}

				Logger.Info("Countdown: " + countDown.ToString());
			}
        }
		public void Listen()
		{
			Logger.Info("Listening for clients.");

			while (!Console.KeyAvailable || Console.ReadKey().Key != ConsoleKey.Escape)
			{
				NetIncomingMessage message;

				while ((message = server.ReadMessage()) != null)
				{
					Logger.Info("Message recevied.");

					// Get a list of all users
					List<NetConnection> all = server.Connections;

					switch (message.MessageType)
					{
						// Someone Connects
						case NetIncomingMessageType.StatusChanged:
							NetConnectionStatus status = (NetConnectionStatus)message.ReadByte();

							string reason = message.ReadString();
							Logger.Debug(NetUtility.ToHexString(message.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason);

							if (status == NetConnectionStatus.Connected)
							{
								var player = NetUtility.ToHexString(message.SenderConnection.RemoteUniqueIdentifier);

								//Send player their ID
								SendLocalPlayerPacket(message.SenderConnection, player);
							}
							else if (status == NetConnectionStatus.Disconnected)
                            {
								PlayerDisconnectsPacket disconnectPacket = new PlayerDisconnectsPacket();
								disconnectPacket.player = NetUtility.ToHexString(message.SenderConnection.RemoteUniqueIdentifier);
								SendPlayerDisconnectPacket(all , disconnectPacket);
							}
							break;
						case NetIncomingMessageType.Data:
							// Get packet type
							byte type = message.ReadByte();

							// Create Packet
							Packet packet;

							switch (type)
							{
								case (byte)PacketTypes.ChosenHeroPacket:
									packet = new ChosenHeroPacket();
									packet.NetIncomingMessageToPacket(message);
									// send back spawn packages
									string local = NetUtility.ToHexString(message.SenderConnection.RemoteUniqueIdentifier);
									SpawnAllEntities(all, message.SenderConnection, local, (ChosenHeroPacket)packet);				
									break;
								case (byte)PacketTypes.PlayerDisconnectsPacket:
									packet = new PlayerDisconnectsPacket();
									packet.NetIncomingMessageToPacket(message);
									SendPlayerDisconnectPacket(all, (PlayerDisconnectsPacket)packet);
									break;
								case (byte)PacketTypes.MoveIndicatorPacket:
									packet = new MoveIndicatorPacket();
									packet.NetIncomingMessageToPacket(message);
									RegisterMoveIndicator((MoveIndicatorPacket)packet);
									break;
								case (byte)PacketTypes.SpellIndicatorPacket:
									packet = new SpellIndicatorPacket();
									packet.NetIncomingMessageToPacket(message);
									RegisterSpellIndicator((SpellIndicatorPacket)packet);
									break;
								default:
									Logger.Error("Unhandled data / packet type: " + type);
									break;
							}

							break;
						case NetIncomingMessageType.DebugMessage:
						case NetIncomingMessageType.ErrorMessage:
						case NetIncomingMessageType.WarningMessage:
						case NetIncomingMessageType.VerboseDebugMessage:
							string text = message.ReadString();
							Logger.Debug(text);
							break;
						default:
							Logger.Error("Unhandled type: " + message.MessageType + " " + message.LengthBytes + " bytes " + message.DeliveryMethod + "|" + message.SequenceChannel);
							break;
					}

					server.Recycle(message);
				}
			}
		}

        private void RegisterSpellIndicator(SpellIndicatorPacket packet)
        {
			if (logic.UpdateSpellIndicator(packet.playerID, packet.spellID, (int)packet.X, (int)packet.Y))
			{
				List<NetConnection> all = server.Connections;
				if (all.Count > 0)
				{
					SendSpellIndicatorToAll(all, packet.playerID, packet.spellID, packet.X, packet.Y);
				}
			}
		}

        private void SendSpellIndicatorToAll(List<NetConnection> all, string playerID, float spellID, float X, float Y)
        {
			NetOutgoingMessage outgoingMessage = server.CreateMessage();
			new SpellIndicatorPacket() { playerID = playerID, spellID = spellID, X = X, Y = Y }.PacketToNetOutGoingMessage(outgoingMessage);
			server.SendMessage(outgoingMessage, all, NetDeliveryMethod.ReliableOrdered, 0);
		}

        private void RegisterMoveIndicator(MoveIndicatorPacket packet)
		{
			if (logic.UpdateMoveIndicator(packet.playerID, (int)packet.X, (int)packet.Y))
			{
				List<NetConnection> all = server.Connections;
				if (all.Count > 0)
				{
					SendMoveIndicatorToAll(all, packet.playerID, packet.X, packet.Y);
				}
			}
		}
		private void UpdateTurn()
        {
			logic.UpdateTurn();
			// get all entities
			List<NetConnection> all = server.Connections;
			List<string> allIDs = logic.GetAllEntityIDs();
			
			if (allIDs.Count > 0 && all.Count > 0)
            {
                foreach (var id in allIDs)
                {
                    PositionPacket positionPacket = new PositionPacket();
                    int[] playerPosition = logic.GetEntityPosition(id);
                    positionPacket.entityID = id;
                    positionPacket.X = playerPosition[0];
                    positionPacket.Y = playerPosition[1];
                    SendPositionPacket(all, positionPacket);

					HealthPacket healthPacket = new HealthPacket();
					healthPacket.entityID = id;
					healthPacket.Health = logic.GetEntitiyHealth(id);
					SendHealthPacket(all, healthPacket);
				}
            }

		}
        public void SpawnAllEntities(List<NetConnection> all, NetConnection local, string player, ChosenHeroPacket packet)
		{
			// Spawn all the entities on the local player
			List<string> allIds = logic.GetAllEntityIDs();
            foreach (var id in allIds)
            {
				int[] entityPosition = logic.GetEntityPosition(id);
				SendSpawnPacketToLocal(local, id, entityPosition[0], entityPosition[1], logic.GetHeroType(id), logic.GetEntityFaction(id));
				int entityHealth = logic.GetEntitiyHealth(id);
				SendHealthPacket(new List<NetConnection> { local }, new HealthPacket() { entityID = id, Health = entityHealth });
            }

			// Spawn the local player on all clients
			Random random = new Random();
			int X = random.Next(0, 5);
			int Y = random.Next(0, 5);
			SendSpawnPacketToAll(all, player, X, Y, packet.heroType, "players");
			logic.AddEntity(player, X, Y, "players", packet.heroType);
			SendHealthPacket(all, new HealthPacket() { entityID = player, Health = logic.GetEntitiyHealth(player) });
		}
		public void SendLocalPlayerPacket(NetConnection local, string player)
		{
			Logger.Info("Sending player their user ID: " + player);

			NetOutgoingMessage outgoingMessage = server.CreateMessage();
			new LocalPlayerPacket() { ID = player }.PacketToNetOutGoingMessage(outgoingMessage);
			server.SendMessage(outgoingMessage, local, NetDeliveryMethod.ReliableOrdered, 0);
		}
		public void SendSpawnPacketToLocal(NetConnection local, string entityID, float X, float Y, string heroType, string factionID)
		{
			Logger.Info("Sending user spawn message for player " + entityID);

			NetOutgoingMessage outgoingMessage = server.CreateMessage();
			new SpawnPacket() {	
				entityID = entityID, 
				X = X, Y = Y , 
				heroType = heroType, 
				factionID = factionID
			}.PacketToNetOutGoingMessage(outgoingMessage);
			server.SendMessage(outgoingMessage, local, NetDeliveryMethod.ReliableOrdered, 0);
		}
		public void SendSpawnPacketToAll(List<NetConnection> all, string player, float X, float Y, string heroType, string factionID)
		{
			Logger.Info("Sending user spawn message for player " + player);

			NetOutgoingMessage outgoingMessage = server.CreateMessage();
			new SpawnPacket()
			{
				entityID = player,
				X = X,
				Y = Y,
				heroType = heroType,
				factionID = factionID
			}.PacketToNetOutGoingMessage(outgoingMessage);
			server.SendMessage(outgoingMessage, all, NetDeliveryMethod.ReliableOrdered, 0);
		}
		public void SendPositionPacket(List<NetConnection> all, PositionPacket packet)
		{
			Logger.Info("Sending position for " + packet.entityID);

			NetOutgoingMessage outgoingMessage = server.CreateMessage();
			packet.PacketToNetOutGoingMessage(outgoingMessage);
			server.SendMessage(outgoingMessage, all, NetDeliveryMethod.ReliableOrdered, 0);
		}
		public void SendHealthPacket(List<NetConnection> all, HealthPacket packet)
		{
			NetOutgoingMessage outgoingMessage = server.CreateMessage();
			packet.PacketToNetOutGoingMessage(outgoingMessage);
			server.SendMessage(outgoingMessage, all, NetDeliveryMethod.ReliableOrdered, 0);
		}
		public void SendPlayerDisconnectPacket(List<NetConnection> all, PlayerDisconnectsPacket packet)
		{
			Logger.Info("Disconnecting for " + packet.player);

			logic.DeletePlayer(packet.player);

			if (all.Count > 0)
			{
				NetOutgoingMessage outgoingMessage = server.CreateMessage();
				packet.PacketToNetOutGoingMessage(outgoingMessage);
				server.SendMessage(outgoingMessage, all, NetDeliveryMethod.ReliableOrdered, 0);
			}
		}
		public void SendTimerToAll(List<NetConnection> all, int counter)
        {
			NetOutgoingMessage outgoingMessage = server.CreateMessage();
			new TimerPacket() { Counter = counter }.PacketToNetOutGoingMessage(outgoingMessage);
			server.SendMessage(outgoingMessage, all, NetDeliveryMethod.ReliableOrdered, 0);
		}
		public void SendMoveIndicatorToAll(List<NetConnection> all, string player, float X, float Y)
		{
			Logger.Info("Sending user move indicator for player " + player);

			NetOutgoingMessage outgoingMessage = server.CreateMessage();
			new MoveIndicatorPacket() { playerID = player, X = X, Y = Y }.PacketToNetOutGoingMessage(outgoingMessage);
			server.SendMessage(outgoingMessage, all, NetDeliveryMethod.ReliableOrdered, 0);
		}
	}
}
