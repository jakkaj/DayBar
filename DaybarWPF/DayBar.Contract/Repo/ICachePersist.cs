namespace DayBar.Contract.Repo
{
    public interface ICachePersist
    {
        void Write(byte[] cache);
        byte[] Read();
        void Clear();
    }
}