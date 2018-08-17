using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Quest
{
        public class GetFilterQuery_QuestResponse : Response
        {
            public object ResponseContent { get; set; }

        }
        public class ConnectionStringCls
        {
            public string _QUERY { get; set; }

            public int _INTERNAL_ENTITY_TYPE_ID { get; set; }

            public string _CONNECTION_STRING { get; set; }

            public string _FILTER_QUERY { get; set; }
        }
}
