using System.Collections.Generic;

namespace CollectionInterfaces
{
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
}