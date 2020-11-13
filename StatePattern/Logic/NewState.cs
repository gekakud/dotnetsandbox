using System;

namespace StatePattern.Logic
{
    class NewState:BookingState
    {
        public override void EnterState(StateMachine bookingCtx)
        {
            bookingCtx.BookingId = new Random().Next();
            bookingCtx.ShowState("New");
            bookingCtx.View.ShowEntryPage();
        }

        public override void Cancel(StateMachine bookingCtx)
        {
            bookingCtx.TransitionToState(new ClosedState("Booking cancelled"));
        }

        public override void DatePassed(StateMachine bookingCtx)
        {
            bookingCtx.TransitionToState(new ClosedState("Booking expired"));
        }

        public override void EnterDetails(StateMachine bookingCtx, string attendee, int ticketCount)
        {
            bookingCtx.Attendee = attendee;
            bookingCtx.TicketCount = ticketCount;
            bookingCtx.TransitionToState(new PendingState());
        }
    }
}