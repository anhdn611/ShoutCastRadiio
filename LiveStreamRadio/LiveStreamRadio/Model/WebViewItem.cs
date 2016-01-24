namespace LiveStreamRadio.Model
{
    public class WebViewItem : NotifyBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (Name == value) return;
                _name = value;
                RaisePropertyChanged(() => Name);
            }

        }

        private string _path;
        public string Path
        {
            get { return _path; }
            set
            {
                if (Path == value) return;
                _path = value;
                RaisePropertyChanged(() => Path);
            }

        }

        private string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                if (ImagePath == value) return;
                _imagePath = value;
                RaisePropertyChanged(() => ImagePath);
            }

        }
    }
}
