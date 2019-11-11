using System.ComponentModel.DataAnnotations;

namespace Notes.Dtos
{
    public class NoteForAddingDto
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }
    }
}
