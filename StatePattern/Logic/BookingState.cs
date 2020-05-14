namespace StatePattern.Logic
{
    public abstract class BookingState
    {
        public abstract void EnterState(BookingContext bookingCtx);
        public abstract void Cancel(BookingContext bookingCtx);
        public abstract void DatePassed(BookingContext bookingCtx);
        public abstract void EnterDetails(BookingContext bookingCtx, string attendee, int ticketCount);
    }
}
