using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EventManagerClient.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateEvent_Click(object sender, RoutedEventArgs e)
        {
            //CreateEventWindow createEventWindow = new CreateEventWindow();
            //createEventWindow.ShowDialog();
        }

        private void ViewEvents_Click(object sender, RoutedEventArgs e)
        {
            //ViewEventsWindow viewEventsWindow = new ViewEventsWindow();
            //viewEventsWindow.ShowDialog();
        }
    }
}

