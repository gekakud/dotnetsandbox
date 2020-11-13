using System;
using System.Threading;
using System.Threading.Tasks;

namespace StatePattern.Logic
{
    class PendingState : BookingState
    {
        private CancellationTokenSource cancellationToken;

        public override void EnterState(StateMachine bookingCtx)
        {
            cancellationToken = new CancellationTokenSource();

            bookingCtx.ShowState("Pending");
            bookingCtx.View.ShowStatus("Processing booking");

            StaticFunctions.ProcessBooking(bookingCtx, BookingProcessResultCallback, cancellationToken.Token);
        }

        private void BookingProcessResultCallback(StateMachine booking, ProcessingResult result)
        {
            switch (result)
            {
                case ProcessingResult.Success:
                    booking.TransitionToState(new BookedState());
                    break;

                case ProcessingResult.Cancelled:
                    booking.TransitionToState(new ClosedState("Processing cancelled"));
                    break;

                case ProcessingResult.Failed:
                    booking.View.ShowError("Booking failed.... bla bla...");
                    booking.TransitionToState(new NewState());
                    break;
            }
        }

        public override void Cancel(StateMachine bookingCtx)
        {
            cancellationToken.Cancel();
        }

        public override void DatePassed(StateMachine bookingCtx)
        {
        }

        public override void EnterDetails(StateMachine bookingCtx, string attendee, int ticketCount)
        {
        }
    }

    public enum ProcessingResult { Success, Failed, Cancelled}

    public class StaticFunctions
    {
        public static async void ProcessBooking(StateMachine booking,
            Action<StateMachine, ProcessingResult> callback, CancellationToken cancellation)
        {
            try
            {
                await Task.Run(() => Task.Delay(3000), cancellation);
            }
            catch (OperationCanceledException)
            {
                callback(booking, ProcessingResult.Cancelled);
            }
            catch (Exception exception)
            {
                callback(booking, ProcessingResult.Failed);
            }

            callback(booking, ProcessingResult.Success);
        }
    }
}