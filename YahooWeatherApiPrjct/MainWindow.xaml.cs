using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace YahooWeatherApiPrjct
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void operateBtn_Click(object sender, RoutedEventArgs e)
        {

            resultBlock.Text = "";
            
            //string builder contains the adress of the api and the query
            StringBuilder webAdress = new StringBuilder();
            webAdress.Append("https://query.yahooapis.com/v1/public/yql?");
            webAdress.Append("q=" + System.Web.HttpUtility.UrlEncode("select astronomy.sunset, astronomy.sunrise from weather.forecast where woeid in (select woeid from geo.places(1) where text='" + cityBox.Text +"," + countryBox.Text + "')"));
            webAdress.Append("&format=json");
            webAdress.Append("&diagnostics=false");

            string[] info = executeQuery(webAdress);

            if (info.Length == 1)
            {
                MessageBox.Show("cant find the location");
            }
            else
            {
            resultBlock.AppendText("Sunrise : " + info[0]);
            resultBlock.AppendText(Environment.NewLine);
            resultBlock.AppendText("Sunset : " + info[1]);
            }

        }

        //function which does the process of sending the query to the server ad getting back the result than store it in a table
        private string[] executeQuery(StringBuilder query)
        {
            string result = "";

            //executing the query using the webclient class
            using (WebClient wc = new WebClient())
            {
                result = wc.DownloadString(query.ToString());
            }
            
            //creating a jason object which contain th eresult of the query
            JObject jresult = JObject.Parse(result);

            try
            {
                //create tokens from the jason object, each contains an info
                JToken riseTime = jresult["query"]["results"]["channel"]["astronomy"]["sunrise"];
                JToken setTime = jresult["query"]["results"]["channel"]["astronomy"]["sunset"];
                string[] returnTab = {riseTime.ToString(), setTime.ToString()};

                return returnTab;
            }
            catch (Exception)
            {
                string[] returnTab = {"error"};

                return returnTab;
            }
        }
    }
}
