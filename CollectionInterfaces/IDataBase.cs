using System.Collections.Generic;

namespace CollectionInterfaces
{
    public interface IDataBase
    {
        List<UserAccount> RetrieveAccountsList(string accountType);
        Dictionary<int, UserAccount> RetrieveAccountsDictionary(string accountType);
    }
}