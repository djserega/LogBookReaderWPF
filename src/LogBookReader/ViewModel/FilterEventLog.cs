using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LogBookReader.ViewModel
{
    public class FilterEventLog : DependencyObject
    {

        public FilterEventLog()
        {
            SetSource(new List<Filters.FilterEventLog>());
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

    }
}
