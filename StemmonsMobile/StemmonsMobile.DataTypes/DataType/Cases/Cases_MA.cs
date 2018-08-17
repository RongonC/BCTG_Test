using System;
using System.Collections.Generic;
using System.Linq;



namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class Cases_MA
    {
        public int CASE_ID { get; set; }
        public int CASE_TYPE_ID { get; set; }
        public List<AssocType> ASSOC_TYPE_COLLECTION { get; set; }
    }

    public class AssocType
    {
        public int? SYSTEM_PRIORITY;
        public int ASSOC_TYPE_ID;
        public string ASSOC_TYPE_SYSTEM_CODE;
        public string ASSOC_TYPE_DESCRIPTION;
        public string ASSOC_TYPE_NAME;
        public List<ExternalDatasource> EXTERNAL_DATASOURCE_Collection;
        public List<AssocDecode> Assoc_Decode_Collection;
        //public List<AssocMetadata> ASSOC_METADATA_CL;
        //public List<AssocMetadataText> ASSOC_METADATA_TEXT_CL;
    }

    public class AssocDecode
    {
        public string ASSOC_DECODE_SYSTEM_CODE;
        public string ASSOC_DECODE_NAME;
        public string ASSOC_DECODE_DESCRIPTION;
    }

    public class ExternalDatasource
    {
        public int EXTERNAL_DATASOURCE_ID;
        public string EXTERNAL_DATASOURCE_NAME;
        public string EXTERNAL_DATASOURCE_DESCRIPTION;
        public int count;
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class CASE_LIST
    {
        private CASE_LISTMA[] cCASESField;
        [System.Xml.Serialization.XmlElementAttribute("CASE")]
        public CASE_LISTMA[] CASES
        {
            get
            {
                return this.cCASESField;
            }
            set
            {
                this.cCASESField = value;
            }
        }

        public List<Cases_MA> DeserializeCases()
        {
            List<Cases_MA> CasesList = new List<Cases_MA>();
            foreach (CASE_LISTMA cItem in this.cCASESField)
            {
                Cases_MA cases = new Cases_MA()
                {
                    CASE_ID = cItem.CASE_ID,
                    CASE_TYPE_ID = cItem.CASE_TYPE_ID,
                    ASSOC_TYPE_COLLECTION = new List<AssocType>()
                };
                if (cItem.ASSOC_TYPE != null)
                {
                    foreach (var item in cItem.ASSOC_TYPE)
                    {
                        AssocType AssocType = new AssocType()
                        {
                            SYSTEM_PRIORITY = item.SYSTEM_PRIORITY,
                            ASSOC_TYPE_ID = item.ASSOC_TYPE_ID,
                            ASSOC_TYPE_NAME = item.ASSOC_TYPE_NAME,
                            ASSOC_TYPE_SYSTEM_CODE = item.ASSOC_TYPE_SYSTEM_CODE,
                            ASSOC_TYPE_DESCRIPTION = item.ASSOC_TYPE_DESCRIPTION,
                            EXTERNAL_DATASOURCE_Collection = new List<ExternalDatasource>(),
                            Assoc_Decode_Collection = new List<AssocDecode>()
                        };
                        if (item.EXTERNAL_DATASOURCE != null)
                        {
                            foreach (var externalDatasource in item.EXTERNAL_DATASOURCE)
                            {
                                AssocType.EXTERNAL_DATASOURCE_Collection.Add(new ExternalDatasource()
                                {
                                    EXTERNAL_DATASOURCE_ID = externalDatasource.EXTERNAL_DATASOURCE_ID,
                                    EXTERNAL_DATASOURCE_NAME = externalDatasource.EXTERNAL_DATASOURCE_NAME,
                                    EXTERNAL_DATASOURCE_DESCRIPTION = externalDatasource.EXTERNAL_DATASOURCE_DESCRIPTION,
                                    count = externalDatasource.count
                                });
                            }
                        }
                        if (item.ASSOC_DECODE != null)
                        {
                            foreach (var AssocDecode in item.ASSOC_DECODE)
                            {
                                AssocType.Assoc_Decode_Collection.Add(new AssocDecode
                                {
                                    ASSOC_DECODE_DESCRIPTION = AssocDecode.ASSOC_DECODE_DESCRIPTION,
                                    ASSOC_DECODE_NAME = AssocDecode.ASSOC_DECODE_NAME,
                                    ASSOC_DECODE_SYSTEM_CODE = AssocDecode.ASSOC_DECODE_SYSTEM_CODE
                                });
                            }
                        }
                        cases.ASSOC_TYPE_COLLECTION.Add(AssocType);
                    }
                    CasesList.Add(cases);
                }
            }
            return CasesList;
        }
    }
    public partial class CASE_LISTMA
    {
        #region "variable property"
        private int cCASE_IDField;
        private int cCASE_TYPE_IDField;
        private ASSOC_TYPE[] cASSOC_TYPE;

        public int CASE_ID
        {
            get
            {
                return this.cCASE_IDField;
            }
            set
            {
                this.cCASE_IDField = value;
            }
        }

        public int CASE_TYPE_ID
        {
            get
            {
                return this.cCASE_TYPE_IDField;
            }
            set
            {
                this.cCASE_TYPE_IDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ASSOC_TYPE")]
        public ASSOC_TYPE[] ASSOC_TYPE
        {
            get
            {
                return this.cASSOC_TYPE;
            }
            set
            {
                this.cASSOC_TYPE = value;
            }
        }
        #endregion
    }

    public partial class ASSOC_TYPE
    {
        private int? cSYSTEM_PRIORITYField;
        private int cASSOC_TYPE_IDField;
        private string cASSOC_TYPE_SYSTEM_CODEField;
        private string cASSOC_TYPE_DESCRIPTIONField;
        private string cASSOC_TYPE_NAMEField;
        private EXTERNAL_DATASOURCE[] cEXTERNAL_DATASOURCEField;
        private ASSOC_DECODE[] cASSOC_DECODEField;

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? SYSTEM_PRIORITY
        {
            get
            {
                return this.cSYSTEM_PRIORITYField;
            }
            set
            {
                this.cSYSTEM_PRIORITYField = value;
            }
        }

        public int ASSOC_TYPE_ID
        {
            get
            {
                return this.cASSOC_TYPE_IDField;
            }
            set
            {
                this.cASSOC_TYPE_IDField = value;
            }
        }

        public string ASSOC_TYPE_SYSTEM_CODE
        {
            get
            {
                return this.cASSOC_TYPE_SYSTEM_CODEField;
            }
            set
            {
                this.cASSOC_TYPE_SYSTEM_CODEField = value;
            }
        }

        public string ASSOC_TYPE_DESCRIPTION
        {
            get
            {
                return this.cASSOC_TYPE_DESCRIPTIONField;
            }
            set
            {
                this.cASSOC_TYPE_DESCRIPTIONField = value;
            }
        }

        public string ASSOC_TYPE_NAME
        {
            get
            {
                return this.cASSOC_TYPE_NAMEField;
            }
            set
            {
                this.cASSOC_TYPE_NAMEField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("EXTERNAL_DATASOURCE")]
        public EXTERNAL_DATASOURCE[] EXTERNAL_DATASOURCE
        {
            get
            {
                return this.cEXTERNAL_DATASOURCEField;
            }
            set
            {
                this.cEXTERNAL_DATASOURCEField = value;
            }
        }
        [System.Xml.Serialization.XmlElementAttribute("ASSOC_DECODE")]
        public ASSOC_DECODE[] ASSOC_DECODE
        {
            get
            {
                return this.cASSOC_DECODEField;
            }
            set
            {
                this.cASSOC_DECODEField = value;
            }
        }

    }

    public partial class ASSOC_DECODE
    {
        private string cASSOC_DECODE_SYSTEM_CODEField;
        private string cASSOC_DECODE_NAMEField;
        private string cASSOC_DECODE_DESCRIPTIONField;

        public string ASSOC_DECODE_SYSTEM_CODE
        {
            get
            {
                return this.cASSOC_DECODE_SYSTEM_CODEField;
            }
            set
            {
                this.cASSOC_DECODE_SYSTEM_CODEField = value;
            }
        }

        public string ASSOC_DECODE_NAME
        {
            get
            {
                return this.cASSOC_DECODE_NAMEField;
            }
            set
            {
                this.cASSOC_DECODE_NAMEField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ASSOC_DECODE_DESCRIPTION
        {
            get
            {
                return this.cASSOC_DECODE_DESCRIPTIONField;
            }
            set
            {
                this.cASSOC_DECODE_DESCRIPTIONField = value;
            }
        }
    }

    public partial class EXTERNAL_DATASOURCE
    {
        private int cEXTERNAL_DATASOURCE_IDField;
        private string cEXTERNAL_DATASOURCE_NAMEField;
        private string cEXTERNAL_DATASOURCE_DESCRIPTIONField;
        private int cCountFields;

        public int count
        {
            get
            {
                return this.cCountFields;
            }
            set
            {
                this.cCountFields = value;
            }
        }
        public int EXTERNAL_DATASOURCE_ID
        {
            get
            {
                return this.cEXTERNAL_DATASOURCE_IDField;
            }
            set
            {
                this.cEXTERNAL_DATASOURCE_IDField = value;
            }
        }


        public string EXTERNAL_DATASOURCE_NAME
        {
            get
            {
                return this.cEXTERNAL_DATASOURCE_NAMEField;
            }
            set
            {
                this.cEXTERNAL_DATASOURCE_NAMEField = value;
            }
        }


        public string EXTERNAL_DATASOURCE_DESCRIPTION
        {
            get
            {
                return this.cEXTERNAL_DATASOURCE_DESCRIPTIONField;
            }
            set
            {
                this.cEXTERNAL_DATASOURCE_DESCRIPTIONField = value;
            }
        }
    }
}