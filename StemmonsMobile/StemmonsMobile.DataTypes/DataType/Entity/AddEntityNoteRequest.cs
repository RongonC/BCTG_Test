
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class AddEntityNoteRequest
    {
        public int ENTITY_ID { get; set; }
        public int ENTITY_NOTE_TYPE_ID { get; set; }
        public string NOTE { get; set; }
        public string CREATED_BY { get; set; }
    }
}