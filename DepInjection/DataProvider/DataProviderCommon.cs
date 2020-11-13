namespace DepInjection.DataProvider
{
    public class DataProviderCommon
    {
        public string GetDataProviderType(IDataActions pDataActions)
        {
            return pDataActions.GetType().Name;
        }
    }
}
