using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class Questcls
    {
        public class Arealist
        {
            public int Area_Id
            {
                get; set;
            }

            public List<Item> ItemCollection { get; set; }

            
        }
        public class Item
        {
            public int ITEM_ID;
            public List<ItemInfoFields> Item_Info_Field_MA;
        }

        public class ItemInfoFields
        {
            public int Display_Order;
            public int List_Display_Order;
            public string Email_Display_Order;
            public int Item_Info_Field_id;
            public string Item_Info_Field_Description;
            public string Item_Info_Field_Name;
            public string Show_On_Page;
            public string Display_Name;
            public char Is_Required;
        }
    }
}