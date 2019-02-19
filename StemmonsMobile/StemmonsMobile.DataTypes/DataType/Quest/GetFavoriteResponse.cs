using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetFavoriteResponse : Response
    {
        public object ResponseContent { get; set; }

        public class GetFavorite
        {
            public int FavoriteId { get; set; }
            //public int AppTypeInfoId { get; set; }
            public string FavoriteName { get; set; }
            public string FieldValues { get; set; }
            public string IsActive { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedDateTime { get; set; }
            public DateTime ModifiedDateTime { get; set; }
            public DateTime LastSyncDateTime { get; set; }
            public int ApplicationID { get; set; }
            public int? QuestAreaID { get; set; }
            public int? TypeID { get; set; }
        }
    }
}
