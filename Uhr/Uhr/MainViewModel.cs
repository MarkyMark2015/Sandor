using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Uhr
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private Timer _timerUpdate;
        private Ziffernblatt _ziffern;
        private Random _rnd;
        private DateTime _time;

        public MainViewModel()
        {
            _ziffern = new Ziffernblatt();
            Selected = new ObservableCollection<bool>(_ziffern.GetDigitsVisibility());
            Text = new ObservableCollection<char>( Ziffernblatt.GetDigitList().ToArray());
            Title = "Uhr";
            _timerUpdate = new Timer(Tick, null, 0, 1000);
            _rnd = new Random();
            _time = DateTime.Now;
        }

        private void Tick(object state)
        {
            _time = DateTime.Now; 
            //_time = _time.AddMinutes(5);
            _ziffern.Update(Selected, _time);
            Title = _time.ToString();
            // Scramble-Option
            //for (int i = 0; i < Selected.Count; i++)
            //    Text[i] = Selected[i] ? Ziffernblatt.GetDigitList()[i] : Convert.ToChar(65 + _rnd.Next(0, 24));
        }

        #region Properties

        private ObservableCollection<char> _text;
        public ObservableCollection<char> Text
        {
            get { return this._text; }
            set { this._text = value; NotifyPropertyChanged(); }
        }

        private string _title;
        public string Title
        {
            get { return this._title; }
            set { this._title = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<bool> _selected;
        public ObservableCollection<bool> Selected
        {
            get { return this._selected; }
            set { this._selected = value; NotifyPropertyChanged(); }
        }

        #endregion

        #region Helper

        // INotifyProperytChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }

}
