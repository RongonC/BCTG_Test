

using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetNoteTypesResponse : Response
    {

        public List<EntityNoteType> result { get; set; }
        public object ResponseContent { get; set; }
        //public int ENTITY_NOTE_TYPE_ID { get; set; }
        //public string  NAME { get; set; }
        //public string DESCRIPTION { get; set; }
        //public string BCOLOR { get; set; }
        //public string FCOLOR { get; set; }
        //public int SYSTEM_CODE { get; set; }
        //public char ALLOW_MANUAL_USE { get; set; }
        //public char IS_ACTIVE { get; set; }
        //public DateTime  CREATED_DATETIME { get; set; }
        //public string  CREATED_BY { get; set; }
        //public DateTime MODIFIED_DATETIME { get; set; }
        //public string  MODIFIED_BY { get; set; }
    }
}