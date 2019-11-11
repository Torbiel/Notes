using System;

namespace Notes.Dtos
{
    public class NoteForHistoryDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public bool Deleted { get; set; }
        public int Version { get; set; }
    }
}
