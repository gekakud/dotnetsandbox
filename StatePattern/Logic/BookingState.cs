namespace StatePattern.Logic
{
    public abstract class BookingState
    {
        public abstract void EnterState(StateMachine bookingCtx);
        public abstract void Cancel(StateMachine bookingCtx);
        public abstract void DatePassed(StateMachine bookingCtx);
        public abstract void EnterDetails(StateMachine bookingCtx, string attendee, int ticketCount);
    }
}
