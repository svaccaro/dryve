using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace InstrumentControlLibrary.Comm
{
    public class TcpPort : ITcpPort
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public TcpPort(string ip, int port)
        {
            try
            {
                _client = new TcpClient(ip, port);
                _stream = _client.GetStream();
            }
            catch
            {
                Console.WriteLine("Unable to open connection to IP {0} on Port {1}", ip, port);
            }
        }

        public bool IsConnected
        {
            get => _client.Connected;
        }
        public void SendMessage(byte[] msg)
        {
            _stream.Write(msg, 0, msg.Length);
        }

        public byte[] ReceiveMessage(int length)
        {
            var data = new Byte[length];
            String responseData = String.Empty;
            _stream.Read(data, 0, data.Length);
            return data;
        }

        public void Disconnect()
        {
            _client.Close();
        }
    }
}
