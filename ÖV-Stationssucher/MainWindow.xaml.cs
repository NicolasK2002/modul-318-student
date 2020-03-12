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

namespace ÖV_Stationssucher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        SwissTransport.Transport transport = new SwissTransport.Transport();
        List<string> sn = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void load(object sender, RoutedEventArgs e)
        {
            string time;
            for (int i = 0; i != 24; i++)
            {
                for (int ii = 0; ii != 60; ii++)
                {
                    time = "";
                    if (i < 10)
                        time += "0";
                    time += i.ToString() + ":";
                    if (ii < 10)
                        time += "0";
                    time += ii.ToString();
                    fp_cb_time.Items.Add(time);
                }
            }
        }

        private void stationRecommend(object sender, TextChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            string stationname = cb.Text;

            SwissTransport.Stations stationNames = transport.GetStations(stationname);

            cb.Items.Clear();

            foreach (SwissTransport.Station station in stationNames.StationList)
                cb.Items.Add(station.Name);

            if (cb.HasItems)
                cb.IsDropDownOpen = true;
            else
                cb.IsDropDownOpen = false;

            cb.Text = stationname;

            TextBox tb = (cb.Template.FindName("PART_EditableTextBox", cb) as TextBox);

            tb.SelectionLength = 0;

            string[] elementClass = (e.Source as FrameworkElement).Name.Split('_');

            switch (elementClass[0])
            {
                case "fp":
                    fp_InputChange();
                    break;
                case "abf":
                    abf_InputChange();
                    break;
            }
        }

        private void goTime(object sender, RoutedEventArgs e)
        {
            string stationStart = fp_cb_start.Text;
            string stationEnd = fp_cb_end.Text;
            byte isArrival = 0;
            string time = fp_list_arrival.Items[3].ToString();
            string date = fp_list_dateArrival.Items[3].ToString();
            if(((Button)sender).Name == "fp_bnt_goBack") {
                    isArrival = 1;
                    time = fp_list_departure.Items[0].ToString();
                    date = fp_list_dateDeparture.Items[0].ToString();
            }
            update_ListBox(stationStart, stationEnd, date, time, isArrival);
        }

        private void fp_InputChange(object sender = null, TextChangedEventArgs e = null)
        {
            if(fp_cb_start.Text.Length == 0 || fp_cb_end.Text.Length == 0 || fp_dp.Text.Length == 0 || fp_cb_time.Text.Length == 0)
                fp_bnt_findConnection.IsEnabled = false;
            else
                fp_bnt_findConnection.IsEnabled = true;
        }

        private void fp_searchConnection(object sender, RoutedEventArgs e)
        {
            string stationStart = fp_cb_start.Text;
            string stationEnd = fp_cb_end.Text;
            string date = accessDate(fp_dp.SelectedDate.ToString());
            string time = fp_cb_time.Text;
            bool isArrival = Convert.ToBoolean(fp_rb_arrival.IsChecked);

            update_ListBox(stationStart, stationEnd, date, time, Convert.ToByte(isArrival));

            fp_bnt_goBack.IsEnabled = true;
            fp_bnt_goForward.IsEnabled = true;
        }

        private void update_ListBox(string stationStart, string stationEnd, string date, string time, byte isArrival)
        {
            SwissTransport.Connections connections = transport.GetConnections(stationStart, stationEnd, date, time, isArrival.ToString());

            fp_list_from.Items.Clear();
            fp_list_to.Items.Clear();
            fp_list_departure.Items.Clear();
            fp_list_arrival.Items.Clear();
            fp_list_dateDeparture.Items.Clear();
            fp_list_dateArrival.Items.Clear();

            foreach (SwissTransport.Connection connection in connections.ConnectionList)
            {
                fp_list_from.Items.Add(connection.From.Station.Name);
                fp_list_to.Items.Add(connection.To.Station.Name);
                fp_list_departure.Items.Add(accessTime(connection.From.Departure));
                fp_list_arrival.Items.Add(accessTime(connection.To.Arrival));
                fp_list_dateDeparture.Items.Add(accessTime(connection.From.Departure, true));
                fp_list_dateArrival.Items.Add(accessTime(connection.To.Arrival, true));
            }
        }

        private string accessTime(string time, bool getDate = false)
        {
            string result = "";
            string[] timeDay = time.Split('T');
            string[] timeHours = timeDay[1].Split('+');
            string[] timeMinuts = timeHours[0].Split(':');
            if (getDate)
                result = timeDay[0];
            else
                result = timeMinuts[0] + ":" + timeMinuts[1];
            return result;
        }

        private string accessDate(string date)
        {
            string result = "";
            string[] timedate = date.Split(" ");
            string[] timeResult = timedate[0].Split(".");
            result = timeResult[2] + "-" + timeResult[1] + "-" + timeResult[0];
            return result;
        }

        private void abf_InputChange()
        {
            if (abf_cb_name.Text.Length == 0)
                abf_bnt_searchAbfahrt.IsEnabled = false;
            else
                abf_bnt_searchAbfahrt.IsEnabled = true;
        }

        private void abf_searchAbfahrt(object sender, RoutedEventArgs e)
        {
            string station = abf_cb_name.Text;
            string stationId;

            SwissTransport.Stations stationNames = transport.GetStations(station);

            stationId = stationNames.StationList[0].Id;

            abf_gb_abfahrt.Header = "Abfahrtsplan von " + stationNames.StationList[0].Name;

            SwissTransport.StationBoardRoot stationBoardRoot = transport.GetStationBoard(station, stationId);

            abf_list_abfahrt.Items.Clear();

            foreach(SwissTransport.StationBoard sbr in stationBoardRoot.Entries)
                abf_list_abfahrt.Items.Add(sbr.To);
        }

        private void st_findStations(object sender, TextChangedEventArgs e)
        {
            string stationInput = st_inp_name.Text;

            SwissTransport.Stations stationNames = transport.GetStations(stationInput);

            st_list_stationnames.Items.Clear();

            foreach (SwissTransport.Station station in stationNames.StationList)
                st_list_stationnames.Items.Add(station.Name);

        }
    }
}
