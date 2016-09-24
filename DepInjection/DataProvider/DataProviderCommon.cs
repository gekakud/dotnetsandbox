namespace DepInjection.DataProvider
{
    public class DataProviderCommon
    {
        public string GetDataProviderType(IData p_data)
        {
            return p_data.GetType().Name;
        }
    }
}
