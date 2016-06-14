using System.ComponentModel;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight.Command;
using GeoLib.ClientApp.Annotations;

namespace GeoLib.ClientApp
{
    internal class WindowViewModel : INotifyPropertyChanged
    {
        private string _zipCode;
        private string _city;
        private string _country;

        private readonly DataModel dataProvider;

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
                PropertyChanged(this,new PropertyChangedEventArgs("City"));
            }
        }

        public WindowViewModel()
        {
            dataProvider = DataModel.Instance;
            
            FindCityByZipButtonClicked = new RelayCommand(FindCityByZipButtonClickedInternal);
        }

        public RelayCommand FindCityByZipButtonClicked { get; private set; }

        private void FindCityByZipButtonClickedInternal()
        {
            City = dataProvider.GeoServiceDataProvider.GetGeoData(ZipCode).City;
            Country = dataProvider.GeoServiceDataProvider.GetGeoData(ZipCode).Country;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate {};

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string p_propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(p_propertyName));
        }
    }
}