#region Master API Entity List Model
/* 
Licensed under the Apache License, Version 2.0

http://www.apache.org/licenses/LICENSE-2.0
*/
using System;
using System.Xml.Serialization;
using System.Collections.Generic;

[XmlRoot(ElementName = "ASSOC_TYPE")]
public class ASSOC_TYPE
{
    [XmlElement(ElementName = "ENTITY_ASSOC_TYPE_ID")]
    public int ENTITY_ASSOC_TYPE_ID { get; set; }

    [XmlElement(ElementName = "ENTITY_TYPE_ID")]
    public string ENTITY_TYPE_ID { get; set; }

    [XmlElement(ElementName = "FIELD_TYPE")]
    public string FIELD_TYPE { get; set; }

    [XmlElement(ElementName = "FIELD_SUB_TYPE")]
    public string FIELD_SUB_TYPE { get; set; }

    [XmlElement(ElementName = "ENTITY_CURRENCY_TYPE_ID")]
    public int? ENTITY_CURRENCY_TYPE_ID { get; set; }

    [XmlElement(ElementName = "ENTITY_CURRENCY_TYPE_INDICATOR")]
    public string ENTITY_CURRENCY_TYPE_INDICATOR { get; set; }

    [XmlElement(ElementName = "ASSOC_TYPE_NAME")]
    public string ASSOC_TYPE_NAME { get; set; }

    [XmlElement(ElementName = "ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID")]
    public int? ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID { get; set; }

    [XmlElement(ElementName = "ASSOC_TYPE_SYSTEM_CODE")]
    public string ASSOC_TYPE_SYSTEM_CODE { get; set; }

    [XmlElement(ElementName = "SHOW_ON_LIST")]
    public string SHOW_ON_LIST { get; set; }

    [XmlElement(ElementName = "IS_REQUIRED")]
    public string IS_REQUIRED { get; set; }

    [XmlElement(ElementName = "IS_READ_ONLY")]
    public string IS_READ_ONLY { get; set; }
    [XmlElement(ElementName = "MAX_LENGTH")]
    public int? MAX_LENGTH { get; set; }

    [XmlElement(ElementName = "ALLOWED_REGEX")]
    public string ALLOWED_REGEX { get; set; }

    [XmlElement(ElementName = "CALCULATION_FORMULA")]
    public string CALCULATION_FORMULA { get; set; }

    [XmlElement(ElementName = "DECIMAL_PRECESION")]
    public int? DECIMAL_PRECESION { get; set; }

    [XmlElement(ElementName = "CALCULATION_FREQUENCY_MIN")]
    public int? CALCULATION_FREQUENCY_MIN { get; set; }

    [XmlElement(ElementName = "ASSOC_DECODE_COLLECTION")]
    public ASSOC_DECODE_COLLECTION ASSOC_DECODE_COLLECTION { get; set; }

    [XmlElement(ElementName = "ITEM_MOBILE_PRIORITY_VALUE")]
    public int? ITEM_MOBILE_PRIORITY_VALUE { get; set; }

    [XmlElement(ElementName = "EXTERNAL_DATASOURCE")]
    public EXTERNAL_DATASOURCE EXTERNAL_DATASOURCE { get; set; }


    [XmlElement(ElementName = "ENTITY_ASSOC_TYPE_CASCADE_COLLECTION")]
    public ENTITY_ASSOC_TYPE_CASCADE_COLLECTION ENTITY_ASSOC_TYPE_CASCADE_COLLECTION { get; set; }

    [XmlElement(ElementName = "LIST_DESKTOP_PRIORITY_VALUE")]
    public int? LIST_DESKTOP_PRIORITY_VALUE { get; set; }

    [XmlElement(ElementName = "LIST_MOBILE_PRIORITY_VALUE")]
    public int? LIST_MOBILE_PRIORITY_VALUE { get; set; }

    [XmlElement(ElementName = "ITEM_DESKTOP_PRIORITY_VALUE")]
    public int? ITEM_DESKTOP_PRIORITY_VALUE { get; set; }

    [XmlElement(ElementName = "SECURITY_TYPE")]
    public string SECURITY_TYPE { get; set; }
}

[XmlRoot(ElementName = "ASSOC_DECODE_COLLECTION")]
public class ASSOC_DECODE_COLLECTION
{
    [XmlElement(ElementName = "ASSOC_DECODE")]
    public List<ASSOC_DECODE> ASSOC_DECODE { get; set; }

}

[XmlRoot(ElementName = "ASSOC_DECODE")]
public class ASSOC_DECODE
{
    [XmlElement(ElementName = "ENTITY_ASSOC_DECODE_ID")]
    public int ENTITY_ASSOC_DECODE_ID { get; set; }

    [XmlElement(ElementName = "ENTITY_ASSOC_TYPE_ID")]
    public int ENTITY_ASSOC_TYPE_ID { get; set; }

    [XmlElement(ElementName = "ASSOC_DECODE_NAME")]
    public string ASSOC_DECODE_NAME { get; set; }

    [XmlElement(ElementName = "DISPLAY_PRIORITY")]
    public int? DISPLAY_PRIORITY { get; set; }


