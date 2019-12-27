using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using WpfTest.Annotations;
using WpfTest.Data;

namespace WpfTest
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly DataModel _zonesDataModel;

        private string _zoneName;

        private string _top;
        private string _bottom;
        private string _left;
        private string _right;

        private string _loggerText;

        public MainWindowViewModel()
        {
            _zonesDataModel = DataModel.Instance;
            _zoneName = string.Empty;
            _top = string.Empty; 
            _bottom = string.Empty; 
            _left = string.Empty;
            _right = string.Empty;

            OnClear = new RelayCommand(OnClearInternal);
            OnCreate = new RelayCommand(OnCreateInternal);
            OnListZones = new RelayCommand(OnListZonesInternal);
        }

        #region View props
        public string LoggerText
        {
            get { return _loggerText; }
            set
            {
                _loggerText = value;
                PropertyChanged(this, new PropertyChangedEventArgs("LoggerText"));
            }
        }

        public string ZoneName
        {
            get { return _zoneName; }
            set
            {
                _zoneName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ZoneName"));
            }
        }

        public string TopText
        {
            get { return _top; }
            set
            {
                _top = value;
                PropertyChanged(this, new PropertyChangedEventArgs("TopText"));
            }
        }

        public string BottomText
        {
            get { return _bottom; }
            set
            {
                _bottom = value;
                PropertyChanged(this, new PropertyChangedEventArgs("BottomText"));
            }
        }

        public string LeftText
        {
            get { return _left; }
            set
            {
                _left = value;
                PropertyChanged(this, new PropertyChangedEventArgs("LeftText"));
            }
        }

        public string RightText
        {
            get { return _right; }
            set
            {
                _right = value;
                PropertyChanged(this, new PropertyChangedEventArgs("RightText"));
            }
        }
        #endregion

        public RelayCommand OnClear { get; private set; }
        public RelayCommand OnCreate { get; private set; }
        public RelayCommand OnListZones { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private async void OnCreateInternal()
        {
            if (ZoneName.Length == 0)
            {
                ShowStuff("There are some empty fields!");
                return;
            }

            int zoom = 0;
            try
            {
                Extent extent = new Extent
                {
                    Bottom = Convert.ToDouble(BottomText),
                    Top = Convert.ToDouble(TopText),
                    Left = Convert.ToDouble(LeftText),
                    Right = Convert.ToDouble(RightText),
                };

                SkiResortInfo info = new SkiResortInfo
                {
                    ResortExtent = extent,
                    ResortFullName = ZoneName
                };

                

                var res = await _zonesDataModel.AddNewZone(info);

                if (res == "OK")
                {
                    LoggerText = await _zonesDataModel.GetUpdatedList();
                    OnClearInternal();
                }
                else
                {
                    ShowStuff(res);
                }

            }
            catch (Exception e)
            {
                ShowStuff(e.Message);
            }
        }

        private async void OnDownloadZip()
        {
            
        }

        private async void OnListZonesInternal()
        {
            LoggerText = await _zonesDataModel.GetUpdatedList();
        }

        private void OnClearInternal()
        {
            ZoneName = string.Empty;
            TopText = string.Empty;
            BottomText = string.Empty;
            LeftText = string.Empty;
            RightText = string.Empty;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string p_propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(p_propertyName));
        }

        private void ShowStuff(string p_messageToShow)
        {
            MessageBox.Show(p_messageToShow, "Error", MessageBoxButton.OK);
        }
    }
}