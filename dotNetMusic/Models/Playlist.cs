using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace dotNetMusic.Models
{
    public class Playlist
    {
        public int Id { get; set; }

        [Required]
        public String name { get; set; }
        public String description { get; set; }
        public virtual List<Track> Tracks { get; set; }
    }
}