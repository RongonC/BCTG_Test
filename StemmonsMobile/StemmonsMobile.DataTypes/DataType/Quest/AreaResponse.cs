
using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class AreaResponse : Response
    {
        public object ResponseContent { get; set; }
        public class Area
        {
            public int intAreaID { get; set; }

            public string strAreaName { get; set; }

            public string strDepartment { get; set; }

            public string strAreaDesc { get; set; }

            public string securityType { get; set; }
            public Area(int AreaID, string AreaName, string strsecurityType)
            {
                intAreaID = AreaID;
                strAreaName = AreaName;
                securityType = strsecurityType;
            }

            public Area()
            { }

            public char? IsActive { get; set; }
        }
    }
}