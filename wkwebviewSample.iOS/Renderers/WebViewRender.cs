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
}