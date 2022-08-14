using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using WorkFlowManager.Core.Contract;

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

        private void button1_Click(object sender, EventArgs e)
        {
            var assembly =
                Assembly.LoadFile(
                    @"C:\Projects\WorkFlowManager\WorkFlowManager\WorkFlowManager.Test\bin\Debug\net6.0\WorkFlowManager.Test.dll");

            var tt=assembly.GetTypes();
            List<Type> types = new();
            foreach (var t in tt)
            {
                if (typeof(IWorker).IsAssignableFrom(t))
                    types.Add(t);
            }


            // var type = assembly.GetType("WorkFlowManager.Test.ShoppingCard.IsExists");
            // IWorker worker = (IWorker) Activator.CreateInstance(type);
        }
    }
}