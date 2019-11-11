using Client.Hubs.Auth;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Hubs
{
    public class Hub
    {
        private static Hub _instance;
        public static Hub Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Hub();
                }

                return _instance;
            }
        }

        private CancellationToken _ct;
        private Channel _grpcChannel;
        private Dictionary<Type, IHub> _hubs = new Dictionary<Type, IHub>();

        private Hub()
        {
            _grpcChannel = new Channel("localhost", 12345, ChannelCredentials.Insecure);

            Register(new AuthHubClient());
        }

        private void Register(IHub hub)
        {
            hub.Connect(_grpcChannel);
            _hubs.Add(hub.GetType(), hub);
        }

        public T GetHub<T>()
        {
            _hubs.TryGetValue(typeof(T), out IHub hub);
            return (T)hub;
        }

        public async Task DisconnectAllHubs(ManualResetEventSlim done)
        {
            Console.WriteLine("Starting dispose");
            foreach(var kvp in _hubs)
            {
                IHub hub = kvp.Value;
                await hub.Disconnect();
            }


            await _grpcChannel.ShutdownAsync();
            Console.WriteLine("Ending dispose");

            done.Set();
        }

        public void SetCancelationToken(CancellationToken ct, ManualResetEventSlim done)
        {
            _ct = ct;
            _ct.Register(() => DisconnectAllHubs(done));
        }
    }
}
