using System.Collections.Generic;
using System.Threading.Tasks;
using Notes.Data.Abstract;
using Notes.Models;
using System.Linq;

namespace Notes.Data
{
    public class NotesRepository : GenericRepository, INotesRepository
    {
        public NotesRepository(NotesDbContext context) : base(context) { }

        public async Task<IEnumerable<Note>> GetHistoryById(int id)
        {
            var originalNote = await GetById<Note>(id);

            return _context.Set<Note>().Where(note => note.OriginalNoteId == originalNote.OriginalNoteId);
        }

        public int GetHighestOriginalNoteId()
        {
            if(_context.Set<Note>().Count() == 0)
            {
                return 0;
            }
            else
            {
                return _context.Set<Note>().Max(n => n.OriginalNoteId);
            }
        }

        public Note GetLatestById(int id)
        {
            var maxVersion = _context.Set<Note>().Where(note => note.Id == id).Max(note => note.Version);
            
            return _context.Set<Note>().FirstOrDefault(note => note.Id == id && 
                                                               note.Version == maxVersion && 
                                                               note.Deleted == false);
        }
    }
}
