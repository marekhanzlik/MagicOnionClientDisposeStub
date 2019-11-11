using Grpc.Core;
using MagicOnion.Client;
using System.Threading.Tasks;

namespace Client.Hubs.Auth
{
    public class AuthHubClient : IAuthHubReceiver, IHub
    {
        public IAuthHub Client { get; private set; }

        public void Connect(Channel grpcChannel)
        {
            Client = StreamingHubClient.Connect<IAuthHub, IAuthHubReceiver>(grpcChannel, this);
        }

        public async Task Disconnect()
        {
            await Client.DisposeAsync();
        }
    }
}
