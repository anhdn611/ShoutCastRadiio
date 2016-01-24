using System;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace LiveStreamRadio
{
    public partial class WebViews : PhoneApplicationPage
    {
        public WebViews()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.WebBrowser.Navigate(new Uri(WebviewsDataSource.CurrentWebView.Path));
            this.WebBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.IsIndeterminate = true;
            SystemTray.ProgressIndicator.Text = "Loading";
            SystemTray.ProgressIndicator.IsVisible = true;
        }

        void WebBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (SystemTray.ProgressIndicator == null) return;


            SystemTray.ProgressIndicator.IsVisible = false;
        }

        private void RefreshAppBarBtnClick(object sender, EventArgs e)
        {
            this.WebBrowser.Navigate(new Uri(WebviewsDataSource.CurrentWebView.Path));
            SystemTray.IsVisible = true;
            SystemTray.ProgressIndicator.IsVisible = true;
        }

        private void BackAppBarBtn_Click(object sender, EventArgs e)
        {
            if (WebBrowser.CanGoBack) WebBrowser.GoBack();
        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            if (WebBrowser.CanGoForward) WebBrowser.GoForward();
        }
    }
}