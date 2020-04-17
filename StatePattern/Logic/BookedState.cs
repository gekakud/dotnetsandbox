namespace StatePattern.Logic
{
    class BookedState : BookingState
    {
        public override void EnterState(BookingContext bookingCtx)
        {
            bookingCtx.ShowState("Booked");
            bookingCtx.View.ShowStatus("Enjoy the event");
        }

        public override void Cancel(BookingContext bookingCtx)
        {
            bookingCtx.TransitionToState(new ClosedState("Booking canceled: expect a refund"));
        }

        public override void DatePassed(BookingContext bookingCtx)
        {
            bookingCtx.TransitionToState(new ClosedState("We hope you enjoyed the event"));
        }

        public override void EnterDetails(BookingContext bookingCtx, string attendee, int ticketCount)
        {
        }
    }
}