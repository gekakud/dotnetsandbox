namespace StatePattern.Logic
{
    class ClosedState : BookingState
    {
        private string _reasonClosed;
        public ClosedState(string reason)
        {
            _reasonClosed = reason;
        }

        public override void EnterState(BookingContext bookingCtx)
        {
            bookingCtx.ShowState("Closed");
            bookingCtx.View.ShowStatus(_reasonClosed);
        }

        public override void Cancel(BookingContext bookingCtx)
        {
            bookingCtx.View.ShowError("Invalid action for this state");
        }

        public override void DatePassed(BookingContext bookingCtx)
        {
            bookingCtx.View.ShowError("Invalid action for this state");
        }

        public override void EnterDetails(BookingContext bookingCtx, string attendee, int ticketCount)
        {
            bookingCtx.View.ShowError("Invalid action for this state");
        }
    }
}