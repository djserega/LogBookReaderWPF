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

        private EF.ReaderContext readerContext = new EF.ReaderContext();

        public MainWindow()
        {
            InitializeComponent();
            InitializeProperties();
        }

        private async void InitializeProperties()
        {
            FilterAppCodes = new ObservableCollection<Filters.FilterAppCodes>();
            FilterComputerCodes = new ObservableCollection<Filters.FilterComputerCodes>();
            FilterEventCodes = new ObservableCollection<Filters.FilterEventCodes>();

            await GetFilterData();
        }

        #region DependencyProperty

        public ObservableCollection<Filters.FilterAppCodes> FilterAppCodes
        {
            get { return (ObservableCollection<Filters.FilterAppCodes>)GetValue(FilterAppCodesProperty); }
            set { SetValue(FilterAppCodesProperty, value); }
        }
        public static readonly DependencyProperty FilterAppCodesProperty =
         DependencyProperty.Register("FilterAppCodes", typeof(ObservableCollection<Filters.FilterAppCodes>), typeof(MainWindow), new UIPropertyMetadata(null));

        public ObservableCollection<Filters.FilterComputerCodes> FilterComputerCodes
        {
            get { return (ObservableCollection<Filters.FilterComputerCodes>)GetValue(FiltersComputerCodesProperty); }
            set { SetValue(FiltersComputerCodesProperty, value); }
        }
        public static readonly DependencyProperty FiltersComputerCodesProperty =
            DependencyProperty.Register("FilterComputerCodes", typeof(ObservableCollection<Filters.FilterComputerCodes>), typeof(MainWindow), new UIPropertyMetadata(null));

        public ObservableCollection<Filters.FilterEventCodes> FilterEventCodes
        {
            get { return (ObservableCollection<Filters.FilterEventCodes>)GetValue(FilterEventCodesProperty); }
            set { SetValue(FilterEventCodesProperty, value); }
        }
        public static readonly DependencyProperty FilterEventCodesProperty =
            DependencyProperty.Register("FilterEventCodes", typeof(ObservableCollection<Filters.FilterEventCodes>), typeof(MainWindow), new UIPropertyMetadata(null));

        #endregion

        private async void ButtpnGetFilterData_Click(object sender, RoutedEventArgs e)
        {
            await GetFilterData();
        }

        private async Task GetFilterData()
        {
            try
            {
                await FillDataAppCodes();
                await FillDataComputerCodes();
                await FillDataEventCodes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.InnerException?.Message + "\n" + ex.InnerException?.InnerException?.Message);
                return;
            }
        }

        private async Task FillDataAppCodes(bool isChecked = true)
        {
            FilterAppCodes.Clear();

            var repoAppCodes = new EF.Repository<Models.AppCodes>(readerContext);
            List<Models.AppCodes> appCodes = await repoAppCodes.GetListAsync();
            foreach (Models.AppCodes item in appCodes)
                FilterAppCodes.Add(new Filters.FilterAppCodes(item) { IsChecked = isChecked });
        }

        private async Task FillDataComputerCodes(bool isChecked = true)
        {
            FilterComputerCodes.Clear();

            var repoComputerCodes = new EF.Repository<Models.ComputerCodes>(readerContext);
            List<Models.ComputerCodes> computerCodes = await repoComputerCodes.GetListAsync();
            foreach (Models.ComputerCodes item in computerCodes)
                FilterComputerCodes.Add(new Filters.FilterComputerCodes(item) { IsChecked = isChecked });
        }

        private async Task FillDataEventCodes(bool isChecked = true)
        {
            FilterEventCodes.Clear();

            var repoEventCodes = new EF.Repository<Models.EventCodes>(readerContext);
            List<Models.EventCodes> eventCodes = await repoEventCodes.GetListAsync();
            foreach (Models.EventCodes item in eventCodes)
                FilterEventCodes.Add(new Filters.FilterEventCodes(item) { IsChecked = isChecked });
        }

        private async void MenuItemCommandBarFilter_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                string[] dataTag = ((string)menuItem.Tag).Split('/');

                bool isChecked = dataTag[1] == "Marked";

                switch (dataTag[0])
                {
                    case "FilterAppCodes":
                        await FillDataAppCodes(isChecked);
                        break;
                    case "FilterComputerCodes":
                        await FillDataComputerCodes(isChecked);
                        break;
                    case "FilterEventCodes":
                        await FillDataEventCodes(isChecked);
                        break;
                }
            }
        }
    }
}
