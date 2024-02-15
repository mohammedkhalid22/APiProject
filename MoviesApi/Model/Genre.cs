using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesApi.Model
{
    public class Genre
    {
        [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
        public Byte Id  { get; set; }
        [MaxLength (10)]
        public string Name   { get; set; }
    }
}
