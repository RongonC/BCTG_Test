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

        private STANDARDSALL[] aLLField;

        private STANDARDSMOST_POPULAR[] mOST_POPULARField;

        private STANDARDSWHATS_NEW[] wHATS_NEWField;

        private STANDARDSFOR_ME[] fOR_MEField;



        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ALL")]
        public STANDARDSALL[] ALL
        {
            get
            {
                return this.aLLField;
            }
            set
            {
                this.aLLField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("MOST_POPULAR")]
        public STANDARDSMOST_POPULAR[] MOST_POPULAR
        {
            get
            {
                return this.mOST_POPULARField;
            }
            set
            {
                this.mOST_POPULARField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("WHATS_NEW")]
        public STANDARDSWHATS_NEW[] WHATS_NEW
        {
            get
            {
                return this.wHATS_NEWField;
            }
            set
            {
                this.wHATS_NEWField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("FOR_ME")]
        public STANDARDSFOR_ME[] FOR_ME
        {
            get
            {
                return this.fOR_MEField;
            }
            set
            {
                this.fOR_MEField = value;
            }
        }

        public standards ListDeserialize()
        {
            standards standards = new standards()
            {

                All = new List<STANDARDSALL>(),
                MostPopular = new List<STANDARDSMOST_POPULAR>(),
                WhatsNew = new List<STANDARDSWHATS_NEW>(),
                ForMe = new List<STANDARDSFOR_ME>()
            };

            if (this.WHATS_NEW != null)
            {
                foreach (var item in this.WHATS_NEW)
                {
                    standards.WhatsNew.Add(new STANDARDSWHATS_NEW()
                    {
                        APP_ID = item.APP_ID,
                        APP_NAME = item.APP_NAME,
                        DESCRIPTIONS = item.DESCRIPTIONS,
                        MODIFIED_DATETIME = item.MODIFIED_DATETIME,
                        MODIFIED_BY = item.MODIFIED_BY,
                        LAST_VISIT_Date = item.LAST_VISIT_Date,

                    });
                }
            }

            if (this.MOST_POPULAR != null)
            {
                foreach (var item in this.MOST_POPULAR)
                {
                    standards.MostPopular.Add(new STANDARDSMOST_POPULAR()
                    {
                        APP_ASSOC_META_DATA_ID = item.APP_ASSOC_META_DATA_ID,

                        TYPE_ID = item.TYPE_ID,

                        TYPE_NAME = item.TYPE_NAME,

                        NAME = item.NAME,

                        DESCRIPTIONS = item.DESCRIPTIONS,

                        PARENT_META_DATA_ID = item.PARENT_META_DATA_ID,

                        DISPLAY_ORDER = item.DISPLAY_ORDER,

                        APP_LOGO = item.APP_LOGO,

                        APP_HEADER = item.APP_HEADER,

                        APP_FOOTER = item.APP_FOOTER,

                        IS_ACTIVE = item.IS_ACTIVE,

                        CREATED_DATETIME = item.CREATED_DATETIME,

                        CREATED_BY = item.CREATED_BY,

                        PARENT_LEVEL = item.PARENT_LEVEL,

                        APP_ID = item.APP_ID,

                        APP_NAME = item.APP_NAME,

                        TOTAL_POSOTIVE_FEEDBACK = item.TOTAL_POSOTIVE_FEEDBACK,

                        TOTAL_NEGITIVE_FEEDBACK = item.TOTAL_NEGITIVE_FEEDBACK,

                        TOTAL_REVIEW_RANK = item.TOTAL_REVIEW_RANK,

                        TOTAL_VISIT = item.TOTAL_VISIT,

                        MODIFIED_DATETIME = item.MODIFIED_DATETIME,

                        MODIFIED_BY = item.MODIFIED_BY,


                    });
                }
            }
            if (this.FOR_ME != null)
            {
                foreach (var item in this.FOR_ME)
                {
                    standards.ForMe.Add(new STANDARDSFOR_ME()
                    {

                        APP_ASSOC_META_DATA_ID = item.APP_ASSOC_META_DATA_ID,

                        TYPE_ID = item.TYPE_ID,

                        TYPE_NAME = item.TYPE_NAME,

                        NAME = item.NAME,

                        DESCRIPTIONS = item.DESCRIPTIONS,

                        PARENT_META_DATA_ID = item.PARENT_META_DATA_ID,

                        DISPLAY_ORDER = item.DISPLAY_ORDER,

                        APP_LOGO = item.APP_LOGO,

                        APP_HEADER = item.APP_HEADER,

                        APP_FOOTER = item.APP_FOOTER,

                        IS_ACTIVE = item.IS_ACTIVE,

                        CREATED_DATETIME = item.CREATED_DATETIME,

                        CREATED_BY = item.CREATED_BY,

                        PARENT_LEVEL = item.PARENT_LEVEL,

                        APP_ID = item.APP_ID,

                        APP_NAME = item.APP_NAME,

                        TOTAL_POSOTIVE_FEEDBACK = item.TOTAL_POSOTIVE_FEEDBACK,

                        TOTAL_NEGITIVE_FEEDBACK = item.TOTAL_NEGITIVE_FEEDBACK,

                        TOTAL_REVIEW_RANK = item.TOTAL_REVIEW_RANK,

                        TOTAL_VISIT = item.TOTAL_VISIT,

                        MODIFIED_DATETIME = item.MODIFIED_DATETIME,

                        MODIFIED_BY = item.MODIFIED_BY,


                    });
                }
            }
            if (this.ALL != null)
            {
                foreach (var item in this.ALL)
                {
                    standards.All.Add(new STANDARDSALL()
                    {
                        APP_ID = item.APP_ID,

                        APP_NAME = item.APP_NAME,

                        MODIFIED_DATETIME = item.MODIFIED_DATETIME,

                        MODIFIED_BY = item.MODIFIED_BY,


                    });
                }
            }
            return standards;
        }

    }

    /// <remarks/>
    //[System.SerializableAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class STANDARDSALL
    {

        //public int aPP_IDField;

        //public string aPP_NAMEField;

        //public string aPP_DESCField;

        //public System.DateTime mODIFIED_DATETIMEField;

        //public string mODIFIED_BYField;

        //public string lAST_VISIT_DATEField;


        //[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "aPP_IDField", DbType = "Int NOT NULL")]
        /// <remarks/>
        public int APP_ID { get; set; }
        
        /// <remarks/>
        public string APP_NAME
        { get; set; }

        /// <remarks/>
        public string APP_DESC
        { get; set; }

        /// <remarks/>
        public string MODIFIED_DATETIME
        { get; set; }
        /// <remarks/>
        public string MODIFIED_BY
        { get; set; }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string LAST_VISIT_DATE
        { get; set; }
    }

    /// <remarks/>
    //[System.SerializableAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class STANDARDSMOST_POPULAR
    {

        //public int aPP_ASSOC_META_DATA_IDField;

        //public int tYPE_IDField;

        //public string tYPE_NAMEField;

        //public string nAMEField;

        //public string dESCRIPTIONSField;

        //public int pARENT_META_DATA_IDField;

        //public int dISPLAY_ORDERField;

        //public string aPP_LOGOField;

        //public string aPP_HEADERField;

        //public string aPP_FOOTERField;

        //public string iS_ACTIVEField;

        //public System.DateTime cREATED_DATETIMEField;

        //public string cREATED_BYField;

        //public int pARENT_LEVELField;

        //public int aPP_IDField;

        //public string aPP_NAMEField;

        //public System.DateTime pUBLISH_DATEField;

        //public string aUTHORField;

        //public string pUBLISH_REMARKField;

        //public int tOTAL_POSOTIVE_FEEDBACKField;

        //public int tOTAL_NEGITIVE_FEEDBACKField;

        //public int tOTAL_REVIEW_RANKField;

        //public int tOTAL_VISITField;

        //public System.DateTime mODIFIED_DATETIMEField;

        //public string mODIFIED_BYField;

        //public int nO_OF_DAYSField;

        //public System.DateTime lAST_VISIT_DateField;


        /// <remarks/>
        public int? APP_ASSOC_META_DATA_ID { get; set; }

        /// <remarks/>
        public int? TYPE_ID { get; set; }

        /// <remarks/>
        public string TYPE_NAME { get; set; }

        /// <remarks/>
        public string NAME { get; set; }

        /// <remarks/>
        public string DESCRIPTIONS { get; set; }

        /// <remarks/>
        public int? PARENT_META_DATA_ID { get; set; }

        /// <remarks/>
        public int? DISPLAY_ORDER { get; set; }

        /// <remarks/>
        public string APP_LOGO { get; set; }

        /// <remarks/>
        public string APP_HEADER { get; set; }

        /// <remarks/>
        public string APP_FOOTER { get; set; }

        /// <remarks/>
        public string IS_ACTIVE { get; set; }

        /// <remarks/>
        public string CREATED_DATETIME { get; set; }

        /// <remarks/>
        public string CREATED_BY { get; set; }
        /// <remarks/>
        public int? PARENT_LEVEL { get; set; }

        /// <remarks/>
        public int? APP_ID { get; set; }

        /// <remarks/>
        public string APP_NAME { get; set; }

        /// <remarks/>
        public int? TOTAL_POSOTIVE_FEEDBACK { get; set; }

        /// <remarks/>
        public int? TOTAL_NEGITIVE_FEEDBACK { get; set; }


        /// <remarks/>
        public int? TOTAL_REVIEW_RANK { get; set; }

        /// <remarks/>
        public int? TOTAL_VISIT { get; set; }

        /// <remarks/>
        public System.DateTime MODIFIED_DATETIME { get; set; }

        /// <remarks/>
        public string MODIFIED_BY { get; set; }
    }

    /// <remarks/>
    //[System.SerializableAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class STANDARDSWHATS_NEW
    {

        //public int aPP_ASSOC_META_DATA_IDField;

        //public int tYPE_IDField;

        //public string tYPE_NAMEField;

        //public string dESCRIPTIONSField;

        //public int pARENT_META_DATA_IDField;

        //public int dISPLAY_ORDERField;


        //public string iS_ACTIVEField;

        //public System.DateTime cREATED_DATETIMEField;

        //public string cREATED_BYField;

        //public int pARENT_LEVELField;

        ////public int aPP_IDField;

        //public string aPP_NAMEField;

        //public System.DateTime pUBLISH_DATEField;

        //public string aUTHORField;

        //public string pUBLISH_REMARKField;

        //public int tOTAL_POSOTIVE_FEEDBACKField;

        //public int tOTAL_NEGITIVE_FEEDBACKField;

        //public int tOTAL_REVIEW_RANKField;

        //public int tOTAL_VISITField;

        //public System.DateTime mODIFIED_DATETIMEField;

        //public string mODIFIED_BYField;

        //public int nO_OF_DAYSField;

        //public System.DateTime lAST_VISIT_DateField;



        /// <remarks/>
        public int APP_ASSOC_META_DATA_ID { get; set; }

        /// <remarks/>
        public int? TYPE_ID { get; set; }

        public int NO_OF_DAYS { get; set; }
        /// <remarks/>
        public string TYPE_NAME { get; set; }
        /// <remarks/>
        public string NAME { get; set; }

        public string AUTHOR { get; set; }

        /// <remarks/>
        public string DESCRIPTIONS { get; set; }

        /// <remarks/>
        public int? PARENT_META_DATA_ID { get; set; }

        /// <remarks/>
        public int? DISPLAY_ORDER { get; set; }

        /// <remarks/>

        /// <remarks/>
        public string PUBLISH_REMARK { get; set; }

        /// <remarks/>
        public string IS_ACTIVE { get; set; }

        /// <remarks/>
        public System.DateTime CREATED_DATETIME { get; set; }

        /// <remarks/>
        public System.DateTime LAST_VISIT_Date { get; set; }

        /// <remarks/>
        public System.DateTime PUBLISH_DATE { get; set; }

        /// <remarks/>
        public string CREATED_BY { get; set; }

        /// <remarks/>
        public int? PARENT_LEVEL { get; set; }

        /// <remarks/>
        public int? APP_ID { get; set; }
        
        /// <remarks/>
        public string APP_NAME { get; set; }

        /// <remarks/>
        public int? TOTAL_POSOTIVE_FEEDBACK { get; set; }

        /// <remarks/>
        public int? TOTAL_NEGITIVE_FEEDBACK { get; set; }

        /// <remarks/>
        public int? TOTAL_REVIEW_RANK { get; set; }

        /// <remarks/>
        public int? TOTAL_VISIT { get; set; }

        /// <remarks/>
        public string MODIFIED_DATETIME { get; set; }

        /// <remarks/>
        public string MODIFIED_BY { get; set; }
    }

    /// <remarks/>
    //[System.SerializableAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class STANDARDSFOR_ME
    {

        //public int aPP_ASSOC_META_DATA_IDField;

        //public int tYPE_IDField;

        //public string tYPE_NAMEField;

        //public string nAMEField;

        //public string dESCRIPTIONSField;

        //public int pARENT_META_DATA_IDField;

        //public int dISPLAY_ORDERField;

        //public string aPP_LOGOField;

        //public string aPP_HEADERField;

        //public string aPP_FOOTERField;

        //public string iS_ACTIVEField;

        //public System.DateTime cREATED_DATETIMEField;

        //public string cREATED_BYField;

        //public int pARENT_LEVELField;

        //public int aPP_IDField;

        //public string aPP_NAMEField;

        //public System.DateTime pUBLISH_DATEField;

        //public string aUTHORField;

        //public string pUBLISH_REMARKField;

        //public int tOTAL_POSOTIVE_FEEDBACKField;

        //public int tOTAL_NEGITIVE_FEEDBACKField;

        //public int tOTAL_REVIEW_RANKField;

        //public int tOTAL_VISITField;

        //public System.DateTime mODIFIED_DATETIMEField;

        //public string mODIFIED_BYField;

        //public int nO_OF_DAYSField;

        //public System.DateTime lAST_VISIT_DateField;



        /// <remarks/>
        public int APP_ASSOC_META_DATA_ID { get; set; }
      

        /// <remarks/>
        public int TYPE_ID { get; set; }

        public int NO_OF_DAYS { get; set; }

        /// <remarks/>
        public string TYPE_NAME { get; set; }

        /// <remarks/>
        public string NAME { get; set; }

        public string AUTHOR { get; set; }

        /// <remarks/>
        public string DESCRIPTIONS { get; set; }

        /// <remarks/>
        public int PARENT_META_DATA_ID { get; set; }

        /// <remarks/>
        public int DISPLAY_ORDER { get; set; }

        /// <remarks/>
        public string APP_LOGO { get; set; }

        /// <remarks/>
        public string PUBLISH_REMARK { get; set; }

        /// <remarks/>
        public string APP_HEADER { get; set; }

        /// <remarks/>
        public string APP_FOOTER { get; set; }

        /// <remarks/>
        public string IS_ACTIVE { get; set; }

        /// <remarks/>
        public System.DateTime CREATED_DATETIME { get; set; }

        /// <remarks/>
        public System.DateTime LAST_VISIT_Date { get; set; }

        /// <remarks/>
        public System.DateTime PUBLISH_DATE { get; set; }

        /// <remarks/>
        public string CREATED_BY { get; set; }

        /// <remarks/>
        public int PARENT_LEVEL { get; set; }

        /// <remarks/>
        public int APP_ID { get; set; }

        /// <remarks/>
        public string APP_NAME { get; set; }

        /// <remarks/>
        public int TOTAL_POSOTIVE_FEEDBACK { get; set; }

        /// <remarks/>
        public int TOTAL_NEGITIVE_FEEDBACK { get; set; }

        /// <remarks/>
        public int TOTAL_REVIEW_RANK { get; set; }

        /// <remarks/>
        public int TOTAL_VISIT { get; set; }

        /// <remarks/>
        public System.DateTime MODIFIED_DATETIME { get; set; }

        /// <remarks/>
        public string MODIFIED_BY { get; set; }
    }


  
}