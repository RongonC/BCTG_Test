using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.Models
{
    public class Topic : INotifyPropertyChanged
    {
        private List<Topic> _subtopics = new List<Topic>();
        private string _aPP_ASSOC_META_DATA_ID;
        private string _nAME;
        private string _aPP_ASSOC_META_DATA_IDField;
        private string _tYPE_IDField;
        private string _tYPE_NAMEField;
        private string _nAMEField;
        private string _pARENT_META_DATA_IDField;
        private string _dISPLAY_ORDERField;
        private string _aPP_LOGOField;
        private string _aPP_HEADERField;
        private string _aPP_FOOTERField;
        private string _iS_ACTIVEField;
        private string _cREATED_DATETIMEField;
        private string _cREATED_BYField;
        private string _pARENT_LEVELField;
        private string _aPP_IDField;
        private string _aPP_ASSOC_META_DATA_CONTENT_IDField;
        private string _mETADATA_DESCField;
        private string _mETADATA_SUMMARYField;
        private string _mETADATA_CONTENTField;
        private string _hAS_DETAILSField;
        private string _tYPE_ID;
        private string _tYPE_NAME;
        private string _pARENT_META_DATA_ID;
        private string _dISPLAY_ORDER;
        private string _aPP_LOGO;
        private string _aPP_HEADER;
        private string _aPP_FOOTER;
        private string _iS_ACTIVE;
        private string _cREATED_DATETIME;
        private string _cREATED_BY;
        private string _pARENT_LEVEL;
        private string _aPP_ID;
        private string _aPP_ASSOC_META_DATA_CONTENT_ID;
        private string _mETADATA_DESC;
        private string _mETADATA_SUMMARY;
        private string _mETADATA_CONTENT;
        private string _hAS_DETAILS;

        public string aPP_ASSOC_META_DATA_IDField
        {
            get { return _aPP_ASSOC_META_DATA_IDField; }
            set
            {
                _aPP_ASSOC_META_DATA_IDField = value;
                OnPropertyChanged("aPP_ASSOC_META_DATA_IDField");
            }
        }

        public string tYPE_IDField
        {
            get { return _tYPE_IDField; }
            set
            {
                _tYPE_IDField = value;
                OnPropertyChanged("tYPE_IDField");
            }
        }
        public string tYPE_NAMEField
        {
            get { return tYPE_NAMEField; }
            set
            {
                _tYPE_NAMEField = value;
                OnPropertyChanged("tYPE_NAMEField");
            }
        }
        public string nAMEField
        {
            get { return _nAMEField; }
            set
            {
                _nAMEField = value;
                OnPropertyChanged("nAMEField");
            }
        }
        public string pARENT_META_DATA_IDField
        {
            get { return _pARENT_META_DATA_IDField; }

            set
            {
                _pARENT_META_DATA_IDField = value;
                OnPropertyChanged("pARENT_META_DATA_IDField");
            }
        }
        public string dISPLAY_ORDERField
        {


            set
            {
                _dISPLAY_ORDERField = value;
                OnPropertyChanged("dISPLAY_ORDERField");
            }
        }
        public string aPP_LOGOField
        {
            get { return _aPP_LOGOField; }

            set
            {
                _aPP_LOGOField = value;
                OnPropertyChanged("_aPP_LOGOField");
            }
        }
        public string aPP_HEADERField
        {
            get { return _aPP_HEADERField; }
            set
            {
                _aPP_HEADERField = value;
                OnPropertyChanged("aPP_HEADERField");
            }
        }
        public string aPP_FOOTERField
        {
            get { return _aPP_FOOTERField; }
            set
            {
                _aPP_FOOTERField = value;
                OnPropertyChanged("aPP_FOOTERField");
            }
        }
        public string iS_ACTIVEField
        {
            get { return _iS_ACTIVEField; }
            set
            {
                _iS_ACTIVEField = value; OnPropertyChanged("iS_ACTIVEField");
            }
        }
        public string cREATED_DATETIMEField
        {
            get { return _cREATED_DATETIMEField; }
            set
            {
                _cREATED_DATETIMEField = value; OnPropertyChanged("cREATED_DATETIMEField");
            }
        }
        public string cREATED_BYField
        {
            get { return _cREATED_BYField; }
            set
            {
                _cREATED_BYField = value; OnPropertyChanged("cREATED_BYField");
            }
        }
        public string pARENT_LEVELField
        {
            get { return _pARENT_LEVELField; }
            set
            {
                _pARENT_LEVELField = value; OnPropertyChanged("pARENT_LEVELField");
            }
        }
        public string aPP_IDField
        {
            get { return _aPP_IDField; }
            set
            {
                _aPP_IDField = value; OnPropertyChanged("aPP_IDField");
            }
        }
        public string aPP_ASSOC_META_DATA_CONTENT_IDField
        {
            get { return _aPP_ASSOC_META_DATA_CONTENT_IDField; }

            set
            {
                _aPP_ASSOC_META_DATA_CONTENT_IDField = value; OnPropertyChanged("aPP_ASSOC_META_DATA_CONTENT_IDField");
            }
        }
        public string mETADATA_DESCField
        {
            get { return _mETADATA_DESCField; }
            set
            {
                _mETADATA_DESCField = value; OnPropertyChanged("mETADATA_DESCField");
            }
        }
        public string mETADATA_SUMMARYField
        {
            get { return _mETADATA_SUMMARYField; }
            set
            {
                _mETADATA_SUMMARYField = value; OnPropertyChanged("mETADATA_SUMMARYField");
            }
        }
        public string mETADATA_CONTENTField
        {
            get { return _mETADATA_CONTENTField; }
            set
            {
                _mETADATA_CONTENTField = value; OnPropertyChanged("mETADATA_CONTENTField");
            }
        }
        public string hAS_DETAILSField
        {
            get { return _hAS_DETAILSField; }
            set
            {
                _hAS_DETAILSField = value; OnPropertyChanged("hAS_DETAILSField");
            }
        }
        public string APP_ASSOC_META_DATA_ID
        {
            get { return _aPP_ASSOC_META_DATA_ID; }
            set
            {
                _aPP_ASSOC_META_DATA_ID = value;
                OnPropertyChanged("APP_ASSOC_META_DATA_ID");
            }
        }
        public string TYPE_ID
        {
            get { return _tYPE_ID; }
            set
            {
                _tYPE_ID = value;
                OnPropertyChanged("TYPE_ID");
            }
        }
        public string TYPE_NAME
        {
            get { return _tYPE_NAME; }
            set
            {
                _tYPE_NAME = value;
                OnPropertyChanged("TYPE_NAME");
            }
        }
        public string NAME
        {
            get { return _nAME; }
            set
            {
                _nAME = value;
                OnPropertyChanged("NAME");
            }
        }
        public string PARENT_META_DATA_ID
        {
            get { return _pARENT_META_DATA_ID; }
            set
            {
                _pARENT_META_DATA_ID = value;
                OnPropertyChanged("PARENT_META_DATA_ID");
            }
        }
        public string DISPLAY_ORDER
        {
            get { return _dISPLAY_ORDER; }
            set
            {
                _dISPLAY_ORDER = value;
                OnPropertyChanged("DISPLAY_ORDER");
            }
        }
        public string APP_LOGO
        {
            get { return _aPP_LOGO; }
            set
            {
                _aPP_LOGO = value;
                OnPropertyChanged("APP_LOGO");
            }
        }
        public string APP_HEADER
        {
            get { return _aPP_HEADER; }
            set
            {
                _aPP_HEADER = value;
                OnPropertyChanged("APP_HEADER");
            }
        }
        public string APP_FOOTER
        {
            get { return _aPP_FOOTER; }
            set
            {
                _aPP_FOOTER = value;
                OnPropertyChanged("APP_FOOTER");
            }
        }
        public string IS_ACTIVE
        {
            get { return _iS_ACTIVE; }
            set
            {
                _iS_ACTIVE = value;
                OnPropertyChanged("IS_ACTIVE");
            }
        }
        public string CREATED_DATETIME
        {
            get { return _cREATED_DATETIME; }
            set
            {
                _cREATED_DATETIME = value;
                OnPropertyChanged("CREATED_DATETIME");
            }
        }
        public string CREATED_BY
        {
            get { return _cREATED_BY; }
            set
            {
                _cREATED_BY = value;

                OnPropertyChanged("CREATED_BY");
            }
        }
        public string PARENT_LEVEL
        {

            get { return _pARENT_LEVEL; }
            set
            {
                _pARENT_LEVEL = value;

                OnPropertyChanged("PARENT_LEVEL");
            }
        }
        public string APP_ID
        {
            get { return _aPP_ID; }
            set
            {
                _aPP_ID = value;
                OnPropertyChanged("APP_ID");
            }
        }
        public string APP_ASSOC_META_DATA_CONTENT_ID
        {
            get { return _aPP_ASSOC_META_DATA_CONTENT_ID; }
            set
            {
                _aPP_ASSOC_META_DATA_CONTENT_ID = value;
                OnPropertyChanged("APP_ASSOC_META_DATA_CONTENT_ID");
            }
        }
        public string METADATA_DESC
        {
            get { return _mETADATA_DESC; }
            set
            {
                _mETADATA_DESC = value;
                OnPropertyChanged("METADATA_DESC");
            }
        }
        public string METADATA_SUMMARY
        {
            get { return _mETADATA_SUMMARY; }
            set
            {
                _mETADATA_SUMMARY = value;
                OnPropertyChanged("METADATA_SUMMARY");
            }
        }
        public string METADATA_CONTENT
        {
            get { return _mETADATA_CONTENT; }
            set
            {
                _mETADATA_CONTENT = value;
                OnPropertyChanged("METADATA_CONTENT");
            }
        }
        public string HAS_DETAILS
        {
            get { return _hAS_DETAILS; }

            set
            {
                _hAS_DETAILS = value;
                OnPropertyChanged("HAS_DETAILS");
            }
        }
        public List<Topic> Subtopics
        {
            get
            {
                return _subtopics;
            }
            set
            {
                _subtopics = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
