using Notes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notes.Data.Abstract
{
    public interface INotesRepository : IGenericRepository
    {
        Task<IEnumerable<Note>> GetHistoryById(int id);
        int GetHighestOriginalNoteId();

        Task<Note> GetLatestById(int id);
    }
}
