


using StemmonsMobile.DataTypes.DataType.Standards.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Standards
{
    public class GetMetadataFilesResponse : Response
    {
        public object ResponseContent { get; set; }

        public List<DownLoadImage> result { get; set; }


    }
}