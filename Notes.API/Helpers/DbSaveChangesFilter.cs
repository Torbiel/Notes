using Microsoft.AspNetCore.Mvc.Filters;
using Notes.Data;
using System.Threading.Tasks;

namespace Notes.Helpers
{
    public class DbSaveChangesFilter : IAsyncActionFilter
    {
        private readonly NotesDbContext _context;

        public DbSaveChangesFilter(NotesDbContext context)
        {
            _context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = await next();

            if (result.Exception == null || result.ExceptionHandled)
            {
                await _context.SaveChangesAsync();
            }
        }
    }
}
