using System;
using System.Collections.Generic;

namespace CollectionInterfaces
{
    class MailService
    {
        //ICollection and INumerable are OK for all
        //IDictionary wiil work for usersDict only
        //IList will work for usersList
        public void SendEmailsToUsers(ICollection<UserAccount> userAccounts)
        {
            Console.WriteLine(userAccounts.GetType()+"\n");
            foreach (var userAccount in userAccounts)
            {
                Console.WriteLine("sent to "+userAccount.Email);
            }
        }
    }
}