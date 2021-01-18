namespace RedisViewer.Core.Services
{
    public enum KeyType
    {
        None = 0,
        String = 1,
        List = 2,
        Set = 3,
        SortedSet = 4,
        Hash = 5,
        Stream = 6,
        Unknown = 7
    }
}