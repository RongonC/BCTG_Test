using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.Models
{
    public class Group_QuestForm : ObservableCollection<Sel_QuestFormList>, INotifyPropertyChanged
    {
        private bool _expanded;

        public string Title { get; set; }

        public string TitleWithItemCount
        {
            get { return string.Format("{0}", Title); }
        }

        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    OnPropertyChanged("Expanded");
                    OnPropertyChanged("StateIcon");
                }
            }
        }

        public string StateIcon
        {
            get { return Expanded ? "Assets/dropdownicon.png" : "Assets/dropdowniconClose.png"; }
        }

        public int FoodCount { get; set; }
        public Group_QuestForm()
        {

        }
        public Group_QuestForm(string title, bool expanded = true)
        {
            Title = title;
            Expanded = expanded;
        }

        public static ObservableCollection<Sel_QuestFormList> All { private set; get; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Sel_QuestFormList
    {
        public int intItemInstanceTranID { get; set; }
        public int TypeId { get; set; }
        public string strItemNAme { get; set; }

        public string strItemName
        {
            get
            {
                return "<a class=\"aLink\" target=\"_blank\" href=\"ViewForm.aspx?intItemInstanceTranID=" + this.intItemInstanceTranID.ToString() + "\">" + this.strItemNAme.ToString() + "</a>";

            }
        }

        public decimal? dcOverallScore { get; set; }
        public string strCol1ItemInfoFieldValue { get; set; }
        public string strCol2ItemInfoFieldValue { get; set; }
        public string strCol3ItemInfoFieldValue { get; set; }
        public string SECURITY_TYPE_AREA { get; set; }
        public string SECURITY_TYPE_ITEM { get; set; }
        public string SECURITY_TYPE_TRAN { get; set; }
        public bool? blnIsEdit { get; set; }
        public bool TypeFlag { get; set; }
    }
}
