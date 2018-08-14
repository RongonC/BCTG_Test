using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetItemInfoFieldsByItemIDRequest
    {
        public int? itemID { get; set; }
        public string showOnPage { get; set; }
    }
}