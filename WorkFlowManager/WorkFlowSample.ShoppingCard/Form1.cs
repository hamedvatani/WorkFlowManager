namespace WorkFlowSample.ShoppingCard
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnCreate_Click(object sender, EventArgs e)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response=await httpClient.GetAsync("https://localhost:7139/WeatherForecast"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    txtResult.AppendText(apiResponse);
                    txtResult.AppendText("\r\n");
                }
            }
        }
    }
}