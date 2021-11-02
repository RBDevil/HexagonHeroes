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

								// Send Use Spawn Message
								SpawnPlayers(all, message.SenderConnection, player);
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
								case (byte)PacketTypes.PositionPacket:
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
			List<NetConnection> all = server.Connections;
			if (all.Count > 0)
            {
                foreach (var item in all)
                {
					PositionPacket packet = new PositionPacket();
					string playerID = NetUtility.ToHexString(item.RemoteUniqueIdentifier);
					int[] playerPosition = logic.GetEntityPosition(playerID);
					packet.player = playerID;
					packet.X = playerPosition[0];
					packet.Y = playerPosition[1];
					SendPositionPacket(all, packet);
                }
            }

		}

        public void SpawnPlayers(List<NetConnection> all, NetConnection local, string player)
		{
			// Spawn all the clients on the local player
			all.ForEach(p => {
				string _player = NetUtility.ToHexString(p.RemoteUniqueIdentifier);
				if (player != _player)
				{
					int[] playerPosition = logic.GetEntityPosition(_player);
					SendSpawnPacketToLocal(local, _player, playerPosition[0], playerPosition[1]);
				}
			});

			// Spawn the local player on all clients
			Random random = new Random();
			int X = random.Next(0, 5);
			int Y = random.Next(0, 5);
			SendSpawnPacketToAll(all, player, X, Y);
			logic.AddPlayer(player, X, Y);
		}

		public void SendLocalPlayerPacket(NetConnection local, string player)
		{
			Logger.Info("Sending player their user ID: " + player);

			NetOutgoingMessage outgoingMessage = server.CreateMessage();
			new LocalPlayerPacket() { ID = player }.PacketToNetOutGoingMessage(outgoingMessage);
			server.SendMessage(outgoingMessage, local, NetDeliveryMethod.ReliableOrdered, 0);
		}

		public void SendSpawnPacketToLocal(NetConnection local, string player, float X, float Y)
		{
			Logger.Info("Sending user spawn message for player " + player);

			NetOutgoingMessage outgoingMessage = server.CreateMessage();
			new SpawnPacket() { player = player, X = X, Y = Y }.PacketToNetOutGoingMessage(outgoingMessage);
			server.SendMessage(outgoingMessage, local, NetDeliveryMethod.ReliableOrdered, 0);
		}

		public void SendSpawnPacketToAll(List<NetConnection> all, string player, float X, float Y)
		{
			Logger.Info("Sending user spawn message for player " + player);

			NetOutgoingMessage outgoingMessage = server.CreateMessage();
			new SpawnPacket() { player = player, X = X, Y = Y }.PacketToNetOutGoingMessage(outgoingMessage);
			server.SendMessage(outgoingMessage, all, NetDeliveryMethod.ReliableOrdered, 0);
		}

		public void SendPositionPacket(List<NetConnection> all, PositionPacket packet)
		{
			Logger.Info("Sending position for " + packet.player);

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
