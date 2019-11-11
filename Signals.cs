using System;
using System.Threading;

internal static class Signals
{
    internal static void AttachCtrlCSigtermShutdown(CancellationTokenSource cts, ManualResetEventSlim resetEvent, string shutdownMessage)
    {
        void ShutDown()
        {
            if (!cts.IsCancellationRequested)
            {
                if (!string.IsNullOrWhiteSpace(shutdownMessage))
                    Console.WriteLine(shutdownMessage);
                try
                {
                    cts.Cancel();
                }
                catch (ObjectDisposedException) { }
            }
            resetEvent.Wait();
            Thread.Sleep(1000);
        }

        AppDomain.CurrentDomain.ProcessExit += delegate { ShutDown(); };

        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            ShutDown();
            eventArgs.Cancel = true;
        };
    }
}