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
    public partial class MainWindow : Window
    {

        SwissTransport.Transport transport = new SwissTransport.Transport();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void load(object sender, RoutedEventArgs e)
        {
        }

        private void textChangeEvent(object sender, TextChangedEventArgs e)
        {
            switch(e.Source as FrameworkElement)
            {
                default:
                    fp_list.Items.Add((e.Source as FrameworkElement).Name);
                    break;
            }
        }

        private void fp_InputChange(object sender, TextChangedEventArgs e)
        {
            if(fp_inp_start.Text.Length == 0 || fp_inp_end.Text.Length == 0)
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
            string stationStart = fp_inp_start.Text;
            string stationEnd = fp_inp_end.Text;

            SwissTransport.Connections connections = transport.GetConnections(stationStart, stationEnd);

            fp_list.Items.Clear();

            foreach(SwissTransport.Connection connection in connections.ConnectionList)
            {
                fp_list.Items.Add(connection.From.Station.Name);
            }
        }

        private void abf_InputChange(object sender, TextChangedEventArgs e)
        {
            if (abf_inp_name.Text.Length == 0)
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
            string station = abf_inp_name.Text;
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

        private void sta_InputChange(object sender, TextChangedEventArgs e)
        {
            if (sta_inp_name.Text.Length == 0)
            {
                sta_bnt_searchSta.IsEnabled = false;
            }
            else
            {
                sta_bnt_searchSta.IsEnabled = true;
            }
        } 

        private void sta_findStations(object sender, RoutedEventArgs e)
        {
            string stationInput = sta_inp_name.Text;

            SwissTransport.Stations stationNames = transport.GetStations(stationInput);

            sta_list_stationnames.Items.Clear();

            foreach (SwissTransport.Station station in stationNames.StationList)
            {
                sta_list_stationnames.Items.Add(station.Name);
            }

        }
    }
}
