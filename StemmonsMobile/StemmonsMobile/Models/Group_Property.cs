using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.Models
{
   public class Group_Property : ObservableCollection<Grp_PropertyDetails>
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
        private List<Grp_PropertyDetails> storage = new List<Grp_PropertyDetails>();

    }


    public class Grp_PropertyDetails
    {
        public string CASE_TYPE_ID {get;set;}
        public string SYSTEM_PRIORITY { get; set; }
        public string ASSOC_TYPE_ID { get; set; }
        public string ASSOC_TYPE_SYSTEM_CODE { get; set; }
        public string ASSOC_TYPE_DESCRIPTION { get; set; }
        public string ASSOC_TYPE_NAME { get; set; }
        public string EXTERNAL_DATASOURCE_ID { get; set; }
        public string EXTERNAL_DATASOURCE_NAME { get; set; }
        public string EXTERNAL_DATASOURCE_DESCRIPTION { get; set; }
        public string count { get; set; }
    }
}
