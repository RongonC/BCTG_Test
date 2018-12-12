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
            this.CasesCount = 0;
            this.DepartmentCount = 0;
            this.EntityCount = 0;
            this.QuestCount = 0;
            this.StandardCount = 0;
        }

        public int EntityCount
        {
            get => _entityCount;
            set
            {
                _entityCount = value;
                NotifyPropertyChanged();
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
            get => _casesCount;
            set
            {
                _casesCount = value;
                NotifyPropertyChanged();
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

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
