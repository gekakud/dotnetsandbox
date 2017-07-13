namespace DepInjection
{
    public class RetreiveData
    {
        public RetreiveData(IData p_data)
        {
            var data = p_data;
            data.SaveData("dummy data");
            data.GetData();
        }
    }
}