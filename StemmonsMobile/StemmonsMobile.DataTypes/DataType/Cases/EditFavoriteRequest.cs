using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class EditFavoriteRequest
    {
        public int? FavoriteId { get; set; }
        public int? AppID { get; set; }
        public string FavoriteName { get; set; }
        public string FieldValues { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public DateTime? LastSyncDateTime { get; set; }




       public int? QuestAreaID { get; set; }
        public int?  pTypeId { get; set; }
    }
}
