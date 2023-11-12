using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ImageTest
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public double RectWidth { get; set; }
        public double RectHeight { get; set; }
        private Timer _timer;
        private Dispatcher _uiDispatcher;
        public Action<int, int, int, int> AddRectangleAction { get; set; }
        public Action<int> ChangeHeightAction { get; set; }

        private BitmapImage _bitmapImage;
        public BitmapImage BitmapImage
        {
            get { return _bitmapImage; }
            set { SetProperty(nameof(BitmapImage), ref _bitmapImage, value); }
        }

        public Command ButtonCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool SetProperty<T>(string propertyName, ref T backingField, T newValue)
        {
            bool changed;

            if (newValue == null && backingField != null || newValue != null && backingField == null)

            {
                changed = true;
            }
            else if ((newValue == null && backingField == null) || backingField.Equals(newValue))
            {
                changed = false;
            }
            else
            {
                changed = true;
            }

            if (changed)
            {
                backingField = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            return changed;
        }

        public void UpdateProperty(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void Callback(object state)
        {
            _uiDispatcher.Invoke(() =>
            {
                AddRectangleAction(167, 360, 219, 24);
                AddRectangleAction(859, 480, 102, 24);
            });

            _timer.Dispose();
            _timer = null;
        }

        private void OnButtonCommand()
        {
            ChangeHeightAction(34);
        }

        public MainViewModel()
        {
            _uiDispatcher = Dispatcher.CurrentDispatcher;

            ButtonCommand = new Command(OnButtonCommand);

            using (var pdf = new Spire.Pdf.PdfDocument())
            {
                pdf.LoadFromFile("Test.pdf");

                using (var mStream = new MemoryStream())
                {
                    var image = pdf.SaveAsImage(1, Spire.Pdf.Graphics.PdfImageType.Bitmap, 600, 600);

                    using (MemoryStream memory = new MemoryStream())
                    {
                        image.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                        memory.Position = 0;
                        BitmapImage = new BitmapImage();
                        BitmapImage.BeginInit();
                        BitmapImage.StreamSource = memory;
                        BitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        BitmapImage.EndInit();
                    }
                }
            }

            _timer = new Timer(Callback, null, 0, 2000);
        }
    }
}
