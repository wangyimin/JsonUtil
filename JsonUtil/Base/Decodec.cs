namespace JsonUtil.Base
{
    public interface Decodec
    {
        string Convert<T>(T obj);
    }
}
