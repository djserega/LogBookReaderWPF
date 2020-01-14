using LogBookReader.Additions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace LogBookReader
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string _mainTitle;
        private EF.ReaderContext _readerContext;
        private readonly List<Filters.FilterEventLog> _filterEventLogsBase = new List<Filters.FilterEventLog>();

        private ViewModel.FilterEventLog _filterEventLogViewModel;
        private ViewModel.PropertyFilters _propertyFiltersViewModel;

        public MainWindow()
        {
            InitializeComponent();

            _mainTitle = Title;

            try
            {
                new EF.Initialize().FindCreateConnectionFile();
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось инициализировать объект подключения." +
                    "\nПроверьте наличие файла 'dbConnection.config'.");
            }

            InitializeReaderContext();
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
            _propertyFiltersViewModel = new ViewModel.PropertyFilters();
            GridPropertyFilters.DataContext = _propertyFiltersViewModel;

            _filterEventLogViewModel = new ViewModel.FilterEventLog();
            GridEventLogs.DataContext = _filterEventLogViewModel;
            GridFilterEventLog.DataContext = _filterEventLogViewModel;

            CountEventLogRows = 100;

            StartPeriodDate = DateTime.Now;
            TimeControlStartPeriod.Value = new TimeSpan(0, 0, 0);
            EndPeriodDate = DateTime.Now;
            TimeControlEndPeriod.Value = new TimeSpan(23, 59, 59);

            GetDataDB(initializeReaderContext: false);
        }

        #region Dependency property


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

        private void InitializeReaderContext()
        {
            try
            {
                _readerContext = new EF.ReaderContext();
                Title = _mainTitle;
                Title += " - " + GetDataSource(_readerContext.Database.Connection.ConnectionString);
            }
            catch (FileNotFoundException ex)
            {
                string message = "Не найден файл логов." +
                    "\nСкопируйте файл 1Cv8.lgd в каталог приложения.";

#if DEBUG
                message += "\n" + ex.Message;
#endif

                MessageBox.Show(message);
            }
        }

        private void ButtonGetFilterData_Click(object sender, RoutedEventArgs e)
        {
            if (IsLoadingEventLog)
                return;

            GetDataDB(true);
        }

        private void GetDataDB(bool readEventLog = false, bool initializeReaderContext = true)
        {
            if (_readerContext == null)
            {
                if (initializeReaderContext)
                {
                    InitializeReaderContext();
                    GetDataDB(initializeReaderContext: false);
                    GetDataDB(readEventLog, false);
                }
                return;
            }

            try
            {
                if (readEventLog)
                    FillDataEventLogs();
                else
                {
                    _propertyFiltersViewModel.Fill(_readerContext);
                    StartPeriodDate = _propertyFiltersViewModel.StartPeriodDate;
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

        private void MenuItemCommandBarFilter_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                string[] dataTag = ((string)menuItem.Tag).Split('/');

                bool isChecked = dataTag[1] == "Marked";

                switch (dataTag[0])
                {
                    case "FilterAppCodes":
                        _propertyFiltersViewModel.FillDataAppCodes(isChecked);
                        break;
                    case "FilterComputerCodes":
                        _propertyFiltersViewModel.FillDataComputerCodes(isChecked);
                        break;
                    case "FilterEventCodes":
                        _propertyFiltersViewModel.FillDataEventCodes(isChecked);
                        break;
                    case "FilterUserCodes":
                        _propertyFiltersViewModel.FillDataUserCodes(isChecked);
                        break;

                }
            }
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
                string appName = _propertyFiltersViewModel.FilterAppCodesBase.FirstOrDefault(f => f.Code == eventLog.AppCode)?.Name;
                string computerName = _propertyFiltersViewModel.FilterComputerCodesBase.FirstOrDefault(f => f.Code == eventLog.ComputerCode)?.Name;
                string userName = _propertyFiltersViewModel.FilterUserCodesBase.FirstOrDefault(f => f.Code == eventLog.UserCode)?.Name;
                string eventName = _propertyFiltersViewModel.FilterEventCodesBase.FirstOrDefault(f => f.Code == eventLog.EventCode)?.Name;

                _filterEventLogsBase.Add(
                    new Filters.FilterEventLog(eventLog)
                    {
                        ComputerName = computerName,
                        AppName = appName,
                        UserName = userName,
                        EventName = eventName
                    });
            }

            _filterEventLogViewModel.SetSource(_filterEventLogsBase);
        }

        private Expression<Func<Models.EventLog, bool>> GetExpressionFilterLogs()
        {
            ExpressionEventLogCreator expressionCreator = new ExpressionEventLogCreator();

            expressionCreator.FillExpression(_propertyFiltersViewModel);

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
            long dateSqlite = date.DateToSQLite();
            dateSqlite += (long)time.TotalMilliseconds * 10;

            return dateSqlite;
        }

        private void ButtonClearTextFilter_Click(object sender, RoutedEventArgs e)
        {
            _filterEventLogViewModel.TextFilter = "";
        }

        private void ButtonSelectLogBookFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                AddExtension = false,
                CheckFileExists = true,
                DefaultExt = "*.lgd",
                Filter = "Файл журнала регистрации (*.lgd)|*.lgd",
                Multiselect = false
            };
            fileDialog.ShowDialog(this);
            string logBookFileName = fileDialog.FileName;
            if (!string.IsNullOrEmpty(logBookFileName))
            {
                if (new EF.Initialize().ChangeDataSourceConfigSQLite(logBookFileName))
                {
                    Process.Start(new ProcessStartInfo(Assembly.GetExecutingAssembly().Location));
                    Application.Current.Shutdown();

                }
                else
                {
                    MessageBox.Show("Не удалось изменить источник данных.");
                }
            }
        }

        private string GetDataSource(string connectioString)
        {
            string result = string.Empty;

            string startPositionText = "Data Source";
            int startPosition = connectioString.IndexOf(startPositionText);
            if (startPosition >= 0)
            {
                startPosition += startPositionText.Length;

                int endPosition = connectioString.IndexOf(";", startPosition);
                if (endPosition > 0)
                {
                    result = connectioString.Substring(startPosition + 1, endPosition - startPosition - 1);
                }
            }

            return result;
        }

    }
}
