using Grpc.Core;
using System.Threading.Tasks;

namespace Client.Hubs
{
    public interface IHub
    {
        public void Connect(Channel grpcChannel);
        public Task Disconnect();
    }
}
