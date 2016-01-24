using System.Collections.ObjectModel;
using LiveStreamRadio.Model;

namespace LiveStreamRadio
{
    public static class WebviewsDataSource
    {
        public static ObservableCollection<WebViewItem> CreatWebViews()
        {
            ObservableCollection<WebViewItem> Webviews = new ObservableCollection<WebViewItem>();
            Webviews.Add(new WebViewItem()
            {
                Name = "Stereo Pesaro Page",
                ImagePath = "\\Assets\\Facebook.png",
                Path = "https://m.facebook.com/pages/Stereo-Pesaro-20/459320760821411?fref=ts"
            });
            Webviews.Add(new WebViewItem()
            {
                Name = "Stereo Pesaro",
                ImagePath = "\\Assets\\stereopesaro.png",
                Path = "http://www.stereopesaro.it/mobile"
            });
            Webviews.Add(new WebViewItem()
            {
                Name = "Pesaroprima",
                ImagePath = "\\Assets\\pesaroprima.png",
                Path = "http://www.pesaroprima.it"
            });
            return Webviews;
        }

        public static WebViewItem CurrentWebView =new WebViewItem();
    }
}
