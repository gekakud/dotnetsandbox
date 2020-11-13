using System;

namespace StatePattern
{
    public class ConsoleUi
    {
        // UI fields
        private string AttendeeText { get; set; }
        private string TicketCountText { get; set; }
        private string StateNameText { get; set; }

        private StateMachine bookingStateMachine;

        public ConsoleUi()
        {
            bookingStateMachine = new StateMachine(this);
            bookingStateMachine.InitFirstBooking();
        }

        public void PrintToConsole(string stateName, string attendee, int ticketCount)
        {
            // "mapping":)
            StateNameText = stateName;
            AttendeeText = attendee;
            TicketCountText = Convert.ToString(ticketCount);

            Console.WriteLine($"state:{StateNameText}, Attendee:{AttendeeText}, Ticket count:{TicketCountText}");
        }

        public void ShowEntryPage()
        {
            Console.WriteLine("Hi, you are going to book new ticket...");
            Console.WriteLine("Enter your name");
            AttendeeText = Console.ReadLine();

            Console.WriteLine("enter number of desired ticket...");

            TicketCountText = Console.ReadLine();

            bookingStateMachine.SubmitDetails(AttendeeText, Int32.Parse(TicketCountText));
        }

        public void ShowStatus(string status)
        {
            Console.WriteLine($"status:{status}");
        }

        public void ShowError(string errorText)
        {
            Console.WriteLine($"error:{errorText}");
        }
    }
}