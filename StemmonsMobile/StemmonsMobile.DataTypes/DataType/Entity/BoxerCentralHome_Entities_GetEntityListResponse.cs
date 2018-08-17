

using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class BoxerCentralHome_Entities_GetEntityListResponse : Response
    {
        public object ResponseContent { get; set; }
    }
    public class BoxerCentralHomePage_EntityList
    {
        public string ASSIGNED_TO { get; set; }
        public List<AssociationField> AssociationFieldCollection { get; set; }
        public string CATEG_NAME { get; set; }
        public string CATEG_VALUE { get; set; }
        public string COST_NAME { get; set; }
        public string COST_VALUE { get; set; }
        private string _created_by = "";
        public string CREATED_BY
        {
            get
            {
                return "Created By: " + _created_by;
            }
            set
            {
                _created_by = value ?? "";
            }
        }
        public string CREATED_BY_NAME { get; set; }

        public string _created_datetime = "";
        public string CREATED_DATETIME
        {
            get
            {
                return "Created On: " + Convert.ToDateTime(_created_datetime).ToString("d");
            }
            set
            {
                _created_datetime = value ?? "";
            }
        }
        public string DEPT_NAME { get; set; }
        public string DEPT_VALUE { get; set; }
        public string DESC_NAME { get; set; }
        public string DESC_VALUE { get; set; }
        public string ENTITY_ID { get; set; }
        public string ENTITY_TITLE { get; set; }
        public string ENTITY_TYPE_ID { get; set; }
        public string ENTITY_TYPE_NAME { get; set; }
        public string EXP_NAME { get; set; }
        public string EXP_VALUE { get; set; }

        public string _list_id = "";
        public string LIST_ID
        {
            get
            {
                return "List Id: " + _list_id;
            }
            set
            {
                _list_id = value ?? "";
            }
        }
        public string MKT_NAME { get; set; }
        public string MKT_VALUE { get; set; }
        private string _modified_by = "";
        public string MODIFIED_BY
        {
            get
            {
                return "Modified By: " + _modified_by;
            }
            set
            {
                _modified_by = value ?? "";
            }
        }
        public string MODIFIED_BY_NAME { get; set; }

        public string _modified_datetime = "";
        public string MODIFIED_DATETIME
        {
            get
            {
                return "Modified On: " + Convert.ToDateTime(_modified_datetime).ToString("d");
            }
            set
            {
                _modified_datetime = value ?? "";
            }
        }
        public string PHGAL_NAME { get; set; }
        public string PHGAL_VALUE { get; set; }
        public string PRPTY_NAME { get; set; }
        public string PRPTY_VALUE { get; set; }
        public string REGON_NAME { get; set; }
        public string REGON_VALUE { get; set; }
        public string ROLEC_NAME { get; set; }
        public string ROLEC_VALUE { get; set; }
        public string SBTTL_NAME { get; set; }
        public string SBTTL_VALUE { get; set; }
        public string STTUS_NAME { get; set; }
        public string STTUS_VALUE { get; set; }
        public string SUBMK_NAME { get; set; }
        public string SUBMK_VALUE { get; set; }
        public string TITLE_VALUE { get; set; }
        public string URL_NAME { get; set; }
        public string URL_VALUE { get; set; }

    }

    public class BoxerCentralHomePage_EntityList_Mob
    {
        public string ASSIGNED_TO { get; set; }
        public List<AssociationField> AssociationFieldCollection { get; set; }
        public string CATEG_NAME { get; set; }
        public string CATEG_VALUE { get; set; }
        public string COST_NAME { get; set; }
        public string COST_VALUE { get; set; }
        public string CREATED_BY { get; set; }
        public string CREATED_BY_NAME { get; set; }

        public string _created_datetime = "";
        public string CREATED_DATETIME { get; set; }
        public string DEPT_NAME { get; set; }
        public string DEPT_VALUE { get; set; }
        public string DESC_NAME { get; set; }
        public string DESC_VALUE { get; set; }
        public string ENTITY_ID { get; set; }
        public string ENTITY_TITLE { get; set; }
        public string ENTITY_TYPE_ID { get; set; }
        public string ENTITY_TYPE_NAME { get; set; }
        public string EXP_NAME { get; set; }
        public string EXP_VALUE { get; set; }

        public string _list_id = "";
        public string LIST_ID { get; set; }
        public string MKT_NAME { get; set; }
        public string MKT_VALUE { get; set; }
        public string MODIFIED_BY { get; set; }
        public string MODIFIED_BY_NAME { get; set; }

        public string MODIFIED_DATETIME { get; set; }
        public string PHGAL_NAME { get; set; }
        public string PHGAL_VALUE { get; set; }
        public string PRPTY_NAME { get; set; }
        public string PRPTY_VALUE { get; set; }
        public string REGON_NAME { get; set; }
        public string REGON_VALUE { get; set; }
        public string ROLEC_NAME { get; set; }
        public string ROLEC_VALUE { get; set; }
        public string SBTTL_NAME { get; set; }
        public string SBTTL_VALUE { get; set; }
        public string STTUS_NAME { get; set; }
        public string STTUS_VALUE { get; set; }
        public string SUBMK_NAME { get; set; }
        public string SUBMK_VALUE { get; set; }
        public string TITLE_VALUE { get; set; }
        public string URL_NAME { get; set; }
        public string URL_VALUE { get; set; }

    }

}