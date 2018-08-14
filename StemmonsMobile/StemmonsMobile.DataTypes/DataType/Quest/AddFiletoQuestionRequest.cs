using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class AddFiletoQuestionRequest
    {
        /// <summary>
        /// Item instance Transaction Id
        /// </summary>
        public int itemInstanceTranID { get; set; }
        /// <summary>
        /// Item question field id
        /// </summary>
        public int itemQuestionFieldId { get; set; }
        /// <summary>
        /// Description should be the MIME type of file. <see cref="https://msdn.microsoft.com/en-us/library/bb742440.aspx"/>
        /// </summary>
        public string fileType { get; set; }
        public string fileName { get; set; }
        public int fileSizeBytes { get; set; }
        public byte[] fileBinary { get; set; }
        public string createdBy { get; set; }
    }
}