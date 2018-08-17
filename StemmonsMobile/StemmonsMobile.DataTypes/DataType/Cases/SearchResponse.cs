
using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class SearchResponse : Response
    {
        public object ResponseContent { get; set; }
    }

    public partial class SearchResult
    {
        public System.Nullable<int> LINK_ID { get; set; }
        public System.Nullable<int> TypeID { get; set; }
        public System.Nullable<byte> _APPLICATION_ID { get; set; }
        public string APPLICATION_NAME { get; set; }

        private string _name = string.Empty;
        public string NAME
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = string.IsNullOrEmpty(value) ? "No Title Provided" : value;
            }
        }
        public string TYPE_NAME
        {
            get; set;
        }
        public string FIELD_VALUE
        {
            get; set;
        }
        public string STATUS
        {
            get; set;
        }
        public string REDIRECT_URL
        {
            get; set;
        }
        public System.Nullable<int> ITEM_ID { get; set; }
        public System.Nullable<int> AREA_ID { get; set; }
        public System.Nullable<bool> IS_EDIT { get; set; }
        public string SECURITY_AREA
        {
            get; set;
        }
        public string SECURITY_ITEM
        {
            get; set;
        }
        public string SECURITY_TRAN
        {
            get; set;
        }

    }

    public class Systems
    {
        private int _SystemID;
        private string _SystemName;
        private string _redirectLInk;
        public int SystemID
        {
            get
            {
                return _SystemID;
            }
            set
            {
                _SystemID = value;
            }
        }
        public string SystemName
        {
            get
            {
                return _SystemName;
            }
            set
            {
                _SystemName = value;
            }
        }

        public string RedirectLink
        {
            get
            {
                return _redirectLInk;
            }
            set
            {
                _redirectLInk = value;
            }
        }

        public Systems()
        {
        }
        public Systems(int systemID, string systemName)
        {
            SystemID = systemID;
            SystemName = systemName;
        }
    }

}