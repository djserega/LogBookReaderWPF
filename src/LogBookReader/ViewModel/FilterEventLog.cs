using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Data;
using System.Linq.Expressions;

namespace LogBookReader.ViewModel
{
    public class FilterEventLog : DependencyObject
    {
        public FilterEventLog()
        {
            SetSource(new List<Filters.FilterEventLog>());
        }


        public async void GetEventLog(EF.ReaderContext readerContext, PropertyFilters propertyFilters, TimeSpan timeStart, TimeSpan timeEnd)
        {
            var filterExpression = GetExpressionFilterLogs(propertyFilters, timeStart, timeEnd);

            List<Filters.FilterEventLog> list = new List<Filters.FilterEventLog>();

            var repoEventLogs = new EF.Repository<Models.EventLog>(readerContext);
            List<Models.EventLog> eventLogs = await repoEventLogs.GetListTakeAsync(
                filterExpression,
                f => f.OrderBy(o => -o.RowID),
                propertyFilters.CountEventLogRows);

            foreach (Models.EventLog eventLog in eventLogs)
            {
                string appName = propertyFilters.FilterAppCodesBase.FirstOrDefault(f => f.Code == eventLog.AppCode)?.Name;
                string computerName = propertyFilters.FilterComputerCodesBase.FirstOrDefault(f => f.Code == eventLog.ComputerCode)?.Name;
                string userName = propertyFilters.FilterUserCodesBase.FirstOrDefault(f => f.Code == eventLog.UserCode)?.Name;
                string eventName = propertyFilters.FilterEventCodesBase.FirstOrDefault(f => f.Code == eventLog.EventCode)?.Name;

                list.Add(
                    new Filters.FilterEventLog(eventLog)
                    {
                        ComputerName = computerName,
                        AppName = appName,
                        UserName = userName,
                        EventName = eventName
                    });
            }

            SetSource(list);
        }

        public void SetSource(List<Filters.FilterEventLog> list)
        {
            FilterEventLogs = CollectionViewSource.GetDefaultView(list);
            FilterEventLogs.Filter = TextFilterObject;
        }

        public string TextFilter
        {
            get { return (string)GetValue(TextFilterProperty); }
            set { SetValue(TextFilterProperty, value); }
        }
        public static readonly DependencyProperty TextFilterProperty =
            DependencyProperty.Register("TextFilter", typeof(string), typeof(FilterEventLog), new PropertyMetadata("", TextFilter_Changed));

        private static void TextFilter_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FilterEventLog filterEventLog)
            {
                filterEventLog.FilterEventLogs.Filter = null;
                filterEventLog.FilterEventLogs.Filter = filterEventLog.TextFilterObject;
            }
        }

        private bool TextFilterObject(object obj)
        {
            bool result = true;

            if (!string.IsNullOrWhiteSpace(TextFilter)
                && obj is Filters.FilterEventLog eventLog)
            {
                string textFilterLower = TextFilter.ToLower();

                result = eventLog.RowID.ToString().ToLower().Contains(textFilterLower)
                    || (eventLog.ComputerName?.ToLower().Contains(textFilterLower) ?? false)
                    || (eventLog.AppName?.ToLower().Contains(textFilterLower) ?? false)
                    || (eventLog.Comment?.ToLower().Contains(textFilterLower) ?? false)
                    || (eventLog.UserName?.ToLower().Contains(textFilterLower) ?? false);
            }

            return result;
        }

        public ICollectionView FilterEventLogs
        {
            get { return (ICollectionView)GetValue(FilterEventLogProperty); }
            set { SetValue(FilterEventLogProperty, value); }
        }
      
        public static readonly DependencyProperty FilterEventLogProperty =
            DependencyProperty.Register("FilterEventLogs", typeof(ICollectionView), typeof(FilterEventLog));

        public ICommand ClearTextFilter
        {
            get => new DelegateCommand(() => { TextFilter = string.Empty; });
        }

        private Expression<Func<Models.EventLog, bool>> GetExpressionFilterLogs(PropertyFilters propertyFilters, TimeSpan timeStart, TimeSpan timeEnd)
        {
            ExpressionEventLogCreator expressionCreator = new ExpressionEventLogCreator();

            expressionCreator.FillExpression(propertyFilters,
                                             timeStart,
                                             timeEnd);

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

    }
}
