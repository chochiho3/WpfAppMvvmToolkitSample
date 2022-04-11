using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfAppListBindingTest
{
    internal class ClientTcp
    {
        private ClientTcpSample? _clientTcp;

        public event EventHandler<string> OnRecvMsg;

        public async Task ServiceStart(IPAddress address, int port)
        {
            await ServiceStop();
            _clientTcp = new ClientTcpSample(address, port);
            _clientTcp.ConnectAsync();
            _clientTcp.OnRecvMsg += (s, e) => OnRecvMsg?.Invoke(s, e);
        }

        public async Task ServiceStop()
        {
            if (_clientTcp is not null)
            {
                await Task.Run(() => _clientTcp.DisconnectAndStop());
                _clientTcp.Dispose();
                _clientTcp = null;
            }
        }
    }

    internal class ClientTcpSample : NetCoreServer.TcpClient
    {
        private bool _stop;

        public event EventHandler<string> OnRecvMsg;

        public ClientTcpSample(IPAddress address, int port) : base(address, port) { }

        public void DisconnectAndStop()
        {
            _stop = true;
            DisconnectAsync();
            while (IsConnected)
                Thread.Yield();
        }

        protected override void OnConnected()
        {
            Console.WriteLine($"Chat TCP client connected a new session with Id {Id}");
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine($"Chat TCP client disconnected a session with Id {Id}");

            // Wait for a while...
            Thread.Sleep(1000);

            // Try to connect again
            if (!_stop)
                ConnectAsync();
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            var str = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            OnRecvMsg?.Invoke(this, str);
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat TCP client caught an error with code {error}");
        }
    }
}
