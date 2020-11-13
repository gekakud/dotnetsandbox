using StatePattern.Logic;

namespace StatePattern
{
    public class StateMachine
    {
        public ConsoleUi View { get; set; }
        public int BookingId { get; set; }
        public string Attendee { get; set; }
        public int TicketCount { get; set; }

        private BookingState currentState;

        public StateMachine(ConsoleUi view)
        {
            View = view;
        }

        public void InitFirstBooking()
        {
            //first time we create booking we start from New state
            TransitionToState(new NewState());
        }

        public void TransitionToState(BookingState state)
        {
            currentState = state;
            currentState.EnterState(this);
        }

        public void SubmitDetails(string attendee, int ticketCount)
        {
            currentState.EnterDetails(this, attendee, ticketCount);
        }

        public void Cancel()
        {
            currentState.Cancel(this);
        }

        public void DatePassed()
        {
            currentState.DatePassed(this);
        }

        public void ShowState(string stateName)
        {
            View.PrintToConsole(stateName, Attendee, TicketCount);
        }
    }
}