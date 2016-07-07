using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using GeoLib.ClientApp.Annotations;

namespace GeoLib.ClientApp
{
    internal class WindowViewModel : INotifyPropertyChanged
    {
        private string _city;
        private string _connStatus;
        private string _country;

        private DataModel _dataProvider;
        private Task _serverStatusListener;
        private bool _status;
        private string _zipCode;
        private CancellationTokenSource cts;

        public WindowViewModel()
        {
            ConnectToServer();

            FindCityByZipButtonClicked = new RelayCommand(FindCityByZipButtonClickedInternal);
            ReconnectButtonClicked = new RelayCommand(ReconnectButtonClickedInternal);
        }

        public string ConnectionStatus
        {
            get { return _connStatus; }
            set
            {
                _connStatus = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ConnectionStatus"));
            }
        }

        public string ZipCode
        {
            get { return _zipCode ?? ""; }
            set
            {
                _zipCode = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ZipCode"));
            }
        }

        public string Country
        {
            get { return _country ?? ""; }
            set
            {
                _country = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Country"));
            }
        }

        public string City
        {
            get { return _city ?? ""; }
            set
            {
                _city = value;
                PropertyChanged(this, new PropertyChangedEventArgs("City"));
            }
        }

        public RelayCommand FindCityByZipButtonClicked { get; private set; }
        public RelayCommand ReconnectButtonClicked { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void ConnectToServer()
        {
            _dataProvider = DataModel.Instance;
            cts = new CancellationTokenSource();
            var token = cts.Token;
            _serverStatusListener = Task.Factory.StartNew(()=>{CheckServiceIsAlive(token);}, token);
            
        }

        private void DropConnection()
        {
            cts.Cancel();

            try
            {
                _serverStatusListener.Wait();
                _serverStatusListener.Dispose();
                cts = null;
                _dataProvider = null;
            }
            catch (AggregateException ae)
            {
                // catch inner exception 
            }
            catch (Exception crap)
            {
                // catch something else
            }
                     
        }

        public void CheckServiceIsAlive(CancellationToken p_token)
        {
            while (true)
            {
                if (p_token.IsCancellationRequested)
                {
                    // Clean up here, then...
                    
                    p_token.ThrowIfCancellationRequested();
                }
                try
                {
                    var r = _dataProvider.GeoServiceDataProvider.Ping();
                    if (r.Status == 1)
                    {
                        ConnectionStatus = @"C:\Users\EvgenyK\Desktop\dotnettutors\GeoLib.ClientApp\Resources\up.png";
                    }
                    else
                    {
                        ConnectionStatus = @"C:\Users\EvgenyK\Desktop\dotnettutors\GeoLib.ClientApp\Resources\down.png";
                    }
                }
                catch (Exception)
                {
                    ConnectionStatus = @"C:\Users\EvgenyK\Desktop\dotnettutors\GeoLib.ClientApp\Resources\down.png";
                    return;
                }
                Thread.Sleep(1000);
            }
        }

        private void ReconnectButtonClickedInternal()
        {
            DropConnection();
            Thread.Sleep(20);
            ConnectToServer();
        }

        private void FindCityByZipButtonClickedInternal()
        {
            try
            {
                City = _dataProvider.GeoServiceDataProvider.GetGeoData(ZipCode).City;
                Country = _dataProvider.GeoServiceDataProvider.GetGeoData(ZipCode).Country;
            }
            catch (Exception exception)
            {
                MessageBox.Show("No connection!", "My App", MessageBoxButton.OK);
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string p_propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(p_propertyName));
        }
    }
}