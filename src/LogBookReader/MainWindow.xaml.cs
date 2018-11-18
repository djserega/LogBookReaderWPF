using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
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

        private EF.ReaderContext _readerContext = new EF.ReaderContext();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeProperties();
        }

        private async void InitializeProperties()
        {
            FilterAppCodes = new ObservableCollection<Filters.FilterAppCodes>();
            FilterComputerCodes = new ObservableCollection<Filters.FilterComputerCodes>();
            FilterEventCodes = new ObservableCollection<Filters.FilterEventCodes>();
            FilterEventLogs = new ObservableCollection<Filters.FilterEventLog>();

            CountEventLogRows = 100;

            await GetDataDB();
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

        public ObservableCollection<Filters.FilterEventLog> FilterEventLogs
        {
            get { return (ObservableCollection<Filters.FilterEventLog>)GetValue(FilterEventLogProperty); }
            set { SetValue(FilterEventLogProperty, value); }
        }
        public static readonly DependencyProperty FilterEventLogProperty =
            DependencyProperty.Register("FilterEventLogs", typeof(ObservableCollection<Filters.FilterEventLog>), typeof(MainWindow), new UIPropertyMetadata(null));

        public int CountEventLogRows
        {
            get { return (int)GetValue(CountEventLogRowsProperty); }
            set { SetValue(CountEventLogRowsProperty, value); }
        }
        public static readonly DependencyProperty CountEventLogRowsProperty =
            DependencyProperty.Register("CountEventLogRows", typeof(int), typeof(MainWindow), new UIPropertyMetadata(null));

        #endregion

        private async void ButtonGetFilterData_Click(object sender, RoutedEventArgs e)
        {
            await GetDataDB(true);
        }

        private async Task GetDataDB(bool readEventLog = false)
        {
            try
            {
                if (readEventLog)
                    await FillDataEventLogs();
                else
                {
                    await FillDataAppCodes();
                    await FillDataComputerCodes();
                    await FillDataEventCodes();
                }
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

            var repoAppCodes = new EF.Repository<Models.AppCodes>(_readerContext);
            List<Models.AppCodes> appCodes = await repoAppCodes.GetListAsync();
            foreach (Models.AppCodes item in appCodes.OrderBy(f => f.Name))
                FilterAppCodes.Add(new Filters.FilterAppCodes(item) { IsChecked = isChecked });
        }

        private async Task FillDataComputerCodes(bool isChecked = true)
        {
            FilterComputerCodes.Clear();

            var repoComputerCodes = new EF.Repository<Models.ComputerCodes>(_readerContext);
            List<Models.ComputerCodes> computerCodes = await repoComputerCodes.GetListAsync();
            foreach (Models.ComputerCodes item in computerCodes.OrderBy(f => f.Name))
                FilterComputerCodes.Add(new Filters.FilterComputerCodes(item) { IsChecked = isChecked });
        }

        private async Task FillDataEventCodes(bool isChecked = true)
        {
            FilterEventCodes.Clear();

            var repoEventCodes = new EF.Repository<Models.EventCodes>(_readerContext);
            List<Models.EventCodes> eventCodes = await repoEventCodes.GetListAsync();
            foreach (Models.EventCodes item in eventCodes.OrderBy(f => f.Name))
                FilterEventCodes.Add(new Filters.FilterEventCodes(item) { IsChecked = isChecked });
        }

        private async Task FillDataEventLogs()
        {
            FilterEventLogs.Clear();

            var repoEventLogs = new EF.Repository<Models.EventLog>(_readerContext);
            List<Models.EventLog> eventLogs = await repoEventLogs.GetListTakeAsync(
                GetExpressionFilterLogs(),
                orderBy: f => f.OrderBy(o => -o.RowID),
                count: CountEventLogRows);


            int countEvents = eventLogs.Count;

            int maxRows = Math.Min(CountEventLogRows, countEvents);
            for (int i = 0; i < maxRows; i++)
            {
                string appName = FilterAppCodes.FirstOrDefault(f => f.Code == eventLogs[i].AppCode && f.IsChecked)?.Name;
                string computerName = FilterComputerCodes.FirstOrDefault(f => f.Code == eventLogs[i].ComputerCode && f.IsChecked)?.Name;

                if (string.IsNullOrEmpty(appName)
                    || string.IsNullOrEmpty(computerName))
                {
                    if (maxRows < countEvents)
                        maxRows++;
                }
                else
                {
                    FilterEventLogs.Add(
                        new Filters.FilterEventLog(eventLogs[i])
                        {
                            ComputerName = computerName,
                            AppName = appName
                        });
                }
            }
        }

        private Expression<Func<Models.EventLog, bool>> GetExpressionFilterLogs()
        {
            Expression<Func<Models.EventLog, bool>> expression = f => f.ComputerCode == 0;

            return expression;
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
