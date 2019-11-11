using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notes.Models
{
    public class Note
    {
        // Title and Content have to have the Required attribute, so the EF Core makes them not nullable in db
        // (strings are nullable by default)

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OriginalNoteId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Created { get; set;  }
        public DateTime Modified { get; set; }
        public bool Deleted { get; set; }
        public int Version { get; set; }
    }
}
