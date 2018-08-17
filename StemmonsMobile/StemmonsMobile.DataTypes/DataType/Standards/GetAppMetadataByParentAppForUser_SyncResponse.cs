

using StemmonsMobile.DataTypes.DataType.Standards.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Standards
{
    public class GetAppMetadataByParentAppForUser_SyncResponse : Response
    {
        public object ResponseContent { get; set; }

        public standardsWebView result { get; set; }


    }
}