using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace LogBookReader
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly EF.ReaderContext _readerContext;
        private readonly List<Filters.FilterEventLog> _filterEventLogsBase = new List<Filters.FilterEventLog>();

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                new EF.Initialize().FindCreateConnectionFile();
                _readerContext = new EF.ReaderContext();
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось инициализировать объект подключения." +
                    "\nПроверьте наличие файла 'dbConnection.config'.");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeProperties();

            Events.ChangeIsLoadingEventLogEvents.ChangeIsLoadingEventLog += (bool newValue) => 
            {
                Dispatcher.Invoke(new ThreadStart(delegate { IsLoadingEventLog = newValue; }));
            };
        }

        private void InitializeProperties()
        {
            FilterAppCodes = new ObservableCollection<Filters.FilterAppCodes>();
            FilterComputerCodes = new ObservableCollection<Filters.FilterComputerCodes>();
            FilterEventCodes = new ObservableCollection<Filters.FilterEventCodes>();
            FilterEventLogs = new ObservableCollection<Filters.FilterEventLog>();

            CountEventLogRows = 100;

            StartPeriodDate = DateTime.Now;
            TimeControlStartPeriod.Value = new TimeSpan(0, 0, 0);
            EndPeriodDate = DateTime.Now;
            TimeControlEndPeriod.Value = new TimeSpan(23, 59, 59);

            GetDataDB();
        }

     
        #region Dependency property

        public ObservableCollection<Filters.FilterAppCodes> FilterAppCodes
        {
            get { return (ObservableCollection<Filters.FilterAppCodes>)GetValue(FilterAppCodesProperty); }
            set { SetValue(FilterAppCodesProperty, value); }
        }
        public static readonly DependencyProperty FilterAppCodesProperty =
         DependencyProperty.Register("FilterAppCodes", typeof(ObservableCollection<Filters.FilterAppCodes>), typeof(MainWindow));

        public ObservableCollection<Filters.FilterComputerCodes> FilterComputerCodes
        {
            get { return (ObservableCollection<Filters.FilterComputerCodes>)GetValue(FiltersComputerCodesProperty); }
            set { SetValue(FiltersComputerCodesProperty, value); }
        }
        public static readonly DependencyProperty FiltersComputerCodesProperty =
            DependencyProperty.Register("FilterComputerCodes", typeof(ObservableCollection<Filters.FilterComputerCodes>), typeof(MainWindow));

        public ObservableCollection<Filters.FilterEventCodes> FilterEventCodes
        {
            get { return (ObservableCollection<Filters.FilterEventCodes>)GetValue(FilterEventCodesProperty); }
            set { SetValue(FilterEventCodesProperty, value); }
        }
        public static readonly DependencyProperty FilterEventCodesProperty =
            DependencyProperty.Register("FilterEventCodes", typeof(ObservableCollection<Filters.FilterEventCodes>), typeof(MainWindow));

        public ObservableCollection<Filters.FilterEventLog> FilterEventLogs
        {
            get { return (ObservableCollection<Filters.FilterEventLog>)GetValue(FilterEventLogProperty); }
            set { SetValue(FilterEventLogProperty, value); }
        }
        public static readonly DependencyProperty FilterEventLogProperty =
            DependencyProperty.Register("FilterEventLogs", typeof(ObservableCollection<Filters.FilterEventLog>), typeof(MainWindow));

        public int CountEventLogRows
        {
            get { return (int)GetValue(CountEventLogRowsProperty); }
            set { SetValue(CountEventLogRowsProperty, value); }
        }
        public static readonly DependencyProperty CountEventLogRowsProperty =
            DependencyProperty.Register("CountEventLogRows", typeof(int), typeof(MainWindow));

        public bool CommentIsFilled
        {
            get { return (bool)GetValue(CommentIsFilledProperty); }
            set { SetValue(CommentIsFilledProperty, value); }
        }
        public static readonly DependencyProperty CommentIsFilledProperty =
            DependencyProperty.Register("CommentIsFilled", typeof(bool), typeof(MainWindow));

        public string TextFilter
        {
            get { return (string)GetValue(TextFilterProperty); }
            set { SetValue(TextFilterProperty, value); }
        }
        public static readonly DependencyProperty TextFilterProperty =
            DependencyProperty.Register("TextFilter", typeof(string), typeof(MainWindow));

        public DateTime StartPeriodDate
        {
            get { return (DateTime)GetValue(StartPeriodDateProperty); }
            set { SetValue(StartPeriodDateProperty, value); }
        }
        public static readonly DependencyProperty StartPeriodDateProperty =
            DependencyProperty.Register("StartPeriodDate", typeof(DateTime), typeof(MainWindow));

        public TimeSpan StartPeriodTime
        {
            get { return (TimeSpan)GetValue(StartPeriodTimeProperty); }
            set { SetValue(StartPeriodTimeProperty, value); }
        }
        public static readonly DependencyProperty StartPeriodTimeProperty =
            DependencyProperty.Register("StartPeriodTime", typeof(TimeSpan), typeof(MainWindow));

        public DateTime EndPeriodDate
        {
            get { return (DateTime)GetValue(EndPeriodDateProperty); }
            set { SetValue(EndPeriodDateProperty, value); }
        }
        public static readonly DependencyProperty EndPeriodDateProperty =
            DependencyProperty.Register("EndPeriodDate", typeof(DateTime), typeof(MainWindow));

        public TimeSpan EndPeriodTime
        {
            get { return (TimeSpan)GetValue(EndPeriodTimeProperty); }
            set { SetValue(EndPeriodTimeProperty, value); }
        }
        public static readonly DependencyProperty EndPeriodTimeProperty =
            DependencyProperty.Register("EndPeriodTime", typeof(TimeSpan), typeof(MainWindow));

        public bool IsLoadingEventLog
        {
            get { return (bool)GetValue(IsLoadingEventLogProperty); }
            set { SetValue(IsLoadingEventLogProperty, value); }
        }
        public static readonly DependencyProperty IsLoadingEventLogProperty =
            DependencyProperty.Register("IsLoadingEventLog", typeof(bool), typeof(MainWindow));

        #endregion

        private void ButtonGetFilterData_Click(object sender, RoutedEventArgs e)
        {
            if (IsLoadingEventLog)
                return;

            GetDataDB(true);
        }

        private void GetDataDB(bool readEventLog = false)
        {
            if (_readerContext == null)
                return;

            try
            {
                if (readEventLog)
                    FillDataEventLogs();
                else
                {
                    FillDataAppCodes();
                    FillDataComputerCodes();
                    FillDataEventCodes();
                }
            }
            catch (EntityCommandExecutionException ex)
            {
                MessageBox.Show("Ошибка подключения к базе данных.\n" + ex.Message + "\n" + ex.InnerException?.Message + "\n" + ex.InnerException?.InnerException?.Message);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.InnerException?.Message + "\n" + ex.InnerException?.InnerException?.Message);
                return;
            }
        }

        private async void FillDataAppCodes(bool isChecked = true)
        {
            FilterAppCodes.Clear();

            var repoAppCodes = new EF.Repository<Models.AppCodes>(_readerContext);
         
            List<Models.AppCodes> appCodes = await repoAppCodes.GetListAsync();
            
            foreach (Models.AppCodes item in appCodes.OrderBy(f => f.Name))
                FilterAppCodes.Add(new Filters.FilterAppCodes(item) { IsChecked = isChecked });
        }

        private async void FillDataComputerCodes(bool isChecked = true)
        {
            FilterComputerCodes.Clear();

            var repoComputerCodes = new EF.Repository<Models.ComputerCodes>(_readerContext);
           
            List<Models.ComputerCodes> computerCodes = await repoComputerCodes.GetListAsync();
            
            foreach (Models.ComputerCodes item in computerCodes.OrderBy(f => f.Name))
                FilterComputerCodes.Add(new Filters.FilterComputerCodes(item) { IsChecked = isChecked });
        }

        private async void FillDataEventCodes(bool isChecked = true)
        {
            FilterEventCodes.Clear();

            var repoEventCodes = new EF.Repository<Models.EventCodes>(_readerContext);
            
            List<Models.EventCodes> eventCodes = await repoEventCodes.GetListAsync();

            foreach (Models.EventCodes item in eventCodes.OrderBy(f => f.Name))
                FilterEventCodes.Add(new Filters.FilterEventCodes(item) { IsChecked = isChecked });
        }

        private async void FillDataEventLogs()
        {
            _filterEventLogsBase.Clear();

            var repoEventLogs = new EF.Repository<Models.EventLog>(_readerContext);
            List<Models.EventLog> eventLogs = await repoEventLogs.GetListTakeAsync(
                GetExpressionFilterLogs(),
                f => f.OrderBy(o => -o.RowID),
                CountEventLogRows);

            foreach (Models.EventLog eventLog in eventLogs)
            {
                string appName = FilterAppCodes.FirstOrDefault(f => f.Code == eventLog.AppCode)?.Name;
                string computerName = FilterComputerCodes.FirstOrDefault(f => f.Code == eventLog.ComputerCode)?.Name;

                _filterEventLogsBase.Add(
                    new Filters.FilterEventLog(eventLog)
                    {
                        ComputerName = computerName,
                        AppName = appName
                    });
            }

            FilterEventLogs = new ObservableCollection<Filters.FilterEventLog>(_filterEventLogsBase);
        }

        private Expression<Func<Models.EventLog, bool>> GetExpressionFilterLogs()
        {
            ExpressionEventLogCreator expressionCreator = new ExpressionEventLogCreator();

            expressionCreator.AddExpression(FilterAppCodes, "AppCode");
            expressionCreator.AddExpression(FilterComputerCodes, "ComputerCode");
            expressionCreator.AddExpression(FilterEventCodes, "EventCode");

            if (StartPeriodDate.Date != new DateTime(1, 1, 1))
            {
                expressionCreator.AddExpression("Date",
                                                ComparsionType.GreaterThanOrEqual,
                                                GetDateSQLite(StartPeriodDate.Date, TimeControlStartPeriod.Value));
            }

            if (EndPeriodDate.Date != new DateTime(1, 1, 1))
            {
                expressionCreator.AddExpression("Date",
                                                ComparsionType.LessThanOrEqual,
                                                GetDateSQLite(EndPeriodDate.Date, TimeControlEndPeriod.Value));
            }

            if (CommentIsFilled)
                expressionCreator.AddExpression("Comment", ComparsionType.NotEqual, "");

            try
            {
                return expressionCreator.GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private static long GetDateSQLite(DateTime date, TimeSpan time)
        {
            long dateSqlite = (long)(date - DateTime.MinValue).TotalMilliseconds * 10;
            dateSqlite += (long)time.TotalMilliseconds * 10;
            
            return dateSqlite;
        }

        private void MenuItemCommandBarFilter_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                string[] dataTag = ((string)menuItem.Tag).Split('/');

                bool isChecked = dataTag[1] == "Marked";

                switch (dataTag[0])
                {
                    case "FilterAppCodes":
                        FillDataAppCodes(isChecked);
                        break;
                    case "FilterComputerCodes":
                        FillDataComputerCodes(isChecked);
                        break;
                    case "FilterEventCodes":
                        FillDataEventCodes(isChecked);
                        break;
                }
            }
        }

        private void ButtonClearTextFilter_Click(object sender, RoutedEventArgs e)
        {
            TextFilter = string.Empty;

            FilterListEventLog();
        }

        private void TextBoxTextFilter_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            FilterListEventLog();
        }

        private void FilterListEventLog()
        {
            if (string.IsNullOrWhiteSpace(TextFilter))
                FilterEventLogs = new ObservableCollection<Filters.FilterEventLog>(_filterEventLogsBase);
            else
            {
                string textFilterLower = TextFilter.ToLower();

                FilterEventLogs = new ObservableCollection<Filters.FilterEventLog>(_filterEventLogsBase?.Where(
                    el =>
                        (el.RowID.ToString().ToLower().Contains(textFilterLower))
                        || (el.ComputerName?.ToLower().Contains(textFilterLower) ?? false)
                        || (el.AppName?.ToLower().Contains(textFilterLower) ?? false)
                        || (el.Comment?.ToLower().Contains(textFilterLower) ?? false)
                    ));

            }
        }
    }
}
