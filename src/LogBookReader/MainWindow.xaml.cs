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

namespace LogBookReader
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ButtpnConnectToLog_Click(object sender, RoutedEventArgs e)
        {
            EF.ReaderContext readerContext = new EF.ReaderContext();

            var repoAppCodes = new EF.Repository<Models.AppCodes>(readerContext);
            List<Models.AppCodes> appCodes = await repoAppCodes.GetListAsync();

            var repoEventLogs = new EF.Repository<Models.EventLog>(readerContext);
            List<Models.EventLog> eventLog = await repoEventLogs.GetListAsync();

        }
    }
}
