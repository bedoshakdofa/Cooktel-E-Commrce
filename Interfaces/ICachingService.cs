namespace Cooktel_E_commrece.Interfaces
{
    public interface ICachingService
    {
        T GetData<T>(string key);

        void SetData<T>(string key,T data);

        Task RemoveCache<T>(string key);
    }
}
