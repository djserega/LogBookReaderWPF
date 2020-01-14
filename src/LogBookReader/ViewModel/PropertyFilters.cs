using LogBookReader.Additions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogBookReader.ViewModel
{
    public class PropertyFilters : DependencyObject
    {
        private EF.ReaderContext _readerContext;
        private readonly List<Filters.FilterUserCodes> _filterUserCodesBase = new List<Filters.FilterUserCodes>();


        public PropertyFilters()
        {
            FilterAppCodes = new ObservableCollection<Filters.FilterAppCodes>();
            FilterComputerCodes = new ObservableCollection<Filters.FilterComputerCodes>();
            FilterEventCodes = new ObservableCollection<Filters.FilterEventCodes>();
            FilterUserCodes = new ObservableCollection<Filters.FilterUserCodes>();
        }

        internal DateTime StartPeriodDate { get; set; }

        #region Dependency property

        public ObservableCollection<Filters.FilterAppCodes> FilterAppCodes
        {
            get { return (ObservableCollection<Filters.FilterAppCodes>)GetValue(FilterAppCodesProperty); }
            set { SetValue(FilterAppCodesProperty, value); }
        }
        public static readonly DependencyProperty FilterAppCodesProperty =
         DependencyProperty.Register("FilterAppCodes", typeof(ObservableCollection<Filters.FilterAppCodes>), typeof(PropertyFilters));

        public ObservableCollection<Filters.FilterComputerCodes> FilterComputerCodes
        {
            get { return (ObservableCollection<Filters.FilterComputerCodes>)GetValue(FiltersComputerCodesProperty); }
            set { SetValue(FiltersComputerCodesProperty, value); }
        }
        public static readonly DependencyProperty FiltersComputerCodesProperty =
            DependencyProperty.Register("FilterComputerCodes", typeof(ObservableCollection<Filters.FilterComputerCodes>), typeof(PropertyFilters));

        public ObservableCollection<Filters.FilterEventCodes> FilterEventCodes
        {
            get { return (ObservableCollection<Filters.FilterEventCodes>)GetValue(FilterEventCodesProperty); }
            set { SetValue(FilterEventCodesProperty, value); }
        }
        public static readonly DependencyProperty FilterEventCodesProperty =
            DependencyProperty.Register("FilterEventCodes", typeof(ObservableCollection<Filters.FilterEventCodes>), typeof(PropertyFilters));

        public ObservableCollection<Filters.FilterUserCodes> FilterUserCodes
        {
            get { return (ObservableCollection<Filters.FilterUserCodes>)GetValue(FilterUserCodesProperty); }
            set { SetValue(FilterUserCodesProperty, value); }
        }
        public static readonly DependencyProperty FilterUserCodesProperty =
            DependencyProperty.Register("FilterUserCodes", typeof(ObservableCollection<Filters.FilterUserCodes>), typeof(PropertyFilters));

        #endregion

        #region Fill filter list

        internal void Fill(EF.ReaderContext readerContext)
        {
            _readerContext = readerContext;

            FillDataMinMaxDate();
            FillDataAppCodes();
            FillDataComputerCodes();
            FillDataEventCodes();
            FillDataUserCodes();
        }

        internal void FillDataMinMaxDate()
        {
            var repoEventLog = new EF.Repository<Models.EventLog>(_readerContext);

            long dateMinLong = repoEventLog.GetMin<long>(f => f.Date);

            StartPeriodDate = dateMinLong.DateToSQLite();
        }

        internal async void FillDataAppCodes(bool isChecked = true)
        {
            FilterAppCodes.Clear();

            var repoAppCodes = new EF.Repository<Models.AppCodes>(_readerContext);

            List<Models.AppCodes> appCodes = await repoAppCodes.GetListAsync();

            foreach (Models.AppCodes item in appCodes.OrderBy(f => f.Name))
                FilterAppCodes.Add(new Filters.FilterAppCodes(item) { IsChecked = isChecked });
        }

        internal async void FillDataComputerCodes(bool isChecked = true)
        {
            FilterComputerCodes.Clear();

            var repoComputerCodes = new EF.Repository<Models.ComputerCodes>(_readerContext);

            List<Models.ComputerCodes> computerCodes = await repoComputerCodes.GetListAsync();

            foreach (Models.ComputerCodes item in computerCodes.OrderBy(f => f.Name))
                FilterComputerCodes.Add(new Filters.FilterComputerCodes(item) { IsChecked = isChecked });
        }

        internal async void FillDataEventCodes(bool isChecked = true)
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

        internal async void FillDataUserCodes(bool? isChecked = true)
        {
            if (isChecked == null)
            {
                _filterUserCodesBase.ForEach(f => f.IsChecked = true);
                FilterUserCodes = new ObservableCollection<Filters.FilterUserCodes>(_filterUserCodesBase);
            }
            else
            {
                FilterUserCodes.Clear();

                var repoUserCodes = new EF.Repository<Models.UserCodes>(_readerContext);

                List<Models.UserCodes> userCodes = await repoUserCodes.GetListAsync();

                foreach (Models.UserCodes item in userCodes.OrderBy(f => f.Name))
                {
                    var newFilter = new Filters.FilterUserCodes(item) { IsChecked = (bool)isChecked };

                    FilterUserCodes.Add(newFilter);
                    _filterUserCodesBase.Add(newFilter);
                }
            }
        }

        #endregion
    }
}
