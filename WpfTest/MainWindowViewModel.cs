using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using GalaSoft.MvvmLight.Command;

using WpfTest.Annotations;
using WpfTest.Data;

namespace WpfTest
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly DataModel _personsDataModel;
        private string _ageText;

        private string _loggerText;
        private string _nameText;

        public MainWindowViewModel()
        {
            _personsDataModel = DataModel.Instance;
            _nameText = "";
            _ageText = "";

            OnClear = new RelayCommand(OnClearInternal);
            OnCreate = new RelayCommand(OnCreateInternal);
        }

        public string LoggerText
        {
            get { return _loggerText; }
            set
            {
                _loggerText = value;
                PropertyChanged(this, new PropertyChangedEventArgs("LoggerText"));
            }
        }

        public string AgeText
        {
            get { return _ageText; }
            set
            {
                _ageText = value;
                PropertyChanged(this, new PropertyChangedEventArgs("AgeText"));
            }
        }

        public string NameText
        {
            get { return _nameText; }
            set
            {
                _nameText = value;
                PropertyChanged(this, new PropertyChangedEventArgs("NameText"));
            }
        }

        public RelayCommand OnClear { get; private set; }
        public RelayCommand OnCreate { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void OnCreateInternal()
        {
            if (NameText.Length == 0 || AgeText.Length == 0)
            {
                ShowStuff("Man, there are some empty fields!");
                return;
            }

            if (_personsDataModel.AddNewPerson(new Person {Age = AgeText, Name = NameText}) == StatusCheck.Ok)
            {
                LoggerText = _personsDataModel.GetUpdatedList();
                return;
            }

            ShowStuff("fdfnmgbvdfndfv");
        }

        private void OnClearInternal()
        {
            LoggerText = "";
            NameText = "";
            AgeText = "";
            LoggerText = "";
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