using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace dotNetMusic.Models
{
    public class Track
    {
        public int id { get; set; }

        [Required]
        public String Title { get; set; }
        public String Artist { get; set; }
        public DateTime? AddedDate { get; set; }
        [Required]
        [Url(ErrorMessage = "Please enter a valid Youtube URL")]
        public string EmbedURL { get; set; }
        public String Description { get; set; }

        public int? rating{ get; set; }

        public byte[] QRcode { get; set; }
        public virtual int PlaylistId { get; set; }
        public virtual Playlist Playlist { get; set; }

    }
}