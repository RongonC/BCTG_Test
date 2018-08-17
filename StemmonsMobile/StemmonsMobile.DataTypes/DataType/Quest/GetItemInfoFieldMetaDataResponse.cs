
using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetItemInfoFieldMetaDataResponse : Response
    {
        public object ResponseContent { get; set; }
        public class ItemInfoFieldMetaData
        {
            public ItemInfoFieldMetaData()
            { }

            public int intItemInstanceTranID { get; set; }

            public int intItemID { get; set; }

            public int intListID { get; set; }

            //public bool? blnIsEdit
            //{
            //    get
            //    {
            //        return strIsActive.ToLower() == "y" ? true : false;
            //    }
            //}

            //private bool? bIsEdit;
            //public bool? blnIsEdit
            //{
            //    get
            //    {
            //        return bIsEdit;
            //    }
            //    set
            //    {
            //        bIsEdit = strIsActive.ToUpper() == "Y" ? true : false;
            //    }
            //}

            public bool? blnIsEdit { get; set; }

            public string strIsActive { get; set; }

            public string strItemName { get; set; }

            public string strOtherComments { get; set; }

            public int intItemInfoMetadataID { get; set; }

            public int intItemInfoFieldID { get; set; }

            public string strItemInfoFieldName { get; set; }

            public string strItemInfoFieldValue { get; set; }

            public int intAreaID { get; set; }

            public string strAreaName { get; set; }

            public string strSupervisorEmail { get; set; }

            public bool? blnSuppressAlert { get; set; }

            public bool? blnHidePoints { get; set; }

            public string strDisplayName { get; set; }

            public int? intExternalDatasourceID { get; set; }

            public string strExternalDatasourceObjectID { get; set; }

            public string strShowOnPage { get; set; }

            public string strExtraField1 { get; set; }

            public string strIsEmployeeLookup { get; set; }

            public string strExternalDataSourceName { get; set; }

            public string strFormCreator { get; set; }

            public DateTime? dtFormCreatedDatetime { get; set; }

            public decimal? sFormScore { get; set; }

            public string strQuestion { get; set; }

            public string strFieldType { get; set; }

            public string _IS_REQUIRED { get; set; }

            public string FIELD_SECURITY { get; set; }
        }
    }
}