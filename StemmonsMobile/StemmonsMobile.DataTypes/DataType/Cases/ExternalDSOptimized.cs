using System;
using System.Collections.Generic;
using System.Linq;

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class EXTERNAL_DATASOURCE_DATA
    {

        private EXTERNAL_DATASOURCE_DATAEXTERNAL_DATASOURCE[] eXTERNAL_DATASOURCEField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("EXTERNAL_DATASOURCE")]
        public EXTERNAL_DATASOURCE_DATAEXTERNAL_DATASOURCE[] EXTERNAL_DATASOURCE
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
    public partial class EXTERNAL_DATASOURCE_DATAEXTERNAL_DATASOURCE
    {

        private ushort eXTERNAL_DATASOURCE_IDField;

        private EXTERNAL_DATASOURCE_DATAEXTERNAL_DATASOURCEEXTERNAL_DATASOURCE_OBJECT[] eXTERNAL_DATASOURCE_OBJECTField;

        /// <remarks/>
        public ushort EXTERNAL_DATASOURCE_ID
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
        [System.Xml.Serialization.XmlElementAttribute("EXTERNAL_DATASOURCE_OBJECT")]
        public EXTERNAL_DATASOURCE_DATAEXTERNAL_DATASOURCEEXTERNAL_DATASOURCE_OBJECT[] EXTERNAL_DATASOURCE_OBJECT
        {
            get
            {
                return this.eXTERNAL_DATASOURCE_OBJECTField;
            }
            set
            {
                this.eXTERNAL_DATASOURCE_OBJECTField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class EXTERNAL_DATASOURCE_DATAEXTERNAL_DATASOURCEEXTERNAL_DATASOURCE_OBJECT
    {

        private uint idField;

        private string nAMEField;

        private string dESCRIPTIONField;

        /// <remarks/>
        public uint ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
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
        public string DESCRIPTION
        {
            get
            {
                return this.dESCRIPTIONField;
            }
            set
            {
                this.dESCRIPTIONField = value;
            }
        }
    }

}