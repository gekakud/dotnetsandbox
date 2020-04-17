using System;

namespace StatePattern.Logic
{
    class NewState:BookingState
    {
        public override void EnterState(BookingContext bookingCtx)
        {
            bookingCtx.BookingId = new Random().Next();
            bookingCtx.ShowState("New");
            bookingCtx.View.ShowEntryPage();
        }

        public override void Cancel(BookingContext bookingCtx)
        {
            bookingCtx.TransitionToState(new ClosedState("Booking cancelled"));
        }

        public override void DatePassed(BookingContext bookingCtx)
        {
            bookingCtx.TransitionToState(new ClosedState("Booking expired"));
        }

        public override void EnterDetails(BookingContext bookingCtx, string attendee, int ticketCount)
        {
            bookingCtx.Attendee = attendee;
            bookingCtx.TicketCount = ticketCount;
            bookingCtx.TransitionToState(new PendingState());
        }
    }
}