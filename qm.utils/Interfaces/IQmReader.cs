namespace qm.utils.Interfaces
{
    public interface IQmReader
    {
        byte[][]? LoadFromFile(string filename);
    }
}