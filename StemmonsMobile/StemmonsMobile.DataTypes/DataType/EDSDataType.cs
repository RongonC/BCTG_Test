

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    //[System.SerializableAttribute()]
   // [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class FREQUENT
    {

        private FREQUENTCASE[] cASE_LISTField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CASE", IsNullable = false)]
        public FREQUENTCASE[] CASE_LIST
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
   // [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FREQUENTCASE
    {

        private byte cASE_TYPE_IDField;

        private FREQUENTCASEASSOC_TYPE[] aSSOC_TYPEField;

        /// <remarks/>
        public byte CASE_TYPE_ID
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
        [System.Xml.Serialization.XmlElementAttribute("ASSOC_TYPE")]
        public FREQUENTCASEASSOC_TYPE[] ASSOC_TYPE
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
    public partial class FREQUENTCASEASSOC_TYPE
    {

        private string sYSTEM_PRIORITYField;

        private ushort aSSOC_TYPE_IDField;

        private string aSSOC_TYPE_SYSTEM_CODEField;

        private string aSSOC_TYPE_DESCRIPTIONField;

        private string aSSOC_TYPE_NAMEField;

        private FREQUENTCASEASSOC_TYPEEXTERNAL_DATASOURCE[] eXTERNAL_DATASOURCEField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string SYSTEM_PRIORITY
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
        public ushort ASSOC_TYPE_ID
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
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
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
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
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
        public FREQUENTCASEASSOC_TYPEEXTERNAL_DATASOURCE[] EXTERNAL_DATASOURCE
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
   // [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FREQUENTCASEASSOC_TYPEEXTERNAL_DATASOURCE
    {

        private uint eXTERNAL_DATASOURCE_IDField;

        private string eXTERNAL_DATASOURCE_NAMEField;

        private string eXTERNAL_DATASOURCE_DESCRIPTIONField;

        /// <remarks/>
        public uint EXTERNAL_DATASOURCE_ID
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

}
