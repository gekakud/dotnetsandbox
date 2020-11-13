namespace StatePattern.Logic
{
    class BookedState : BookingState
    {
        public override void EnterState(StateMachine bookingCtx)
        {
            bookingCtx.ShowState("Booked");
            bookingCtx.View.ShowStatus("Enjoy the event");
        }

        public override void Cancel(StateMachine bookingCtx)
        {
            bookingCtx.TransitionToState(new ClosedState("Booking canceled: expect a refund"));
        }

        public override void DatePassed(StateMachine bookingCtx)
        {
            bookingCtx.TransitionToState(new ClosedState("We hope you enjoyed the event"));
        }

        public override void EnterDetails(StateMachine bookingCtx, string attendee, int ticketCount)
        {
        }
    }
}