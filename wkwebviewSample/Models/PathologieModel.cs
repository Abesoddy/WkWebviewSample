using System.Collections.Generic;

namespace wkwebviewSample.Models
{
    public class PathologieModel
    {
        public string Title { get; set; }
        public string File { get; set; }
        public string Coverage { get; set; }
        public string Keywords { get; set; }
        public string Authors { get; set; }
        public string Speciality { get; set; }
        public int Version { get; set; }

        public bool IsFavoris { get; set; }
        public bool IsNote { get; set; }

        public List<string> ListSpeciality { get; set; } = new List<string>();
        public List<string> ListChapters { get; set; } = new List<string>();
        public List<MultimediaModel> ListMedias { get; set; } = new List<MultimediaModel>();
    }
}
