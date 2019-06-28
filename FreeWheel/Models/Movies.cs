using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreeWheel.Models
{
    public class Movies
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string title { get; set; }
        [Column(TypeName = "int")]
        public int yearOfRelease { get; set; }
        [Column(TypeName = "int")]
        public int runningTime { get; set; }

        public IEnumerable<Ratings> ratings { get; set; }

        public IEnumerable<MoviesGenres> MoviesGenres { get; set; }
    }
}
