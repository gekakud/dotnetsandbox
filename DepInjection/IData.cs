namespace DepInjection
{
    public interface IData
    {
        object GetData();
        bool SaveData(string p_data);
    }
}
