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

            if(originalNote == null)
            {
                return null;
            }

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

        public async Task<Note> GetLatestById(int id)
        {
            var note = await _context.Set<Note>().FindAsync(id);

            if (note == null)
            {
                return null;
            }

            var notes = _context.Set<Note>().Where(n => n.OriginalNoteId == note.OriginalNoteId).ToList();
            var maxVersion = 1;

            if(notes.Count() > 1)
            {
                maxVersion = notes.Max(note => note.Version);
            }

            return _context.Set<Note>().FirstOrDefault(note => note.Version == maxVersion && 
                                                               note.Deleted == false);
        }
    }
}
