using System.Collections.Generic;
using System.Linq;

namespace CollectionInterfaces
{
    internal class UsersDB : IDataBase
    {
        private readonly IEnumerable<UserAccount> data;

        public UsersDB(IEnumerable<UserAccount> usersAccounts)
        {
            data = usersAccounts;
        }

        public List<UserAccount> RetrieveAccountsList(string accountType)
        {
            return new List<UserAccount>(data);
        }

        public Dictionary<int, UserAccount> RetrieveAccountsDictionary(string accountType)
        {
            return new Dictionary<int, UserAccount>(data.ToDictionary(user => user.AccountId));
        }
    }
}