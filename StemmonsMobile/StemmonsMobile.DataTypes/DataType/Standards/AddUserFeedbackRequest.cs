
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Standards
{
    public class AddUserFeedbackRequest

    {
        public string sUserSAM { get; set; }
        public int iAppAssocMetaDataID { get; set; }

        public string sComments { get; set; }
        public char cUserResponse { get; set; }

    }
}