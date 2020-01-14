using LogBookReader.Additions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LogBookReader.ViewModel
{
    public class PropertyFilters : DependencyObject
    {
        private EF.ReaderContext _readerContext;
        

        public PropertyFilters()
        {
            FilterAppCodes = CollectionViewSource.GetDefaultView(new List<Filters.FilterAppCodes>());
            FilterComputerCodes = CollectionViewSource.GetDefaultView(new List<Filters.FilterComputerCodes>());
            FilterEventCodes = CollectionViewSource.GetDefaultView(new List<Filters.FilterEventCodes>());
            FilterUserCodes = CollectionViewSource.GetDefaultView(new List<Filters.FilterUserCodes>());
        }

        internal DateTime StartPeriodDate { get; set; }

        #region Property


        public string TextFilterUserCodes
        {
            get { return (string)GetValue(TextFilterUserCodesProperty); }
            set { SetValue(TextFilterUserCodesProperty, value); }
        }
        public static readonly DependencyProperty TextFilterUserCodesProperty =
            DependencyProperty.Register("TextFilterUserCodes", typeof(string), typeof(PropertyFilters), new PropertyMetadata("", TextFilter_Changed));

        private static void TextFilter_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PropertyFilters PropertyFilters)
            {
                PropertyFilters.FilterUserCodes.Filter = null;
                PropertyFilters.FilterUserCodes.Filter = PropertyFilters.TextFilterUserCodesObject;
            }
        }

        private bool TextFilterUserCodesObject(object obj)
        {
            bool result = true;

            if (!string.IsNullOrWhiteSpace(TextFilterUserCodes)
                && obj is Filters.FilterUserCodes userCodes)
            {
                string textFilterLower = TextFilterUserCodes.ToLower();

                result = userCodes.Name?.ToLower().Contains(textFilterLower) ?? false;
            }

            return result;
        }

        internal List<Filters.FilterAppCodes> FilterAppCodesBase { get; set; } = new List<Filters.FilterAppCodes>();
        internal List<Filters.FilterComputerCodes> FilterComputerCodesBase { get; set; } = new List<Filters.FilterComputerCodes>();
        internal List<Filters.FilterEventCodes> FilterEventCodesBase { get; set; } = new List<Filters.FilterEventCodes>();
        internal List<Filters.FilterUserCodes> FilterUserCodesBase { get; set; } = new List<Filters.FilterUserCodes>();


        public ICollectionView FilterAppCodes
        {
            get { return (ICollectionView)GetValue(FilterAppCodesProperty); }
            set { SetValue(FilterAppCodesProperty, value); }
        }
        public static readonly DependencyProperty FilterAppCodesProperty =
         DependencyProperty.Register("FilterAppCodes", typeof(ICollectionView), typeof(PropertyFilters));

        public ICollectionView FilterComputerCodes
        {
            get { return (ICollectionView)GetValue(FiltersComputerCodesProperty); }
            set { SetValue(FiltersComputerCodesProperty, value); }
        }
        public static readonly DependencyProperty FiltersComputerCodesProperty =
            DependencyProperty.Register("FilterComputerCodes", typeof(ICollectionView), typeof(PropertyFilters));

        public ICollectionView FilterEventCodes
        {
            get { return (ICollectionView)GetValue(FilterEventCodesProperty); }
            set { SetValue(FilterEventCodesProperty, value); }
        }
        public static readonly DependencyProperty FilterEventCodesProperty =
            DependencyProperty.Register("FilterEventCodes", typeof(ICollectionView), typeof(PropertyFilters));

        public ICollectionView FilterUserCodes
        {
            get { return (ICollectionView)GetValue(FilterUserCodesProperty); }
            set { SetValue(FilterUserCodesProperty, value); }
        }
        public static readonly DependencyProperty FilterUserCodesProperty =
            DependencyProperty.Register("FilterUserCodes", typeof(ICollectionView), typeof(PropertyFilters));

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
            var repoAppCodes = new EF.Repository<Models.AppCodes>(_readerContext);

            List<Models.AppCodes> appCodes = await repoAppCodes.GetListAsync();

            List<Filters.FilterAppCodes> filterAppCodes = new List<Filters.FilterAppCodes>();
            foreach (Models.AppCodes item in appCodes.OrderBy(f => f.Name))
            {
                var newFilter = new Filters.FilterAppCodes(item) { IsChecked = isChecked };

                filterAppCodes.Add(newFilter);
                FilterAppCodesBase.Add(newFilter);
            }

            filterAppCodes.Sort((a, b) => a.Name.CompareTo(b.Name));

            FilterAppCodes = CollectionViewSource.GetDefaultView(filterAppCodes);
        }

        internal async void FillDataComputerCodes(bool isChecked = true)
        {
            var repoComputerCodes = new EF.Repository<Models.ComputerCodes>(_readerContext);

            List<Models.ComputerCodes> computerCodes = await repoComputerCodes.GetListAsync();

            List<Filters.FilterComputerCodes> filterComputerCodes = new List<Filters.FilterComputerCodes>();
            foreach (Models.ComputerCodes item in computerCodes.OrderBy(f => f.Name))
            {
                var newFilter = new Filters.FilterComputerCodes(item) { IsChecked = isChecked };

                filterComputerCodes.Add(newFilter);
                FilterComputerCodesBase.Add(newFilter);
            }

            filterComputerCodes.Sort((a, b) => a.Name.CompareTo(b.Name));

            FilterComputerCodes = CollectionViewSource.GetDefaultView(filterComputerCodes);
        }

        internal async void FillDataEventCodes(bool isChecked = true)
        {
            var repoEventCodes = new EF.Repository<Models.EventCodes>(_readerContext);

            List<Models.EventCodes> eventCodes = await repoEventCodes.GetListAsync();

            List<Filters.FilterEventCodes> filterEventCodes = new List<Filters.FilterEventCodes>();
            foreach (Models.EventCodes item in eventCodes.OrderBy(f => f.Name))
            {
                var newFilter = new Filters.FilterEventCodes(item) { IsChecked = isChecked };

                filterEventCodes.Add(newFilter);
                FilterEventCodesBase.Add(newFilter);
            }

            filterEventCodes.Sort((a, b) => a.Name.CompareTo(b.Name));

            FilterEventCodes = CollectionViewSource.GetDefaultView(filterEventCodes);
        }

        internal async void FillDataUserCodes(bool isChecked = true)
        {

            var repoUserCodes = new EF.Repository<Models.UserCodes>(_readerContext);

            List<Models.UserCodes> userCodes = await repoUserCodes.GetListAsync();

            List<Filters.FilterUserCodes> filterUsersCode = new List<Filters.FilterUserCodes>();
            foreach (Models.UserCodes item in userCodes.OrderBy(f => f.Name))
            {
                var newFilter = new Filters.FilterUserCodes(item) { IsChecked = isChecked };

                filterUsersCode.Add(newFilter);
                FilterUserCodesBase.Add(newFilter);
            }

            filterUsersCode.Sort((a, b) => a.Name.CompareTo(b.Name));

            FilterUserCodes = CollectionViewSource.GetDefaultView(filterUsersCode);
        }

        #endregion
    }
}
