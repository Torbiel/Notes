using System.Threading.Tasks;
using Notes.Data.Abstract;

namespace Notes.Data
{
    public class GenericRepository : IGenericRepository
    {
        protected readonly NotesDbContext _context;

        public GenericRepository(NotesDbContext context)
        {
            _context = context;
        }

        public async void Add<T>(T entity) where T : class
        {
            await _context.AddAsync(entity);
        }

        public async Task<T> GetById<T>(int id) where T : class
        {
           return await _context.Set<T>().FindAsync(id);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Set<T>().Update(entity);
        }
    }
}
