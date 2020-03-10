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

        char tab = 'F';
        SwissTransport.Transport transport = new SwissTransport.Transport();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void load(object sender, RoutedEventArgs e)
        {
        }

        private void findStationNames(object sender, RoutedEventArgs e)
        {
            string stationInput = station_input.Text;

            SwissTransport.Stations stationNames = transport.GetStations(stationInput);

            stationname_list.Items.Clear();

            foreach(SwissTransport.Station station in stationNames.StationList)
            {
                stationname_list.Items.Add(station.Name);
            }

        }
    }
}
