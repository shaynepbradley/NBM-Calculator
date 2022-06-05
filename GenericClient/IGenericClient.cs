namespace GenericClient
{
    public interface IGenericClient
    {
        public Task<T?> GetData<T>(string api, CancellationToken cancelToken);
        public Task<T?> GetData<T>(Uri api, CancellationToken cancelToken);
    }
}