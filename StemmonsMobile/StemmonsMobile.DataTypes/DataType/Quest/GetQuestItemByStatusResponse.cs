using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetQuestItemByStatusResponse : Response
    {
        public object ResponseContent { get; set; }
        public class ItemInstanceTran
        {
            public ItemInstanceTran()
            { }


            public int intItemInstanceTranID { get; set; }

            public int intItemID { get; set; }

            public int intListId { get; set; }

            public bool? blnIsEdit { get; set; }

            public string strItemNAme { get; set; }

            public string strOtherComments { get; set; }

            public string strFormCretedBy { get; set; }

            public DateTime? FormCredtedDate { get; set; }

            public int? intInforTranId { get; set; }

            public int? intCol1ItemInfoFieldID { get; set; }
            public int? intCol2ItemInfoFieldID { get; set; }
            public int? intCol3ItemInfoFieldID { get; set; }
            public int? intCol4ItemInfoFieldID { get; set; }
            public int? intCol5ItemInfoFieldID { get; set; }
            public int? intCol6ItemInfoFieldID { get; set; }
            public int? intCol7ItemInfoFieldID { get; set; }
            public int? intCol8ItemInfoFieldID { get; set; }
            public int? intCol9ItemInfoFieldID { get; set; }
            public int? intCol10ItemInfoFieldID { get; set; }
            public int? intCol11ItemInfoFieldID { get; set; }
            public int? intCol12ItemInfoFieldID { get; set; }
            public int? intCol13ItemInfoFieldID { get; set; }
            public int? intCol14ItemInfoFieldID { get; set; }
            public int? intCol15ItemInfoFieldID { get; set; }
            public int? intCol16ItemInfoFieldID { get; set; }
            public int? intCol17ItemInfoFieldID { get; set; }
            public int? intCol18ItemInfoFieldID { get; set; }
            public int? intCol19ItemInfoFieldID { get; set; }
            public int? intCol20ItemInfoFieldID { get; set; }

            public string strCol1ItemInfoFieldName { get; set; }
            public string strCol2ItemInfoFieldName { get; set; }
            public string strCol3ItemInfoFieldName { get; set; }
            public string strCol4ItemInfoFieldName { get; set; }
            public string strCol5ItemInfoFieldName { get; set; }
            public string strCol6ItemInfoFieldName { get; set; }
            public string strCol7ItemInfoFieldName { get; set; }
            public string strCol8ItemInfoFieldName { get; set; }
            public string strCol9ItemInfoFieldName { get; set; }
            public string strCol10ItemInfoFieldName { get; set; }
            public string strCol11ItemInfoFieldName { get; set; }
            public string strCol12ItemInfoFieldName { get; set; }
            public string strCol13ItemInfoFieldName { get; set; }
            public string strCol14ItemInfoFieldName { get; set; }
            public string strCol15ItemInfoFieldName { get; set; }
            public string strCol16ItemInfoFieldName { get; set; }
            public string strCol17ItemInfoFieldName { get; set; }
            public string strCol18ItemInfoFieldName { get; set; }
            public string strCol19ItemInfoFieldName { get; set; }
            public string strCol20ItemInfoFieldName { get; set; }

            public string strCol1ItemInfoFieldValue { get; set; }
            public string strCol2ItemInfoFieldValue { get; set; }
            public string strCol3ItemInfoFieldValue { get; set; }
            public string strCol4ItemInfoFieldValue { get; set; }
            public string strCol5ItemInfoFieldValue { get; set; }
            public string strCol6ItemInfoFieldValue { get; set; }
            public string strCol7ItemInfoFieldValue { get; set; }
            public string strCol8ItemInfoFieldValue { get; set; }
            public string strCol9ItemInfoFieldValue { get; set; }
            public string strCol10ItemInfoFieldValue { get; set; }
            public string strCol11ItemInfoFieldValue { get; set; }
            public string strCol12ItemInfoFieldValue { get; set; }
            public string strCol13ItemInfoFieldValue { get; set; }
            public string strCol14ItemInfoFieldValue { get; set; }
            public string strCol15ItemInfoFieldValue { get; set; }
            public string strCol16ItemInfoFieldValue { get; set; }
            public string strCol17ItemInfoFieldValue { get; set; }
            public string strCol18ItemInfoFieldValue { get; set; }
            public string strCol19ItemInfoFieldValue { get; set; }
            public string strCol20ItemInfoFieldValue { get; set; }



            public string strIsLocked { get; set; }

            public string dcOverallScore { get; set; }


            public string strInspectedByName { get; set; }

            public string Url { get; set; }
            public string SECURITY_TYPE_AREA { get; set; }
            public string SECURITY_TYPE_ITEM { get; set; }
            public string SECURITY_TYPE_TRAN { get; set; }
            public string strItemName
            {
                get
                {
                    return "<a class=\"aLink\" target=\"_blank\" href=\"ViewForm.aspx?intItemInstanceTranID=" + this.intItemInstanceTranID.ToString() + "\">" + this.strItemNAme.ToString() + "</a>";

                }
            }
            public string strItemChk
            {
                get
                {
                    return "<input type=\"checkbox\" title=\"Select\"  ondblclick=\"javascript:selectCheckboxIE(" + this.intItemInstanceTranID.ToString() + ")\"  onclick=\"javascript:SelectCheckbox(" + this.intItemInstanceTranID.ToString() + ")\" id=\"chk" + this.intItemInstanceTranID.ToString() + "\"/>";

                }
            }
            public string strViewEditImage
            {
                get
                {
                    return "<a class=\"aLink\" target=\"_blank\"  href=\"ViewImage.aspx?intItemInstanceTranId=" + this.intItemInstanceTranID.ToString() + "\">Image</a>";

                }
            }
        }
    }
}
