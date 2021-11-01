using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using Lidgren.Network;
using Packets;

namespace Networking
{
	public class PlayerPosition
	{
		public float X { get; set; }
		public float Y { get; set; }
	}

	public class Client
	{
		public delegate void MessageRecived(NetIncomingMessage message);
		public event MessageRecived _MessageRecived;
		public NetClient client { get; set; }

		public Client(int port, string server, string serverName)
		{
			var config = new NetPeerConfiguration(serverName)
			{
				AutoFlushSendQueue = false
			};

			client = new NetClient(config);
			client.RegisterReceivedCallback(new SendOrPostCallback(ReceiveMessage));
			client.Start();

			client.Connect(server, port);
		}

		public void ReceiveMessage(object peer)
		{
			NetIncomingMessage message;

			while ((message = client.ReadMessage()) != null)
			{
				switch (message.MessageType)
				{
					case NetIncomingMessageType.Data:
						_MessageRecived?.Invoke(message);
						break;
					case NetIncomingMessageType.DebugMessage:
					case NetIncomingMessageType.ErrorMessage:
					case NetIncomingMessageType.WarningMessage:
					case NetIncomingMessageType.VerboseDebugMessage:
						string text = message.ReadString();
						break;
					case NetIncomingMessageType.StatusChanged:
						NetConnectionStatus status = (NetConnectionStatus)message.ReadByte();
						string reason = message.ReadString();
						break;
					default:
						break;
				}

				client.Recycle(message);
			}
		}
	}
}
