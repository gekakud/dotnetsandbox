using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionInterfaces
{
    class Program
    {
        static void Main(string[] args)
        {
            MailService mailServ = new MailService();

            List<UserAccount> usersList = UsersGenerator.GenerateUserAccounts();
            Dictionary<int, UserAccount> usersDict= usersList.ToDictionary(x => x.AccountId);
            HashSet<UserAccount> usersHashSet = new HashSet<UserAccount>(usersList);
            usersHashSet.Add(usersList.First());
            
            // we can pass any object that implements ICollection or IEnumerable interface,
            // but not IList, since the Dictionary does not implement IList
            // see the picture in this project foldel
            mailServ.SendEmailsToUsers(usersList);
            mailServ.SendEmailsToUsers(usersDict.Values);
            mailServ.SendEmailsToUsers(usersHashSet);

            Console.ReadKey();
        }
    }

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

    class UsersGenerator
    {
        public static List<UserAccount> GenerateUserAccounts()
        {
            var newList = new List<UserAccount>();
            for (int i = 0; i < 4; i++)
            {
                newList.Add(new UserAccount
                {
                    AccountId = i,
                    Email = "user" + i + "@my.com",
                    FirstName = "Bla",
                    LastName = "Bla"
                });
            }
            return newList;
        }
    }

    class UserAccount:Human
    {
        public int AccountId { get; set; }
        public string Email { get; set; }
    }

    class Human
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
