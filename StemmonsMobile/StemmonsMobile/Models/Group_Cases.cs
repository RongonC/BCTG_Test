using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using static StemmonsMobile.DataTypes.DataType.Cases.OriginationCenterDataResponse;

namespace StemmonsMobile.Models
{
    public class Group_Cases : ObservableCollection<OriginationCenterData>
    {
        public string Heading { get; set; }

        public bool IsExpanded
        {
            get { return this.isExpanded; }
            set
            {
                if (this.isExpanded != value)
                {
                    this.isExpanded = value;
                    if (this.isExpanded)
                    {
                        foreach (var item in this.storage)
                        {
                            this.Add(item);
                        }
                        this.storage.Clear();
                    }
                    else
                    {
                        this.storage.AddRange(this);
                        for (int i = this.Count - 1; i >= 0; i--)
                        {
                            this.RemoveAt(i);
                        }
                    }
                    this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.IsExpanded)));
                }
            }
        }

        private bool isExpanded = true;
        private List<OriginationCenterData> storage = new List<OriginationCenterData>();

    }
}
