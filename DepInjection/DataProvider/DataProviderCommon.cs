using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.DataProvider
{
    public class DataProviderCommon
    {
        public string GetDataProviderType(IData p_data)
        {
            return p_data.GetType().Name;
        }
    }
}
