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
        }

        private void stationRecommend(object sender, TextChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            string stationname = cb.Text;

            SwissTransport.Stations stationNames = transport.GetStations(stationname);

            cb.Items.Clear();

            foreach (SwissTransport.Station station in stationNames.StationList)
            {
                cb.Items.Add(station.Name);
            }

            if (cb.HasItems)
            {
                cb.IsDropDownOpen = true;
            }
            else
            {
                cb.IsDropDownOpen = false;
            }

            cb.Text = stationname;

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

        private void fp_InputChange()
        {
            if(fp_cb_start.Text.Length == 0 || fp_cb_end.Text.Length == 0)
            {
                fp_bnt_findConnection.IsEnabled = false;
            }
            else
            {
                fp_bnt_findConnection.IsEnabled = true;
            }
        }

        private void fp_searchConnection(object sender, RoutedEventArgs e)
        {
            string stationStart = fp_cb_start.Text;
            string stationEnd = fp_cb_end.Text;

            SwissTransport.Connections connections = transport.GetConnections(stationStart, stationEnd);

            fp_list.Items.Clear();

            foreach(SwissTransport.Connection connection in connections.ConnectionList)
            {
                fp_list.Items.Add(connection.From.Station.Name);
            }
        }

        private void abf_InputChange()
        {
            if (abf_cb_name.Text.Length == 0)
            {
                abf_bnt_searchAbfahrt.IsEnabled = false;
            }
            else
            {
                abf_bnt_searchAbfahrt.IsEnabled = true;
            }
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
            {
                abf_list_abfahrt.Items.Add(sbr.To);
            }
        }

        private void st_findStations(object sender, TextChangedEventArgs e)
        {
            string stationInput = st_inp_name.Text;

            SwissTransport.Stations stationNames = transport.GetStations(stationInput);

            st_list_stationnames.Items.Clear();

            foreach (SwissTransport.Station station in stationNames.StationList)
            {
                st_list_stationnames.Items.Add(station.Name);
            }

        }
    }
}
