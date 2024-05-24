using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ZXing.Net.Maui;
using System.Text.Json;

namespace BarcodeReader
{
    public partial class MainPage : ContentPage
    {
        private static readonly HttpClient client = new HttpClient();

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
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    barcodeResult.Text = $"{barcode.Format}: {barcode.Value}";
                    await CallPhpFileAsync("http://yourserver.com/insert_barcode.php", barcode.Format.ToString(), barcode.Value);
                });
            }
        }

        public async Task CallPhpFileAsync(string url, string format, string value)
        {
            try
            {
                var content = new FormUrlEncodedContent(new[]
                {
            new KeyValuePair<string, string>("format", format),
            new KeyValuePair<string, string>("value", value)
        });

                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                // Parse the JSON response using System.Text.Json
                JsonDocument jsonResponse = JsonDocument.Parse(responseBody);
                bool success = jsonResponse.RootElement.GetProperty("success").GetBoolean();
                string message = jsonResponse.RootElement.GetProperty("message").GetString();

                // Display success or error message
                if (success)
                {
                    barcodeResult.Text += "\nSuccess: " + message;
                }
                else
                {
                    barcodeResult.Text += "\nError: " + message;
                }
            }
            catch (HttpRequestException e)
            {
                barcodeResult.Text += "\nRequest error: " + e.Message;
            }
        }

    }
}
