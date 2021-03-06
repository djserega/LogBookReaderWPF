﻿using DevExpress.Mvvm;
using LogBookReader.Additions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace LogBookReader.ViewModel
{
    public partial class PropertyFilters : DependencyObject
    {
        private EF.ReaderContext _readerContext;

        public PropertyFilters()
        {
            FilterAppCodes = CollectionViewSource.GetDefaultView(new List<Filters.FilterAppCodes>());
            FilterComputerCodes = CollectionViewSource.GetDefaultView(new List<Filters.FilterComputerCodes>());
            FilterEventCodes = CollectionViewSource.GetDefaultView(new List<Filters.FilterEventCodes>());
            FilterUserCodes = CollectionViewSource.GetDefaultView(new List<Filters.FilterUserCodes>());
        }

        #region Property

        public DateTime StartPeriodDate
        {
            get { return (DateTime)GetValue(StartPeriodDateProperty); }
            set { SetValue(StartPeriodDateProperty, value); }
        }
        public static readonly DependencyProperty StartPeriodDateProperty =
            DependencyProperty.Register("StartPeriodDate", typeof(DateTime), typeof(PropertyFilters));

        public TimeSpan StartPeriodTime
        {
            get { return (TimeSpan)GetValue(StartPeriodTimeProperty); }
            set { SetValue(StartPeriodTimeProperty, value); }
        }
        public static readonly DependencyProperty StartPeriodTimeProperty =
            DependencyProperty.Register("StartPeriodTime", typeof(TimeSpan), typeof(PropertyFilters));

        public DateTime EndPeriodDate
        {
            get { return (DateTime)GetValue(EndPeriodDateProperty); }
            set { SetValue(EndPeriodDateProperty, value); }
        }
        public static readonly DependencyProperty EndPeriodDateProperty =
            DependencyProperty.Register("EndPeriodDate", typeof(DateTime), typeof(PropertyFilters));

        public TimeSpan EndPeriodTime
        {
            get { return (TimeSpan)GetValue(EndPeriodTimeProperty); }
            set { SetValue(EndPeriodTimeProperty, value); }
        }
        public static readonly DependencyProperty EndPeriodTimeProperty =
            DependencyProperty.Register("EndPeriodTime", typeof(TimeSpan), typeof(PropertyFilters));

        public int CountEventLogRows
        {
            get { return (int)GetValue(CountEventLogRowsProperty); }
            set { SetValue(CountEventLogRowsProperty, value); }
        }
        public static readonly DependencyProperty CountEventLogRowsProperty =
            DependencyProperty.Register("CountEventLogRows", typeof(int), typeof(PropertyFilters), new PropertyMetadata(100));

        public bool CommentIsFilled
        {
            get { return (bool)GetValue(CommentIsFilledProperty); }
            set { SetValue(CommentIsFilledProperty, value); }
        }
        public static readonly DependencyProperty CommentIsFilledProperty =
            DependencyProperty.Register("CommentIsFilled", typeof(bool), typeof(PropertyFilters));


        internal List<Filters.FilterAppCodes> FilterAppCodesBase { get; set; } = new List<Filters.FilterAppCodes>();
        internal List<Filters.FilterComputerCodes> FilterComputerCodesBase { get; set; } = new List<Filters.FilterComputerCodes>();
        internal List<Filters.FilterEventCodes> FilterEventCodesBase { get; set; } = new List<Filters.FilterEventCodes>();
        internal List<Filters.FilterUserCodes> FilterUserCodesBase { get; set; } = new List<Filters.FilterUserCodes>();
        internal List<Filters.FilterMetadataCodes> FilterMetadataCodesBase { get; set; } = new List<Filters.FilterMetadataCodes>();

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



        public ObservableCollection<Filters.MetadataCodesNode> FilterMetadataCodesTree
        {
            get { return (ObservableCollection<Filters.MetadataCodesNode>)GetValue(FilterMetadataCodesTreeProperty); }
            set { SetValue(FilterMetadataCodesTreeProperty, value); }
        }
        public static readonly DependencyProperty FilterMetadataCodesTreeProperty =
            DependencyProperty.Register("FilterMetadataCodesTree", typeof(ObservableCollection<Filters.MetadataCodesNode>), typeof(PropertyFilters), new PropertyMetadata(null));
        
        public ICollectionView FilterMetadataCodes
        {
            get { return (ICollectionView)GetValue(FilterMetadataCodesProperty); }
            set { SetValue(FilterMetadataCodesProperty, value); }
        }
        public static readonly DependencyProperty FilterMetadataCodesProperty =
            DependencyProperty.Register("FilterMetadataCodes", typeof(ICollectionView), typeof(PropertyFilters));

        #endregion

        #region Property_FilterList

        #region FilterUsers

        public string TextFilterUserCodes
        {
            get { return (string)GetValue(TextFilterUserCodesProperty); }
            set { SetValue(TextFilterUserCodesProperty, value); }
        }
        public static readonly DependencyProperty TextFilterUserCodesProperty =
            DependencyProperty.Register("TextFilterUserCodes",
                                        typeof(string),
                                        typeof(PropertyFilters),
                                        new PropertyMetadata("", TextFilterUserCodes_Changed));

        private static void TextFilterUserCodes_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
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

        #endregion

        #region FilterMetadata

        public string TextFilterMetadataCodes
        {
            get { return (string)GetValue(TextFilterMetadataCodesProperty); }
            set { SetValue(TextFilterMetadataCodesProperty, value); }
        }
        public static readonly DependencyProperty TextFilterMetadataCodesProperty =
            DependencyProperty.Register("TextFilterMetadataCodes",
                                        typeof(string),
                                        typeof(PropertyFilters),
                                        new PropertyMetadata("", TextFilterMetadataCodes_Changed));

        private static void TextFilterMetadataCodes_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PropertyFilters PropertyFilters)
            {
                PropertyFilters.FilterMetadataCodes.Filter = null;
                PropertyFilters.FilterMetadataCodes.Filter = PropertyFilters.TextFilterMetadataCodesObject;
            }
        }

        private bool TextFilterMetadataCodesObject(object obj)
        {
            bool result = true;

            if (!string.IsNullOrWhiteSpace(TextFilterMetadataCodes)
                && obj is Filters.FilterMetadataCodes metadataCodes)
            {
                string textFilterLower = TextFilterMetadataCodes.ToLower();

                result = metadataCodes.Name?.ToLower().Contains(textFilterLower) ?? false;
            }

            return result;
        }

        #endregion

        #endregion

        #region ICommand

        public ICommand FillDataAppCodesCommand
        {
            get => new DelegateCommand<bool>((bool marked) => FillDataAppCodes(marked));
        }

        public ICommand FillDataComputerCodesCommand
        {
            get => new DelegateCommand<bool>((bool marked) => FillDataComputerCodes(marked));
        }

        public ICommand FillDataEventCodesCommand
        {
            get => new DelegateCommand<bool>((bool marked) => FillDataEventCodes(marked));
        }

        public ICommand FillDataUserCodesCommand
        {
            get => new DelegateCommand<bool>((bool marked) => FillDataUserCodes(marked));
        }
        public ICommand FillDataMetadataCodesCommand
        {
            get => new DelegateCommand<bool>((bool marked) => FillDataMetadataCodes(marked));
        }

        #endregion

        internal void Fill(EF.ReaderContext readerContext)
        {
            _readerContext = readerContext;

            FillDataMinMaxDate();
            FillDataAppCodes();
            FillDataComputerCodes();
            FillDataEventCodes();
            FillDataUserCodes();
            FillDataMetadataCodes();
        }

        #region FillData
        internal void FillDataMinMaxDate()
        {
            var repoEventLog = new EF.Repository<Models.EventLog>(_readerContext);

            long dateMinLong = repoEventLog.GetMin(f => f.Date);
            long dateMaxLong = repoEventLog.GetMax(f => f.Date);

            StartPeriodDate = dateMinLong.DateToSQLite();
            EndPeriodDate = dateMaxLong.DateToSQLite();
        }

        private async void FillDataAppCodes(bool isChecked = true)
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

        private async void FillDataComputerCodes(bool isChecked = true)
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

        private async void FillDataEventCodes(bool isChecked = true)
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

        private async void FillDataUserCodes(bool isChecked = true)
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

        private async void FillDataMetadataCodes(bool isChecked = true)
        {
            var repoMetadataCodes = new EF.Repository<Models.MetadataCodes>(_readerContext);

            List<Models.MetadataCodes> metadataCodes = await repoMetadataCodes.GetListAsync();

            List<Filters.FilterMetadataCodes> filterMetadataCode = new List<Filters.FilterMetadataCodes>();
            foreach (Models.MetadataCodes item in metadataCodes.OrderBy(f => f.Name))
            {
                if (!string.IsNullOrWhiteSpace(item.Name))
                {
                    var newFilter = new Filters.FilterMetadataCodes(item) { IsChecked = isChecked };

                    filterMetadataCode.Add(newFilter);
                    FilterMetadataCodesBase.Add(newFilter);
                }
            }

            filterMetadataCode.Sort((a, b) => a.Name.CompareTo(b.Name));
          
            ObservableCollection<Filters.MetadataCodesNode> nodes = new ObservableCollection<Filters.MetadataCodesNode>();
            foreach (Filters.FilterMetadataCodes item in filterMetadataCode)
            {
                var nodesItemName = nodes.FirstOrDefault(f => f.Name == item.Parent);
                if (nodesItemName == null)
                {
                    nodesItemName = new Filters.MetadataCodesNode
                    {
                        IsChecked = true,
                        Name = item.Parent
                    };
                    nodes.Add(nodesItemName);
                }

                var child = new Filters.MetadataCodesNode
                {
                    IsChecked = true,
                    Name = item.Child
                };

                nodesItemName.Nodes.Add(child);
            }

            FilterMetadataCodesTree = nodes;

            FilterMetadataCodes = CollectionViewSource.GetDefaultView(FilterMetadataCodesTree);
        }

        #endregion
    }
}
