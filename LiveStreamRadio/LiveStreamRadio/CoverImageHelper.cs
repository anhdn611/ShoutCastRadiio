using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Resources;


namespace LiveStreamRadio
{
    public class CoverImageHelper
    {
        private static Image _image;
        public  static void GetImage(string path, Image image)
        {
            _image = image;
            var client = new WebClient();
            client.OpenReadCompleted += (s, e) =>
            {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        BitmapImage bi = new BitmapImage();
                        bi.SetSource(e.Result);
                        _image.Source = bi;
                    });
            };
            client.OpenReadAsync(new Uri(path, UriKind.Absolute));
        }
    }
}
