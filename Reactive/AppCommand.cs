using System;

namespace Reactive
{
    public abstract class AppCommand
    {
        public DateTime Issued { get; private set; }

        public string Issuer { get; set; }

        public AppCommand()
        {
            Issued = DateTime.Now;

            Issuer = "geka";
        }
    }

    public class QueryCommand : AppCommand
    {
        public string Query { get; private set; }

        public QueryCommand(string q) : base()
        {
            Query = q;
        }
    }

    public class NewMessageCommand : AppCommand
    {
        public Message Message { get; private set; }

        public NewMessageCommand(Message message) : base()
        {
            Message = message;
        }
    }

    public class Message
    {
        public string MsgText { get; set; }

        public Message(string msg)
        {
            MsgText = msg;
        }
    }
}
