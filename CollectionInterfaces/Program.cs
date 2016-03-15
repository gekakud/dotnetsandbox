using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace CollectionInterfaces
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var t = ConfigurationManager.AppSettings.Get("1");
            Console.WriteLine(t);

            var mailServ = new MailService();
            var usersList = UsersGenerator.GenerateUserAccounts();
            UsersDB db = new UsersDB(usersList);
            var usersDict = usersList.ToDictionary(x => x.AccountId);
            var usersHashSet = new HashSet<UserAccount>(usersList);

            // we can pass any object that implements ICollection or IEnumerable interface,
            // but not IList, since the Dictionary does not implement IList
            // see the picture in this project folder
            mailServ.SendEmailsToUsers(usersList);
            mailServ.SendEmailsToUsers(usersDict.Values);
            mailServ.SendEmailsToUsers(usersHashSet);

            Console.WriteLine("Retrieving all accounts from DB...");
            //it does not matter how we get our users from a db class
            //whenever we get List or Dict.  these both are OK, since
            //implementing IEnumerable interface
            IEnumerable<UserAccount> someEnumerable = db.RetrieveAccountsDictionary("any type").Values;

            //cast IEnumerable to ICollection
            mailServ.SendEmailsToUsers((ICollection<UserAccount>)someEnumerable);

            Console.ReadKey();
        }
    }
}