using Client.Hubs;
using Client.UI;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cancellationToken = default(CancellationToken);
            var done = new ManualResetEventSlim(false);
            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                Signals.AttachCtrlCSigtermShutdown(cts, done, shutdownMessage: "SIGTERM received...");

                Hub.Instance.SetCancelationToken(cts.Token, done);
                var ui = new UILoader();
            }
        }
    }
}
