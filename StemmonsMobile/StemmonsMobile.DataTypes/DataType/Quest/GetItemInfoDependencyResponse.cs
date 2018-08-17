
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetItemInfoDependencyResponse:Response
    {
        public object ResponseContent { get; set; }
        public class ItemInfoDependancy
        {
            public ItemInfoDependancy()
            {
                // statements
            }

            public int intItemInfoFieldDependencyID { get; set; }

            public int intItemID { get; set; }

            public int intItemInfoFieldIDParent { get; set; }

            public int intItemInfoFieldIDChild { get; set; }

            public string strDependencyType { get; set; }

            // Parent cols
            public string strInfoFieldNameParent { get; set; }

            public string strInfoFieldDisplayNameParent { get; set; }

            public int intExternalDatasourceIDParent { get; set; }

            public string strExternalDatasourceNameParent { get; set; }

            public string strConnectionStringParent { get; set; }

            public string strQueryParent { get; set; }

            // Child cols
            public string strInfoFieldNameChild { get; set; }

            public string strInfoFieldDisplayNameChild { get; set; }

            public int intExternalDatasourceIDChild { get; set; }

            public string strExternalDatasourceNameChild { get; set; }

            public string strConnectionStringChild { get; set; }

            public string strQueryChild { get; set; }
        }
    }
}