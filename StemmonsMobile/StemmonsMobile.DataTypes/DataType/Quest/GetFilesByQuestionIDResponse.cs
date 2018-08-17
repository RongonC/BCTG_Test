
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetFilesByQuestionIDResponse:Response
    {
        public object ResponseContent { get; set; }

        public class GetFilesByQuestionIDModel
        {
            public int intFileID { get; set; }
            public int intItemQuestionMetaDataID { get; set; }
            public string strDescription { get; set; }
            public string strFileName { get; set; }
            public int intFileSizeBytes { get; set; }
        }
    }
}