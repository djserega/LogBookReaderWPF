using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
            InitializeProperties();
        }

        private void InitializeProperties()
        {
            FilterAppCodes = new ObservableCollection<Filters.FiltersAppCodes>();
            FiltersComputerCodes = new ObservableCollection<Filters.FiltersComputerCodes>();
        }

        public ObservableCollection<Filters.FiltersAppCodes> FilterAppCodes
        {
            get { return (ObservableCollection<Filters.FiltersAppCodes>)GetValue(FilterAppCodesProperty); }
            set { SetValue(FilterAppCodesProperty, value); }
        }
        public static readonly DependencyProperty FilterAppCodesProperty =
         DependencyProperty.Register("FilterAppCodes", typeof(ObservableCollection<Filters.FiltersAppCodes>), typeof(MainWindow), new UIPropertyMetadata(null));

        public ObservableCollection<Filters.FiltersComputerCodes> FiltersComputerCodes
        {
            get { return (ObservableCollection<Filters.FiltersComputerCodes>)GetValue(FiltersComputerCodesProperty); }
            set { SetValue(FiltersComputerCodesProperty, value); }
        }
        public static readonly DependencyProperty FiltersComputerCodesProperty =
            DependencyProperty.Register("FiltersComputerCodes", typeof(ObservableCollection<Filters.FiltersComputerCodes>), typeof(MainWindow), new UIPropertyMetadata(null));


        private async void ButtpnGetFilterData_Click(object sender, RoutedEventArgs e)
        {
            FilterAppCodes.Clear();

            EF.ReaderContext readerContext = new EF.ReaderContext();

            try
            {
                await FillDataAppCodes(readerContext);
                await FillDataComputerCodes(readerContext);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.InnerException?.Message + "\n" + ex.InnerException?.InnerException?.Message);
                return;
            }
        }

        private async Task FillDataAppCodes(EF.ReaderContext readerContext)
        {
            var repoAppCodes = new EF.Repository<Models.AppCodes>(readerContext);
            List<Models.AppCodes> appCodes = await repoAppCodes.GetListAsync();
            foreach (Models.AppCodes item in appCodes)
                FilterAppCodes.Add(new Filters.FiltersAppCodes(item) { IsChecked = true });
        }

        private async Task FillDataComputerCodes(EF.ReaderContext readerContext)
        {
            var repoComputerCodes = new EF.Repository<Models.ComputerCodes>(readerContext);
            List<Models.ComputerCodes> computerCodes = await repoComputerCodes.GetListAsync();
            foreach (Models.ComputerCodes item in computerCodes)
                FiltersComputerCodes.Add(new Filters.FiltersComputerCodes(item) { IsChecked = true });
        }
    }
}
