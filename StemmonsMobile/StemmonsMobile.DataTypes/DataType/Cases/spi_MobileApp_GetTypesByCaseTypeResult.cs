using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public partial class spi_MobileApp_GetTypesByCaseTypeResult
    {

        private int _ASSOC_TYPE_ID;

        private string _ASSOC_FIELD_TYPE;

        private int _CASE_TYPE_ID;

        private string _NAME;

        private string _DESCRIPTION;

        private System.Nullable<int> _EXTERNAL_DATASOURCE_ID;

        private string _SYSTEM_CODE;

        private System.Nullable<int> _SYSTEM_PRIORITY;

        private string _SHOW_ON_LIST;

        private System.Nullable<int> _UI_WIDTH;

        private string _IS_REQUIRED;

        private string _IS_ACTIVE;

        private System.DateTime _CREATED_DATETIME;

        private string _CREATED_BY;

        private System.DateTime _MODIFIED_DATETIME;

        private string _MODIFIED_BY;

        private System.Nullable<char> _USE_COMMA_SEPARATOR;

        private string _SEPARATOR_CHARACTOR;

        private System.Nullable<int> _LIST_DESKTOP_PRIORITY_VALUE;

        private System.Nullable<int> _LIST_MOBILE_PRIORITY_VALUE;

        private System.Nullable<int> _ITEM_MOBILE_PRIORITY_VALUE;

        private string _CALCULATION_FORMULA;

        private System.Nullable<int> _CALCULATION_FREQUENCY_MIN;

        private System.Nullable<char> _IS_FORCE_RECALCULATION;

        private int _CHILD_ID;

        private int _PARENT_ID;

        private string _SECURITY_TYPE;

        private string _ASSOC_SECURITY_TYPE;

        public FREQUENT ExternalDatasourceList { get; set; }

        public List<FREQUENT> ExternalDatasourceinfo { get; set; }

        public spi_MobileApp_GetTypesByCaseTypeResult()
        {
        }


        public int ASSOC_TYPE_ID
        {
            get
            {
                return this._ASSOC_TYPE_ID;
            }
            set
            {
                if ((this._ASSOC_TYPE_ID != value))
                {
                    this._ASSOC_TYPE_ID = value;
                }
            }
        }
        public string SECURITY_TYPE
        {
            get
            {
                return this._SECURITY_TYPE;
            }
            set
            {
                if ((this._SECURITY_TYPE != value))
                {
                    this._SECURITY_TYPE = value;
                }
            }
        }

        public string ASSOC_SECURITY_TYPE
        {
            get
            {
                return this._ASSOC_SECURITY_TYPE;
            }
            set
            {
                if ((this._ASSOC_SECURITY_TYPE != value))
                {
                    this._ASSOC_SECURITY_TYPE = value;
                }
            }
        }

        public string ASSOC_FIELD_TYPE
        {
            get
            {
                return this._ASSOC_FIELD_TYPE;
            }
            set
            {
                if ((this._ASSOC_FIELD_TYPE != value))
                {
                    this._ASSOC_FIELD_TYPE = value;
                }
            }
        }


        public int CASE_TYPE_ID
        {
            get
            {
                return this._CASE_TYPE_ID;
            }
            set
            {
                if ((this._CASE_TYPE_ID != value))
                {
                    this._CASE_TYPE_ID = value;
                }
            }
        }


        public string NAME
        {
            get
            {
                return this._NAME;
            }
            set
            {
                if ((this._NAME != value))
                {
                    this._NAME = value;
                }
            }
        }


        public string DESCRIPTION
        {
            get
            {
                return this._DESCRIPTION;
            }
            set
            {
                if ((this._DESCRIPTION != value))
                {
                    this._DESCRIPTION = value;
                }
            }
        }


        public int? EXTERNAL_DATASOURCE_ID
        {
            get
            {
                return this._EXTERNAL_DATASOURCE_ID;
            }
            set
            {
                if ((this._EXTERNAL_DATASOURCE_ID != value))
                {
                    this._EXTERNAL_DATASOURCE_ID = value;
                }
            }
        }


        public string SYSTEM_CODE
        {
            get
            {
                return this._SYSTEM_CODE;
            }
            set
            {
                if ((this._SYSTEM_CODE != value))
                {
                    this._SYSTEM_CODE = value;
                }
            }
        }


        public int? SYSTEM_PRIORITY
        {
            get
            {
                return this._SYSTEM_PRIORITY ?? 5000;
            }
            set
            {
                if ((this._SYSTEM_PRIORITY != value))
                {
                    this._SYSTEM_PRIORITY = value;
                }
            }
        }


        public string SHOW_ON_LIST
        {
            get
            {
                return this._SHOW_ON_LIST;
            }
            set
            {
                if ((this._SHOW_ON_LIST != value))
                {
                    this._SHOW_ON_LIST = value;
                }
            }
        }


        public int? UI_WIDTH
        {
            get
            {
                return this._UI_WIDTH;
            }
            set
            {
                if ((this._UI_WIDTH != value))
                {
                    this._UI_WIDTH = value;
                }
            }
        }


        public string IS_REQUIRED
        {
            get
            {
                return this._IS_REQUIRED;
            }
            set
            {
                if ((this._IS_REQUIRED != value))
                {
                    this._IS_REQUIRED = value;
                }
            }
        }


        public string IS_ACTIVE
        {
            get
            {
                return this._IS_ACTIVE;
            }
            set
            {
                if ((this._IS_ACTIVE != value))
                {
                    this._IS_ACTIVE = value;
                }
            }
        }


        public System.DateTime CREATED_DATETIME
        {
            get
            {
                return this._CREATED_DATETIME;
            }
            set
            {
                if ((this._CREATED_DATETIME != value))
                {
                    this._CREATED_DATETIME = value;
                }
            }
        }


        public string CREATED_BY
        {
            get
            {
                return this._CREATED_BY;
            }
            set
            {
                if ((this._CREATED_BY != value))
                {
                    this._CREATED_BY = value;
                }
            }
        }


        public System.DateTime MODIFIED_DATETIME
        {
            get
            {
                return this._MODIFIED_DATETIME;
            }
            set
            {
                if ((this._MODIFIED_DATETIME != value))
                {
                    this._MODIFIED_DATETIME = value;
                }
            }
        }


        public string MODIFIED_BY
        {
            get
            {
                return this._MODIFIED_BY;
            }
            set
            {
                if ((this._MODIFIED_BY != value))
                {
                    this._MODIFIED_BY = value;
                }
            }
        }


        public char? USE_COMMA_SEPARATOR
        {
            get
            {
                return this._USE_COMMA_SEPARATOR;
            }
            set
            {
                if ((this._USE_COMMA_SEPARATOR != value))
                {
                    this._USE_COMMA_SEPARATOR = value;
                }
            }
        }


        public string SEPARATOR_CHARACTOR
        {
            get
            {
                return this._SEPARATOR_CHARACTOR;
            }
            set
            {
                if ((this._SEPARATOR_CHARACTOR != value))
                {
                    this._SEPARATOR_CHARACTOR = value;
                }
            }
        }


        public int? LIST_DESKTOP_PRIORITY_VALUE
        {
            get
            {
                return this._LIST_DESKTOP_PRIORITY_VALUE;
            }
            set
            {
                if ((this._LIST_DESKTOP_PRIORITY_VALUE != value))
                {
                    this._LIST_DESKTOP_PRIORITY_VALUE = value;
                }
            }
        }


        public int? LIST_MOBILE_PRIORITY_VALUE
        {
            get
            {
                return this._LIST_MOBILE_PRIORITY_VALUE;
            }
            set
            {
                if ((this._LIST_MOBILE_PRIORITY_VALUE != value))
                {
                    this._LIST_MOBILE_PRIORITY_VALUE = value;
                }
            }
        }


        public int? ITEM_MOBILE_PRIORITY_VALUE
        {
            get
            {
                return this._ITEM_MOBILE_PRIORITY_VALUE;
            }
            set
            {
                if ((this._ITEM_MOBILE_PRIORITY_VALUE != value))
                {
                    this._ITEM_MOBILE_PRIORITY_VALUE = value;
                }
            }
        }


        public string CALCULATION_FORMULA
        {
            get
            {
                return this._CALCULATION_FORMULA;
            }
            set
            {
                if ((this._CALCULATION_FORMULA != value))
                {
                    this._CALCULATION_FORMULA = value;
                }
            }
        }


        public int? CALCULATION_FREQUENCY_MIN
        {
            get
            {
                return this._CALCULATION_FREQUENCY_MIN;
            }
            set
            {
                if ((this._CALCULATION_FREQUENCY_MIN != value))
                {
                    this._CALCULATION_FREQUENCY_MIN = value;
                }
            }
        }


        public char? IS_FORCE_RECALCULATION
        {
            get
            {
                return this._IS_FORCE_RECALCULATION;
            }
            set
            {
                if ((this._IS_FORCE_RECALCULATION != value))
                {
                    this._IS_FORCE_RECALCULATION = value;
                }
            }
        }


        public int CHILD_ID
        {
            get
            {
                return this._CHILD_ID;
            }
            set
            {
                if ((this._CHILD_ID != value))
                {
                    this._CHILD_ID = value;
                }
            }
        }


        public int PARENT_ID
        {
            get
            {
                return this._PARENT_ID;
            }
            set
            {
                if ((this._PARENT_ID != value))
                {
                    this._PARENT_ID = value;
                }
            }
        }

        public override string ToString()
        {
            string format = "{0}, ASSOC_FIELD_TYPE = {1}";
            return string.Format(format, this.ASSOC_TYPE_ID, this.ASSOC_FIELD_TYPE);
        }
        public string ExternalDataSourceEntityTypeID { get; set; }
    }
    public partial class spi_MobileApp_GetCaseTypeDataByUserResult
    {

        private int _CASE_TYPE_ID;

        private string _NAME;

        private string _INSTANCE_NAME;

        private string _INSTANCE_NAME_PLURAL;

        private string _DESCRIPTION;

        private string _SPORG_OVERRIDE;

        private char _IS_ACTIVE;

        private System.DateTime _CREATED_DATETIME;

        private string _CREATED_BY;

        private System.DateTime _MODIFIED_DATETIME;

        private string _MODIFIED_BY;

        private System.Nullable<int> _DEFAULT_HOPPER_ID;

        private System.Nullable<char> _NEWEST_NOTES_ON_TOP;

        private System.Nullable<char> _LOCK_WHEN_UNOWNED;

        private string _ALLOWED_SECURITY_GROUPS;

        private string _CASE_TYPE_OWNER;

        private string _HELP_URL;

        private System.Nullable<char> _IS_ALLOW_APPROVAL;

        private System.Nullable<char> _IS_FORCE_SYNC_TO_FACTS;

        private System.Nullable<char> _IS_EXPORT_TO_FACTS;

        private System.Nullable<char> _IS_EXPORT_CLOSED_TO_FACTS_OPTION;

        private System.Nullable<char> _IS_SHOW_PERMISSION_ICON_TO_CASE_OWNER;

        private System.Nullable<char> _IS_ALLOW_OUTSIDE_EMAIL_TO_CASES;

        private char _IS_COPY_NOTES_TO_CHILD;

        private char _IS_COPY_STATUS_TO_CHILD;

        private string _SECURITY_TYPE;

        public spi_MobileApp_GetCaseTypeDataByUserResult()
        {
        }

        public int CASE_TYPE_ID
        {
            get
            {
                return this._CASE_TYPE_ID;
            }
            set
            {
                if ((this._CASE_TYPE_ID != value))
                {
                    this._CASE_TYPE_ID = value;
                }
            }
        }

        public string NAME
        {
            get
            {
                return this._NAME;
            }
            set
            {
                if ((this._NAME != value))
                {
                    this._NAME = value;
                }
            }
        }

        public string INSTANCE_NAME
        {
            get
            {
                return this._INSTANCE_NAME;
            }
            set
            {
                if ((this._INSTANCE_NAME != value))
                {
                    this._INSTANCE_NAME = value;
                }
            }
        }

        public string INSTANCE_NAME_PLURAL
        {
            get
            {
                return this._INSTANCE_NAME_PLURAL;
            }
            set
            {
                if ((this._INSTANCE_NAME_PLURAL != value))
                {
                    this._INSTANCE_NAME_PLURAL = value;
                }
            }
        }

        public string DESCRIPTION
        {
            get
            {
                return this._DESCRIPTION;
            }
            set
            {
                if ((this._DESCRIPTION != value))
                {
                    this._DESCRIPTION = value;
                }
            }
        }

        public string SPORG_OVERRIDE
        {
            get
            {
                return this._SPORG_OVERRIDE;
            }
            set
            {
                if ((this._SPORG_OVERRIDE != value))
                {
                    this._SPORG_OVERRIDE = value;
                }
            }
        }

        public char IS_ACTIVE
        {
            get
            {
                return this._IS_ACTIVE;
            }
            set
            {
                if ((this._IS_ACTIVE != value))
                {
                    this._IS_ACTIVE = value;
                }
            }
        }

        public System.DateTime CREATED_DATETIME
        {
            get
            {
                return this._CREATED_DATETIME;
            }
            set
            {
                if ((this._CREATED_DATETIME != value))
                {
                    this._CREATED_DATETIME = value;
                }
            }
        }

        public string CREATED_BY
        {
            get
            {
                return this._CREATED_BY;
            }
            set
            {
                if ((this._CREATED_BY != value))
                {
                    this._CREATED_BY = value;
                }
            }
        }

        public System.DateTime MODIFIED_DATETIME
        {
            get
            {
                return this._MODIFIED_DATETIME;
            }
            set
            {
                if ((this._MODIFIED_DATETIME != value))
                {
                    this._MODIFIED_DATETIME = value;
                }
            }
        }

        public string MODIFIED_BY
        {
            get
            {
                return this._MODIFIED_BY;
            }
            set
            {
                if ((this._MODIFIED_BY != value))
                {
                    this._MODIFIED_BY = value;
                }
            }
        }

        public System.Nullable<int> DEFAULT_HOPPER_ID
        {
            get
            {
                return this._DEFAULT_HOPPER_ID;
            }
            set
            {
                if ((this._DEFAULT_HOPPER_ID != value))
                {
                    this._DEFAULT_HOPPER_ID = value;
                }
            }
        }

        public System.Nullable<char> NEWEST_NOTES_ON_TOP
        {
            get
            {
                return this._NEWEST_NOTES_ON_TOP;
            }
            set
            {
                if ((this._NEWEST_NOTES_ON_TOP != value))
                {
                    this._NEWEST_NOTES_ON_TOP = value;
                }
            }
        }

        public System.Nullable<char> LOCK_WHEN_UNOWNED
        {
            get
            {
                return this._LOCK_WHEN_UNOWNED;
            }
            set
            {
                if ((this._LOCK_WHEN_UNOWNED != value))
                {
                    this._LOCK_WHEN_UNOWNED = value;
                }
            }
        }

        public string ALLOWED_SECURITY_GROUPS
        {
            get
            {
                return this._ALLOWED_SECURITY_GROUPS;
            }
            set
            {
                if ((this._ALLOWED_SECURITY_GROUPS != value))
                {
                    this._ALLOWED_SECURITY_GROUPS = value;
                }
            }
        }

        public string CASE_TYPE_OWNER
        {
            get
            {
                return this._CASE_TYPE_OWNER;
            }
            set
            {
                if ((this._CASE_TYPE_OWNER != value))
                {
                    this._CASE_TYPE_OWNER = value;
                }
            }
        }

        public string HELP_URL
        {
            get
            {
                return this._HELP_URL;
            }
            set
            {
                if ((this._HELP_URL != value))
                {
                    this._HELP_URL = value;
                }
            }
        }

        public System.Nullable<char> IS_ALLOW_APPROVAL
        {
            get
            {
                return this._IS_ALLOW_APPROVAL;
            }
            set
            {
                if ((this._IS_ALLOW_APPROVAL != value))
                {
                    this._IS_ALLOW_APPROVAL = value;
                }
            }
        }

        public System.Nullable<char> IS_FORCE_SYNC_TO_FACTS
        {
            get
            {
                return this._IS_FORCE_SYNC_TO_FACTS;
            }
            set
            {
                if ((this._IS_FORCE_SYNC_TO_FACTS != value))
                {
                    this._IS_FORCE_SYNC_TO_FACTS = value;
                }
            }
        }

        public System.Nullable<char> IS_EXPORT_TO_FACTS
        {
            get
            {
                return this._IS_EXPORT_TO_FACTS;
            }
            set
            {
                if ((this._IS_EXPORT_TO_FACTS != value))
                {
                    this._IS_EXPORT_TO_FACTS = value;
                }
            }
        }

        public System.Nullable<char> IS_EXPORT_CLOSED_TO_FACTS_OPTION
        {
            get
            {
                return this._IS_EXPORT_CLOSED_TO_FACTS_OPTION;
            }
            set
            {
                if ((this._IS_EXPORT_CLOSED_TO_FACTS_OPTION != value))
                {
                    this._IS_EXPORT_CLOSED_TO_FACTS_OPTION = value;
                }
            }
        }

        public System.Nullable<char> IS_SHOW_PERMISSION_ICON_TO_CASE_OWNER
        {
            get
            {
                return this._IS_SHOW_PERMISSION_ICON_TO_CASE_OWNER;
            }
            set
            {
                if ((this._IS_SHOW_PERMISSION_ICON_TO_CASE_OWNER != value))
                {
                    this._IS_SHOW_PERMISSION_ICON_TO_CASE_OWNER = value;
                }
            }
        }

        public System.Nullable<char> IS_ALLOW_OUTSIDE_EMAIL_TO_CASES
        {
            get
            {
                return this._IS_ALLOW_OUTSIDE_EMAIL_TO_CASES;
            }
            set
            {
                if ((this._IS_ALLOW_OUTSIDE_EMAIL_TO_CASES != value))
                {
                    this._IS_ALLOW_OUTSIDE_EMAIL_TO_CASES = value;
                }
            }
        }

        public char IS_COPY_NOTES_TO_CHILD
        {
            get
            {
                return this._IS_COPY_NOTES_TO_CHILD;
            }
            set
            {
                if ((this._IS_COPY_NOTES_TO_CHILD != value))
                {
                    this._IS_COPY_NOTES_TO_CHILD = value;
                }
            }
        }

        public char IS_COPY_STATUS_TO_CHILD
        {
            get
            {
                return this._IS_COPY_STATUS_TO_CHILD;
            }
            set
            {
                if ((this._IS_COPY_STATUS_TO_CHILD != value))
                {
                    this._IS_COPY_STATUS_TO_CHILD = value;
                }
            }
        }

        public string SECURITY_TYPE
        {
            get
            {
                return this._SECURITY_TYPE;
            }
            set
            {
                if ((this._SECURITY_TYPE != value))
                {
                    this._SECURITY_TYPE = value;
                }
            }
        }
    }
}
