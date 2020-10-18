using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using wkwebviewSample.Models;
using Xamarin.Forms;

namespace wkwebviewSample
{
    public partial class WebViewPage : ContentPage
    {
        PathologieModel _pathologie { get; set; }

        public Func<string, Task<string>> EvaluateJavascript { get; set; }

        public WebViewPage()
        {
            InitializeComponent();

            // Init webview
            InitWebView();
        }

        void InitWebView()
        {
            // Binding context
            BindingContext = this;

            var urlSource = new UrlWebViewSource();

            _pathologie = App.listPathologies.First();

            // Init TempUrl
            string TempUrl = null;

            if (_pathologie != null)
                TempUrl = Path.Combine((Application.Current as App).localAppData, _pathologie.Version.ToString(), "pathologies", _pathologie.File);

            urlSource.Url = string.Concat("file://", TempUrl);

            bool doesExist = File.Exists(TempUrl);

            webView.Source = urlSource;

            // Intercept actions on webview
            webView.Navigated += OnNavigated;
        }

        private async void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            SetFontSize(200);

            // Replace url style css
            await ReplaceUrlStyle();
        }

        async void SetFontSize(int fontSize)
        {
            // Call JS in webview
            var js = "document.getElementsByTagName('body')[0].setAttribute('style','font-size: " + fontSize + "% !important')";
            await EvaluateJavascript(js);
        }

        async Task ReplaceUrlStyle()
        {
            // Replace style url of all class .image (background-image: with directory version)
            string result = await EvaluateJavascript("(function() { var arrayFiles = []; for(var i in document.styleSheets) { if (document.styleSheets[i].href != undefined) { var fileWithSlash = document.styleSheets[i].href.substring(document.styleSheets[i].href.lastIndexOf('/')); var file = fileWithSlash.substr(1, fileWithSlash.length); arrayFiles.push(file); } } return arrayFiles.join(';'); })();");

            // Remove quotes
            result = result.Trim('"');

            string[] files = result.Split(';');
            foreach (string file in files)
            {
                // Find correct ressource object
                RessourceModel ressource = App.listRessources.Find(r => r.File == file);

                if (ressource != null)
                {
                    string path = Path.Combine((Application.Current as App).localAppData, ressource.Version.ToString(), "ressources", file);
                    bool doesExist = File.Exists(path);

                    var js = "document.querySelector(\"link[href='" + file + "']\").href = \"" + path + "\";";

                    await EvaluateJavascript(js);
                }
            }
        }
    }
}