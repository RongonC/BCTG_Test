using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Default
{
    public class HomeScreenCount : INotifyPropertyChanged
    {
        private int _entityCount = 0;
        private int _standardCount = 0;
        private int _questCount = 0;
        private int _casesCount = 0;
        private int _departmentCount = 0;

        public HomeScreenCount()
        {
            _entityCount = 0;
            _standardCount = 0;
            _questCount = 0;
            _casesCount = 0;
            _departmentCount = 0;
        }

        public int EntityCount
        {
            get
            {
                return this._entityCount;
            }

            set
            {
                if (value != this._entityCount)
                {
                    this._entityCount = value;
                    NotifyPropertyChanged();
                }
                //_entityCount = value;
                //NotifyPropertyChanged();
            }
        }

        public int StandardCount
        {
            get => _standardCount;
            set
            {
                _standardCount = value;
                NotifyPropertyChanged();
            }
        }

        public int QuestCount
        {
            get => _questCount;
            set
            {
                _questCount = value;
                NotifyPropertyChanged();
            }
        }

        public int CasesCount
        {
            get
            {
                return _casesCount;
            }

            set
            {
                if (value != this._casesCount)
                {
                    this._casesCount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int DepartmentCount
        {
            get => _departmentCount;
            set
            {
                _departmentCount = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
