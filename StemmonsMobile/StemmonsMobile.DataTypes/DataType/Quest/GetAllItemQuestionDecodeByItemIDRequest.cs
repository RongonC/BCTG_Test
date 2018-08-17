using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetAllItemQuestionDecodeByItemIDRequest
    {
        public int? ItemID { get; set; }

    }

    public partial class ITEM_QUESTION_DECODE
    {

        public int _ITEM_QUESTION_DECODE_ID;

        public int _ITEM_QUESTION_FIELD_ID;

        public string _MEETS_STANDARDS;

        public decimal? _POINTS_AVAILABLE;

        public decimal? _POINTS_EARNED;

        public decimal? _DISPLAY_ORDER;

        public decimal? _IS_ACTIVE;

        public decimal? _IS_HIGHLIGHT_ROW;

        public decimal? _IS_DEFAULT;
    }
}
