using System;
using System.Collections.Generic;
using Notes.Data;
using Notes.Models;

namespace Notes.Tests.Helpers
{
    public class Utilities
    {
        public static void InitializeDbForTests(NotesDbContext db)
        {
            db.Set<Note>().AddRange(GetNotesSeed());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(NotesDbContext db)
        {
            db.Set<Note>().RemoveRange(db.Set<Note>());
            InitializeDbForTests(db);
        }

        public static List<Note> GetNotesSeed()
        {
            return new List<Note>()
            {
                new Note()
                {
                    OriginalNoteId = 1,
                    Title = "title 1",
                    Content = "content 1",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    Deleted = false,
                    Version = 1
                },
                new Note()
                {
                    OriginalNoteId = 1,
                    Title = "title 2",
                    Content = "content 2",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    Deleted = false,
                    Version = 2
                },
                new Note()
                {
                    OriginalNoteId = 3,
                    Title = "title 3",
                    Content = "content 3",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    Deleted = true,
                    Version = 1
                },
                new Note()
                {
                    OriginalNoteId = 3,
                    Title = "title 4",
                    Content = "content 4",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    Deleted = false,
                    Version = 2
                }
            };
        }
    }
}
