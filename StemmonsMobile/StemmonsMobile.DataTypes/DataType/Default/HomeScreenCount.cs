using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        public int EntityCount
        {
            get => _entityCount;
            set
            {
                _entityCount = value;
                OnPropertyChanged("EntityCount");
            }
        }

        public int StandardCount
        {
            get => _standardCount;
            set
            {
                _standardCount = value;
                OnPropertyChanged("StandardCount");
            }
        }

        public int QuestCount
        {
            get => _questCount;
            set
            {
                _questCount = value;
                OnPropertyChanged("QuestCount");
            }
        }

        public int CasesCount
        {
            get => _casesCount;
            set
            {
                _casesCount = value;
                OnPropertyChanged("CasesCount");
            }
        }

        public int DepartmentCount
        {
            get => _departmentCount;
            set
            {
                _departmentCount = value;
                OnPropertyChanged("DepartmentCount");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
