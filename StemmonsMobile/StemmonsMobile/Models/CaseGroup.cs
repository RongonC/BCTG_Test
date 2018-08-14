using System;
using System.Collections.Generic;
using StemmonsMobile.Models;

namespace StemmonsMobile.Models
{

        public class CaseGroup : List<Case>
    {
        public string Heading { get; set; }
    }

    public class RelatedItemGroup : List<StaticTypes>
    {
        public string Heading { get; set; }
    }

}