    [XmlElement(ElementName = "ASSOC_DECODE_SYSTEM_CODE")]
    public string ASSOC_DECODE_SYSTEM_CODE { get; set; }
}

[XmlRoot(ElementName = "ASSOC_TYPE_COLLECTION")]
public class ASSOC_TYPE_COLLECTION
{
    [XmlElement(ElementName = "ASSOC_TYPE")]
    public List<ASSOC_TYPE> ASSOC_TYPE { get; set; }

}

[XmlRoot(ElementName = "ENTITY_TYPE")]
public class ENTITY_TYPEs
{
    [XmlElement(ElementName = "ENTITY_TYPE_ID")]
    public int ENTITY_TYPE_ID { get; set; }

    [XmlElement(ElementName = "ENTITY_TYPE_NAME")]
    public string ENTITY_TYPE_NAME { get; set; }

    [XmlElement(ElementName = "ACTIVE_COUNT")]
    public string ACTIVE_COUNT { get; set; }

    [XmlElement(ElementName = "INACTIVE_COUNT")]
    public string INACTIVE_COUNT { get; set; }

    [XmlElement(ElementName = "OWNER")]
    public string OWNER { get; set; }

    [XmlElement(ElementName = "NEWEST_NOTE_ON_TOP")]
    public string NEWEST_NOTE_ON_TOP { get; set; }

    [XmlElement(ElementName = "HELP_URL")]
    public string HELP_URL { get; set; }


    [XmlElement(ElementName = "HAS_EXTERNALENTITIES_ASSOC_TYPE")]
    public string HAS_EXTERNALENTITIES_ASSOC_TYPE { get; set; }

    [XmlElement(ElementName = "ENTITY_TYPE_CATEGORY_ID")]
    public string ENTITY_TYPE_CATEGORY_ID { get; set; }

    [XmlElement(ElementName = "ENTITY_TYPE_CATEGORY_NAME")]
    public string ENTITY_TYPE_CATEGORY_NAME { get; set; }

    [XmlElement(ElementName = "CATEGORY_SYSTEM_CODE")]
    public string CATEGORY_SYSTEM_CODE { get; set; }

    [XmlElement(ElementName = "TOTAL_COUNT")]
    public string TOTAL_COUNT { get; set; }

    [XmlElement(ElementName = "ASSIGNED_TO_USER")]
    public string ASSIGNED_TO_USER { get; set; }

    [XmlElement(ElementName = "CREATED_BY_USER")]
    public string CREATED_BY_USER { get; set; }

    [XmlElement(ElementName = "OWNED_BY_USER")]
    public string OWNED_BY_USER { get; set; }

    [XmlElement(ElementName = "ASSOCIATED_TO_USER")]
    public string ASSOCIATED_TO_USER { get; set; }

    [XmlElement(ElementName = "INACTIVATED_BY_USER")]
    public string INACTIVATED_BY_USER { get; set; }

    [XmlElement(ElementName = "ASSOC_TYPE_COLLECTION")]
    public ASSOC_TYPE_COLLECTION ASSOC_TYPE_COLLECTION { get; set; }

    [XmlElement(ElementName = "IS_ALLOW_SECURITY_CONFIG")]
    public string IS_ALLOW_SECURITY_CONFIG { get; set; }

    [XmlElement(ElementName = "IS_SHOW_AS_RSS")]
    public string IS_SHOW_AS_RSS { get; set; }


    [XmlElement(ElementName = "ENTITY_TYPE_SECURITY_TYPE")]
    public string ENTITY_TYPE_SECURITY_TYPE { get; set; }

    [XmlElement(ElementName = "ENTITY_TYPE_RELATIONSHIP_COLLECTION")]
    public ENTITY_TYPE_RELATIONSHIP_COLLECTIONSs ENTITY_TYPE_RELATIONSHIP_COLLECTION { get; set; }

    [XmlElement(ElementName = "INSTANCE_NAME_PLURAL")]
    public string INSTANCE_NAME_PLURAL { get; set; }

    [XmlElement(ElementName = "INSTANCE_NAME")]
    public string INSTANCE_NAME { get; set; }

}

[XmlRoot(ElementName = "EXTERNAL_DATASOURCE")]
public class EXTERNAL_DATASOURCE
{
    [XmlElement(ElementName = "ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID")]
    public int ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID { get; set; }

    [XmlElement(ElementName = "EXTERNAL_DATASOURCE_NAME")]
    public string EXTERNAL_DATASOURCE_NAME { get; set; }

    [XmlElement(ElementName = "CONNECTION_STRING")]
    public string CONNECTION_STRING { get; set; }

    [XmlElement(ElementName = "QUERY")]
    public string QUERY { get; set; }

    [XmlElement(ElementName = "URL_DRILL_INTO")]
    public string URL_DRILL_INTO { get; set; }

    [XmlElement(ElementName = "FIELD_SYSTEM_CODE")]
    public string FIELD_SYSTEM_CODE { get; set; }

    [XmlElement(ElementName = "VALUE_SYSTEM_CODE")]
    public string VALUE_SYSTEM_CODE { get; set; }


