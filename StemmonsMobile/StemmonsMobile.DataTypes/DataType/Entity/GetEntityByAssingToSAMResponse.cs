
using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetEntityByAssingToSAMResponse : Response
    {
        public object ResponseContent { get; set; }
        public int result { get; set; }
    }
    public partial class AssignToUser
    {

        public int? ENTITY_ID { get; set; }

        public int? LIST_ID { get; set; }

        public string ENTITY_TYPE_NAME { get; set; }

        public string ENTITY_TITLE { get; set; }

        public string CREATED_BY { get; set; }

        public string CREATED_DATETIME { get; set; }

        public string MODIFIED_BY { get; set; }

        public string MODIFIED_DATETIME { get; set; }

        public string OPEN_PAGE_TYPE { get; set; }
    }
}