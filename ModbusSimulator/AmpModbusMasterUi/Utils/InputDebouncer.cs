using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmpModbusMasterUi.Utils
{
    public class InputDebouncer
    {
        private readonly TimeSpan debounceTime;
        private readonly Action<bool> signalChangeHandler;
        private bool? lastState;
        private CancellationTokenSource cts;

        public InputDebouncer(TimeSpan debounceTime, Action<bool> signalChangeHandler)
        {
            this.debounceTime = debounceTime;
            this.signalChangeHandler = signalChangeHandler;
        }

        public async void Debounce(bool state)
        {
            if (!lastState.HasValue)
            {
                lastState = state;
                //Trigger immediately, if no change recently:
                signalChangeHandler(state);

                //Wait for debounce anyway to reset "lastState":
                if (await WaitForDebounceAsync())
                {
                    lastState = null;
                }
            }
            else if (lastState.Value != state)
            {
                lastState = state;
                CancelDebounceWait();
                if (await WaitForDebounceAsync())
                {
                    //If waiting was not interrupted by another state change:
                    lastState = null;
                    signalChangeHandler(state);
                }
            }
        }

        private void CancelDebounceWait()
        {
            cts?.Cancel();
        }

        private async Task<bool> WaitForDebounceAsync()
        {
            var cts = new CancellationTokenSource();
            this.cts = cts;
            try
            {
                await Task.Delay(debounceTime, cts.Token);
                return true;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            finally
            {
                cts.Dispose();
            }
        }
    }

}
