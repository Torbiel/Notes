using System.Threading.Tasks;

namespace Notes.Data.Abstract
{
    public interface IGenericRepository
    {
        void Add<T>(T entity) where T : class;
        Task<T> GetById<T>(int id) where T : class;
        void Update<T>(T entity) where T : class;
    }
}
