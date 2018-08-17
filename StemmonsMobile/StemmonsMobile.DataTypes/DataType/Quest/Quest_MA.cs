using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class Quest_MA
    {
        public class Areas_MA
        {
            public int Area_Id { get; set; }
            public List<Item> Item_Collection { get; set; }
        }

        public class Item
        {
            public int ITEM_ID;
            public List<ItemInfoFields> Item_Info_Field_Collection;
        }

        public class ItemInfoFields
        {
            public int Display_Order;
            public int? List_Display_Order;
            public string Email_Display_Order;
            public int Item_Info_Field_id;
            public string Item_Info_Field_Description;
            public string Item_Info_Field_Name;
            public string Show_On_Page;
            public string Display_Name;
            public string Is_Required;
            public List<ExternalDatasource> ExternalDatasourceCollection;
        }

        public class ExternalDatasource
        {
            public int EXTERNAL_DATASOURCE_ID;
            public string EXTERNAL_DATASOURCE_NAME;
            public string EXTERNAL_DATASOURCE_DESCRIPTION;
            public int count;
        }

        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute("AREA", Namespace = "", IsNullable = false)]
        public partial class AREA
        {

            private int cAREA_IDField;
            public ITEM[] cITEM;

            public int AREA_ID
            {
                get
                {
                    return this.cAREA_IDField;
                }
                set
                {
                    this.cAREA_IDField = value;
                }
            }

            [System.Xml.Serialization.XmlElementAttribute("ITEM")]
            public ITEM[] sITEM_LIST
            {
                get
                {
                    return this.cITEM;
                }
                set
                {
                    this.cITEM = value;
                }
            }

            public Areas_MA DeserializedQuest()
            {
                Areas_MA objArea = new Areas_MA()
                {
                    Area_Id = this.AREA_ID,
                    Item_Collection = new List<Item>()
                };

                if (this.sITEM_LIST != null)
                {
                    foreach (var item in this.sITEM_LIST)
                    {
                        #region Item Collection
                        Item itemCollection = new Item()
                        {
                            ITEM_ID = item.ITEM_ID,
                            Item_Info_Field_Collection = new List<ItemInfoFields>()
                        };
                        #endregion

                        #region Info field
                        if (item.ITEM_INFO_FIELD != null)
                        {
                            foreach (var subitem in item.ITEM_INFO_FIELD)
                            {
                                ItemInfoFields ItemInfoFieldscollection = new ItemInfoFields()
                                {
                                    Display_Name = subitem.cDISPLAY_NAMEField,
                                    Display_Order = subitem.cDISPLAY_ORDERField,
                                    Email_Display_Order = subitem.cEMAIL_DISPLAY_ORDERField,
                                    Is_Required = subitem.IS_REQUIRED,
                                    Item_Info_Field_Description = subitem.ITEM_INFO_FIELD_DESCRIPTION,
                                    Item_Info_Field_id = subitem.ITEM_INFO_FIELD_ID,
                                    Item_Info_Field_Name = subitem.ITEM_INFO_FIELD_NAME,
                                    List_Display_Order = subitem.LIST_DISPLAY_ORDER,
                                    Show_On_Page = subitem.SHOW_ON_PAGE,
                                    ExternalDatasourceCollection = new List<ExternalDatasource>()
                                };
                                #region External Data source
                                if (subitem.cEXTERNAL_DATASOURCE_Collection != null)
                                {
                                    foreach (var subitemexd in subitem.cEXTERNAL_DATASOURCE_Collection)
                                    {
                                        ItemInfoFieldscollection.ExternalDatasourceCollection.Add(new ExternalDatasource()
                                        {
                                            count = subitemexd.count,
                                            EXTERNAL_DATASOURCE_DESCRIPTION = subitemexd.EXTERNAL_DATASOURCE_DESCRIPTION,
                                            EXTERNAL_DATASOURCE_ID = subitemexd.EXTERNAL_DATASOURCE_ID,
                                            EXTERNAL_DATASOURCE_NAME = subitemexd.EXTERNAL_DATASOURCE_NAME
                                        });
                                    }
                                }
                                #endregion
                                itemCollection.Item_Info_Field_Collection.Add(ItemInfoFieldscollection);
                            }
                        }
                        #endregion

                        objArea.Item_Collection.Add(itemCollection);

                    }
                }
                return objArea;


            }
            public partial class ITEM
            {
                public int cITEM_IDField;
                public ITEM_INFO_FIELD[] cITEM_INFO_FIELD;

                public int ITEM_ID
                {
                    get
                    {
                        return this.cITEM_IDField;
                    }
                    set
                    {
                        this.cITEM_IDField = value;
                    }
                }

                [System.Xml.Serialization.XmlElementAttribute("ITEM_INFO_FIELD")]
                public ITEM_INFO_FIELD[] ITEM_INFO_FIELD
                {
                    get
                    {
                        return this.cITEM_INFO_FIELD;
                    }
                    set
                    {
                        this.cITEM_INFO_FIELD = value;
                    }
                }

            }

            public partial class ITEM_INFO_FIELD
            {
                public int cDISPLAY_ORDERField;
                public int? cLIST_DISPLAY_ORDERField;
                public string cEMAIL_DISPLAY_ORDERField;
                public int cITEM_INFO_FIELD_IDField;
                public string cITEM_INFO_FIELD_DESCRIPTIONField;
                public string cITEM_INFO_FIELD_NAMEField;
                public string cSHOW_ON_PAGEField;
                public string cDISPLAY_NAMEField;
                public string cIS_REQUIREDField;


                public EXTERNAL_DATASOURCE[] cEXTERNAL_DATASOURCE_Collection;

                public int DISPLAY_ORDER
                {
                    get
                    {
                        return this.cDISPLAY_ORDERField;
                    }
                    set
                    {
                        this.cDISPLAY_ORDERField = value;
                    }

                }

                [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
                public int? LIST_DISPLAY_ORDER
                {
                    get
                    {
                        return this.cLIST_DISPLAY_ORDERField;
                    }
                    set
                    {
                        this.cLIST_DISPLAY_ORDERField = value;
                    }

                }

                [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
                public string EMAIL_DISPLAY_ORDER
                {
                    get
                    {
                        return this.cEMAIL_DISPLAY_ORDERField;
                    }
                    set
                    {
                        this.cEMAIL_DISPLAY_ORDERField = value;
                    }

                }

                public int ITEM_INFO_FIELD_ID
                {
                    get
                    {
                        return this.cITEM_INFO_FIELD_IDField;
                    }
                    set
                    {
                        this.cITEM_INFO_FIELD_IDField = value;
                    }

                }

                public string ITEM_INFO_FIELD_DESCRIPTION
                {
                    get
                    {
                        return this.cITEM_INFO_FIELD_DESCRIPTIONField;
                    }
                    set
                    {
                        this.cITEM_INFO_FIELD_DESCRIPTIONField = value;
                    }

                }

                public string ITEM_INFO_FIELD_NAME
                {
                    get
                    {
                        return this.cITEM_INFO_FIELD_NAMEField;
                    }
                    set
                    {
                        this.cITEM_INFO_FIELD_NAMEField = value;
                    }

                }

                public string SHOW_ON_PAGE
                {
                    get
                    {
                        return this.cSHOW_ON_PAGEField;
                    }
                    set
                    {
                        this.cSHOW_ON_PAGEField = value;
                    }

                }

                public string DISPLAY_NAME
                {
                    get
                    {
                        return this.cDISPLAY_NAMEField;
                    }
                    set
                    {
                        this.cDISPLAY_NAMEField = value;
                    }

                }

                public string IS_REQUIRED
                {
                    get
                    {
                        return this.cIS_REQUIREDField;
                    }
                    set
                    {
                        this.cIS_REQUIREDField = value;
                    }

                }

                [System.Xml.Serialization.XmlElementAttribute("EXTERNAL_DATASOURCE")]
                public EXTERNAL_DATASOURCE[] EXTERNAL_DATASOURCE
                {
                    get
                    {
                        return this.cEXTERNAL_DATASOURCE_Collection;
                    }
                    set
                    {
                        this.cEXTERNAL_DATASOURCE_Collection = value;
                    }
                }

            }

            public partial class EXTERNAL_DATASOURCE
            {
                private int cEXTERNAL_DATASOURCE_IDField;
                private string cEXTERNAL_DATASOURCE_NAMEField;
                private string cEXTERNAL_DATASOURCE_DESCRIPTIONField;
                private int ccount;


                public int count
                {
                    get
                    {
                        return this.ccount;
                    }
                    set
                    {
                        this.ccount = value;
                    }
                }
                public int EXTERNAL_DATASOURCE_ID
                {
                    get
                    {
                        return this.cEXTERNAL_DATASOURCE_IDField;
                    }
                    set
                    {
                        this.cEXTERNAL_DATASOURCE_IDField = value;
                    }
                }

                public string EXTERNAL_DATASOURCE_NAME
                {
                    get
                    {
                        return this.cEXTERNAL_DATASOURCE_NAMEField;
                    }
                    set
                    {
                        this.cEXTERNAL_DATASOURCE_NAMEField = value;
                    }
                }

                public string EXTERNAL_DATASOURCE_DESCRIPTION
                {
                    get
                    {
                        return this.cEXTERNAL_DATASOURCE_DESCRIPTIONField;
                    }
                    set
                    {
                        this.cEXTERNAL_DATASOURCE_DESCRIPTIONField = value;
                    }
                }
            }
        }
    }
}