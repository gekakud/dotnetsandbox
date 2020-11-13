namespace DepInjection
{
    public class RetreiveData
    {
        public RetreiveData(IDataActions pDataActions)
        {
            IDataActions dataActions = pDataActions;
            dataActions.SaveData("dummy data");
            dataActions.GetData();
        }
    }
}