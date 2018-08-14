using StemmonsMobile.DataTypes.DataType.Standards.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Standards
{

    /// <remarks/>
    //[System.SerializableAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    //[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class STANDARDS
    {

        public standardsBookView BookViewDeserialize()
        {
            standardsBookView standardsBookView = new standardsBookView()
            {

                BookView = new List<STANDARDSBOOK_VIEW>()

            };

            if (this.BOOK_VIEW != null)
            {
                foreach (var item in this.BOOK_VIEW)
                {

                    standardsBookView.BookView.Add(new STANDARDSBOOK_VIEW()
                    {
                        aPP_ASSOC_META_DATA_IDField = item.APP_ASSOC_META_DATA_ID,
                        tYPE_IDField = item.TYPE_ID,
                        tYPE_NAMEField = item.TYPE_NAME,
                        nAMEField = item.NAME,
                        pARENT_META_DATA_IDField = item.PARENT_META_DATA_ID,
                        dISPLAY_ORDERField = item.DISPLAY_ORDER,
                        aPP_LOGOField = item.APP_LOGO,
                        aPP_HEADERField = item.APP_HEADER,
                        aPP_FOOTERField = item.APP_FOOTER,
                        iS_ACTIVEField = item.IS_ACTIVE,
                        cREATED_DATETIMEField = item.CREATED_DATETIME,
                        cREATED_BYField = item.CREATED_BY,
                        pARENT_LEVELField = item.PARENT_LEVEL,
                        aPP_IDField = item.APP_ID,
                        aPP_ASSOC_META_DATA_CONTENT_IDField = item.APP_ASSOC_META_DATA_CONTENT_ID,
                        mETADATA_DESCField = item.METADATA_DESC,
                        mETADATA_SUMMARYField = item.METADATA_SUMMARY,
                        mETADATA_CONTENTField = item.METADATA_CONTENT,

                    });
                }
            }
            return standardsBookView;
        }



        private STANDARDSBOOK_VIEW[] bOOK_VIEWField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("BOOK_VIEW")]
        public STANDARDSBOOK_VIEW[] BOOK_VIEW
        {
            get
            {
                return this.bOOK_VIEWField;
            }
            set
            {
                this.bOOK_VIEWField = value;
            }
        }
    }

    /// <remarks/>
  //  [System.SerializableAttribute()]
  //  [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class STANDARDSBOOK_VIEW
    {

        public int aPP_ASSOC_META_DATA_IDField;

        public int tYPE_IDField;

        public string tYPE_NAMEField;

        public string nAMEField;

        public int pARENT_META_DATA_IDField;

        public int dISPLAY_ORDERField;

        public string aPP_LOGOField;

        public string aPP_HEADERField;

        public string aPP_FOOTERField;

        public string iS_ACTIVEField;

        public System.DateTime cREATED_DATETIMEField;

        public string cREATED_BYField;

        public int pARENT_LEVELField;

        public int aPP_IDField;

        public int aPP_ASSOC_META_DATA_CONTENT_IDField;

        public string mETADATA_DESCField;

        public string mETADATA_SUMMARYField;

        public string mETADATA_CONTENTField;

        /// <remarks/>
        public int APP_ASSOC_META_DATA_ID
        {
            get
            {
                return this.aPP_ASSOC_META_DATA_IDField;
            }
            set
            {
                this.aPP_ASSOC_META_DATA_IDField = value;
            }
        }

        /// <remarks/>
        public int TYPE_ID
        {
            get
            {
                return this.tYPE_IDField;
            }
            set
            {
                this.tYPE_IDField = value;
            }
        }

        /// <remarks/>
        public string TYPE_NAME
        {
            get
            {
                return this.tYPE_NAMEField;
            }
            set
            {
                this.tYPE_NAMEField = value;
            }
        }

        /// <remarks/>
        public string NAME
        {
            get
            {
                return this.nAMEField;
            }
            set
            {
                this.nAMEField = value;
            }
        }

        /// <remarks/>
        public int PARENT_META_DATA_ID
        {
            get
            {
                return this.pARENT_META_DATA_IDField;
            }
            set
            {
                this.pARENT_META_DATA_IDField = value;
            }
        }

        /// <remarks/>
        public int DISPLAY_ORDER
        {
            get
            {
                return this.dISPLAY_ORDERField;
            }
            set
            {
                this.dISPLAY_ORDERField = value;
            }
        }

        /// <remarks/>
        public string APP_LOGO
        {
            get
            {
                return this.aPP_LOGOField;
            }
            set
            {
                this.aPP_LOGOField = value;
            }
        }

        /// <remarks/>
        public string APP_HEADER
        {
            get
            {
                return this.aPP_HEADERField;
            }
            set
            {
                this.aPP_HEADERField = value;
            }
        }

        /// <remarks/>
        public string APP_FOOTER
        {
            get
            {
                return this.aPP_FOOTERField;
            }
            set
            {
                this.aPP_FOOTERField = value;
            }
        }

        /// <remarks/>
        public string IS_ACTIVE
        {
            get
            {
                return this.iS_ACTIVEField;
            }
            set
            {
                this.iS_ACTIVEField = value;
            }
        }

        /// <remarks/>
        public System.DateTime CREATED_DATETIME
        {
            get
            {
                return this.cREATED_DATETIMEField;
            }
            set
            {
                this.cREATED_DATETIMEField = value;
            }
        }

        /// <remarks/>
        public string CREATED_BY
        {
            get
            {
                return this.cREATED_BYField;
            }
            set
            {
                this.cREATED_BYField = value;
            }
        }

        /// <remarks/>
        public int PARENT_LEVEL
        {
            get
            {
                return this.pARENT_LEVELField;
            }
            set
            {
                this.pARENT_LEVELField = value;
            }
        }

        /// <remarks/>
        public int APP_ID
        {
            get
            {
                return this.aPP_IDField;
            }
            set
            {
                this.aPP_IDField = value;
            }
        }

        /// <remarks/>
        public int APP_ASSOC_META_DATA_CONTENT_ID
        {
            get
            {
                return this.aPP_ASSOC_META_DATA_CONTENT_IDField;
            }
            set
            {
                this.aPP_ASSOC_META_DATA_CONTENT_IDField = value;
            }
        }

        /// <remarks/>
        public string METADATA_DESC
        {
            get
            {
                return this.mETADATA_DESCField;
            }
            set
            {
                this.mETADATA_DESCField = value;
            }
        }

        /// <remarks/>
        public string METADATA_SUMMARY
        {
            get
            {
                return this.mETADATA_SUMMARYField;
            }
            set
            {
                this.mETADATA_SUMMARYField = value;
            }
        }

        /// <remarks/>
        public string METADATA_CONTENT
        {
            get
            {
                return this.mETADATA_CONTENTField;
            }
            set
            {
                this.mETADATA_CONTENTField = value;
            }
        }
    }


}