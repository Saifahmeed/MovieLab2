using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLab.Models
{
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // to increment by it self
        public byte Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
    }
}
