using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace wkwebviewSample.Models
{
    public class MultimediaModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Speciality { get; set; }
        public string Type { get; set; }
        public string Protocol { get; set; }
        public string Thumb { get; set; }
        public string File { get; set; }
        public string Pathologies { get; set; }
        public string Techniques { get; set; }
        public string Keywords { get; set; }
        public int Version { get; set; }

        public List<string> ListSpeciality { get; set; } = new List<string>();

        public ImageSource ThumbSource
        {
            get
            {
                return ImageSource.FromFile(Path.Combine((Application.Current as App).localAppData, Version.ToString(), "vignettes", Thumb));
            }
        }
    }
}