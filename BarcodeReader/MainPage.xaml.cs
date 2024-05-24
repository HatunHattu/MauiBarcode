using Data;
using MySql.Data.MySqlClient;
using System.Data.Common;
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

        public void ConnSQL()
        {
            var dbCon = DBConnection.Instance();
            dbCon.Server = "loalchost";
            dbCon.DatabaseName = "qrcodereader";
            dbCon.UserName = "root";
            dbCon.Password = "";
            if (dbCon.IsConnect())
            {
                string query = "INSERT INTO barcodes";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string someStringFromColumnZero = reader.GetString(0);
                    string someStringFromColumnOne = reader.GetString(1);
                    Console.WriteLine(someStringFromColumnZero + "," + someStringFromColumnOne);
                }
                dbCon.Close();
            }
        }
    }

}
