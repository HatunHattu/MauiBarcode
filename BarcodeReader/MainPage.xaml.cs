using ZXing.Net.Maui;

namespace BarcodeReader
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            cameraBarcodeReaderView.Options = new BarcodeReaderOptions
            {
                Formats = BarcodeFormats.OneDimensional,
                AutoRotate = true,
                Multiple = true
            };

        }

        protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
        {
            foreach (var barcode in e.Results)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    barcodeResult.Text = $"{barcode.Format}: {barcode.Value}";
                });
            }
        }
    }

}
