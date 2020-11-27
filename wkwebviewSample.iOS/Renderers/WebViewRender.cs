using System;
using System.IO;
using Foundation;
using WebKit;
using wkwebviewSample.iOS.Renderers;
using wkwebviewSample.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(WebViewer), typeof(WebViewRender))]
namespace wkwebviewSample.iOS.Renderers
{
    public class WebViewRender : WkWebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                //string filename = Path.Combine(NSBundle.MainBundle.BundlePath, "1");
                //LoadRequest(new NSUrlRequest(Url));

                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.Resources);
                string filename = Path.Combine(localAppData, "1");


                LoadFileUrl(Url, new NSUrl(filename, false));

                var webView = e.NewElement as WebViewer;

                if (webView != null)
                {
                    webView.EvaluateJavascript = async (js) =>
                    {
                        return await webView.EvaluateJavaScriptAsync(js);
                    };
                }
            }
        }

        //protected override void OnElementChanged(VisualElementChangedEventArgs e)
        //{
        //    base.OnElementChanged(e);

        //    var webView = e.NewElement as WebViewer;

        //    if (webView != null)
        //    {
        //        webView.EvaluateJavascript = async (js) =>
        //        {
        //            return await webView.EvaluateJavaScriptAsync(js);
        //        };
        //    }
        //}
    }

    //public class MyNavigationDelegate : WKNavigationDelegate
    //{
    //    public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
    //    {
    //        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.Resources);

    //        string path = Path.Combine(localAppData, "1");

    //        string test = string.Concat("file://", path);


    //        //get url here
    //        var url = webView.Url;

    //        //webView.LoadFileUrl();

    //        webView.LoadFileUrl(url, readAccessUrl: new NSUrl(test));
    //    }
    //}
}