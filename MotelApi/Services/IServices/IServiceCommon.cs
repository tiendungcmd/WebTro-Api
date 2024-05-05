namespace MotelApi.Services.IServices
{
    public interface IServiceCommon<T>
    {
        public Task<T> Create(T model);
        public Task<bool> Delete(Guid id);
        public Task<T> GetById(Guid id);
        public Task<List<T>> GetAll();
        public Task<T> Update(T model);
    }
}
