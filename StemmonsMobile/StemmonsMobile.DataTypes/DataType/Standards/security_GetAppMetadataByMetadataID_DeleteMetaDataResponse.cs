


using StemmonsMobile.DataTypes.DataType.Standards.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Standards
{
    public class security_GetAppMetadataByMetadataID_DeleteMetaDataResponse : Response
    {
        public object ResponseContent { get; set; }
        public List<MetaData> result { get; set; }


    }
}