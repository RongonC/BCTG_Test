
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetExternalDatasourceInfoByIDResponse:Response
    {
        public object ResponseContent { get; set; }


        public class ExternalDatasourceInfo
        {
            public int _EXTERNAL_DATASOURCE_ID;

            public string _EXTERNAL_DATASOURCE_NAME;

            public string _EXTERNAL_DATASOURCE_DESCRIPTION;

            public string _CONNECTION_STRING;

            public string _QUERY;

            public string _OBJECT_ID;

            public string _OBJECT_DISPLAY;

            public string _OBJECT_DESCRIPTION;

            public string _URL_DRILL_INTO;

            public string _SYSTEM_CODE;

            public string _IS_ACTIVE;

        }



        //public class ExternalDataSource
        //{
        //    public ExternalDataSource()
        //    {
        //    }
        //    public int intExternalDataSourceID { get; set; }
        //    public string strName { get; set; }
        //    public string strDescription { get; set; }
        //    public string strConnectionString { get; set; }
        //    public string strQuery { get; set; }
        //    public string strObjectID { get; set; }
        //    public string strObjectDisplay { get; set; }
        //    public string strObjectDescription { get; set; }
        //    public string URLDrillInto { get; set; }
        //    public string strSystemCode { get; set; }
        //    public string strIsActive { get; set; }
        //    public DateTime dtCreatedDataTime { get; set; }
        //    public string strCreatedBy { get; set; }
        //    public DateTime dtModifiedDatetime { get; set; }
        //    public string strModifiedBy { get; set; }
        //}
    }
}