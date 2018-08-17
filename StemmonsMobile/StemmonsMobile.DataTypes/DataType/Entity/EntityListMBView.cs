using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Entity
{


    public class EntityListMBView
    {
        public EntityListMBView()
        {
        }

        public EntityListMBView(string _title, string _listId, string _assocField2, string _assocField3, string _assocField4, EntityClass _EntityDetails)
        {
            Title = _title;
            ListId = _listId;
            Field2 = _assocField2;
            Field3 = _assocField3;
            Field4 = _assocField4;
            EntityDetails = _EntityDetails;
        }


        public string Title { get; set; }
        public string ListId { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }

        public EntityClass EntityDetails { get; set; }
        public override string ToString()
        {
            string format = "{0}, ListId = {1},Field4 = {2}";
            return string.Format(format, Title, this.ListId, this.Field4);
        }
    }

}
