using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreeWheel.Models
{
    public class MoviesGenres
    {
        public Guid MoviesId { get; set; }
        public Movies Movies { get; set; }
        public Guid GenresId { get; set; }
        public Genres Genres { get; set; }
    }
}