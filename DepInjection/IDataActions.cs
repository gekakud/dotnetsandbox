namespace DepInjection
{
    public interface IDataActions
    {
        object GetData();
        bool SaveData(string p_data);
    }
}
