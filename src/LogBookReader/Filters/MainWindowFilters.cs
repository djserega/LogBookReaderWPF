using LogBookReader.Additions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LogBookReader
{
    public partial class MainWindow : Window
    {
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

        public ObservableCollection<Filters.FilterUserCodes> FilterUserCodes
        {
            get { return (ObservableCollection<Filters.FilterUserCodes>)GetValue(FilterUserCodesProperty); }
            set { SetValue(FilterUserCodesProperty, value); }
        }
        public static readonly DependencyProperty FilterUserCodesProperty =
            DependencyProperty.Register("FilterUserCodes", typeof(ObservableCollection<Filters.FilterUserCodes>), typeof(MainWindow));

        #endregion

        #region Fill filter list

        private void FillDataMinMaxDate()
        {
            var repoEventLog = new EF.Repository<Models.EventLog>(_readerContext);

            long dateMinLong = repoEventLog.GetMin<long>(f => f.Date);

            StartPeriodDate = dateMinLong.DateToSQLite();
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

            List<Filters.FilterEventCodes> eventCodeList = new List<Filters.FilterEventCodes>();

            foreach (Models.EventCodes item in eventCodes.OrderBy(f => f.Name))
                eventCodeList.Add(new Filters.FilterEventCodes(item) { IsChecked = isChecked });

            eventCodeList.Sort((a, b) => a.Name.CompareTo(b.Name));

            FilterEventCodes = new ObservableCollection<Filters.FilterEventCodes>(eventCodeList);
        }

        private async void FillDataUserCodes(bool isChecked = true)
        {
            FilterUserCodes.Clear();

            var repoUserCodes = new EF.Repository<Models.UserCodes>(_readerContext);

            List<Models.UserCodes> userCodes = await repoUserCodes.GetListAsync();

            foreach (Models.UserCodes item in userCodes.OrderBy(f => f.Name))
            {
                var newFilter = new Filters.FilterUserCodes(item) { IsChecked = isChecked };

                FilterUserCodes.Add(newFilter);
                _filterUserCodesBase.Add(newFilter);
            }
        }

        #endregion

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
                    case "FilterUserCodes":
                        _filterUserCodesBase.ForEach(f => f.IsChecked = isChecked);
                        FilterUserCodes = new ObservableCollection<Filters.FilterUserCodes>(_filterUserCodesBase);
                        break;

                }
            }
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
                        || (el.UserName?.ToLower().Contains(textFilterLower) ?? false)
                    ));

            }
        }

        private void TextBoxFilterUserCodes_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            string textFilterUserCodesLower = TextFilterUserCodes.ToLower();

            foreach (Filters.FilterUserCodes item in FilterUserCodes)
                _filterUserCodesBase.Where(f => f == item).Select(f => f.IsChecked = item.IsChecked);

            if (string.IsNullOrEmpty(textFilterUserCodesLower))
                FilterUserCodes = new ObservableCollection<Filters.FilterUserCodes>(_filterUserCodesBase);
            else
                FilterUserCodes = new ObservableCollection<Filters.FilterUserCodes>(
                    _filterUserCodesBase.Where(f => f.Name.ToLower().Contains(textFilterUserCodesLower)));
        }

    }
}
