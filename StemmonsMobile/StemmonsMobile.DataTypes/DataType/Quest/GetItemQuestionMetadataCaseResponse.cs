
using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetItemQuestionMetadataCaseResponse : Response
    {
        public object ResponseContent { get; set; }
        public class ItemQuestionMetadataCase
        {
            public ItemQuestionMetadataCase()
            {
                // statements
            }


            public int intItemQuestionMetadataCaseID { get; set; }

            public int intItemQuestionMetadataID { get; set; }

            public int? intCaseID { get; set; }

            public string strNotes { get; set; }

            public string usreName { get; set; }

            public string itemInstanceTranID { get; set; }

            public string strCaseTypeID { get; set; }
        }
    }
}