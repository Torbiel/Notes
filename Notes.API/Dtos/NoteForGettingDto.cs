using System;

namespace Notes.Dtos
{
    public class NoteForGettingDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public int Version { get; set; }
    }
}
