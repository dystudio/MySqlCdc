using MySqlCdc.Constants;
using MySqlCdc.Protocol;

namespace MySqlCdc.Commands
{
    /// <summary>
    /// SSLRequest packet used in SSL/TLS connection.
    /// <a href="https://mariadb.com/kb/en/library/connection/#sslrequest-packet">See more</a>
    /// </summary>
    internal class SslRequestCommand : ICommand
    {
        public int ClientCapabilities { get; }
        public int ClientCollation { get; }
        public int MaxPacketSize { get; }

        public SslRequestCommand(int clientCollation, int maxPacketSize = 0)
        {
            ClientCollation = clientCollation;
            MaxPacketSize = maxPacketSize;

            ClientCapabilities = (int)CapabilityFlags.LONG_FLAG
                | (int)CapabilityFlags.PROTOCOL_41
                | (int)CapabilityFlags.SECURE_CONNECTION
                | (int)CapabilityFlags.SSL
                | (int)CapabilityFlags.PLUGIN_AUTH;
        }

        public byte[] CreatePacket(byte sequenceNumber)
        {
            var writer = new PacketWriter(sequenceNumber);
            writer.WriteInt(ClientCapabilities, 4);
            writer.WriteInt(MaxPacketSize, 4);
            writer.WriteInt(ClientCollation, 1);

            // Fill reserved bytes 
            for (int i = 0; i < 23; i++)
                writer.WriteByte(0);

            return writer.CreatePacket();
        }
    }
}
