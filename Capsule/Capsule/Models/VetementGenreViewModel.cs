using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
namespace Capsule.Models
{
    public class VetementGenreViewModel
    {
        public List<Vetement>? Vetements { get; set; }
        public SelectList? Genres { get; set; }
        public string? VetementGenre { get; set; }
        public string? SearchString { get; set; }
    }
}
