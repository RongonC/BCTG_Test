using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class ItemInfoField
    {
        public int IntAreaID { get; set; }
        public string StrAreaName { get; set; }
        public int intItemInfoFieldID { get; set; }

        public int intItemID { get; set; }

        public string strItemInfoFieldName { get; set; }

        public string strItemInfoFieldDesc { get; set; }

        public int? intDisplayOrder { get; set; }

        public ItemInfoField()
        { }


        public string strExtraField1 { get; set; }

        public string strExtraField2 { get; set; }

        public string strItemName { get; set; }

        public string strSupervisorEmail { get; set; }

        public bool? blnSuppressAlert { get; set; }

        public bool? blnHidePoints { get; set; }

        public int? intExternalDatasourceID { get; set; }

        public string strDisplayName { get; set; }

        public int? intChildCount { get; set; }

        public int? intParentCount { get; set; }

        public string strExternalDatasourceName { get; set; }

        public string strConnectionString { get; set; }

        public string strExternalDatasourceQuery { get; set; }

        // Ashish 09/10/2012
        public int intSelectedVal { get; set; }


        public string strIsRequired { get; set; }

        public string strShowOnPage { get; set; }

        public string strIsProcessCMECase { get; set; }

        public string strIsActive { get; set; }

        public string strIsEmployeeLookup { get; set; }

        public int? ItemInfoFieldParentID { get; set; }

        public string strIsShowCMECheckbox { get; set; }

        public string strIsShowAdditionalNotes { get; set; }
        public string strCreateCaseOnSaveForm { get; set; }
        public string strFieldType { get; set; }

        public string FIELD_SECURITY { get; set; }
    }
}