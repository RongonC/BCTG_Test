
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class AddFileToEntityRequest

    {

        public int ENTITY_ID { get; set; }
        public string DESCRIPTION { get; set; }
        public DateTime FILE_DATE_TIME { get; set; }
        public string FILE_NAME { get; set; }
        public int FILE_SIZE_BYTES { get; set; }
        public Byte[] FILE_BLOB { get; set; }
        public string EXTERNAL_URI { get; set; }
        public Char SHOW_INLINE_NOTES { get; set; }
        
        public string SYSTEM_CODE { get; set; }
        public Char IS_ACTIVE { get; set; }
        public string CREATED_BY { get; set; }
    }
}