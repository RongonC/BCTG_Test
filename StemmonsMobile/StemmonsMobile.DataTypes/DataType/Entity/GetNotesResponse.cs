

using Entities.DataAccess.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetNotesResponse : Response
    {
        public object ResponseContent { get; set; }

        public List<EntityNoteItem> result { get; set; }
        //public int ENTITY_NOTE_ID { get; set; }
        //public int ENTITY_ID { get; set; }
        //public int ENTITY_NOTE_TYPE_ID { get; set; }
        //public string NOTE { get; set; }
        //public char  IS_ACTIVE { get; set; }
        //public DateTime  CREATED_DATETIME { get; set; }
        //public string CREATED_BY { get; set; }
        //public DateTime MODIFIED_DATETIME { get; set; }
        //public string  MODIFIED_BY { get; set; }
        //public string MODIFIED_BY_FULL_NAME { get; set; }
        //public string ENTITY_NOTE_TYPE_NAME { get; set; }
        //public string BCOLOR { get; set; }
        //public string FCOLOR { get; set; }
        //public string SYSTEM_CODE { get; set; }

        //public char ALLOW_MANUAL_USE { get; set; }


    }
}