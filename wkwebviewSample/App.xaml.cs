using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PCLStorage;
using wkwebviewSample.Helpers;
using wkwebviewSample.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace wkwebviewSample
{
    public partial class App : Application
    {
        public string localAppData { get; set; }
        public string documents { get; set; }

        public static List<PathologieModel> listPathologies { get; set; } = new List<PathologieModel>();
        public static List<RessourceModel> listRessources { get; set; } = new List<RessourceModel>();
        Dictionary<string, dynamic> dicoPathologies { get; set; } = new Dictionary<string, dynamic>();

        public App()
        {
            InitializeComponent();

            // Get correct path (IOS and Android)
            if (Device.RuntimePlatform == Device.iOS)
            {
                localAppData = Environment.GetFolderPath(Environment.SpecialFolder.Resources);
                //localAppData = Path.Combine(documents, "..", "Library");
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                localAppData = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }

            Task.Run(async () =>
            {
                await InitDataModel();
            });

            MainPage = new NavigationPage(new MainPage());
        }

        // DATAMODEL INIT
        private async Task InitDataModel()
        {
            if (VersionTracking.IsFirstLaunchEver)
            {
                // No dataset in documents directory, extract bundle zip to documents directory
                bool successUnzip = Task.Run(async () => await DataHelper.UnzipFileInDirectory()).Result;

                if (successUnzip)
                    Debug.WriteLine("Zip extracted sucessfully !");
                else
                    Debug.WriteLine("Error zip extract !");
            }

            // Parsing files
            await ParsingAllFiles();
        }

        async Task<bool> ParsingAllFiles()
        {
            // Get file content json
            var fileContent = await PCLHelper.ReadAllTextAsync("1.json", new FileSystemFolder(Path.Combine(localAppData, "1")));

            if (fileContent.Length != 0)
            {
                // Dico full JSON  
                Dictionary<string, object> dicoJSON = JsonConvert.DeserializeObject<Dictionary<string, object>>(fileContent);

                foreach (KeyValuePair<string, object> keyValuePairJSON in dicoJSON)
                {
                    // Process keys fiches
                    if (keyValuePairJSON.Key == "pathologies")
                    {
                        Dictionary<string, object> dicoGrouping = JsonConvert.DeserializeObject<Dictionary<string, object>>(keyValuePairJSON.Value.ToString());

                        if (keyValuePairJSON.Key == "pathologies")
                        {
                            dicoPathologies = new Dictionary<string, dynamic>(dicoGrouping);
                            InitPathologies();
                        }
                    }

                    // Process ressources key
                    if (keyValuePairJSON.Key == "ressources")
                    {
                        List<Dictionary<string, object>> dico = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(keyValuePairJSON.Value.ToString());
                        InitRessources(dico);
                    }
                }

                return true;
            }

            return false;
        }

        public void InitPathologies()
        {
            // Iteration section header
            foreach (KeyValuePair<string, object> pair in dicoPathologies)
            {
                List<Dictionary<string, object>> dico = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(pair.Value.ToString());

                // Iteration content of section
                foreach (Dictionary<string, dynamic> item in dico)
                {
                    PathologieModel pathologie = new PathologieModel();

                    if (item.ContainsKey("title"))
                        pathologie.Title = item["title"];

                    if (item.ContainsKey("file"))
                        pathologie.File = item["file"];

                    if (item.ContainsKey("coverage"))
                        pathologie.Coverage = item["coverage"];

                    if (item.ContainsKey("keywords"))
                        pathologie.Keywords = item["keywords"];

                    if (item.ContainsKey("authors"))
                        pathologie.Authors = item["authors"];

                    if (item.ContainsKey("speciality"))
                    {
                        pathologie.Speciality = item["speciality"];
                        var splitArray = pathologie.Speciality.Split(',');
                        pathologie.ListSpeciality = splitArray.ToList();
                    }

                    if (item.ContainsKey("chapters"))
                    {
                        List<string> chapters = JsonConvert.DeserializeObject<List<string>>(item["chapters"].ToString());
                        pathologie.ListChapters = chapters;
                        chapters = null;
                    }

                    if (item.ContainsKey("medias"))
                    {
                        List<MultimediaModel> medias = JsonConvert.DeserializeObject<List<MultimediaModel>>(item["medias"].ToString());
                        pathologie.ListMedias = medias;
                    }

                    pathologie.IsFavoris = false;

                    pathologie.Version = 1;

                    pathologie.IsNote = false;

                    listPathologies.Add(pathologie);

                }
            }
        }

        public void InitRessources(List<Dictionary<string, object>> dico)
        {
            // Iteration content of section
            foreach (Dictionary<string, dynamic> item in dico)
            {
                RessourceModel ressource = new RessourceModel();

                if (item.ContainsKey("action"))
                    ressource.Action = item["action"];

                if (item.ContainsKey("file"))
                    ressource.File = item["file"];

                ressource.Version = 1;

                listRessources.Add(ressource);
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
