using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using LiveStreamRadio.Model;
using LiveStreamRadio.Resources;
using Microsoft.Phone;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Shoutcast.Sample.Phone.Background.PlaybackAgent;
using Silverlight.Media;
using Microsoft.Phone.BackgroundAudio;

namespace LiveStreamRadio
{
    public partial class MainPage : PhoneApplicationPage
    {
        /// <summary>
        /// Private field that stores the timer for updating the UI
        /// </summary>
        private DispatcherTimer _timer;
        private const int PlayButtonIndex = 0;

        public MainPage()
        {
            InitializeComponent();
            WebListBox.ItemsSource = WebviewsDataSource.CreatWebViews();
            Loaded += MainPageLoaded;
        }

        async void MainPageLoaded(object sender, RoutedEventArgs e)
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(0.5);
            _timer.Tick += UpdateInfo;

            BackgroundAudioPlayer.Instance.PlayStateChanged += new EventHandler(InstancePlayStateChanged);

            if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Playing)
            {
                _timer.Start();
                UpdatePlayButton();
                InstancePlayStateChanged(null, null);
                UpdateInfo(null, null);
            }
        }

        private void UpdateInfo(object sender, EventArgs eventArgs)
        {
            AudioTrack audioTrack = BackgroundAudioPlayer.Instance.Track;
            if (audioTrack != null)
            {
                if (audioTrack.Title != SongTextBlock.Text && audioTrack.Artist != ArtistTextBlock.Text && audioTrack.Artist != null)
                {
                    SongTextBlock.Text = audioTrack.Title;
                    ArtistTextBlock.Text = audioTrack.Artist;
                    GetAlbumArt(audioTrack.Tag);
                }
            }
        }

        private void GetAlbumArt(string info)
        {
            var request = WebRequest.Create(String.Format("https://itunes.apple.com/search?term={0}&entity=musicTrack",
                info));
            request.BeginGetResponse(asyncResponse =>
            {
                try
                {
                    var responseRequest = (HttpWebRequest)asyncResponse.AsyncState;
                    var response = (HttpWebResponse)responseRequest.EndGetResponse(asyncResponse);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var streamReader = new StreamReader(response.GetResponseStream());
                        try
                        {
                            string content = streamReader.ReadToEnd();
                            int CoverPos = content.IndexOf("artworkUrl100\":\"", System.StringComparison.Ordinal) + 16;
                            if (CoverPos > 15)
                            {
                                var coverLink = content.Substring(CoverPos,
                                    content.IndexOf("\"", CoverPos, System.StringComparison.Ordinal) - CoverPos);
                                Debug.WriteLine(coverLink);
                                CoverImageHelper.GetImage(coverLink, ImageCover);
                            }
                            else
                            {
                                Deployment.Current.Dispatcher.BeginInvoke(() =>
                                                                          {
                                                                              ImageCover.Source =
                                                                                  new BitmapImage(
                                                                                      new Uri("\\Assets\\Radio.jpg", UriKind.RelativeOrAbsolute));
                                                                          });

                            }
                        }
                        finally
                        {
                            streamReader.Close();
                        }
                    }
                }
                catch (WebException webExc)
                {

                }
            }, request);

        }

        void InstancePlayStateChanged(object sender, EventArgs e)
        {
            switch (BackgroundAudioPlayer.Instance.PlayerState)
            {
                case PlayState.TrackEnded:
                    break;
                case PlayState.TrackReady:
                    BackgroundAudioPlayer.Instance.Play();
                    break;
                case PlayState.Playing:
                    ProgressBar.Visibility = Visibility.Collapsed;
                    break;
                default:
                    ProgressBar.Visibility = Visibility.Collapsed;
                    break;
            }
            UpdatePlayButton();
        }


        private void BtnPlayClick(object sender, System.EventArgs e)
        {

            if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Playing)
            {
                BackgroundAudioPlayer.Instance.Pause();
            }
            else
            {
                BackgroundAudioPlayer.Instance.Track = new AudioTrack(null, "STEREO PESARO", null, null, null, AppResources.RadioSource, EnabledPlayerControls.Pause);
                ProgressBar.Visibility = Visibility.Visible;
                _timer.Start();
            }
        }

        private void UpdatePlayButton()
        {
            var playBtn = ((ApplicationBarIconButton)ApplicationBar.Buttons[PlayButtonIndex]);
            if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Playing)
            {
                playBtn.IconUri = new Uri("\\Assets\\AppBar\\transport.pause.png", UriKind.RelativeOrAbsolute);
                playBtn.Text = "pause";
            }
            else
            {
                playBtn.IconUri = new Uri("\\Assets\\AppBar\\transport.play.png", UriKind.RelativeOrAbsolute);
                playBtn.Text = "play";
            }
        }

        private void WebListBoxTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (WebListBox.SelectedItem == null) return;
            WebviewsDataSource.CurrentWebView = WebListBox.SelectedItem as WebViewItem;
            NavigationService.Navigate(new Uri("/WebViews.xaml", UriKind.RelativeOrAbsolute));
        }

        private void HyperlinkButtonEmailClick(object sender, RoutedEventArgs e)
        {
            var emailComposeTask = new EmailComposeTask
                                   {
                                       Subject = "STEREO PESARO",
                                       To = "info@stereopesaro.it"
                                   };
            emailComposeTask.Show();
        }

        private void PhoneHyperLinkButtonClick(object sender, RoutedEventArgs e)
        {
            var phoneCallTask = new PhoneCallTask { PhoneNumber = "+393336008820", DisplayName = "STEREO PESARO" };
            phoneCallTask.Show();
        }

        private void SmsHyperLinkBtnClick(object sender, RoutedEventArgs e)
        {
            SmsComposeTask sms = new SmsComposeTask()
                               {
                                   Body = "",
                                   To = "+393336008820"
                               };
            sms.Show();
        }

        private void BtnShareClick(object sender, EventArgs e)
        {
            if (BackgroundAudioPlayer.Instance.Track == null) return;
            ShareStatusTask share = new ShareStatusTask()
            {
                Status = String.Format("YOU ARE LISTENING {0} ON STEREO PESARO www.stereopesaro.it", BackgroundAudioPlayer.Instance.Track.Tag)
            };
            share.Show();
        }

    }


}