    [XmlElement(ElementName = "EXTERNAL_DATASOURCE_DESCRIPTION")]
    public string EXTERNAL_DATASOURCE_DESCRIPTION { get; set; }

    [XmlElement(ElementName = "OBJECT_ID")]
    public string OBJECT_ID { get; set; }

    [XmlElement(ElementName = "OBJECT_DISPLAY")]
    public string OBJECT_DISPLAY { get; set; }

    [XmlElement(ElementName = "OBJECT_DESCRIPTION")]
    public string OBJECT_DESCRIPTION { get; set; }
}

[XmlRoot(ElementName = "ENTITY_ASSOC_TYPE_CASCADE_COLLECTION")]
public class ENTITY_ASSOC_TYPE_CASCADE_COLLECTION
{
    [XmlElement(ElementName = "ENTITY_ASSOC_TYPE_CASCADE")]
    public List<ENTITY_ASSOC_TYPE_CASCADEs> ENTITY_ASSOC_TYPE_CASCADE { get; set; }
}

[XmlRoot(ElementName = "ENTITY_ASSOC_TYPE_CASCADE")]
public class ENTITY_ASSOC_TYPE_CASCADEs
{
    [XmlElement(ElementName = "ENTITY_ASSOC_TYPE_CASCADE_ID")]
    public int ENTITY_ASSOC_TYPE_CASCADE_ID { get; set; }

    [XmlElement(ElementName = "CASCADE_TYPE")]
    public string CASCADE_TYPE { get; set; }

    [XmlElement(ElementName = "ENTITY_ASSOC_TYPE_ID_PARENT")]
    public int ENTITY_ASSOC_TYPE_ID_PARENT { get; set; }

    [XmlElement(ElementName = "ENTITY_ASSOC_TYPE_NAME_PARENT")]
    public string ENTITY_ASSOC_TYPE_NAME_PARENT { get; set; }

    [XmlElement(ElementName = "ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID_PARENT")]
    public int ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID_PARENT { get; set; }

    [XmlElement(ElementName = "ENTITY_ASSOC_EXTERNAL_DATASOURCE_NAME_PARENT")]
    public string ENTITY_ASSOC_EXTERNAL_DATASOURCE_NAME_PARENT { get; set; }

    [XmlElement(ElementName = "FILTER_QUERY_PARENT")]
    public string FILTER_QUERY_PARENT { get; set; }

    [XmlElement(ElementName = "ENTITY_ASSOC_TYPE_ID_CHILD")]
    public int ENTITY_ASSOC_TYPE_ID_CHILD { get; set; }

    [XmlElement(ElementName = "ENTITY_ASSOC_TYPE_NAME_CHILD")]
    public string ENTITY_ASSOC_TYPE_NAME_CHILD { get; set; }

    [XmlElement(ElementName = "ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID_CHILD")]
    public int ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID_CHILD { get; set; }

    [XmlElement(ElementName = "ENTITY_ASSOC_EXTERNAL_DATASOURCE_NAME_CHILD")]
    public string ENTITY_ASSOC_EXTERNAL_DATASOURCE_NAME_CHILD { get; set; }

    [XmlElement(ElementName = "FILTER_QUERY_CHILD")]
    public string FILTER_QUERY_CHILD { get; set; }

}

[XmlRoot(ElementName = "ENTITY_TYPE_RELATIONSHIP")]
public class ENTITY_TYPE_RELATIONSHIP
{
    [XmlElement(ElementName = "ENTITY_TYPE_ID")]
    public string ENTITY_TYPE_ID { get; set; }

    [XmlElement(ElementName = "ENTITY_TYPE_NAME")]
    public string ENTITY_TYPE_NAME { get; set; }
}

[XmlRoot(ElementName = "ENTITY_TYPE_RELATIONSHIP_COLLECTION")]
public class ENTITY_TYPE_RELATIONSHIP_COLLECTIONSs
{
    [XmlElement(ElementName = "ENTITY_TYPE_RELATIONSHIP")]
    public List<ENTITY_TYPE_RELATIONSHIP> ENTITY_TYPE_RELATIONSHIP { get; set; }
}

[XmlRoot(ElementName = "ENTITY_TYPE_LIST")]
public class ENTITY_TYPE_LIST
{
    [XmlElement(ElementName = "ENTITY_TYPE")]
    public List<ENTITY_TYPEs> ENTITY_TYPE { get; set; }

}


#endregion


#region External DataSource Model For Master Sync
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

    private byte eNTITY_ASSOC_EXTERNAL_DATASOURCE_IDField;

    private EXTERNAL_DATASOURCE_DATAEXTERNAL_DATASOURCEEXTERNAL_DATASOURCE_OBJECT[] eXTERNAL_DATASOURCE_OBJECTField;

    /// <remarks/>
    public byte ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID
    {
        get
        {
            return this.eNTITY_ASSOC_EXTERNAL_DATASOURCE_IDField;
        }
        set
        {
            this.eNTITY_ASSOC_EXTERNAL_DATASOURCE_IDField = value;
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


#endregion