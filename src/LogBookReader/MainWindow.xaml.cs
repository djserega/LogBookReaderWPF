using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Core;
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
using Expression = System.Linq.Expressions.Expression;

namespace LogBookReader
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly EF.ReaderContext _readerContext;

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
                MessageBox.Show("Не удалось инициализировать объект подключения.\nПроверьте наличие файла 'dbConnection.config'");
            }
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

        #endregion

        private async void ButtonGetFilterData_Click(object sender, RoutedEventArgs e)
        {
            await GetDataDB(true);
        }

        private async Task GetDataDB(bool readEventLog = false)
        {
            if (_readerContext == null)
                return;

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

            foreach (Models.EventLog eventLog in eventLogs)
            {
                string appName = FilterAppCodes.FirstOrDefault(f => f.Code == eventLog.AppCode)?.Name;
                string computerName = FilterComputerCodes.FirstOrDefault(f => f.Code == eventLog.ComputerCode)?.Name;

                FilterEventLogs.Add(
                    new Filters.FilterEventLog(eventLog)
                    {
                        ComputerName = computerName,
                        AppName = appName
                    });
            }
        }

        private Expression<Func<Models.EventLog, bool>> GetExpressionFilterLogs()
        {
            Expression result = null;

            ParameterExpression parameter = Expression.Parameter(typeof(Models.EventLog));

            AddExpression(parameter, ref result, FilterAppCodes, "AppCode");
            AddExpression(parameter, ref result, FilterComputerCodes, "ComputerCode");
            AddExpression(parameter, ref result, FilterEventCodes, "EventCode");
            AddExpression<IModels.IFilterBase>(parameter, ref result, null, "Comment");

            if (result == null)
                return null;

            Expression<Func<Models.EventLog, bool>> expression = null;

            try
            {
                expression = Expression.Lambda<Func<Models.EventLog, bool>>(result, parameter);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return expression;
        }

        private void AddExpression<T>(ParameterExpression parameter,
                                      ref Expression result,
                                      ObservableCollection<T> listData,
                                      string field) where T : IModels.IFilterBase
        {
            Expression resultExpression = null;

            if (field == "Comment")
            {
                if (CommentIsFilled)
                    resultExpression = Expression.NotEqual(Expression.Property(parameter, "Comment"), Expression.Constant(""));
            }
            else
            {
                int countCheckedElement = listData.Count(f => f.IsChecked);
                if (countCheckedElement > 0 && countCheckedElement != listData.Count)
                {
                    foreach (T item in listData.Where(f => f.IsChecked))
                    {
                        Expression currentExpression = Expression.Equal(Expression.Property(parameter, field), Expression.Constant(item.Code));

                        if (result == null)
                            resultExpression = currentExpression;
                        else
                            resultExpression = Expression.Or(result, currentExpression);
                    }
                }
            }

            if (resultExpression == null)
                return;

            if (result == null)
                result = resultExpression;
            else
                result = Expression.AndAlso(result, resultExpression);
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
