using Lidgren.Network;

namespace Packets
{
    public enum PacketTypes
    {
        LocalPlayerPacket,
        PlayerDisconnectsPacket,
        PositionPacket,
        SpawnPacket,
        CounterPacket,
        MoveIndicatorPacket,
        HealthPacket,
        ChosenHeroPacket,
        SpellIndicatorPacket,
    }

    public interface IPacket
    {
        void PacketToNetOutGoingMessage(NetOutgoingMessage message);
        void NetIncomingMessageToPacket(NetIncomingMessage message);
    }

    public abstract class Packet : IPacket
    {
        public abstract void PacketToNetOutGoingMessage(NetOutgoingMessage message);
        public abstract void NetIncomingMessageToPacket(NetIncomingMessage message);
    }
    public class LocalPlayerPacket : Packet
    {
        public string ID { get; set; }

        public override void PacketToNetOutGoingMessage(NetOutgoingMessage message)
        {
            message.Write((byte)PacketTypes.LocalPlayerPacket);
            message.Write(ID);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message)
        {
            ID = message.ReadString();
        }
    }
    public class PlayerDisconnectsPacket : Packet
    {
        public string player { get; set; }

        public override void PacketToNetOutGoingMessage(NetOutgoingMessage message)
        {
            message.Write((byte)PacketTypes.PlayerDisconnectsPacket);
            message.Write(player);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message)
        {
            player = message.ReadString();
        }
    }
    public class PositionPacket : Packet
    {
        public float X { get; set; }
        public float Y { get; set; }
        public string entityID { get; set; }

        public override void PacketToNetOutGoingMessage(NetOutgoingMessage message)
        {
            message.Write((byte)PacketTypes.PositionPacket);
            message.Write(X);
            message.Write(Y);
            message.Write(entityID);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message)
        {
            X = message.ReadFloat();
            Y = message.ReadFloat();
            entityID = message.ReadString();
        }
    }
    public class SpawnPacket : Packet
    {
        public float X { get; set; }
        public float Y { get; set; }
        public string entityID { get; set; }
        public string heroType { get; set; }
        public string factionID { get; set; }
        public override void PacketToNetOutGoingMessage(NetOutgoingMessage message)
        {
            message.Write((byte)PacketTypes.SpawnPacket);
            message.Write(X);
            message.Write(Y);
            message.Write(entityID);
            message.Write(heroType);
            message.Write(factionID);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message)
        {
            X = message.ReadFloat();
            Y = message.ReadFloat();
            entityID = message.ReadString();
            heroType = message.ReadString();
            factionID = message.ReadString();
        }
    }
    public class TimerPacket : Packet
    {
        public float Counter { get; set; }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message)
        {
            Counter = message.ReadFloat();
        }

        public override void PacketToNetOutGoingMessage(NetOutgoingMessage message)
        {
            message.Write((byte)PacketTypes.CounterPacket);
            message.Write(Counter);
        }
    }
    public class MoveIndicatorPacket : Packet
    {
        public float X;
        public float Y;
        public string playerID;

        public override void NetIncomingMessageToPacket(NetIncomingMessage message)
        {
            X = message.ReadFloat();
            Y = message.ReadFloat();
            playerID = message.ReadString();
        }

        public override void PacketToNetOutGoingMessage(NetOutgoingMessage message)
        {
            message.Write((byte)PacketTypes.MoveIndicatorPacket);
            message.Write(X);
            message.Write(Y);
            message.Write(playerID);
        }
    }
    public class HealthPacket : Packet
    {
        public float Health { get; set; }
        public string entityID { get; set; }

        public override void PacketToNetOutGoingMessage(NetOutgoingMessage message)
        {
            message.Write((byte)PacketTypes.HealthPacket);
            message.Write(Health);
            message.Write(entityID);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message)
        {
            Health = message.ReadFloat();
            entityID = message.ReadString();
        }
    }
    public class ChosenHeroPacket : Packet
    {
        public string heroType { get; set; }
        public string entityID { get; set; }

        public override void PacketToNetOutGoingMessage(NetOutgoingMessage message)
        {
            message.Write((byte)PacketTypes.ChosenHeroPacket);
            message.Write(heroType);
            message.Write(entityID);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message)
        {
            heroType = message.ReadString();
            entityID = message.ReadString();
        }
    }
    public class SpellIndicatorPacket : Packet
    {
        public string playerID { get; set; }
        public float spellID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public override void PacketToNetOutGoingMessage(NetOutgoingMessage message)
        {
            message.Write((byte)PacketTypes.SpellIndicatorPacket);
            message.Write(playerID);
            message.Write(spellID);
            message.Write(X);
            message.Write(Y);
        }

        public override void NetIncomingMessageToPacket(NetIncomingMessage message)
        {
            playerID = message.ReadString();
            spellID = message.ReadFloat();
            X = message.ReadFloat();
            Y = message.ReadFloat();
        }
    }
}
