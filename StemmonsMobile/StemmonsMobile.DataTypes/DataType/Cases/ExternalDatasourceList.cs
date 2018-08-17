using System;
using System.Collections.Generic;
using System.Linq;

namespace StemmonsMobile.DataTypes.DataType.Cases
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class FREQUENT
    {

        private FREQUENTCASE_LIST cASE_LISTField;

        /// <remarks/>
        public FREQUENTCASE_LIST CASE_LIST
        {
            get
            {
                return this.cASE_LISTField;
            }
            set
            {
                this.cASE_LISTField = value;
            }
        }
    }

    /// <remarks/>

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FREQUENTCASE_LIST
    {

        private FREQUENTCASE_LISTCASE cASEField;

        /// <remarks/>
        public FREQUENTCASE_LISTCASE CASE
        {
            get
            {
                return this.cASEField;
            }
            set
            {
                this.cASEField = value;
            }
        }
    }

    /// <remarks/>

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FREQUENTCASE_LISTCASE
    {

        private int cASE_TYPE_IDField;

        private FREQUENTCASE_LISTCASEASSOC_TYPE aSSOC_TYPEField;

        /// <remarks/>
        public int CASE_TYPE_ID
        {
            get
            {
                return this.cASE_TYPE_IDField;
            }
            set
            {
                this.cASE_TYPE_IDField = value;
            }
        }

        /// <remarks/>
        public FREQUENTCASE_LISTCASEASSOC_TYPE ASSOC_TYPE
        {
            get
            {
                return this.aSSOC_TYPEField;
            }
            set
            {
                this.aSSOC_TYPEField = value;
            }
        }
    }

    /// <remarks/>

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FREQUENTCASE_LISTCASEASSOC_TYPE
    {

        private int? sYSTEM_PRIORITYField;

        private int aSSOC_TYPE_IDField;

        private string aSSOC_TYPE_SYSTEM_CODEField;

        private string aSSOC_TYPE_DESCRIPTIONField;

        private string aSSOC_TYPE_NAMEField;

        private FREQUENTCASE_LISTCASEASSOC_TYPEEXTERNAL_DATASOURCE[] eXTERNAL_DATASOURCEField;

        /// <remarks/>
        public int? SYSTEM_PRIORITY
        {
            get
            {
                return this.sYSTEM_PRIORITYField;
            }
            set
            {
                this.sYSTEM_PRIORITYField = value;
            }
        }

        /// <remarks/>
        public int ASSOC_TYPE_ID
        {
            get
            {
                return this.aSSOC_TYPE_IDField;
            }
            set
            {
                this.aSSOC_TYPE_IDField = value;
            }
        }

        /// <remarks/>
        public string ASSOC_TYPE_SYSTEM_CODE
        {
            get
            {
                return this.aSSOC_TYPE_SYSTEM_CODEField;
            }
            set
            {
                this.aSSOC_TYPE_SYSTEM_CODEField = value;
            }
        }

        /// <remarks/>
        public string ASSOC_TYPE_DESCRIPTION
        {
            get
            {
                return this.aSSOC_TYPE_DESCRIPTIONField;
            }
            set
            {
                this.aSSOC_TYPE_DESCRIPTIONField = value;
            }
        }

        /// <remarks/>
        public string ASSOC_TYPE_NAME
        {
            get
            {
                return this.aSSOC_TYPE_NAMEField;
            }
            set
            {
                this.aSSOC_TYPE_NAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("EXTERNAL_DATASOURCE")]
        public FREQUENTCASE_LISTCASEASSOC_TYPEEXTERNAL_DATASOURCE[] EXTERNAL_DATASOURCE
        {
            get
            {
                return this.eXTERNAL_DATASOURCEField;
            }
            set
            {
                this.eXTERNAL_DATASOURCEField = value;
            }
        }
    }

    /// <remarks/>

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FREQUENTCASE_LISTCASEASSOC_TYPEEXTERNAL_DATASOURCE
    {

        private int eXTERNAL_DATASOURCE_IDField;

        private string eXTERNAL_DATASOURCE_NAMEField;

        private string eXTERNAL_DATASOURCE_DESCRIPTIONField;

        /// <remarks/>
        public int EXTERNAL_DATASOURCE_ID
        {
            get
            {
                return this.eXTERNAL_DATASOURCE_IDField;
            }
            set
            {
                this.eXTERNAL_DATASOURCE_IDField = value;
            }
        }

        /// <remarks/>
        public string EXTERNAL_DATASOURCE_NAME
        {
            get
            {
                return this.eXTERNAL_DATASOURCE_NAMEField;
            }
            set
            {
                this.eXTERNAL_DATASOURCE_NAMEField = value;
            }
        }

        /// <remarks/>
        public string EXTERNAL_DATASOURCE_DESCRIPTION
        {
            get
            {
                return this.eXTERNAL_DATASOURCE_DESCRIPTIONField;
            }
            set
            {
                this.eXTERNAL_DATASOURCE_DESCRIPTIONField = value;
            }
        }
    }




    public class ExternalDatasourceList
    {
        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class EXTERNAL_DATASOURCE_LIST
        {

            private EXTERNAL_DATASOURCE_LISTFREQUENT fREQUENTField;

            private EXTERNAL_DATASOURCE_LISTFULL fULLField;

            /// <remarks/>
            public EXTERNAL_DATASOURCE_LISTFREQUENT FREQUENT
            {
                get
                {
                    return this.fREQUENTField;
                }
                set
                {
                    this.fREQUENTField = value;
                }
            }

            /// <remarks/>
            public EXTERNAL_DATASOURCE_LISTFULL FULL
            {
                get
                {
                    return this.fULLField;
                }
                set
                {
                    this.fULLField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class EXTERNAL_DATASOURCE_LISTFREQUENT
        {

            private EXTERNAL_DATASOURCE_LISTFREQUENTCASE_LIST cASE_LISTField;

            /// <remarks/>
            public EXTERNAL_DATASOURCE_LISTFREQUENTCASE_LIST CASE_LIST
            {
                get
                {
                    return this.cASE_LISTField;
                }
                set
                {
                    this.cASE_LISTField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class EXTERNAL_DATASOURCE_LISTFREQUENTCASE_LIST
        {

            private EXTERNAL_DATASOURCE_LISTFREQUENTCASE_LISTCASE cASEField;

            /// <remarks/>
            public EXTERNAL_DATASOURCE_LISTFREQUENTCASE_LISTCASE CASE
            {
                get
                {
                    return this.cASEField;
                }
                set
                {
                    this.cASEField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class EXTERNAL_DATASOURCE_LISTFREQUENTCASE_LISTCASE
        {

            private int cASE_TYPE_IDField;

            private EXTERNAL_DATASOURCE_LISTFREQUENTCASE_LISTCASEASSOC_TYPE aSSOC_TYPEField;

            /// <remarks/>
            public int CASE_TYPE_ID
            {
                get
                {
                    return this.cASE_TYPE_IDField;
                }
                set
                {
                    this.cASE_TYPE_IDField = value;
                }
            }

            /// <remarks/>
            public EXTERNAL_DATASOURCE_LISTFREQUENTCASE_LISTCASEASSOC_TYPE ASSOC_TYPE
            {
                get
                {
                    return this.aSSOC_TYPEField;
                }
                set
                {
                    this.aSSOC_TYPEField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class EXTERNAL_DATASOURCE_LISTFREQUENTCASE_LISTCASEASSOC_TYPE
        {

            private int? sYSTEM_PRIORITYField;

            private long aSSOC_TYPE_IDField;

            private string aSSOC_TYPE_SYSTEM_CODEField;

            private string aSSOC_TYPE_DESCRIPTIONField;

            private string aSSOC_TYPE_NAMEField;

            private EXTERNAL_DATASOURCE_LISTFREQUENTCASE_LISTCASEASSOC_TYPEEXTERNAL_DATASOURCE[] eXTERNAL_DATASOURCEField;

            /// <remarks/>
            public int? SYSTEM_PRIORITY
            {
                get
                {
                    return this.sYSTEM_PRIORITYField;
                }
                set
                {
                    this.sYSTEM_PRIORITYField = value;
                }
            }

            /// <remarks/>
            public long ASSOC_TYPE_ID
            {
                get
                {
                    return this.aSSOC_TYPE_IDField;
                }
                set
                {
                    this.aSSOC_TYPE_IDField = value;
                }
            }

            /// <remarks/>
            public string ASSOC_TYPE_SYSTEM_CODE
            {
                get
                {
                    return this.aSSOC_TYPE_SYSTEM_CODEField;
                }
                set
                {
                    this.aSSOC_TYPE_SYSTEM_CODEField = value;
                }
            }

            /// <remarks/>
            public string ASSOC_TYPE_DESCRIPTION
            {
                get
                {
                    return this.aSSOC_TYPE_DESCRIPTIONField;
                }
                set
                {
                    this.aSSOC_TYPE_DESCRIPTIONField = value;
                }
            }

            /// <remarks/>
            public string ASSOC_TYPE_NAME
            {
                get
                {
                    return this.aSSOC_TYPE_NAMEField;
                }
                set
                {
                    this.aSSOC_TYPE_NAMEField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("EXTERNAL_DATASOURCE")]
            public EXTERNAL_DATASOURCE_LISTFREQUENTCASE_LISTCASEASSOC_TYPEEXTERNAL_DATASOURCE[] EXTERNAL_DATASOURCE
            {
                get
                {
                    return this.eXTERNAL_DATASOURCEField;
                }
                set
                {
                    this.eXTERNAL_DATASOURCEField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class EXTERNAL_DATASOURCE_LISTFREQUENTCASE_LISTCASEASSOC_TYPEEXTERNAL_DATASOURCE
        {

            private int eXTERNAL_DATASOURCE_IDField;

            private string eXTERNAL_DATASOURCE_NAMEField;

            private string eXTERNAL_DATASOURCE_DESCRIPTIONField;

            private string cONNECTION_STRINGField;

            private string qUERYField;

            private string nAMEField;

            private string oBJECT_IDField;

            private string oBJECT_DISPLAYField;

            private string oBJECT_DESCRIPTIONField;

            private object uRL_DRILL_INTOField;

            private object sYSTEM_CODEField;

            private string entity_type_IDField;

            private int countField;

            /// <remarks/>
            public int EXTERNAL_DATASOURCE_ID
            {
                get
                {
                    return this.eXTERNAL_DATASOURCE_IDField;
                }
                set
                {
                    this.eXTERNAL_DATASOURCE_IDField = value;
                }
            }

            /// <remarks/>
            public string EXTERNAL_DATASOURCE_NAME
            {
                get
                {
                    return this.eXTERNAL_DATASOURCE_NAMEField;
                }
                set
                {
                    this.eXTERNAL_DATASOURCE_NAMEField = value;
                }
            }

            /// <remarks/>
            public string EXTERNAL_DATASOURCE_DESCRIPTION
            {
                get
                {
                    return this.eXTERNAL_DATASOURCE_DESCRIPTIONField;
                }
                set
                {
                    this.eXTERNAL_DATASOURCE_DESCRIPTIONField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string CONNECTION_STRING
            {
                get
                {
                    return this.cONNECTION_STRINGField;
                }
                set
                {
                    this.cONNECTION_STRINGField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string QUERY
            {
                get
                {
                    return this.qUERYField;
                }
                set
                {
                    this.qUERYField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
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
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string OBJECT_ID
            {
                get
                {
                    return this.oBJECT_IDField;
                }
                set
                {
                    this.oBJECT_IDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string OBJECT_DISPLAY
            {
                get
                {
                    return this.oBJECT_DISPLAYField;
                }
                set
                {
                    this.oBJECT_DISPLAYField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string OBJECT_DESCRIPTION
            {
                get
                {
                    return this.oBJECT_DESCRIPTIONField;
                }
                set
                {
                    this.oBJECT_DESCRIPTIONField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object URL_DRILL_INTO
            {
                get
                {
                    return this.uRL_DRILL_INTOField;
                }
                set
                {
                    this.uRL_DRILL_INTOField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object SYSTEM_CODE
            {
                get
                {
                    return this.sYSTEM_CODEField;
                }
                set
                {
                    this.sYSTEM_CODEField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string Entity_type_ID
            {
                get
                {
                    return this.entity_type_IDField;
                }
                set
                {
                    this.entity_type_IDField = value;
                }
            }

            /// <remarks/>
            public int count
            {
                get
                {
                    return this.countField;
                }
                set
                {
                    this.countField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class EXTERNAL_DATASOURCE_LISTFULL
        {

            private EXTERNAL_DATASOURCE_LISTFULLCASE_LIST cASE_LISTField;

            /// <remarks/>
            public EXTERNAL_DATASOURCE_LISTFULLCASE_LIST CASE_LIST
            {
                get
                {
                    return this.cASE_LISTField;
                }
                set
                {
                    this.cASE_LISTField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class EXTERNAL_DATASOURCE_LISTFULLCASE_LIST
        {

            private EXTERNAL_DATASOURCE_LISTFULLCASE_LISTCASE cASEField;

            /// <remarks/>
            public EXTERNAL_DATASOURCE_LISTFULLCASE_LISTCASE CASE
            {
                get
                {
                    return this.cASEField;
                }
                set
                {
                    this.cASEField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class EXTERNAL_DATASOURCE_LISTFULLCASE_LISTCASE
        {

            private int cASE_TYPE_IDField;

            private EXTERNAL_DATASOURCE_LISTFULLCASE_LISTCASEASSOC_TYPE aSSOC_TYPEField;

            /// <remarks/>
            public int CASE_TYPE_ID
            {
                get
                {
                    return this.cASE_TYPE_IDField;
                }
                set
                {
                    this.cASE_TYPE_IDField = value;
                }
            }

            /// <remarks/>
            public EXTERNAL_DATASOURCE_LISTFULLCASE_LISTCASEASSOC_TYPE ASSOC_TYPE
            {
                get
                {
                    return this.aSSOC_TYPEField;
                }
                set
                {
                    this.aSSOC_TYPEField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class EXTERNAL_DATASOURCE_LISTFULLCASE_LISTCASEASSOC_TYPE
        {

            private int? sYSTEM_PRIORITYField;

            private long aSSOC_TYPE_IDField;

            private string aSSOC_TYPE_SYSTEM_CODEField;

            private string aSSOC_TYPE_DESCRIPTIONField;

            private string aSSOC_TYPE_NAMEField;

            private EXTERNAL_DATASOURCE_LISTFULLCASE_LISTCASEASSOC_TYPEEXTERNAL_DATASOURCE[] eXTERNAL_DATASOURCEField;

            /// <remarks/>
            public int? SYSTEM_PRIORITY
            {
                get
                {
                    return this.sYSTEM_PRIORITYField;
                }
                set
                {
                    this.sYSTEM_PRIORITYField = value;
                }
            }

            /// <remarks/>
            public long ASSOC_TYPE_ID
            {
                get
                {
                    return this.aSSOC_TYPE_IDField;
                }
                set
                {
                    this.aSSOC_TYPE_IDField = value;
                }
            }

            /// <remarks/>
            public string ASSOC_TYPE_SYSTEM_CODE
            {
                get
                {
                    return this.aSSOC_TYPE_SYSTEM_CODEField;
                }
                set
                {
                    this.aSSOC_TYPE_SYSTEM_CODEField = value;
                }
            }

            /// <remarks/>
            public string ASSOC_TYPE_DESCRIPTION
            {
                get
                {
                    return this.aSSOC_TYPE_DESCRIPTIONField;
                }
                set
                {
                    this.aSSOC_TYPE_DESCRIPTIONField = value;
                }
            }

            /// <remarks/>
            public string ASSOC_TYPE_NAME
            {
                get
                {
                    return this.aSSOC_TYPE_NAMEField;
                }
                set
                {
                    this.aSSOC_TYPE_NAMEField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("EXTERNAL_DATASOURCE")]
            public EXTERNAL_DATASOURCE_LISTFULLCASE_LISTCASEASSOC_TYPEEXTERNAL_DATASOURCE[] EXTERNAL_DATASOURCE
            {
                get
                {
                    return this.eXTERNAL_DATASOURCEField;
                }
                set
                {
                    this.eXTERNAL_DATASOURCEField = value;
                }
            }
        }

        /// <remarks/>
        //[System.SerializableAttribute()]
        //[System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class EXTERNAL_DATASOURCE_LISTFULLCASE_LISTCASEASSOC_TYPEEXTERNAL_DATASOURCE
        {

            private int eXTERNAL_DATASOURCE_IDField;

            private string eXTERNAL_DATASOURCE_NAMEField;

            private string eXTERNAL_DATASOURCE_DESCRIPTIONField;

            private string cONNECTION_STRINGField;

            private string qUERYField;

            private string nAMEField;

            private string oBJECT_IDField;

            private string oBJECT_DISPLAYField;

            private string oBJECT_DESCRIPTIONField;

            private object uRL_DRILL_INTOField;

            private object sYSTEM_CODEField;

            private string entity_type_IDField;

            /// <remarks/>
            public int EXTERNAL_DATASOURCE_ID
            {
                get
                {
                    return this.eXTERNAL_DATASOURCE_IDField;
                }
                set
                {
                    this.eXTERNAL_DATASOURCE_IDField = value;
                }
            }

            /// <remarks/>
            public string EXTERNAL_DATASOURCE_NAME
            {
                get
                {
                    return this.eXTERNAL_DATASOURCE_NAMEField;
                }
                set
                {
                    this.eXTERNAL_DATASOURCE_NAMEField = value;
                }
            }

            /// <remarks/>
            public string EXTERNAL_DATASOURCE_DESCRIPTION
            {
                get
                {
                    return this.eXTERNAL_DATASOURCE_DESCRIPTIONField;
                }
                set
                {
                    this.eXTERNAL_DATASOURCE_DESCRIPTIONField = value;
                }
            }

            /// <remarks/>
            public string CONNECTION_STRING
            {
                get
                {
                    return this.cONNECTION_STRINGField;
                }
                set
                {
                    this.cONNECTION_STRINGField = value;
                }
            }

            /// <remarks/>
            public string QUERY
            {
                get
                {
                    return this.qUERYField;
                }
                set
                {
                    this.qUERYField = value;
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
            public string OBJECT_ID
            {
                get
                {
                    return this.oBJECT_IDField;
                }
                set
                {
                    this.oBJECT_IDField = value;
                }
            }

            /// <remarks/>
            public string OBJECT_DISPLAY
            {
                get
                {
                    return this.oBJECT_DISPLAYField;
                }
                set
                {
                    this.oBJECT_DISPLAYField = value;
                }
            }

            /// <remarks/>
            public string OBJECT_DESCRIPTION
            {
                get
                {
                    return this.oBJECT_DESCRIPTIONField;
                }
                set
                {
                    this.oBJECT_DESCRIPTIONField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object URL_DRILL_INTO
            {
                get
                {
                    return this.uRL_DRILL_INTOField;
                }
                set
                {
                    this.uRL_DRILL_INTOField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object SYSTEM_CODE
            {
                get
                {
                    return this.sYSTEM_CODEField;
                }
                set
                {
                    this.sYSTEM_CODEField = value;
                }
            }

            /// <remarks/>
            public string Entity_type_ID
            {
                get
                {
                    return this.entity_type_IDField;
                }
                set
                {
                    this.entity_type_IDField = value;
                }
            }
        }
    }

}